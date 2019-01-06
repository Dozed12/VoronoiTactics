using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://github.com/Zalgo2462/VoronoiLib
using VoronoiLib;
using VoronoiLib.Structures;

public struct MapSettings
{
    //Pixel width of map
    public int WIDTH;
    //Pixel height of map
    public int HEIGHT;
    //Number of estimated sites
    public int NUMBER_SITES;
    //Disperses sites more
    public int SITE_RELAXATION;

    public MapSettings(int width, int height, int number_sites, int site_relaxation)
    {
        this.WIDTH = width;
        this.HEIGHT = height;
        this.NUMBER_SITES = number_sites;
        this.SITE_RELAXATION = site_relaxation;
    }
}

/* 
    TODO Terrain Model

    - User picks TerrainBiome upon map generation
    - Each Biome has several terrain types possible
    - Use Noise to distribute terrains
    - Assign height values as a secondary feature
    - HIGH_MOUNTAINS completly replaces the terrain as their own terrain type
    - Overlap with special terrain types (Village, Outskirts, City) (probably using noise too)

    Biomes:
    https://www.researchgate.net/profile/Rudi_Van_Aarde/publication/274288653/figure/fig1/AS:458704500858882@1486375083230/World-map-of-coverage-of-14-terrestrial-biomes-The-14-terrestrial-biomes-adapted-from.png

*/

//Biomes to choose, on MapData
public enum TerrainBiome
{
    TUNDRA,
    TAIGA,
    WOODLAND,
    SAVANNA,
    DESERT,
    TEMPERATE_FOREST,
    TROPICAL_FOREST,
    TEMPERATE_GRASSLAND,
    TROPICAL_GRASSLAND,
    FLOODED_GRASSLAND,
    MONTANE_GRASSLAND
}

//Height types, on ProvinceData
public enum TerrainHeight
{
    FLAT,
    HILLS,
    LOW_MOUNTAINS,
    HIGH_MOUNTAINS
}

//TODO Terrains possible for biomes, on ProvinceData
//Can be overlapped with special terrain types
public enum TerrainType
{
    
}

public class ProvinceData
{
    public int id;
    public Vector2 pos;
    public List<FortuneSite> neighborsRAW;
    public List<ProvinceData> neighbors;
    public UnitData unit = null;
    public TerrainHeight height;
    public TerrainType type;
}

public struct MapGraphics
{
    public Texture2D HEIGHTMAP;
    public Texture2D FINAL;
}

public struct MapGeography
{
    public float[,] HEIGHTMAP;
}

public class MapData
{

    private FastNoise fastnoise = new FastNoise();
    private List<FortuneSite> points;
    private LinkedList<VEdge> edges;
    private Graph graph = new Graph();

    public MapSettings settings;
    public MapGraphics graphics;
    public MapGeography geography;
    public TerrainBiome biome;
    public List<ProvinceData> provinces;

    //Generate the map data
    public void Generate()
    {

        //Generate points (TODO in seperate function)
        points = GeneratePoints();

        //Run Voronoi
        edges = FortunesAlgorithm.Run(points, 0, 0, settings.WIDTH, settings.HEIGHT);

        //Generate Geography
        GenerateGeography();

        //Generate all graphics
        GenerateGraphics();

        //TODO Create Provinces (with neighbors)
        provinces = CreateProvinces();

        Debug.Log("Map Generation Complete");

    }

