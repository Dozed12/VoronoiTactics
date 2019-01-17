using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

//https://github.com/Zalgo2462/VoronoiLib
using VoronoiLib;
using VoronoiLib.Structures;

//https://github.com/Auburns/FastNoise_CSharp

public struct MapSettings
{
    //Pixel width of map
    public int WIDTH;
    //Pixel height of map
    public int HEIGHT;
    //Number of estimated sites
    public int NUMBER_SITES;
    //Disperses sites more
    public float SITE_RELAXATION;

    public MapSettings(int width, int height, int number_sites, float site_relaxation)
    {
        this.WIDTH = width;
        this.HEIGHT = height;
        this.NUMBER_SITES = number_sites;
        this.SITE_RELAXATION = site_relaxation;
    }
}

public class ProvinceData
{
    public int id;
    public VPoint pos;
    public List<FortuneSite> neighborsRAW;
    public List<VPoint> vertices;
    public VPoint center;
    //TODO neighbor will be a costum struct
    public List<ProvinceData> neighbors;
    public UnitData unit = null;
    public float heightVal;
    public TerrainHeight height;
    public float terrainVal;
    public TerrainType terrain;
    public float structureVal;
    public TerrainStructure structure;
}

public struct MapGeography
{
    public float[,] HEIGHTMAP;
    public float[,] TERRAINMAP;
}

public class MapData
{

    private FastNoise fastnoise = new FastNoise();
    private List<FortuneSite> points;
    private float pointsHorizontal;
    private float pointsVertical;
    private float pointsHorizontalSeparation;
    private float pointsVerticalSeparation;
    private LinkedList<VEdge> edges;
    private Graph graph = new Graph();

    public Data data;
    public MapSettings settings;
    public Dictionary<string, Texture2D> mapModes;
    public MapGeography geography;
    public Biome biome;
    public List<ProvinceData> provinces;

    public MapData(Data data)
    {
        this.data = data;
    }

    //Generate the map data
    public void Generate()
    {

        //Generate points
        float time = Time.realtimeSinceStartup;
        points = GeneratePoints();
        Debug.Log("Point Generation took: " + (Time.realtimeSinceStartup - time) + "s");

        //Run Voronoi
        time = Time.realtimeSinceStartup;
        edges = FortunesAlgorithm.Run(points, 0, 0, settings.WIDTH, settings.HEIGHT);
        Debug.Log("Voronoi took: " + (Time.realtimeSinceStartup - time) + "s");

        //Setups Cell for each point (missing in library)
        time = Time.realtimeSinceStartup;
        SetupCells();
        Debug.Log("Setup Cells took: " + (Time.realtimeSinceStartup - time) + "s");

        //Generate Geography
        time = Time.realtimeSinceStartup;
        GenerateGeography();
        Debug.Log("Geography took: " + (Time.realtimeSinceStartup - time) + "s");

        //Create Provinces
        time = Time.realtimeSinceStartup;
        provinces = CreateProvinces();
        Debug.Log("Provinces Setup took: " + (Time.realtimeSinceStartup - time) + "s");

        //Generate all graphics
        time = Time.realtimeSinceStartup;
        GenerateGraphics();
        Debug.Log("Graphics took: " + (Time.realtimeSinceStartup - time) + "s");

        Debug.Log("Map Generation Complete");

    }

    /*
        Generate a list of FortuneSites(just coordinates)
        1 Divides the sites in a grid
        2 Offsets the site by an allowed amount
        Note this doesn't guarantee the correct number of sites because of division 
    */
    private List<FortuneSite> GeneratePoints()
    {
        List<FortuneSite> nPoints = new List<FortuneSite>();

        //Formulas to distribute points evenly in map
        pointsHorizontal = Mathf.Floor(Mathf.Sqrt(settings.NUMBER_SITES * settings.WIDTH / settings.HEIGHT));
        pointsVertical = Mathf.Floor(Mathf.Sqrt(settings.HEIGHT) * Mathf.Sqrt(settings.NUMBER_SITES) / Mathf.Sqrt(settings.WIDTH));
        pointsHorizontalSeparation = settings.WIDTH / pointsHorizontal;
        pointsVerticalSeparation = settings.HEIGHT / pointsVertical;

        //Max allowed separation with relaxation
        float pointsHorizontalAllowedRadius = pointsHorizontalSeparation / settings.SITE_RELAXATION;
        float pointsVerticalAllowedRadius = pointsVerticalSeparation / settings.SITE_RELAXATION;

        //Place points
        int id = 0;
        for (int i = 0; i < pointsHorizontal; i++)
        {
            for (int j = 0; j < pointsVertical; j++)
            {
                //Grid placement
                double x = pointsHorizontalSeparation * (i + 0.5f);
                double y = pointsVerticalSeparation * (j + 0.5f);
                //Randomize angular offset
                float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);
                float a = UnityEngine.Random.Range(0, pointsHorizontalAllowedRadius);
                float b = UnityEngine.Random.Range(0, pointsVerticalAllowedRadius);
                float ab = a * b;
                float aa = a * a;
                float bb = b * b;
                float tanAngle = Mathf.Tan(angle);
                double offX = ab / Mathf.Sqrt((bb) + (aa) * (tanAngle * tanAngle));
                double offY = ab / Mathf.Sqrt((aa) + (bb) / (tanAngle * tanAngle));
                //Quadrant check
                if (angle > -Mathf.PI / 2 && angle < Mathf.PI / 2)
                    nPoints.Add(new FortuneSite(x + offX, y + offY, id));
                else
                    nPoints.Add(new FortuneSite(x - offX, y - offY, id));
                id++;
            }
        }

