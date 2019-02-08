using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//https://github.com/Zalgo2462/VoronoiLib
using VoronoiLib;
using VoronoiLib.Structures;

//https://github.com/Auburns/FastNoise_CSharp

//Map generation settings
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

//Individual province data
public class ProvinceData
{
    public int id;
    public Vector2 pos;
    //Neighbor information from voronoi
    public List<FortuneSite> neighborsRAW;
    public List<Geometry.Vector2X> vertices;
    public Vector2 center;
    public Vector3 center3D;
    //TODO neighbor will be a custom struct(to holdmodifiers)
    public List<ProvinceData> neighbors;
    //Voronoi Site
    public FortuneSite point;
    public Unit unit = null;
    public float heightVal;
    public TerrainHeight height;
    public float terrainVal;
    public TerrainType terrain;
    public float structureVal;
    public TerrainStructure structure;
}

//Map geography information from altitude to terrain noise
public struct MapGeography
{
    public float[,] HEIGHTMAP;
    public float[,] TERRAINMAP;
    public float[,] SHADEMAP;
}

//Map data
public class MapData
{

    //Object holding this map data
    private Map mapObject;

    //Noise server
    private FastNoise fastnoise = new FastNoise();

    //Voronoi Sites
    private List<FortuneSite> points;

    //Site placement information
    private float pointsHorizontal;
    private float pointsVertical;
    private float pointsHorizontalSeparation;
    private float pointsVerticalSeparation;

    //Voronoi edges
    private LinkedList<VEdge> edges;

    //Simplified Voronoi Edges
    private List<Geometry.Vector2Edge> simpleEdges;

    //Jittered Voronoi Edges
    private List<Geometry.Vector2Edge> jitteredEdges;

    //Pathfinding graph (Dijkstra)
    private Graph graph = new Graph();

    //File data
    public Data data;

    //Generation settings
    public MapSettings settings;

    //Map mode textures
    public Dictionary<string, Texture2D> mapModes;

    //Map geography
    public MapGeography geography;

    //Biome set(Could be inside Settings struct)
    public Biome biome;

    //Provinces of map
    public List<ProvinceData> provinces;

