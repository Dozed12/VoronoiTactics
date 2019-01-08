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
    public string ID;
    public string name;
    public int[] color;
    public float attack;
    public float defense;
    public float movement;
}

//Terrain types
public struct TerrainType
{
    public string ID;
    public string name;
    public int[] color;
    public string[] height_names;
    public TerrainHeight[] heights;
    public float attack;
    public float defense;
    public float movement;
}

//Biomes to choose
public struct Biome
{
    public string ID;
    public string name;
    public float heightFreq;
    public string[] height_names;
    public TerrainHeight[] heights;
    public float terrainFreq;
    public string[] terrain_names;
    public TerrainType[] terrains;
    public float structureFreq;
    public string[] structure_names;
    public TerrainStructure[] structures;
    public float attack;
    public float defense;
    public float movement;
}

public class Data
{

    public Dictionary<string, TerrainHeight> heights;
    public Dictionary<string, TerrainStructure> structures;
    public Dictionary<string, TerrainType> terrains;
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

        //TODO
        //Load Terrain
        //Load Structures
        //Load Biomes
    }
}