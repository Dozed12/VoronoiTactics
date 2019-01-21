﻿using System;
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
    //TODO neighbor will be a custom struct
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
    private List<VEdge> simpleEdges;
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
            //X Grid placement  
            double x = pointsHorizontalSeparation * (i + 0.5f);
            for (int j = 0; j < pointsVertical; j++)
            {
                //Y Grid placement                
                double y = pointsVerticalSeparation * (j + 0.5f);

                //Randomize angular offset
                float angle = UnityEngine.Random.Range(0, Utilities.PI2);
                float a = UnityEngine.Random.Range(0, pointsHorizontalAllowedRadius);
                float b = UnityEngine.Random.Range(0, pointsVerticalAllowedRadius);
                float ab = a * b;
                float aa = a * a;
                float bb = b * b;
                float tanAngle = Mathf.Tan(angle);
                double offX = ab / Mathf.Sqrt((bb) + (aa) * (tanAngle * tanAngle));
                double offY = ab / Mathf.Sqrt((aa) + (bb) / (tanAngle * tanAngle));

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
        simpleEdges = new List<VEdge>();
        var edge = edges.First;
        for (int j = 0; j < edges.Count; j++)
        {
            simpleEdges.Add(edge.Value);

            //Next edge
            edge = edge.Next;
        }

        //Jittering
        //TODO Settings should be in another place
        int divisions = 3;
        double magnitude = 5;
        List<VEdge> jitteredEdges = new List<VEdge>();

        for (int j = 0; j < simpleEdges.Count; j++)
        {

            //Dont jitter map border edges
            if (simpleEdges[j].Start.X == 0 && simpleEdges[j].End.X == 0)
            {
                jitteredEdges.Add(simpleEdges[j]);
                continue;
            }
            if (simpleEdges[j].Start.Y == 0 && simpleEdges[j].End.Y == 0)
            {
                jitteredEdges.Add(simpleEdges[j]);
                continue;
            }
            if (simpleEdges[j].Start.X == settings.WIDTH - 1 && simpleEdges[j].End.X == settings.WIDTH - 1)
            {
                jitteredEdges.Add(simpleEdges[j]);
                continue;
            }
            if (simpleEdges[j].Start.Y == settings.HEIGHT - 1 && simpleEdges[j].End.Y == settings.HEIGHT - 1)
            {
                jitteredEdges.Add(simpleEdges[j]);
                continue;
            }

            //Jitter
            List<VPoint> jitter = Geometry.Jitter(simpleEdges[j].Start, simpleEdges[j].End, divisions, magnitude);

            //Add jitter edges
            for (int i = 0; i < jitter.Count - 1; i++)
            {
                VEdge nEdge = new VEdge(jitter[i], jitter[i + 1], simpleEdges[j].Left, simpleEdges[j].Right);
                jitteredEdges.Add(nEdge);
            }
        }

        //Replace edges with jittered version
        simpleEdges = jitteredEdges;

        //For each point
        for (int i = 0; i < points.Count; i++)
        {

            //Each point has to look for its edges
            for (int j = 0; j < simpleEdges.Count; j++)
            {
                //Side with same ID
                if (simpleEdges[j].Left.ID == points[i].ID || simpleEdges[j].Right.ID == points[i].ID)
                {
                    points[i].Cell.Add(simpleEdges[j]);
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
        fastnoise.SetFractalOctaves(4);
        fastnoise.SetFractalLacunarity(2.0f);
        fastnoise.SetFrequency(1);

        //Noise block size
        //TODO probably better in other place
        int terrainBlockSize = 10;
        int heightBlockSize = 10;

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

            //Get noise blocks
            for (int i = 0; i < widthNBlocks; i++)
            {
                int finalI = i * heightBlockSize;
                for (int j = 0; j < heightNBlocks; j++)
                {
                    blocks[i, j] = (fastnoise.GetNoise(finalI, j * heightBlockSize, 0) + 1) / 2;
                }
            }

            //Full noise
            geography.HEIGHTMAP = new float[settings.WIDTH, settings.HEIGHT];

            //Get full noise from blocks
            for (int i = 0; i < widthNBlocks; i++)
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
            }

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
            for (int i = 0; i < widthNBlocks; i++)
            {
                int finalI = i * terrainBlockSize;
                for (int j = 0; j < heightNBlocks; j++)
                {
                    blocks[i, j] = (fastnoise.GetNoise(finalI, j * terrainBlockSize, 0) + 1) / 2;
                }
            }

            //Full noise
            geography.TERRAINMAP = new float[settings.WIDTH, settings.HEIGHT];

            //Get full noise from blocks
            for (int i = 0; i < widthNBlocks; i++)
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

            //Average calculation ranges
            int horizontalAverageRange = (int)pointsHorizontalSeparation / 4;
            int verticalAverageRange = (int)pointsVerticalSeparation / 4;

            //Calculate average terrain and height for this province
            float heightTotal = 0;
            float heightN = 0;
            for (int a = -horizontalAverageRange; a < horizontalAverageRange / 4; a++)
            {
                if ((int)nSite.pos.X + a < 0 || (int)nSite.pos.X + a > settings.WIDTH - 1)
                    continue;
                for (int b = -verticalAverageRange; b < verticalAverageRange; b++)
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
            for (int a = -horizontalAverageRange; a < horizontalAverageRange / 4; a++)
            {
                if ((int)nSite.pos.X + a < 0 || (int)nSite.pos.X + a > settings.WIDTH - 1)
                    continue;
                for (int b = -verticalAverageRange; b < verticalAverageRange; b++)
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

            //Unique vertices
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

            //Give center to vertices for sorting
            for (int j = 0; j < nSite.vertices.Count; j++)
            {
                nSite.vertices[j].findAngle(nSite.center);
            }

            //Sort vertices using angle from center
            nSite.vertices.Sort();

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
                int finalI = i * settings.WIDTH;
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    float val = geography.HEIGHTMAP[i, j];
                    pixels[finalI + j] = new Color(val, val, val);
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
                int finalI = i * settings.WIDTH;
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    float val = geography.TERRAINMAP[i, j];
                    pixels[finalI + j] = new Color(val, val, val);
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

                //Grey value
                float grey = provinces[i].height.color / 255.0f;

                Color cl = new Color(grey, grey, grey);

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

            //Fill sites color
            for (int i = 0; i < provinces.Count; i++)
            {
                //Grey value
                float grey = provinces[i].height.color / 255.0f;

                //Calculate color
                Color c = new Color(grey, grey, grey);

                //Dont draw if background was same color
                if (c == maxCl)
                    continue;

                //Draw polygon
                pixelMatrix = Graphics.FillPolygon(pixelMatrix, provinces[i].vertices, c);
            }

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

            //Fill sites color
            for (int i = 0; i < provinces.Count; i++)
            {
                //Calculate color
                Color c = new Color(provinces[i].terrain.color[0] / 255.0f, provinces[i].terrain.color[1] / 255.0f, provinces[i].terrain.color[2] / 255.0f);

                //Dont draw if background was same color
                if (c == maxCl)
                    continue;

                //Draw polygon
                pixelMatrix = Graphics.FillPolygon(pixelMatrix, provinces[i].vertices, c);
            }

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

            //Add decals
            float halfHorizontalSeparation = pointsHorizontalSeparation / 2;
            float halfVerticalSeparation = pointsVerticalSeparation / 2;

            //Height decals
            for (int i = 0; i < provinces.Count; i++)
            {
                //No decals so skip
                if (provinces[i].height.decals == null)
                    continue;

                //Each decal
                for (int d = 0; d < provinces[i].height.decals.Length; d++)
                {

                    //Decal reach
                    float decalHorizontalReach = halfHorizontalSeparation * provinces[i].height.decals[d].reach;
                    float decalVerticalReach = halfVerticalSeparation * provinces[i].height.decals[d].reach;
                    float reach = Mathf.Max(decalHorizontalReach, decalVerticalReach);

                    //Add decals in a circular way with random angle and radius
                    for (int c = 0; c < provinces[i].height.decals[d].number; c++)
                    {

                        //Chance value defined in JSON
                        float chance = 1;
                        if (provinces[i].height.decals[d].chance != 0)
                            chance = provinces[i].height.decals[d].chance;

                        //Chance to not place this one
                        if (UnityEngine.Random.Range(0, 100) > chance * 100)
                            continue;

                        //Position of decal center
                        float cos = UnityEngine.Random.Range(-1.0f, 1.0f);
                        float sin = UnityEngine.Random.Range(-1.0f, 1.0f);
                        float radius = UnityEngine.Random.Range(0, reach);
                        float x = (float)provinces[i].center.X + cos * radius;
                        float y = (float)provinces[i].center.Y + sin * radius;

                        //Decal rotations available
                        Graphics.PixelMatrix[] decalRotations = data.decals[provinces[i].height.decals[d].name];

                        //Final decal                    
                        Graphics.PixelMatrix decal = data.decals[provinces[i].height.decals[d].name][0];

                        //Pick a random rotation if required
                        if (provinces[i].height.decals[d].rotate)
                            decal = data.decals[provinces[i].height.decals[d].name][UnityEngine.Random.Range(0, 4)];

                        //Add decal
                        pixelMatrix = Graphics.Decal(pixelMatrix, decal, (int)x, (int)y);
                    }
                }
            }

            //Terrain decals
            for (int i = 0; i < provinces.Count; i++)
            {
                //No decals so skip
                if (provinces[i].terrain.decals == null)
                    continue;

                //Each decal
                for (int d = 0; d < provinces[i].terrain.decals.Length; d++)
                {

                    //Decal reach
                    float decalHorizontalReach = halfHorizontalSeparation * provinces[i].terrain.decals[d].reach;
                    float decalVerticalReach = halfVerticalSeparation * provinces[i].terrain.decals[d].reach;
                    float reach = Mathf.Max(decalHorizontalReach, decalVerticalReach);

                    //Add decals in a circular way with random angle and radius
                    for (int c = 0; c < provinces[i].terrain.decals[d].number; c++)
                    {

                        //Chance value defined in JSON
                        float chance = 1;
                        if (provinces[i].terrain.decals[d].chance != 0)
                            chance = provinces[i].terrain.decals[d].chance;

                        //Chance to not place this one
                        if (UnityEngine.Random.Range(0, 100) > chance * 100)
                            continue;

                        //Position of decal center
                        float cos = UnityEngine.Random.Range(-1.0f, 1.0f);
                        float sin = UnityEngine.Random.Range(-1.0f, 1.0f);
                        float radius = UnityEngine.Random.Range(0, reach);
                        float x = (float)provinces[i].center.X + cos * radius;
                        float y = (float)provinces[i].center.Y + sin * radius;

                        //Decal rotations available
                        Graphics.PixelMatrix[] decalRotations = data.decals[provinces[i].terrain.decals[d].name];

                        //Final decal                    
                        Graphics.PixelMatrix decal = data.decals[provinces[i].terrain.decals[d].name][0];

                        //Pick a random rotation if required
                        if (provinces[i].terrain.decals[d].rotate)
                            decal = data.decals[provinces[i].terrain.decals[d].name][UnityEngine.Random.Range(0, 4)];

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
            for (int i = 0; i < provinces.Count; i++)
            {
                for (int j = 0; j < provinces[i].vertices.Count - 1; j++)
                {
                    //Round
                    int startX = Mathf.FloorToInt((float)provinces[i].vertices[j].X);
                    int startY = Mathf.FloorToInt((float)provinces[i].vertices[j].Y);
                    int endX = Mathf.FloorToInt((float)provinces[i].vertices[j + 1].X);
                    int endY = Mathf.FloorToInt((float)provinces[i].vertices[j + 1].Y);

                    //Draw Edge
                    pixelMatrix = Graphics.BresenhamLineThick(pixelMatrix, startX, startY, endX, endY, Color.black, circleRadius);
                }
            }

            //Add Site centers
            //TODO Should be defined elsewhere
            int siteRadius = 2;
            Graphics.PixelMatrix center = Graphics.FilledCircle(siteRadius, Color.black);
            for (int i = 0; i < provinces.Count; i++)
            {
                pixelMatrix = Graphics.Decal(pixelMatrix, center, (int)provinces[i].center.X, (int)provinces[i].center.Y);
            }

            return pixelMatrix;
        }

    }

}

public class Map : MonoBehaviour
{

    Data data;
    MapData mapData;

    public GameObject unit;

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

        //Click on pixel
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = transform.position.z;
            var tpos = transform.InverseTransformPoint(pos);

            if (mapData != null)
            {
                int xPixel = Mathf.RoundToInt(tpos.x * 100);
                xPixel += mapData.settings.WIDTH / 2;
                int yPixel = Mathf.RoundToInt(tpos.y * 100);
                yPixel += mapData.settings.HEIGHT / 2;

                Debug.Log("Click (" + xPixel + ", " + yPixel + ")");

                //Test placement
                GameObject t = Instantiate(unit, new Vector3(pos.x, pos.y, -0.002f), Quaternion.identity) as GameObject;
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

        //Total generation time
        Debug.Log("#### Total generation time: " + (Time.realtimeSinceStartup - totalTime) + "s");

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
