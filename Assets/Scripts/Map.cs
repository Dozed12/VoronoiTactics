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
    public TerrainHeight height;
    public TerrainType type;
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

    public MapSettings settings;
    public Dictionary<string,Texture2D> mapModes;
    public MapGeography geography;
    public Biome biome;
    public List<ProvinceData> provinces;

    //Generate the map data
    public void Generate()
    {

        //Generate points
        points = GeneratePoints();

        //Run Voronoi
        edges = FortunesAlgorithm.Run(points, 0, 0, settings.WIDTH, settings.HEIGHT);

        //Setups Cell for each point (missing in library)
        SetupCells();

        //Generate Geography
        GenerateGeography();

        //TODO Create Provinces (with neighbors)
        provinces = CreateProvinces();

        //Generate all graphics
        GenerateGraphics();

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
                double offX = (a * b) / Mathf.Sqrt((b * b) + (a * a) * (Mathf.Tan(angle) * Mathf.Tan(angle)));
                double offY = (a * b) / Mathf.Sqrt((a * a) + (b * b) / (Mathf.Tan(angle) * Mathf.Tan(angle)));
                //Quadrant check
                if (angle > -Mathf.PI / 2 && angle < Mathf.PI / 2)
                    nPoints.Add(new FortuneSite(x + offX, y + offY, id));
                else
                    nPoints.Add(new FortuneSite(x - offX, y - offY, id));
                id++;
            }
        }

        Debug.Log("FortuneSites Generated");

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
        fastnoise.SetFractalOctaves(8);
        fastnoise.SetFractalLacunarity(2.0f);
        fastnoise.SetFrequency(1);

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

            //Allocate
            geography.HEIGHTMAP = new float[settings.WIDTH, settings.HEIGHT];

            //Get noise
            for (int i = 0; i < settings.WIDTH; i++)
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    geography.HEIGHTMAP[i, j] = (fastnoise.GetNoise(i, j, 0) + 1) / 2;
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

            //Allocate
            geography.TERRAINMAP = new float[settings.WIDTH, settings.HEIGHT];

            //Get noise
            for (int i = 0; i < settings.WIDTH; i++)
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    geography.TERRAINMAP[i, j] = (fastnoise.GetNoise(i, j, 0) + 1) / 2;
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

            //TODO Setup Terrain characteristics here (Height, Terrain, Structure) using Geography data and an average(maybe using pointsHorizontalSeparation from GeneratePoints) or based on center

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

        //Neighbor Connections
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

        //VEdge.Start is a VPoint with location VEdge.Start.X and VEdge.End.Y
        //VEdge.End is the ending point for the edge
        //FortuneSite.Neighbors contains the site's neighbors in the Delaunay Triangulation

        return nProvinces;
    }

    //Generate graphics
    private void GenerateGraphics()
    {

        mapModes = new Dictionary<string, Texture2D>();

        HeightMapTexture();
        TerrainMapTexture();
        FinalTexture();
        //Simplified terrain map

        Debug.Log("Graphics Generated");

        //Generate Heightmap texture (greyscale)
        void HeightMapTexture()
        {
            //Create texture
            Texture2D texture = new Texture2D(settings.WIDTH, settings.HEIGHT);

            //Height Greyscale
            for (int i = 0; i < settings.WIDTH; i++)
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    float val = geography.HEIGHTMAP[i, j];
                    texture.SetPixel(i, j, new Color(val, val, val));
                }
            }

            //Apply to texture
            texture.Apply();

            //Settings
            texture.filterMode = FilterMode.Point;

            //Add to list
            mapModes.Add("Height map",texture);

        }

        //Generate Terrain texture
        void TerrainMapTexture()
        {
            //Create texture
            Texture2D texture = new Texture2D(settings.WIDTH, settings.HEIGHT);

            //Terrain Greyscale
            for (int i = 0; i < settings.WIDTH; i++)
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    float val = geography.TERRAINMAP[i, j];
                    texture.SetPixel(i, j, new Color(val, val, val));
                }
            }

            //Apply to texture
            texture.Apply();

            //Settings
            texture.filterMode = FilterMode.Point;

            //Add to list
            mapModes.Add("Terrain map",texture);

        }

        /*
            Generate Final Texture
            - Terrain texture
            - Voronoi frame
            - Terrain decals
         */
        void FinalTexture()
        {
            //Create texture
            Texture2D texture = new Texture2D(settings.WIDTH, settings.HEIGHT);

            //Blank texture
            Color[] blank = new Color[settings.WIDTH * settings.HEIGHT];
            for (int i = 0; i < settings.WIDTH * settings.HEIGHT; i++)
            {
                blank[i] = Color.white;
            }
            texture.SetPixels(blank);

            //Draw Border
            texture = Graphics.Border(texture, Color.black);

            //Add Site centers
            for (int i = 0; i < provinces.Count; i++)
            {
                texture.SetPixel((int)provinces[i].center.X, (int)provinces[i].center.Y, Color.black);
            }

            //Draw edges
            //TODO Jitter edges for more detail(could be done here or in internal data, the jitter wont affect any calculations so can be just graphical)
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
                texture = Graphics.Bresenham(texture, startX, startY, endX, endY, Color.black);
                edge = edge.Next;
            }

            //Apply to texture
            texture.Apply();

            //Settings
            texture.filterMode = FilterMode.Point;
            
            //Add to list
            mapModes.Add("Terrain",texture);
        }

    }

}

public class Map : MonoBehaviour
{

    Data data;
    MapData mapData;

    public Dictionary<string,Sprite> mapModes;

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

    //Generate new map
    public void Generate()
    {

        //New map data with settings
        //TODO Settings probably wont be here
        mapData = new MapData();
        mapData.settings = new MapSettings(800, 800, 200, 2.0f);

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

    public void OnMapModeChange(){

        //Get Dropdown value
        int idMapMode = mapModePick.value;
        string nameMapMode = mapModePick.options[idMapMode].text;

        //Set sprite to map
        GetComponent<SpriteRenderer>().sprite = mapModes[nameMapMode];

    }

}