    //Generates all the geography of the map
    private void GenerateGeography()
    {

        //Set seed
        fastnoise.SetSeed((int)Time.time);

        //Set Settings
        //TODO Should be set elsewhere
        fastnoise.SetFractalOctaves(8);
        fastnoise.SetFractalLacunarity(2.0f);
        fastnoise.SetFrequency(0.002f);

        HeightMap();

        //Generate Heightmap
        void HeightMap()
        {
            //Allocate
            geography.HEIGHTMAP = new float[settings.WIDTH, settings.HEIGHT];

            //Get noise
            for (int i = 0; i < settings.WIDTH; i++)
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    geography.HEIGHTMAP[i, j] = (fastnoise.GetSimplexFractal(i, j, 0) + 1) / 2;
                }
            }
        }

    }

    //Generate graphics
    private void GenerateGraphics()
    {

        HeightMapTexture();
        FinalTexture();

        Debug.Log("Graphics Generated");

        //Generate Heightmap texture (greyscale)
        void HeightMapTexture()
        {
            //Create texture
            graphics.HEIGHTMAP = new Texture2D(settings.WIDTH, settings.HEIGHT);

            //Height Greyscale
            for (int i = 0; i < settings.WIDTH; i++)
            {
                for (int j = 0; j < settings.HEIGHT; j++)
                {
                    float val = geography.HEIGHTMAP[i, j];
                    graphics.HEIGHTMAP.SetPixel(i, j, new Color(val, val, val));
                }
            }

            //Apply to texture
            graphics.HEIGHTMAP.Apply();

            //Settings
            graphics.HEIGHTMAP.filterMode = FilterMode.Point;

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
            graphics.FINAL = new Texture2D(settings.WIDTH, settings.HEIGHT);

            //Blank texture
            Color[] blank = new Color[settings.WIDTH * settings.HEIGHT];
            for (int i = 0; i < settings.WIDTH * settings.HEIGHT; i++)
            {
                blank[i] = Color.white;
            }
            graphics.FINAL.SetPixels(blank);

            //Draw Border
            graphics.FINAL = Graphics.Border(graphics.FINAL, Color.black);

            //Add Site centers
            for (int i = 0; i < points.Count; i++)
            {
                graphics.FINAL.SetPixel((int)points[i].X, (int)points[i].Y, Color.black);
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
                graphics.FINAL = Graphics.Bresenham(graphics.FINAL, startX, startY, endX, endY, Color.black);
                edge = edge.Next;
            }

            //Apply to texture
            graphics.FINAL.Apply();

            //Settings
            graphics.FINAL.filterMode = FilterMode.Point;
        }

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
        float pointsHorizontal = Mathf.Floor(Mathf.Sqrt(settings.NUMBER_SITES * settings.WIDTH / settings.HEIGHT));
        float pointsVertical = Mathf.Floor(Mathf.Sqrt(settings.HEIGHT) * Mathf.Sqrt(settings.NUMBER_SITES) / Mathf.Sqrt(settings.WIDTH));
        float pointsHorizontalSeparation = settings.WIDTH / pointsHorizontal;
        float pointsVerticalSeparation = settings.HEIGHT / pointsVertical;

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
                float angle = Random.Range(0, Mathf.PI * 2);
                float a = Random.Range(0, pointsHorizontalAllowedRadius);
                float b = Random.Range(0, pointsVerticalAllowedRadius);
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

    //Create provinces from voronoi data
    //TODO Other specifics(might need to wait on terrain features generation like rivers, impassible cliffs)
    private List<ProvinceData> CreateProvinces()
    {
        List<ProvinceData> nProvinces = new List<ProvinceData>();

        //Create Site
        for (int i = 0; i < points.Count; i++)
        {
            ProvinceData nSite = new ProvinceData();
            nSite.id = i;
            nSite.neighborsRAW = points[i].Neighbors;
            nSite.neighbors = new List<ProvinceData>();
            nSite.pos = new Vector2((float)points[i].X, (float)points[i].Y);
            //TODO Setup Terrain characteristics here (Height, Biome) using Geography data

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

}

public class Map : MonoBehaviour
{

    MapData data;

    public Sprite mapTerrain;

    // Start is called before the first frame update
    void Start()
    {

        //Pass settings and generate data
        data = new MapData();
        data.settings = new MapSettings(800, 800, 200, 2);
        data.Generate();

        //Create terrain sprite
        mapTerrain = Sprite.Create(data.graphics.HEIGHTMAP, new Rect(0, 0, data.graphics.HEIGHTMAP.width, data.graphics.HEIGHTMAP.height), new Vector2(0.5f, 0.5f));
        GetComponent<SpriteRenderer>().sprite = mapTerrain;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