    //Constructor just receives the file data
    public MapData(Data data, Map obj)
    {
        this.data = data;
        this.mapObject = obj;
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
            //X Grid placement  
            float x = pointsHorizontalSeparation * (i + 0.5f);
            for (int j = 0; j < pointsVertical; j++)
            {
                //Y Grid placement                
                float y = pointsVerticalSeparation * (j + 0.5f);

                //Randomize angular offset
                float angle = UnityEngine.Random.Range(0, Utilities.PI2);
                float a = UnityEngine.Random.Range(0, pointsHorizontalAllowedRadius);
                float b = UnityEngine.Random.Range(0, pointsVerticalAllowedRadius);
                float ab = a * b;
                float aa = a * a;
                float bb = b * b;
                float tanAngle = Mathf.Tan(angle);
                float offX = ab / Mathf.Sqrt((bb) + (aa) * (tanAngle * tanAngle));
                float offY = ab / Mathf.Sqrt((aa) + (bb) / (tanAngle * tanAngle));

                //Quadrant check
                if (angle > -Utilities.HALFPI && angle < Utilities.HALFPI)
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

        //Convert Linked Edges to simple List
        simpleEdges = new List<Geometry.Vector2Edge>();
        var edge = edges.First;
        for (int j = 0; j < edges.Count; j++)
        {
            simpleEdges.Add(new Geometry.Vector2Edge(new Geometry.Vector2X((float)edge.Value.Start.X, (float)edge.Value.Start.Y), new Geometry.Vector2X((float)edge.Value.End.X, (float)edge.Value.End.Y), edge.Value.Left, edge.Value.Right));

            //Next edge
            edge = edge.Next;
        }

        //Jittering
        //TODO Settings should be in another place
        int divisions = 3;
        float magnitude = 5;
        float minSize = 30;
        jitteredEdges = new List<Geometry.Vector2Edge>();

        for (int j = 0; j < simpleEdges.Count; j++)
        {

            //Dont jitter map border edges
            if (simpleEdges[j].start.value.x == 0 && simpleEdges[j].end.value.x == 0)
            {
                jitteredEdges.Add(simpleEdges[j]);
                continue;
            }
            if (simpleEdges[j].start.value.y == 0 && simpleEdges[j].end.value.y == 0)
            {
                jitteredEdges.Add(simpleEdges[j]);
                continue;
            }
            if (simpleEdges[j].start.value.x == settings.WIDTH - 1 && simpleEdges[j].end.value.x == settings.WIDTH - 1)
            {
                jitteredEdges.Add(simpleEdges[j]);
                continue;
            }
            if (simpleEdges[j].start.value.y == settings.HEIGHT - 1 && simpleEdges[j].end.value.y == settings.HEIGHT - 1)
            {
                jitteredEdges.Add(simpleEdges[j]);
                continue;
            }

            //Minimum size to jitter
            if (Vector2.Distance(simpleEdges[j].start.value, simpleEdges[j].end.value) < minSize)
            {
                jitteredEdges.Add(simpleEdges[j]);
                continue;
            }

            //Jitter
            List<Geometry.Vector2X> jitter = Geometry.Jitter(simpleEdges[j].start, simpleEdges[j].end, divisions, magnitude);

            //Add jitter edges
            for (int i = 0; i < jitter.Count - 1; i++)
            {
                Geometry.Vector2Edge nEdge = new Geometry.Vector2Edge(jitter[i], jitter[i + 1], simpleEdges[j].left, simpleEdges[j].right);
                jitteredEdges.Add(nEdge);
            }
        }

        //For each point
        for (int i = 0; i < points.Count; i++)
        {

            //Each point has to look for its edges
            for (int j = 0; j < simpleEdges.Count; j++)
            {
                //Side with same ID
                if (simpleEdges[j].left.ID == points[i].ID || simpleEdges[j].right.ID == points[i].ID)
                {
                    points[i].Cell.Add(simpleEdges[j]);
                }
            }

            //Each point has to look for its jitter edges
            for (int j = 0; j < jitteredEdges.Count; j++)
            {
                //Side with same ID
                if (jitteredEdges[j].left.ID == points[i].ID || jitteredEdges[j].right.ID == points[i].ID)
                {
                    points[i].CellJitter.Add(jitteredEdges[j]);
                }
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
        fastnoise.SetFractalType(FastNoise.FractalType.FBM);
        fastnoise.SetFractalOctaves(3);
        fastnoise.SetFractalLacunarity(2.0f);
        fastnoise.SetFrequency(1);

        //Noise block size
        //TODO probably better in other place
        int terrainBlockSize = 10;
        int heightBlockSize = 3;

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

            //Number of blocks
            int widthNBlocks = settings.WIDTH / heightBlockSize;
            int heightNBlocks = settings.HEIGHT / heightBlockSize;

            //Temporary noise blocks
            float[,] blocks = new float[widthNBlocks, heightNBlocks];

            //Multithreaded noise blocks
            Parallel.For(0, widthNBlocks, i =>
            {
                int finalI = i * heightBlockSize;
                for (int h = 0; h < heightNBlocks; h++)
                {
                    blocks[i, h] = (fastnoise.GetNoise(finalI, h * heightBlockSize, 0) + 1) / 2;
                }
            });

            //Full noise
            geography.HEIGHTMAP = new float[settings.WIDTH, settings.HEIGHT];

            //Get full noise from blocks
            //Multithreaded
            Parallel.For(0, widthNBlocks, i =>
            {
                int xIndexCorner = i * heightBlockSize;
                for (int j = 0; j < heightNBlocks; j++)
                {
                    int yIndexCorner = j * heightBlockSize;
                    for (int x = 0; x < heightBlockSize; x++)
                    {
                        int xIndexFinal = xIndexCorner + x;
                        for (int y = 0; y < heightBlockSize; y++)
                        {
                            geography.HEIGHTMAP[xIndexFinal, yIndexCorner + y] = blocks[i, j];
                        }
                    }
                }
            });

            //Shademap
            ShadeMap(widthNBlocks, heightNBlocks, blocks);

        }

        //Generate Shademap
        void ShadeMap(int widthNBlocks, int heightNBlocks, float[,] blocks)
        {

            //Generate Shading map
            //TODO Place this in different function
            //TODO Use proper color modification (not direct multiplication)
            geography.SHADEMAP = new float[settings.WIDTH, settings.HEIGHT];

            //TODO settings elsewhere
            int differenceScale = 100;
            int differencePow = 2;
            float differenceLimit = 0.4f;
            //Multithreaded
            Parallel.For(0, widthNBlocks, i =>
            {
                int xIndexCorner = i * heightBlockSize;
                for (int j = 0; j < heightNBlocks; j++)
                {
                    int yIndexCorner = j * heightBlockSize;

                    //Local block
                    float curr = blocks[i, j];

                    //Neighbor blocks
                    float neighbor = curr;

                    //Direction
                    if (j + 1 <= heightNBlocks - 1)
                        neighbor = blocks[i, j + 1];
                    //If out of range use last possible
                    else
                    {
                        curr = neighbor = blocks[i, j - 1];
                        neighbor = blocks[i, j];
                    }

                    //Difference from neighbor
                    float tmp = Mathf.Abs(curr - neighbor);

                    //Difference scale
                    tmp *= differenceScale;

                    //Difference power
                    float diff = 1;
                    for (int p = 0; p < differencePow; p++)
                    {
                        diff *= tmp;
                    }

                    //Limit
                    if (diff > differenceLimit)
                        diff = differenceLimit;

                    //Darken or Lighten
                    if (curr < neighbor)
                        diff = 1 - diff;
                    else
                        diff = 1 + diff;

                    //Store in proper size matrix
                    for (int x = 0; x < heightBlockSize; x++)
                    {
                        int xIndexFinal = xIndexCorner + x;
                        for (int y = 0; y < heightBlockSize; y++)
                        {
                            geography.SHADEMAP[xIndexFinal, yIndexCorner + y] = diff;
                        }
                    }

                }
            });

        }

        //Generate Terrainmap
        void TerrainMap()
        {

            //New seed
            fastnoise.SetSeed(UnityEngine.Random.Range(Int16.MinValue, Int16.MaxValue));

            //Biome terrain frequency
            fastnoise.SetFrequency(biome.terrainFreq);

            //Number of blocks
            int widthNBlocks = settings.WIDTH / terrainBlockSize;
            int heightNBlocks = settings.HEIGHT / terrainBlockSize;

            //Temporary noise blocks
            float[,] blocks = new float[widthNBlocks, heightNBlocks];

            //Get noise blocks
            Parallel.For(0, widthNBlocks, i =>
            {
                int finalI = i * terrainBlockSize;
                for (int j = 0; j < heightNBlocks; j++)
                {
                    blocks[i, j] = (fastnoise.GetNoise(finalI, j * terrainBlockSize, 0) + 1) / 2;
                }
            });

            //Full noise
            geography.TERRAINMAP = new float[settings.WIDTH, settings.HEIGHT];

            //Get full noise from blocks
            //Multithreaded
            Parallel.For(0, widthNBlocks, i =>
            {
                int xIndexCorner = i * terrainBlockSize;
                for (int j = 0; j < heightNBlocks; j++)
                {
                    int yIndexCorner = j * terrainBlockSize;
                    for (int x = 0; x < terrainBlockSize; x++)
                    {
                        int xIndexFinal = xIndexCorner + x;
                        for (int y = 0; y < terrainBlockSize; y++)
                        {
                            geography.TERRAINMAP[xIndexFinal, yIndexCorner + y] = blocks[i, j];
                        }
                    }
                }
            });

        }

    }

    //Create provinces from voronoi data
    //TODO Other specifics(might need to wait on terrain features generation like rivers, impassible cliffs)
    private List<ProvinceData> CreateProvinces()
    {
        List<ProvinceData> nProvinces = new List<ProvinceData>();

        //For each site
        for (int i = 0; i < points.Count; i++)
        {
            ProvinceData nProvince = new ProvinceData();

            //Most data
            nProvince.id = i;
            nProvince.point = points[i];
            nProvince.neighborsRAW = points[i].Neighbors;
            nProvince.neighbors = new List<ProvinceData>();
            nProvince.pos = new Vector2((float)points[i].X, (float)points[i].Y);

            //Average calculation ranges
            int horizontalAverageRange = (int)pointsHorizontalSeparation / 4;
            int verticalAverageRange = (int)pointsVerticalSeparation / 4;

            //Calculate average terrain and height for this province
            float heightTotal = 0;
            float heightN = 0;
            for (int a = -horizontalAverageRange; a < horizontalAverageRange / 4; a++)
            {
                if ((int)nProvince.pos.x + a < 0 || (int)nProvince.pos.x + a > settings.WIDTH - 1)
                    continue;
                for (int b = -verticalAverageRange; b < verticalAverageRange; b++)
                {
                    if ((int)nProvince.pos.y + b < 0 || (int)nProvince.pos.y + b > settings.HEIGHT - 1)
                        continue;
                    heightTotal += geography.HEIGHTMAP[(int)nProvince.pos.x + a, (int)nProvince.pos.y + b];
                    heightN++;
                }
            }
            nProvince.heightVal = heightTotal / heightN;

            float terrainTotal = 0;
            float terrainN = 0;
            for (int a = -horizontalAverageRange; a < horizontalAverageRange / 4; a++)
            {
                if ((int)nProvince.pos.x + a < 0 || (int)nProvince.pos.x + a > settings.WIDTH - 1)
                    continue;
                for (int b = -verticalAverageRange; b < verticalAverageRange; b++)
                {
                    if ((int)nProvince.pos.y + b < 0 || (int)nProvince.pos.y + b > settings.HEIGHT - 1)
                        continue;
                    terrainTotal += geography.TERRAINMAP[(int)nProvince.pos.x + a, (int)nProvince.pos.y + b];
                    terrainN++;
                }
            }
            nProvince.terrainVal = terrainTotal / terrainN;

            //Identify type from data and values
            //Height
            for (int j = 0; j < biome.heights.Length; j++)
            {
                if (biome.heights[j].noiseMin <= nProvince.heightVal && nProvince.heightVal < biome.heights[j].noiseMax)
                {
                    nProvince.height = data.heights[biome.heights[j].name];
                    break;
                }
            }
            //Terrain
            for (int j = 0; j < biome.terrains.Length; j++)
            {
                if (biome.terrains[j].noiseMin <= nProvince.terrainVal && nProvince.terrainVal < biome.terrains[j].noiseMax)
                {
                    //Need to check if height present is accepted by terrain, if not then use fallback instead
                    //Search
                    bool found = false;
                    for (int h = 0; h < biome.terrains[j].type.heights.Length; h++)
                    {
                        if (biome.terrains[j].type.heights[h].name == nProvince.height.name)
                        {
                            found = true;
                            break;
                        }
                    }

                    //Use fallback
                    if (!found)
                    {

                        //Default fallback
                        nProvince.terrain = data.terrains[biome.terrains[j].type.height_default_fallback];

                        //No specific fallbacks, break
                        if (data.terrains[biome.terrains[j].name].height_fallbacks == null)
                            break;

                        //Specific fallbacks
                        for (int f = 0; f < data.terrains[biome.terrains[j].name].height_fallbacks.Count; f++)
                        {
                            if (nProvince.height.name == data.terrains[biome.terrains[j].name].height_fallbacks[f].First.name)
                            {
                                nProvince.terrain = data.terrains[biome.terrains[j].name].height_fallbacks[f].Second;
                                break;
                            }
                        }

                        break;
                    }
                    //Use original
                    else
                    {
                        nProvince.terrain = data.terrains[biome.terrains[j].name];
                        break;
                    }
                }
            }

            //Vertices (Using jittered Cell)
            nProvince.vertices = new List<Geometry.Vector2X>();
            for (int j = 0; j < points[i].CellJitter.Count; j++)
            {
                nProvince.vertices.Add(points[i].CellJitter[j].start);
                nProvince.vertices.Add(points[i].CellJitter[j].end);
            }

            //Make sure vertices are unique
            nProvince.vertices = nProvince.vertices.Distinct().ToList();

            //Add corner vertex for corner provinces
            //TODO This is a bit dangerous if site relaxation is very low, expected corner sites might not actually end in corner
            //TODO Test to make sure ^
            if (i == 0)
                nProvince.vertices.Add(new Geometry.Vector2X(0, 0));
            else if (i == pointsHorizontal - 1)
                nProvince.vertices.Add(new Geometry.Vector2X(0, settings.HEIGHT));
            else if (i == points.Count - pointsHorizontal)
                nProvince.vertices.Add(new Geometry.Vector2X(settings.WIDTH, 0));
            else if (i == points.Count - 1)
                nProvince.vertices.Add(new Geometry.Vector2X(settings.WIDTH, settings.HEIGHT));

            //Geometric Center
            float x = 0, y = 0;
            for (int j = 0; j < nProvince.vertices.Count; j++)
            {
                x += nProvince.vertices[j].value.x;
                y += nProvince.vertices[j].value.y;
            }
            x /= nProvince.vertices.Count;
            y /= nProvince.vertices.Count;
            nProvince.center = new Vector2(x, y);

            //3D World province center
            nProvince.center3D = mapObject.MapToWorld(new Vector2(x, y));

            //Give center to vertices for sorting
            for (int j = 0; j < nProvince.vertices.Count; j++)
            {
                nProvince.vertices[j].findAngle(nProvince.center);
            }

            //Sort vertices by angle from center
            nProvince.vertices.Sort();

            //Add
            nProvinces.Add(nProvince);
        }

        //Neighbor Connections after all provinces created
        //TODO This is currently just the basic neighbor connections(will be struct in the future)
        for (int i = 0; i < nProvinces.Count; i++)
        {

            //Connections for graph
            Dictionary<int, int> connections = new Dictionary<int, int>();

            //For each Voronoi neighbor
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
            //Multithreaded
            Parallel.For(0, settings.WIDTH, i =>
            {
                int finalI = i * settings.WIDTH;
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    float val = geography.HEIGHTMAP[i, j];
                    pixels[finalI + j] = new Color(val, val, val);
                }
            });

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
            //Multithreaded
            Parallel.For(0, settings.WIDTH, i =>
            {
                int finalI = i * settings.WIDTH;
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    float val = geography.TERRAINMAP[i, j];
                    pixels[finalI + j] = new Color(val, val, val);
                }
            });

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

            //List of colors and usage count
            Dictionary<Color, int> colors = new Dictionary<Color, int>();
            for (int i = 0; i < provinces.Count; i++)
            {

                Color cl = provinces[i].height.uColor;

                //Count
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
            //TODO If we need to reuse this maybe we should save it
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

            //Fill province color
            //Multithreaded
            Parallel.For(0, provinces.Count, i =>
            {

                //Calculate color
                Color c = provinces[i].height.uColor;

                //Dont draw if background was same color
                if (c == maxCl)
                    return;

                //Draw polygon
                pixelMatrix = Graphics.FillPolygon(pixelMatrix, provinces[i].vertices, c);

            });

            //Draw frame
            pixelMatrix = DrawFrame(pixelMatrix);

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

            //List of colors and count usage
            Dictionary<Color, int> colors = new Dictionary<Color, int>();
            for (int i = 0; i < provinces.Count; i++)
            {

                Color cl = provinces[i].terrain.uColor;

                //Count
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
            //TODO If we need to reuse this maybe we should save it
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

            //Fill province color
            //Multithreaded
            Parallel.For(0, provinces.Count, i =>
            {
                //Calculate color
                Color c = provinces[i].terrain.uColor;

                //Dont draw if background was same color
                if (c == maxCl)
                    return;

                //Draw polygon
                pixelMatrix = Graphics.FillPolygon(pixelMatrix, provinces[i].vertices, c);
            });

            //Draw frame
            pixelMatrix = DrawFrame(pixelMatrix);

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
            //https://stackoverflow.com/a/29576746/9015010
            //TODO Settings should be in other place
            time = Time.realtimeSinceStartup;
            int differentiation = 2;
            int block = 20;
            int blocksW = settings.WIDTH / block;
            int blocksH = settings.HEIGHT / block;

            //Multithreaded
            Parallel.For(0, settings.WIDTH / (block / 2), i =>
            {
                for (int j = 0; j < settings.HEIGHT; j += block / 2)
                {

                    //Get terrain noise
                    float val = this.geography.TERRAINMAP[i * (block / 2), j];

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

                    pixelMatrix = Graphics.Decal(pixelMatrix, decal, i * (block / 2), j);
                }
            });

            Debug.Log("======== Color blend took: " + (Time.realtimeSinceStartup - time) + "s");
            time = Time.realtimeSinceStartup;

            //Add decals

            //Height decals
            //Multithreaded
            Parallel.For(0, provinces.Count, i =>
            {
                //No decals so skip
                if (provinces[i].height.decals == null)
                    return;

                //Random Number Generator for this thread
                System.Random random = new System.Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));

                //Each decal
                for (int d = 0; d < provinces[i].height.decals.Length; d++)
                {

                    //Add decals in a circular way with random angle and radius
                    for (int c = 0; c < provinces[i].height.decals[d].number; c++)
                    {

                        //Chance value defined in JSON
                        float chance = 1;
                        if (provinces[i].height.decals[d].chance != 0)
                            chance = provinces[i].height.decals[d].chance;

                        //Chance to not place this one
                        if (random.Next(0, 100) > chance * 100)
                            continue;

                        //Reach value defined in JSON
                        float reach = 1;
                        if (provinces[i].height.decals[d].reach != 0)
                            reach = provinces[i].height.decals[d].reach;

                        //Separation allowed
                        int horizontalReach = (int)(pointsHorizontalSeparation * reach / 2);
                        int verticalReach = (int)(pointsVerticalSeparation * reach / 2);

                        //Position of decal center
                        float dX = random.Next(-horizontalReach, horizontalReach);
                        float dY = random.Next(-verticalReach, verticalReach);
                        float x = provinces[i].center.x + dX;
                        float y = provinces[i].center.y + dY;

                        //Decal rotations available
                        Graphics.PixelMatrix[] decalRotations = data.decals[provinces[i].height.decals[d].name];

                        //Pick random rotation if required
                        int randomRotation = 0;

                        //Pick random rotation if required
                        if (provinces[i].height.decals[d].rotate)
                            randomRotation = random.Next(0, 4);

                        //Final decal                    
                        Graphics.PixelMatrix decal = decalRotations[randomRotation];

                        //Add decal
                        pixelMatrix = Graphics.Decal(pixelMatrix, decal, (int)x, (int)y);
                    }
                }
            });

            //Terrain decals
            //Multithreaded
            Parallel.For(0, provinces.Count, i =>
            {

                //No decals so skip
                if (provinces[i].terrain.decals == null)
                    return;

                //Random Number Generator for this thread
                System.Random random = new System.Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));

                //Each decal
                for (int d = 0; d < provinces[i].terrain.decals.Length; d++)
                {

                    //Add decals in a circular way with random angle and radius
                    for (int c = 0; c < provinces[i].terrain.decals[d].number; c++)
                    {

                        //Chance value defined in JSON
                        float chance = 1;
                        if (provinces[i].terrain.decals[d].chance != 0)
                            chance = provinces[i].terrain.decals[d].chance;

                        //Chance to not place this one
                        if (random.Next(0, 100) > chance * 100)
                            continue;

                        //Reach value defined in JSON
                        float reach = 1;
                        if (provinces[i].terrain.decals[d].reach != 0)
                            reach = provinces[i].terrain.decals[d].reach;

                        //Separation allowed
                        int horizontalReach = (int)(pointsHorizontalSeparation * reach / 2);
                        int verticalReach = (int)(pointsVerticalSeparation * reach / 2);

                        //Position of decal center
                        float dX = random.Next(-horizontalReach, horizontalReach);
                        float dY = random.Next(-verticalReach, verticalReach);
                        float x = provinces[i].center.x + dX;
                        float y = provinces[i].center.y + dY;

                        //Decal rotations available
                        Graphics.PixelMatrix[] decalRotations = data.decals[provinces[i].terrain.decals[d].name];

                        //Pick random rotation if required
                        int randomRotation = 0;

                        //Pick random rotation if required
                        if (provinces[i].terrain.decals[d].rotate)
                            randomRotation = random.Next(0, 4);

                        //Final decal                    
                        Graphics.PixelMatrix decal = decalRotations[randomRotation];

                        //Add decal
                        pixelMatrix = Graphics.Decal(pixelMatrix, decal, (int)x, (int)y);
                    }
                }

            });

