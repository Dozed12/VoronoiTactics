using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VoronoiLib;
using VoronoiLib.Structures;

public struct MapGraphics
{
    public Texture2D TERRAIN;
}

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

public class MapData
{

    private List<FortuneSite> points;
    private LinkedList<VEdge> edges;

    public MapSettings settings;
    public MapGraphics graphics;
    public List<SiteData> provinces;

    public class SiteData
    {
        public int x, y;
        public List<SiteData> neighbors;
    }

    //Generate the map data
    public void Generate()
    {

        //Generate points (TODO in seperate function)
        points = GeneratePoints();

        //Run Voronoi
        edges = FortunesAlgorithm.Run(points, 0, 0, settings.WIDTH, settings.HEIGHT);

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
        float pointsHorizontal = Mathf.Floor(Mathf.Sqrt(settings.NUMBER_SITES * settings.WIDTH / settings.HEIGHT));
        float pointsVertical = Mathf.Floor(Mathf.Sqrt(settings.HEIGHT) * Mathf.Sqrt(settings.NUMBER_SITES) / Mathf.Sqrt(settings.WIDTH));
        float pointsHorizontalSeparation = settings.WIDTH / pointsHorizontal;
        float pointsVerticalSeparation = settings.HEIGHT / pointsVertical;

        //Max allowed separation with relaxation
        float pointsHorizontalAllowedRadius = pointsHorizontalSeparation / settings.SITE_RELAXATION;
        float pointsVerticalAllowedRadius = pointsVerticalSeparation / settings.SITE_RELAXATION;

        //Place points
        for (int i = 0; i < pointsHorizontal; i++)
        {
            for (int j = 0; j < pointsVertical; j++)
            {
                //Grid placement
                double x = pointsHorizontalSeparation * (i + 0.5f);
                double y = pointsVerticalSeparation * (j + 0.5f);
                //Randomize angular offset
                float angle = Random.Range(0, Mathf.PI * 2);
                float a = Random.Range(0,pointsHorizontalAllowedRadius);
                float b = Random.Range(0,pointsVerticalAllowedRadius);
                double offX = (a * b) / Mathf.Sqrt((b * b) + (a * a) * (Mathf.Tan(angle) * Mathf.Tan(angle)));
                double offY = (a * b) / Mathf.Sqrt((a * a) + (b * b) / (Mathf.Tan(angle) * Mathf.Tan(angle)));
                //Quadrant check
                if(angle > -Mathf.PI/2 && angle < Mathf.PI/2)
                    nPoints.Add(new FortuneSite(x + offX, y + offY));
                else
                    nPoints.Add(new FortuneSite(x - offX, y - offY));
            }
        }

        Debug.Log("FortuneSites Generated");

        return nPoints;
    }

    //Create provinces from voronoi data
    //TODO neighbors and other specifics(might need to wait on terrain features generation like rivers, impassible cliffs)
    private List<SiteData> CreateProvinces()
    {
        List<SiteData> provinces = new List<SiteData>();

        //VEdge.Start is a VPoint with location VEdge.Start.X and VEdge.End.Y
        //VEdge.End is the ending point for the edge
        //FortuneSite.Neighbors contains the site's neighbors in the Delaunay Triangulation

        return provinces;
    }

    //Generate graphics
    //TODO separate functions for each graphic
    private void GenerateGraphics()
    {

        //Create terrain texture
        graphics.TERRAIN = new Texture2D(settings.WIDTH, settings.HEIGHT);

        //Blank terrain texture
        Color[] blank = new Color[settings.WIDTH * settings.HEIGHT];
        for (int i = 0; i < settings.WIDTH * settings.HEIGHT; i++)
        {
            blank[i] = Color.white;
        }
        graphics.TERRAIN.SetPixels(blank);

        //Add site center
        for (int i = 0; i < points.Count; i++)
        {
            graphics.TERRAIN.SetPixel((int)points[i].X, (int)points[i].Y, Color.black);
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
            graphics.TERRAIN = Graphics.Bresenham(graphics.TERRAIN, startX, startY, endX, endY, Color.black);
            edge = edge.Next;
        }

        //Apply to texture
        graphics.TERRAIN.Apply();

        //Settings
        graphics.TERRAIN.filterMode = FilterMode.Point;

        Debug.Log("Graphics Generated");

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
        data.settings = new MapSettings(800, 800, 200, 3);
        data.Generate();

        //Create terrain sprite
        mapTerrain = Sprite.Create(data.graphics.TERRAIN, new Rect(0, 0, data.graphics.TERRAIN.width, data.graphics.TERRAIN.height), new Vector2(0.5f, 0.5f));
        GetComponent<SpriteRenderer>().sprite = mapTerrain;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
