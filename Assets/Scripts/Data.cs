using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//https://github.com/JamesNK/Newtonsoft.Json
using Newtonsoft.Json;

/* 
    Terrain Model

    - User picks TerrainBiome upon map generation
    - Biome affects possible weathers
    - Weather can be picked at start but can change
    - Weather is wind, rain and temperature
    - Each Biome has several terrain types possible with different densities
    - Use Noise to distribute terrains
    - Assign height values as a secondary feature
    - Overlap with special terrain types (Village, Outskirts, City) (probably using noise too)

    Biomes:
    https://www.researchgate.net/profile/Rudi_Van_Aarde/publication/274288653/figure/fig1/AS:458704500858882@1486375083230/World-map-of-coverage-of-14-terrestrial-biomes-The-14-terrestrial-biomes-adapted-from.png

*/

//Terrain heights
public struct TerrainHeight
{
    public string name;
    public int color;
    public float attack;
    public float defense;
    public float movement;
}

//Human terrain structures
public struct TerrainStructure
{
    public string name;
    public IList<int> color;
    public float attack;
    public float defense;
    public float movement;
}

//Terrain types
public struct TerrainType
{
    public string name;
    public IList<int> color;
    public string[] height_names;
    public TerrainHeight[] heights;
    public float attack;
    public float defense;
    public float movement;
}

//Biomes to choose
public struct Biome
{
    public struct HeightSetting
    {
        public string name;
        public TerrainHeight type;
        public float noiseMax;
        public float noiseMin;
    }

    public struct TypeSetting
    {
        public string name;
        public TerrainType type;
        public float noiseMax;
        public float noiseMin;
    }

    public struct StructureSetting
    {
        public string name;
        public TerrainStructure type;
        public float noiseMax;
        public float noiseMin;
    }

    public string name;
    public float heightFreq;
    public HeightSetting[] heights;
    public float terrainFreq;
    public TypeSetting[] terrains;
    public float structureFreq;
    public StructureSetting[] structures;
    public float attack;
    public float defense;
    public float movement;
}

public class Data
{

    public Dictionary<string, TerrainHeight> heights;
    public Dictionary<string, TerrainType> terrains;
    public Dictionary<string, TerrainStructure> structures;
    public Dictionary<string, Biome> biomes;

    //Load Height.json
    private void LoadHeight()
    {

        //Deserialize
        JsonTextReader reader = new JsonTextReader(new StreamReader("Assets/Data/Height.json"));
        List<TerrainHeight> e = new List<TerrainHeight>();
        while (reader.Read())
        {
            JsonSerializer serializer = new JsonSerializer();
            e = serializer.Deserialize<List<TerrainHeight>>(reader);
        }

        //Add to dictionary
        foreach (var item in e)
        {
            heights.Add(item.name, item);
        }

        Debug.Log("Height types loaded (Height.json)");

    }

    //Load Terrain.json
    private void LoadTerrain()
    {

        //Deserialize
        JsonTextReader reader = new JsonTextReader(new StreamReader("Assets/Data/Terrain.json"));
        List<TerrainType> e = new List<TerrainType>();
        while (reader.Read())
        {
            JsonSerializer serializer = new JsonSerializer();
            e = serializer.Deserialize<List<TerrainType>>(reader);
        }

        //Add to dictionary
        for (int i = 0; i < e.Count; i++)
        {
            TerrainType item = e[i];

            //Get heights
            item.heights = new TerrainHeight[item.height_names.Length];
            for (int j = 0; j < item.height_names.Length; j++)
            {
                item.heights[j] = heights[item.height_names[j]];
            }

            //Add
            terrains.Add(item.name, item);
        }

        Debug.Log("Terrain types loaded (Terrain.json)");

    }

    //Load Structures.json
    private void LoadStructures()
    {

        //Deserialize
        JsonTextReader reader = new JsonTextReader(new StreamReader("Assets/Data/Structure.json"));
        List<TerrainStructure> e = new List<TerrainStructure>();
        while (reader.Read())
        {
            JsonSerializer serializer = new JsonSerializer();
            e = serializer.Deserialize<List<TerrainStructure>>(reader);
        }

        //Add to dictionary
        foreach (var item in e)
        {
            structures.Add(item.name, item);
        }

        Debug.Log("Structure types loaded (Structure.json)");

    }

    //Load Biome.json
    private void LoadBiomes()
    {

        //Deserialize
        JsonTextReader reader = new JsonTextReader(new StreamReader("Assets/Data/Biome.json"));
        List<Biome> e = new List<Biome>();
        while (reader.Read())
        {
            JsonSerializer serializer = new JsonSerializer();
            e = serializer.Deserialize<List<Biome>>(reader);
        }

        //Add to dictionary
        for (int i = 0; i < e.Count; i++)
        {
            Biome item = e[i];

            //Get heights
            for (int j = 0; j < item.heights.Length; j++)
            {
                item.heights[j].type = heights[item.heights[j].name];
            }

            //Get terrains
            for (int j = 0; j < item.terrains.Length; j++)
            {
                item.terrains[j].type = terrains[item.terrains[j].name];
            }

            //Get structures
            for (int j = 0; j < item.structures.Length; j++)
            {
                item.structures[j].type = structures[item.structures[j].name];
            }

            //Add
            biomes.Add(item.name, item);
        }

        Debug.Log("Biomes loaded (Biome.json)");

    }

    public void LoadData()
    {

        //Reset Dictionaries
        heights = new Dictionary<string, TerrainHeight>();
        structures = new Dictionary<string, TerrainStructure>();
        terrains = new Dictionary<string, TerrainType>();
        biomes = new Dictionary<string, Biome>();

        //Load
        LoadHeight();
        LoadTerrain();
        LoadStructures();
        LoadBiomes();
    }
}