            Debug.Log("======== Decals took: " + (Time.realtimeSinceStartup - time) + "s");
            time = Time.realtimeSinceStartup;

            //Add randomization to color
            //TODO Settings should be in other place
            float randomization = 30;
            float variation = 0.05f;
            //Multithreaded
            Parallel.For(0, settings.WIDTH, i =>
            {
                //Random Number Generator for this thread
                System.Random random = new System.Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));

                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    //Randomly skip
                    if (random.Next(0, 101) < 100 - randomization)
                        continue;

                    //Add some difference
                    Color color = pixelMatrix.GetPixelSafe(i, j);
                    color.r = color.r + Utilities.NextFloat(random, -variation, variation);
                    color.g = color.g + Utilities.NextFloat(random, -variation, variation);
                    color.b = color.b + Utilities.NextFloat(random, -variation, variation);
                    pixelMatrix.SetPixelSafe(i, j, color);
                }
            });

            Debug.Log("======== Randomization took: " + (Time.realtimeSinceStartup - time) + "s");
            time = Time.realtimeSinceStartup;

            //Height Shading
            //Apply the Shade map modifier
            //Multithreaded
            Parallel.For(0, settings.WIDTH, i =>
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {

                    Color cl = pixelMatrix.GetPixelSafe(i, j);
                    float modifier = geography.SHADEMAP[i, j];
                    cl.r *= modifier;
                    cl.g *= modifier;
                    cl.b *= modifier;
                    pixelMatrix.SetPixelSafe(i, j, cl);

                }
            });

            Debug.Log("======== Shading took: " + (Time.realtimeSinceStartup - time) + "s");
            time = Time.realtimeSinceStartup;

            //Province Height Border
            //Multithreaded
            Parallel.For(0, provinces.Count, i =>
            {
                for (int j = 0; j < provinces[i].neighbors.Count; j++)
                {
                    if (provinces[i].height == provinces[i].neighbors[j].height)
                        continue;
                    for (int e = 0; e < provinces[i].neighbors[j].point.CellJitter.Count; e++)
                    {
                        for (int h = 0; h < provinces[i].point.CellJitter.Count; h++)
                        {
                            //Found edge of border
                            if (provinces[i].neighbors[j].point.CellJitter[e].start == provinces[i].point.CellJitter[h].start
                            && provinces[i].neighbors[j].point.CellJitter[e].end == provinces[i].point.CellJitter[h].end)
                            {

                                //Color of highest impact
                                Color c;
                                if (provinces[i].height.impact > provinces[i].neighbors[j].height.impact)
                                    c = provinces[i].height.uColor;
                                else
                                    c = provinces[i].neighbors[j].height.uColor;

                                //Draw
                                pixelMatrix = Graphics.BresenhamLineThick(pixelMatrix,
                                (int)provinces[i].point.CellJitter[h].start.value.x,
                                (int)provinces[i].point.CellJitter[h].start.value.y,
                                (int)provinces[i].point.CellJitter[h].end.value.x,
                                (int)provinces[i].point.CellJitter[h].end.value.y,
                                c,
                                4);
                            }
                        }
                    }
                }
            });

            Debug.Log("======== Height borders took: " + (Time.realtimeSinceStartup - time) + "s");
            time = Time.realtimeSinceStartup;

            //Draw frame
            pixelMatrix = DrawFrame(pixelMatrix);

            Debug.Log("======== Frame took: " + (Time.realtimeSinceStartup - time) + "s");

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

        //Draw the frame(edges, border and sites)
        Graphics.PixelMatrix DrawFrame(Graphics.PixelMatrix pixelMatrix)
        {

            //Draw Border
            pixelMatrix = Graphics.Border(pixelMatrix, Color.black);

            //Draw edges
            //TODO Should be defined elsewhere
            int thickness = 2;
            int circleRadius = thickness / 2;
            //Multithreaded
            Parallel.For(0, provinces.Count, i =>
            {
                for (int j = 0; j < provinces[i].vertices.Count - 1; j++)
                {
                    //Round
                    int startX = Mathf.FloorToInt(provinces[i].vertices[j].value.x);
                    int startY = Mathf.FloorToInt(provinces[i].vertices[j].value.y);
                    int endX = Mathf.FloorToInt(provinces[i].vertices[j + 1].value.x);
                    int endY = Mathf.FloorToInt(provinces[i].vertices[j + 1].value.y);

                    //Draw Edge
                    pixelMatrix = Graphics.BresenhamLineThick(pixelMatrix, startX, startY, endX, endY, Color.black, circleRadius);
                }
            });

            //Add Site centers
            //TODO Should be defined elsewhere
            int siteRadius = 2;
            Graphics.PixelMatrix center = Graphics.FilledCircle(siteRadius, Color.black);
            for (int i = 0; i < provinces.Count; i++)
            {
                pixelMatrix = Graphics.Decal(pixelMatrix, center, (int)provinces[i].center.x, (int)provinces[i].center.y);
            }

            return pixelMatrix;
        }

    }

}