        return nPoints;
    }

    //Setup points cell
    private void SetupCells()
    {

        //For each point
        for (int i = 0; i < points.Count; i++)
        {

            //Each point has to look for its edges
            var edge = edges.First;
            for (int j = 0; j < edges.Count; j++)
            {
                //Side with same ID
                if (edge.Value.Left.ID == points[i].ID || edge.Value.Right.ID == points[i].ID)
                {
                    points[i].Cell.Add(edge.Value);
                }

                //Next edge
                edge = edge.Next;
            }

        }

    }

    //Generates all the geography of the map
    private void GenerateGeography()
    {

        //Set Settings
        //TODO Should be set elsewhere
        //TODO Wont need so many octaves considering we dont use all the detail, but maybe we will
        fastnoise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
        fastnoise.SetFractalOctaves(4);
        fastnoise.SetFractalLacunarity(2.0f);
        fastnoise.SetFrequency(1);

        //Noise block size
        //TODO probably better in other place
        int blockSize = 10;

        //Noise lookups
        HeightMap();
        TerrainMap();

        //Generate Heightmap
        void HeightMap()
        {

            //New seed
            fastnoise.SetSeed(UnityEngine.Random.Range(Int16.MinValue, Int16.MaxValue));

            //Biome height frequency
            fastnoise.SetFrequency(biome.heightFreq);

            //Temporary noise blocks
            float[,] blocks = new float[settings.WIDTH / blockSize, settings.HEIGHT / blockSize];

            //Get noise blocks
            for (int i = 0; i < settings.WIDTH / blockSize; i++)
            {
                for (int j = 0; j < settings.HEIGHT / blockSize; j++)
                {
                    blocks[i, j] = (fastnoise.GetNoise(i * blockSize, j * blockSize, 0) + 1) / 2;
                }
            }

            //Full noise
            geography.HEIGHTMAP = new float[settings.WIDTH, settings.HEIGHT];

            //Get full noise from blocks
            for (int i = 0; i < settings.WIDTH / blockSize; i++)
            {
                int xIndexCorner = i * blockSize;
                for (int j = 0; j < settings.HEIGHT / blockSize; j++)
                {
                    int yIndexCorner = j * blockSize;
                    for (int x = 0; x < blockSize; x++)
                    {
                        int xIndexFinal = xIndexCorner + x;
                        for (int y = 0; y < blockSize; y++)
                        {
                            geography.HEIGHTMAP[xIndexFinal, yIndexCorner + y] = blocks[i, j];
                        }
                    }
                }
            }

        }

        //Generate Terrainmap
        void TerrainMap()
        {

            //New seed
            fastnoise.SetSeed(UnityEngine.Random.Range(Int16.MinValue, Int16.MaxValue));

            //Biome height frequency
            fastnoise.SetFrequency(biome.terrainFreq);

            //Temporary noise blocks
            float[,] blocks = new float[settings.WIDTH / blockSize, settings.HEIGHT / blockSize];

            //Get noise blocks
            for (int i = 0; i < settings.WIDTH / blockSize; i++)
            {
                for (int j = 0; j < settings.HEIGHT / blockSize; j++)
                {
                    blocks[i, j] = (fastnoise.GetNoise(i * blockSize, j * blockSize, 0) + 1) / 2;
                }
            }

            //Full noise
            geography.TERRAINMAP = new float[settings.WIDTH, settings.HEIGHT];

            //Get full noise from blocks
            for (int i = 0; i < settings.WIDTH / blockSize; i++)
            {
                int xIndexCorner = i * blockSize;
                for (int j = 0; j < settings.HEIGHT / blockSize; j++)
                {
                    int yIndexCorner = j * blockSize;
                    for (int x = 0; x < blockSize; x++)
                    {
                        int xIndexFinal = xIndexCorner + x;
                        for (int y = 0; y < blockSize; y++)
                        {
                            geography.TERRAINMAP[xIndexFinal, yIndexCorner + y] = blocks[i, j];
                        }
                    }
                }
            }

        }

    }

    //Create provinces from voronoi data
    //TODO Other specifics(might need to wait on terrain features generation like rivers, impassible cliffs)
    private List<ProvinceData> CreateProvinces()
    {
        List<ProvinceData> nProvinces = new List<ProvinceData>();

        //Create Site
        for (int i = 0; i < points.Count; i++)
        {
            ProvinceData nSite = new ProvinceData();

            //Most data
            nSite.id = i;
            nSite.neighborsRAW = points[i].Neighbors;
            nSite.neighbors = new List<ProvinceData>();
            nSite.pos = new VPoint(points[i].X, points[i].Y);

            //Calculate average terrain and height for this province
            float heightTotal = 0;
            float heightN = 0;
            for (int a = (int)(-pointsHorizontalSeparation / 4); a < pointsHorizontalSeparation / 4; a++)
            {
                if ((int)nSite.pos.X + a < 0 || (int)nSite.pos.X + a > settings.WIDTH - 1)
                    continue;
                for (int b = (int)(-pointsVerticalSeparation / 4); b < pointsVerticalSeparation / 4; b++)
                {
                    if ((int)nSite.pos.Y + b < 0 || (int)nSite.pos.Y + b > settings.HEIGHT - 1)
                        continue;
                    heightTotal += geography.HEIGHTMAP[(int)nSite.pos.X + a, (int)nSite.pos.Y + b];
                    heightN++;
                }
            }
            nSite.heightVal = heightTotal / heightN;

            float terrainTotal = 0;
            float terrainN = 0;
            for (int a = (int)(-pointsHorizontalSeparation / 4); a < pointsHorizontalSeparation / 4; a++)
            {
                if ((int)nSite.pos.X + a < 0 || (int)nSite.pos.X + a > settings.WIDTH - 1)
                    continue;
                for (int b = (int)(-pointsVerticalSeparation / 4); b < pointsVerticalSeparation / 4; b++)
                {
                    if ((int)nSite.pos.Y + b < 0 || (int)nSite.pos.Y + b > settings.HEIGHT - 1)
                        continue;
                    terrainTotal += geography.TERRAINMAP[(int)nSite.pos.X + a, (int)nSite.pos.Y + b];
                    terrainN++;
                }
            }
            nSite.terrainVal = terrainTotal / terrainN;

            //Identify type from data and values
            for (int j = 0; j < biome.heights.Length; j++)
            {
                if (biome.heights[j].noiseMin <= nSite.heightVal && nSite.heightVal < biome.heights[j].noiseMax)
                {
                    nSite.height = data.heights[biome.heights[j].name];
                    break;
                }
            }
            for (int j = 0; j < biome.terrains.Length; j++)
            {
                if (biome.terrains[j].noiseMin <= nSite.terrainVal && nSite.terrainVal < biome.terrains[j].noiseMax)
                {
                    //Need to check if height present is accepted by terrain, if not then use fallback instead
                    //Search
                    bool found = false;
                    for (int h = 0; h < biome.terrains[j].type.heights.Length; h++)
                    {
                        if (biome.terrains[j].type.heights[h].name == nSite.height.name)
                        {
                            found = true;
                            break;
                        }
                    }

                    //Use fallback
                    if (!found)
                    {

                        //Default fallback
                        nSite.terrain = data.terrains[biome.terrains[j].type.height_default_fallback];

                        //No specifics
                        if (data.terrains[biome.terrains[j].name].height_fallbacks == null)
                            break;

                        //Specific fallbacks
                        for (int f = 0; f < data.terrains[biome.terrains[j].name].height_fallbacks.Count; f++)
                        {
                            if (nSite.height.name == data.terrains[biome.terrains[j].name].height_fallbacks[f].First.name)
                            {
                                nSite.terrain = data.terrains[biome.terrains[j].name].height_fallbacks[f].Second;
                                break;
                            }
                        }

                        break;
                    }
                    //Use local
                    else
                    {
                        nSite.terrain = data.terrains[biome.terrains[j].name];
                        break;
                    }
                }
            }

            //Vertices
            nSite.vertices = new List<VPoint>();
            for (int j = 0; j < points[i].Cell.Count; j++)
            {
                nSite.vertices.Add(points[i].Cell[j].Start);
                nSite.vertices.Add(points[i].Cell[j].End);
            }
            nSite.vertices = nSite.vertices.Distinct().ToList();

            //Add corner vertex for corner provinces
            if (i == 0)
                nSite.vertices.Add(new VPoint(0, 0));
            else if (i == pointsHorizontal - 1)
                nSite.vertices.Add(new VPoint(0, settings.HEIGHT));
            else if (i == points.Count - pointsHorizontal)
                nSite.vertices.Add(new VPoint(settings.WIDTH, 0));
            else if (i == points.Count - 1)
                nSite.vertices.Add(new VPoint(settings.WIDTH, settings.HEIGHT));

            //Center
            double x = 0, y = 0;
            for (int j = 0; j < nSite.vertices.Count; j++)
            {
                x += nSite.vertices[j].X;
                y += nSite.vertices[j].Y;
            }
            x /= nSite.vertices.Count;
            y /= nSite.vertices.Count;
            nSite.center = new VPoint(x, y);

            //Add
            nProvinces.Add(nSite);
        }

        //Neighbor Connections after all provinces created
        for (int i = 0; i < nProvinces.Count; i++)
        {

            //Connections for graph
            Dictionary<int, int> connections = new Dictionary<int, int>();

            for (int p = 0; p < nProvinces[i].neighborsRAW.Count; p++)
            {

                //Add to pathfind connections
                //TODO assumes cost of 1, terrain might impact this
                connections.Add(nProvinces[i].neighborsRAW[p].ID, 1);

                //Instance match
                for (int j = 0; j < nProvinces.Count; j++)
                {
                    //Dont check with itself
                    if (i == j)
                        continue;

                    //Check ID match
                    if (nProvinces[i].neighborsRAW[p].ID == nProvinces[j].id)
                    {
                        nProvinces[i].neighbors.Add(nProvinces[j]);
                    }
                }
            }

            //Add to graph for pathfind
            graph.vertex(nProvinces[i].id, connections);

        }

        return nProvinces;
    }

    //Generate graphics
    private void GenerateGraphics()
    {

        mapModes = new Dictionary<string, Texture2D>();

        float time = Time.realtimeSinceStartup;

        HeightNoiseMapTexture();

        Debug.Log("==== HeightNoiseMapTexture took: " + (Time.realtimeSinceStartup - time) + "s");
        time = Time.realtimeSinceStartup;

        TerrainNoiseMapTexture();

        Debug.Log("==== TerrainNoiseMapTexture took: " + (Time.realtimeSinceStartup - time) + "s");
        time = Time.realtimeSinceStartup;

        SimplifiedHeightMapTexture();

        Debug.Log("==== SimplifiedHeightMapTexture took: " + (Time.realtimeSinceStartup - time) + "s");
        time = Time.realtimeSinceStartup;

        SimplifiedTerrainMapTexture();

        Debug.Log("==== SimplifiedTerrainMapTexture took: " + (Time.realtimeSinceStartup - time) + "s");
        float timeFinal = Time.realtimeSinceStartup;

        FinalMapTexture();

        Debug.Log("==== FinalMapTexture took: " + (Time.realtimeSinceStartup - timeFinal) + "s");

        //Generate Heightmap texture (greyscale)
        void HeightNoiseMapTexture()
        {

            //Height Greyscale (Quite simple so doesnt need PixelMatrix)
            Color[] pixels = new Color[settings.WIDTH * settings.HEIGHT];
            for (int i = 0; i < settings.WIDTH; i++)
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    float val = geography.HEIGHTMAP[i, j];
                    pixels[i * settings.WIDTH + j] = new Color(val, val, val);
                }
            }

            //Create texture
            Texture2D texture = new Texture2D(settings.WIDTH, settings.HEIGHT);

            //Set pixels
            texture.SetPixels(pixels);

            //Apply to texture
            texture.Apply();

            //Settings
            texture.filterMode = FilterMode.Point;

            //Add to list
            mapModes.Add("Height Noise Map", texture);

        }

        //Generate Terrain texture
        void TerrainNoiseMapTexture()
        {

            //Terrain Greyscale (Quite simple so doesnt need PixelMatrix)
            Color[] pixels = new Color[settings.WIDTH * settings.HEIGHT];
            for (int i = 0; i < settings.WIDTH; i++)
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    float val = geography.TERRAINMAP[i, j];
                    pixels[i * settings.WIDTH + j] = new Color(val, val, val);
                }
            }

            //Create texture
            Texture2D texture = new Texture2D(settings.WIDTH, settings.HEIGHT);

            //Set pixels
            texture.SetPixels(pixels);

            //Apply to texture
            texture.Apply();

            //Settings
            texture.filterMode = FilterMode.Point;

            //Add to list
            mapModes.Add("Terrain Noise Map", texture);

        }

        //Generate Simplifided Height Map Texture
        void SimplifiedHeightMapTexture()
        {

            //List of colors and use number
            Dictionary<Color, int> colors = new Dictionary<Color, int>();
            for (int i = 0; i < provinces.Count; i++)
            {
                Color cl = new Color(provinces[i].height.color / 255.0f, provinces[i].height.color / 255.0f, provinces[i].height.color / 255.0f);

                int curVal;
                if (colors.ContainsKey(cl))
                {
                    curVal = colors[cl];
                    colors.Remove(cl);
                    colors.Add(cl, curVal + 1);
                }
                else
                {
                    colors.Add(cl, 1);
                }
            }

            //Find max value
            int max = -1;
            Color maxCl = new Color(0, 0, 0);
            foreach (var item in colors)
            {
                if (item.Value > max)
                {
                    max = item.Value;
                    maxCl = item.Key;
                }
            }

            //Pixel matrix for texture start with most used color
            Graphics.PixelMatrix pixelMatrix = new Graphics.PixelMatrix(settings.WIDTH, settings.HEIGHT, maxCl);

            //Draw Border
            pixelMatrix = Graphics.Border(pixelMatrix, Color.black);

            //Draw edges
            //TODO Jitter edges for more detail(could be done here or in internal data, the jitter wont affect any calculations so can be just graphical)
            //But it will be shared in many maps so should be internal!
            var edge = edges.First;
            for (int i = 0; i < edges.Count; i++)
            {
                VEdge edgeVal = edge.Value;

                //Round
                int startX = Mathf.FloorToInt((float)edge.Value.Start.X);
                int endX = Mathf.FloorToInt((float)edge.Value.End.X);
                int startY = Mathf.FloorToInt((float)edge.Value.Start.Y);
                int endY = Mathf.FloorToInt((float)edge.Value.End.Y);

                //Draw Edge
                pixelMatrix = Graphics.BresenhamLine(pixelMatrix, startX, startY, endX, endY, Color.black);
                edge = edge.Next;
            }

            //Fill sites color
            for (int i = 0; i < provinces.Count; i++)
            {
                Color c = new Color(provinces[i].height.color / 255.0f, provinces[i].height.color / 255.0f, provinces[i].height.color / 255.0f);
                pixelMatrix = Graphics.FloodFillLine(pixelMatrix, (int)provinces[i].center.X, (int)provinces[i].center.Y, c);
            }

            //Add Site centers
            for (int i = 0; i < provinces.Count; i++)
            {
                pixelMatrix.SetPixel((int)provinces[i].center.X, (int)provinces[i].center.Y, Color.black);
            }

            //Create texture
            Texture2D texture = new Texture2D(settings.WIDTH, settings.HEIGHT);

            //Apply pixel set
            texture.SetPixels(pixelMatrix.pixels);

            //Apply to texture
            texture.Apply();

            //Settings
            texture.filterMode = FilterMode.Point;

            //Add to list
            mapModes.Add("Simplified Height Map", texture);
        }

        //Generate Simplifided Terrain Map Texture
        void SimplifiedTerrainMapTexture()
        {

            //List of colors and use number
            Dictionary<Color, int> colors = new Dictionary<Color, int>();
            for (int i = 0; i < provinces.Count; i++)
            {
                Color cl = new Color(provinces[i].terrain.color[0] / 255.0f, provinces[i].terrain.color[1] / 255.0f, provinces[i].terrain.color[2] / 255.0f);

                int curVal;
                if (colors.ContainsKey(cl))
                {
                    curVal = colors[cl];
                    colors.Remove(cl);
                    colors.Add(cl, curVal + 1);
                }
                else
                {
                    colors.Add(cl, 1);
                }
            }

            //Find max value
            int max = -1;
            Color maxCl = new Color(0, 0, 0);
            foreach (var item in colors)
            {
                if (item.Value > max)
                {
                    max = item.Value;
                    maxCl = item.Key;
                }
            }

            //Pixel matrix for texture start with most used color
            Graphics.PixelMatrix pixelMatrix = new Graphics.PixelMatrix(settings.WIDTH, settings.HEIGHT, maxCl);

            //Draw Border
            pixelMatrix = Graphics.Border(pixelMatrix, Color.black);

            //Draw edges
            //TODO Jitter edges for more detail(could be done here or in internal data, the jitter wont affect any calculations so can be just graphical)
            //But it will be shared in many maps so should be internal!
            var edge = edges.First;
            for (int i = 0; i < edges.Count; i++)
            {
                VEdge edgeVal = edge.Value;

                //Round
                int startX = Mathf.FloorToInt((float)edge.Value.Start.X);
                int endX = Mathf.FloorToInt((float)edge.Value.End.X);
                int startY = Mathf.FloorToInt((float)edge.Value.Start.Y);
                int endY = Mathf.FloorToInt((float)edge.Value.End.Y);

                //Draw Edge
                pixelMatrix = Graphics.BresenhamLine(pixelMatrix, startX, startY, endX, endY, Color.black);
                edge = edge.Next;
            }

            //Fill sites color
            for (int i = 0; i < provinces.Count; i++)
            {
                Color c = new Color(provinces[i].terrain.color[0] / 255.0f, provinces[i].terrain.color[1] / 255.0f, provinces[i].terrain.color[2] / 255.0f);
                pixelMatrix = Graphics.FloodFillLine(pixelMatrix, (int)provinces[i].center.X, (int)provinces[i].center.Y, c);
            }

            //Add Site centers
            for (int i = 0; i < provinces.Count; i++)
            {
                pixelMatrix.SetPixel((int)provinces[i].center.X, (int)provinces[i].center.Y, Color.black);
            }

            //Create texture
            Texture2D texture = new Texture2D(settings.WIDTH, settings.HEIGHT);

            //Apply pixel set
            texture.SetPixels(pixelMatrix.pixels);

            //Apply to texture
            texture.Apply();

            //Settings
            texture.filterMode = FilterMode.Point;

            //Add to list
            mapModes.Add("Simplified Terrain Map", texture);
        }

        //Generate Simplifided Terrain Map Texture
        void FinalMapTexture()
        {

            //Pixel set
            Graphics.PixelMatrix pixelMatrix = new Graphics.PixelMatrix(settings.WIDTH, settings.HEIGHT, Color.white);

            //Weighted color blend for terrain type
            //TODO Settings should be in other place
            //TODO Higher values cause darkening, is that normal?
            time = Time.realtimeSinceStartup;
            int differentiation = 2;
            int block = 10;
            int blocksW = settings.WIDTH / block;
            int blocksH = settings.HEIGHT / block;
            for (int i = 0; i < settings.WIDTH; i += block / 2)
            {
                for (int j = 0; j < settings.HEIGHT; j += block / 2)
                {

                    //Get terrain noise
                    float val = this.geography.TERRAINMAP[i, j];

                    //Calculate distance to noises
                    List<Pair<TerrainType, float>> distances = new List<Pair<TerrainType, float>>();
                    foreach (KeyValuePair<TerrainType, float> item in biome.terrainNoiseMiddles)
                    {
                        float diff = 1 - Mathf.Abs(val - item.Value);
                        float power = 1;
                        for (int d = 0; d < differentiation; d++)
                        {
                            power *= diff;
                        }
                        distances.Add(new Pair<TerrainType, float>(item.Key, power));
                    }

                    //Calculate weighted color
                    float r = 0;
                    float g = 0;
                    float b = 0;
                    float total = 0;
                    for (int d = 0; d < distances.Count; d++)
                    {
                        float t = distances[d].Second / 255.0f;
                        t *= t;
                        r += t * distances[d].First.color[0] * distances[d].First.color[0];
                        g += t * distances[d].First.color[1] * distances[d].First.color[1];
                        b += t * distances[d].First.color[2] * distances[d].First.color[2];
                        total += distances[d].Second;
                    }
                    r = Mathf.Sqrt(r / total);
                    g = Mathf.Sqrt(g / total);
                    b = Mathf.Sqrt(b / total);

                    Color color = new Color(r, g, b);

                    Graphics.PixelMatrix decal = new Graphics.PixelMatrix(block, block, color);

                    pixelMatrix = Graphics.Decal(pixelMatrix, decal, i, j);
                }
            }

            Debug.Log("======== Color blend took: " + (Time.realtimeSinceStartup - time) + "s");
            time = Time.realtimeSinceStartup;

            //Shade pixels based on height and neighbor
            //TODO Settings should be in other place
            //TODO Seems to be creating some graphical artifacts (horizontal lines)
            float shadowPower = 0;
            float lightPower = 0;
            for (int i = 0; i < settings.WIDTH; i++)
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    float height = geography.HEIGHTMAP[i, j];
                    float heightNeighbor;
                    //TODO Lighting direction comes from here
                    //TODO Lighting can be distributed in more than one neighbor(like a vector with magnitude 1)
                    if (i - 1 > 0)
                        heightNeighbor = geography.HEIGHTMAP[i - 1, j];
                    else
                        continue;
                    //Shadow
                    if (height < heightNeighbor)
                    {
                        Color color = pixelMatrix.GetPixel(i, j);
                        color.r = color.r * (1 - (heightNeighbor - height) * shadowPower);
                        color.g = color.g * (1 - (heightNeighbor - height) * shadowPower);
                        color.b = color.b * (1 - (heightNeighbor - height) * shadowPower);
                        pixelMatrix.SetPixel(i, j, color);
                    }
                    //Light
                    else
                    {
                        Color color = pixelMatrix.GetPixel(i, j);
                        color.r = color.r + (1 - color.r) * (heightNeighbor - height) * lightPower;
                        color.g = color.g + (1 - color.g) * (heightNeighbor - height) * lightPower;
                        color.b = color.b + (1 - color.b) * (heightNeighbor - height) * lightPower;
                        pixelMatrix.SetPixel(i, j, color);
                    }
                }
            }

            Debug.Log("======== Shading took: " + (Time.realtimeSinceStartup - time) + "s");
            time = Time.realtimeSinceStartup;

            //Add decals
            //TODO Can also have an option in JSON to randomize if it has decals or not (so we can put trees on Grassland but not look artificial)
            for (int i = 0; i < provinces.Count; i++)
            {
                //No decals so skip
                if (provinces[i].terrain.decals == null)
                    continue;

                //Each decal
                for (int d = 0; d < provinces[i].terrain.decals.Length; d++)
                {

                    //Decal reach
                    float decalHorizontalReach = pointsHorizontalSeparation / 2 * provinces[i].terrain.decals[d].reach;
                    float decalVerticalReach = pointsVerticalSeparation / 2 * provinces[i].terrain.decals[d].reach;
                    float reach = Mathf.Max(decalHorizontalReach, decalVerticalReach);

                    //Add decals in a circular way with random angle and radius
                    for (int c = 0; c < provinces[i].terrain.decals[d].number; c++)
                    {
                        //Position of decal center
                        float angle = UnityEngine.Random.Range(0.0f, 2 * Mathf.PI);
                        float radius = UnityEngine.Random.Range(0, reach);
                        float x = (float)provinces[i].center.X + Mathf.Cos(angle) * radius;
                        float y = (float)provinces[i].center.Y + Mathf.Sin(angle) * radius;

                        //Decal
                        Graphics.PixelMatrix decal = data.decals[provinces[i].terrain.decals[d].name];

                        //Rotate image with random angle (90o jumps)
                        if (provinces[i].terrain.decals[d].rotate)
                            decal = Graphics.Rotate(decal, UnityEngine.Random.Range(0, 3) * Mathf.PI / 2);

                        //Add decal
                        pixelMatrix = Graphics.Decal(pixelMatrix, decal, (int)x, (int)y);
                    }
                }

            }

            Debug.Log("======== Decals took: " + (Time.realtimeSinceStartup - time) + "s");
            time = Time.realtimeSinceStartup;

            //Add randomization to color
            //TODO Settings should be in other place
            float randomization = 30;
            float variation = 0.05f;
            for (int i = 0; i < settings.WIDTH; i++)
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    //Randomly skip
                    if (UnityEngine.Random.Range(1, 100) < 100 - randomization)
                        continue;

                    //Add some difference
                    Color color = pixelMatrix.GetPixel(i, j);
                    color.r = color.r + UnityEngine.Random.Range(-variation, variation);
                    color.g = color.g + UnityEngine.Random.Range(-variation, variation);
                    color.b = color.b + UnityEngine.Random.Range(-variation, variation);
                    pixelMatrix.SetPixel(i, j, color);
                }
            }

            Debug.Log("======== Randomization took: " + (Time.realtimeSinceStartup - time) + "s");
            time = Time.realtimeSinceStartup;

            //Draw Border
            pixelMatrix = Graphics.Border(pixelMatrix, Color.black);

            //Draw edges
            //TODO Jitter edges for more detail(could be done here or in internal data, the jitter wont affect any calculations so can be just graphical)
            //But it will be shared in many maps so should be internal!
            var edge = edges.First;
            for (int i = 0; i < edges.Count; i++)
            {
                VEdge edgeVal = edge.Value;

                //Round
                int startX = Mathf.FloorToInt((float)edge.Value.Start.X);
                int endX = Mathf.FloorToInt((float)edge.Value.End.X);
                int startY = Mathf.FloorToInt((float)edge.Value.Start.Y);
                int endY = Mathf.FloorToInt((float)edge.Value.End.Y);

                //Draw Edge
                pixelMatrix = Graphics.BresenhamLineThick(pixelMatrix, startX, startY, endX, endY, Color.black, 2);
                edge = edge.Next;
            }

            Debug.Log("======== Edges took: " + (Time.realtimeSinceStartup - time) + "s");

            //Add Site centers
            Graphics.PixelMatrix center = Graphics.FilledCircle(5, Color.black);
            for (int i = 0; i < provinces.Count; i++)
            {
                pixelMatrix = Graphics.Decal(pixelMatrix, center, (int)provinces[i].center.X, (int)provinces[i].center.Y);
            }

            //Create texture
            Texture2D texture = new Texture2D(settings.WIDTH, settings.HEIGHT);

            //Apply pixel set
            texture.SetPixels(pixelMatrix.pixels);

            //Apply to texture
            texture.Apply();

            //Settings
            texture.filterMode = FilterMode.Trilinear;

            //Add to list
            mapModes.Add("Map", texture);
        }

    }

}

