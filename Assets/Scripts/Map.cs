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
    public int WIDTH;
    public int HEIGHT;
    public int NUMBER_SITES;
    public float MIN_DISTANCE;

    public MapSettings(int width, int height, int number_sites, float min_distance)
    {
        this.WIDTH = width;
        this.HEIGHT = height;
        this.NUMBER_SITES = number_sites;
        this.MIN_DISTANCE = min_distance;
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

    //Generate a list of FortuneSites
    private List<FortuneSite> GeneratePoints()
    {
        List<FortuneSite> points = new List<FortuneSite>();

        for (int i = 0; i < settings.NUMBER_SITES; i++)
        {

            //Setup
            float x, y;
            bool tooClose = false;

            //Try point
            do
            {
                //Generate coordinates
                x = Random.Range(0.0f, settings.WIDTH - 1);
                y = Random.Range(0.0f, settings.HEIGHT - 1);
                //Check mimimum distance
                for (int j = 0; j < points.Count; j++)
                {
                    if (Vector2.Distance(new Vector2((float)points[j].X, (float)points[j].Y), new Vector2(x, y)) < settings.MIN_DISTANCE)
                    {
                        tooClose = true;
                        break;
                    }
                }
            } while (tooClose);

            //Add point
            points.Add(new FortuneSite(x, y));

        }

        Debug.Log("FortuneSites Generated");

        return points;
    }

    //Create provinces from voronoi data
    //TODO
    private List<SiteData> CreateProvinces()
    {
        List<SiteData> provinces = new List<SiteData>();

        //VEdge.Start is a VPoint with location VEdge.Start.X and VEdge.End.Y
        //VEdge.End is the ending point for the edge
        //FortuneSite.Neighbors contains the site's neighbors in the Delaunay Triangulation

        return provinces;
    }

    //Generate graphics
    //TODO seperate functions for each graphic
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
        var edge = edges.First;
        for (int i = 0; i < edges.Count; i++)
        {
            VEdge edgeVal = edge.Value;
            graphics.TERRAIN = Graphics.Bresenham(graphics.TERRAIN, (int)edgeVal.Start.X, (int)edgeVal.Start.Y, (int)edgeVal.End.X, (int)edgeVal.End.Y, Color.black);   
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
        data.settings = new MapSettings(800, 800, 200, 1);
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