public class Map : MonoBehaviour
{

    //Data
    public Data data;
    public MapData mapData;

    //Prefab Unit
    public GameObject unit;

    //UI Elements
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

    //World coordinates to map
    public Vector2 WorldToMap(Vector3 pos)
    {

        //World position to object
        pos.z = transform.position.z;
        var tpos = transform.InverseTransformPoint(pos);

        //Pixel conversions xy 2D
        int pixelsPerUnit = 100;
        int xPixel = Mathf.RoundToInt(pos.x * pixelsPerUnit);
        xPixel += mapData.settings.WIDTH / 2;
        int yPixel = Mathf.RoundToInt(pos.y * pixelsPerUnit);
        yPixel += mapData.settings.HEIGHT / 2;

        //Flipped so they don't need to be flipped by whatever uses this function
        return new Vector2(yPixel, xPixel);

    }

    //Map coordinates to world
    public Vector3 MapToWorld(Vector2 pos)
    {

        //Axis aligned coordinates
        float axisAllignedX = (pos.x - mapData.settings.WIDTH / 2);
        float axisAllignedY = (pos.y - mapData.settings.HEIGHT / 2);

        //Inverted
        int pixelsPerUnit = 100;
        Vector3 provincePos = new Vector3(axisAllignedY / pixelsPerUnit / transform.localScale.x, axisAllignedX / pixelsPerUnit / transform.localScale.y, -0.002f);

        //Transform to world space
        Vector3 final = transform.TransformPoint(provincePos);

        return new Vector3(final.x, final.y, final.z);

    }