public class Map : MonoBehaviour
{

    Data data;
    MapData mapData;

    public Dictionary<string, Sprite> mapModes;

    public Dropdown biomePick;
    public Dropdown mapModePick;

    // Start is called before the first frame update
    void Start()
    {
        //TODO Wont be here probably
        //Load data
        data = new Data();
        data.LoadData();

        //TODO Wont be here probably
        //Add Biome options to Dropdown
        List<string> biomeStrings = new List<string>();
        foreach (KeyValuePair<string, Biome> entry in data.biomes)
        {
            biomeStrings.Add(entry.Value.name);
        }
        biomePick.AddOptions(biomeStrings);
    }

    //Mouse controls (TEMPORARY)
    public float panSpeed = 50.0f;
    void Update()
    {

        //Mouse Zoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ZoomOrthoCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ZoomOrthoCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), -1);
        }

        //Mouse Drag
        if (Input.GetMouseButton(1))
        {
            transform.position += new Vector3(Input.GetAxis("Mouse X") * Time.deltaTime * panSpeed, Input.GetAxis("Mouse Y") * Time.deltaTime * panSpeed, 0);
        }

    }

    void ZoomOrthoCamera(Vector3 zoomTowards, float amount)
    {

        //Block at limits
        if (Camera.main.GetComponent<Camera>().orthographicSize == 3 && amount == 1)
            return;

        if (Camera.main.GetComponent<Camera>().orthographicSize == 10 && amount == -1)
            return;

        // Calculate how much we will have to move towards the zoomTowards position
        float multiplier = (1.0f / Camera.main.GetComponent<Camera>().orthographicSize * amount);

        // Move camera
        Camera.main.transform.position += (zoomTowards - Camera.main.transform.position) * multiplier;

        // Zoom camera
        Camera.main.GetComponent<Camera>().orthographicSize -= amount;

        // Limit zoom
        Camera.main.GetComponent<Camera>().orthographicSize = Mathf.Clamp(Camera.main.GetComponent<Camera>().orthographicSize, 3, 10);
    }

    //Generate new map
    public void Generate()
    {

        //New map data with settings
        //TODO Settings probably wont be here
        mapData = new MapData(data);
        //TODO Pixel size could be tied to province number to keep it consistent across different make sizes
        mapData.settings = new MapSettings(2000, 2000, 200, 2.0f);

        //Get selected biome
        int idBiome = biomePick.value;
        string nameBiome = biomePick.options[idBiome].text;

        //Set biome for map
        mapData.biome = data.biomes[nameBiome];

        //Master generate
        mapData.Generate();

        //Create sprites
        mapModes = new Dictionary<string, Sprite>();
        foreach (KeyValuePair<string, Texture2D> entry in mapData.mapModes)
        {
            mapModes.Add(entry.Key, Sprite.Create(entry.Value, new Rect(0, 0, entry.Value.width, entry.Value.height), new Vector2(0.5f, 0.5f)));
        }

        //Add to mapmodes Dropdown
        List<string> mapModesStrings = new List<string>();
        foreach (KeyValuePair<string, Sprite> entry in mapModes)
        {
            mapModesStrings.Add(entry.Key);
        }
        mapModePick.ClearOptions();
        mapModePick.AddOptions(mapModesStrings);

        //Set mapmode to whatever is selected
        OnMapModeChange();

    }

    public void OnMapModeChange()
    {

        //Get Dropdown value
        int idMapMode = mapModePick.value;
        string nameMapMode = mapModePick.options[idMapMode].text;

        //Set sprite to map
        GetComponent<SpriteRenderer>().sprite = mapModes[nameMapMode];

    }

}