    //Mouse controls (TEMPORARY)
    public float panSpeed = 10.0f;
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
            Camera.main.transform.position += new Vector3(Input.GetAxis("Mouse X") * Time.deltaTime * -panSpeed, Input.GetAxis("Mouse Y") * Time.deltaTime * -panSpeed, 0);
        }

        //Click on map (Currently for testing unit placement)
        if (Input.GetMouseButtonDown(0))
        {
            //Check there is a map
            if (mapData != null)
            {

                //Ignore if clicked on UI
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    //Mouse position on map
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mapPos = WorldToMap(pos);

                    Debug.Log("Click (" + mapPos.x + ", " + mapPos.y + ")");

                    //Test placement
                    //TODO will be elsewhere probably (on Army Deployment Phase)

                    //Find province clicked using Geometry.PointInPolygon
                    //TODO Could optimize this to check if click is inside map bounds
                    int id = -1;
                    for (int i = 0; i < mapData.provinces.Count; i++)
                    {
                        //Check if inside
                        if (Geometry.PointInPolygon(mapData.provinces[i].vertices, new Geometry.Vector2X(mapPos.x, mapPos.y)))
                        {
                            id = i;
                            break;
                        }
                    }

                    //Check if found any province
                    if (id != -1)
                    {

                        //Check province vacant
                        if (mapData.provinces[id].unit == null)
                        {
                            //Instantiate unit
                            GameObject t = Instantiate(unit);
                            //Place it on map and setup references
                            mapData.provinces[id].unit = t.GetComponent<Unit>().PlaceOnMap(this, mapData.provinces[id]);
                        }

                    }
                }

            }
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

        //Start time
        float totalTime = Time.realtimeSinceStartup;

        //New map data with settings
        //TODO Settings probably wont be here
        mapData = new MapData(data, this);
        //TODO Pixel size could be tied to province number to keep it consistent across different make sizes
        mapData.settings = new MapSettings(2000, 2000, 450, 2.0f);

        //Get selected biome
        int idBiome = biomePick.value;
        string nameBiome = biomePick.options[idBiome].text;

        //Set biome for map
        mapData.biome = data.biomes[nameBiome];

        //Master generate
        mapData.Generate();

        //Add to mapmodes Dropdown
        List<string> mapModesStrings = new List<string>();
        foreach (KeyValuePair<string, Texture2D> entry in mapData.mapModes)
        {
            mapModesStrings.Add(entry.Key);
        }
        mapModePick.ClearOptions();
        mapModePick.AddOptions(mapModesStrings);

        //Set mapmode to whatever is selected
        OnMapModeChange();

        //Total generation time
        Debug.Log("#### Total generation time: " + (Time.realtimeSinceStartup - totalTime) + "s");

    }

    //Assign new mapmode texture from UI Dropdown
    public void OnMapModeChange()
    {

        //Get Dropdown value
        int idMapMode = mapModePick.value;
        string nameMapMode = mapModePick.options[idMapMode].text;

        //Set sprite to map
        GetComponent<Renderer>().material.mainTexture = mapData.mapModes[nameMapMode];

    }

}
