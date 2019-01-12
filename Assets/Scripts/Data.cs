using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
    http://www.boxheadproductions.com.au/deserializing-top-level-arrays-in-json-with-unity/
    This is used to load array of JSON objects straight into C#
    Note that files must start with "Items" like the example below
    {"Items":[{"val";"1"},{"val":"2"}]}
*/

/* 
    Terrain Model

    - User picks TerrainBiome upon map generation
    - Biome affects possible weathers
    - Weather can be picked at start but can change
    - Weather is wind, rain and temperature
    - Each Biome has several terrain types possible with different densities
    - Use Noise to distribute terrains
    - Assign height values as a secondary feature
    - Terrains can accept only some heights and have a fallback terrain in base height doesnt match
    - Overlap with special terrain types (Village, Outskirts, City) (probably using noise too)

    Biomes:
    https://www.researchgate.net/profile/Rudi_Van_Aarde/publication/274288653/figure/fig1/AS:458704500858882@1486375083230/World-map-of-coverage-of-14-terrestrial-biomes-The-14-terrestrial-biomes-adapted-from.png

*/

//Terrain heights
[System.Serializable]
public struct TerrainHeight
{
    //Name of Height
    public string name;
    //Simplified color (greyscale)
    public int color;
    //Attack modifier
    public float attack;
    //Defense modifier
    public float defense;
    //Movement modifier
    public float movement;
}

//Human terrain structures
[System.Serializable]
public struct TerrainStructure
{
    //Name of Type
    public string name;
    //Simplified color
    public int[] color;
    //Attack modifier
    public float attack;
    //Defense modifier
    public float defense;
    //Movement modifier
    public float movement;
}

//Terrain types
[System.Serializable]
public struct TerrainType
{
    //Name of Type
    public string name;
    //Simplified color
    public int[] color;
    //Decals to use
    public string[] decals;
    //Heights allowed (names)
    public string[] height_names;
    //Heights allowed (references)
    public TerrainHeight[] heights;
    //Height fallback, when height present isnt allowed then fallback to type with this name
    public string height_default_fallback;
    //Specific fallbacks, after using default fallback these will be used (just raw Json parse, come in pairs)
    public string[] height_pair_fallbacks;
    //Actual processed pairs of fallbacks
    public List<Pair<TerrainHeight, TerrainType>> height_fallbacks;
    //Attack modifier
    public float attack;
    //Defense modifier
    public float defense;
    //Movement modifier
    public float movement;
}

//Biomes to choose
[System.Serializable]
public struct Biome
{
    [System.Serializable]
    public struct HeightSetting
    {
        public string name;
        public TerrainHeight type;
        public float noiseMax;
        public float noiseMin;
    }

    [System.Serializable]
    public struct TypeSetting
    {
        public string name;
        public TerrainType type;
        public float noiseMax;
        public float noiseMin;
    }

    [System.Serializable]
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
    public List<Pair<TerrainType, float>> terrainNoiseMiddles;
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

    public Dictionary<string, Graphics.PixelMatrix> decals;

    //Load Height.json
    private void LoadHeight()
    {

        //Read data
        string filePath = Application.streamingAssetsPath + "/Data/Height.json";
        TerrainHeight[] e;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            e = JsonHelper.FromJson<TerrainHeight>(dataAsJson);
        }
        else
        {
            Debug.Log("Height.json not found");
            return;
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

        //Read data
        string filePath = Application.streamingAssetsPath + "/Data/Terrain.json";
        TerrainType[] e;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            e = JsonHelper.FromJson<TerrainType>(dataAsJson);
        }
        else
        {
            Debug.Log("Terrain.json not found");
            return;
        }

        //Process heights (still need to process fallbacks)
        for (int i = 0; i < e.Length; i++)
        {
            //Get heights
            e[i].heights = new TerrainHeight[e[i].height_names.Length];
            for (int j = 0; j < e[i].height_names.Length; j++)
            {
                e[i].heights[j] = heights[e[i].height_names[j]];
            }
        }

        //Process specific fallbacks and add to dictionary after
        for (int i = 0; i < e.Length; i++)
        {
            //Process fallbacks
            if (e[i].height_pair_fallbacks != null)
            {
                e[i].height_fallbacks = new List<Pair<TerrainHeight, TerrainType>>();
                for (int h = 0; h < e[i].height_pair_fallbacks.Length; h += 2)
                {
                    e[i].height_fallbacks.Add(new Pair<TerrainHeight, TerrainType>(heights[e[i].height_pair_fallbacks[h]], terrains[e[i].height_pair_fallbacks[h + 1]]));
                }
            }

            //Add to dictionary
            terrains.Add(e[i].name, e[i]);
        }

        Debug.Log("Terrain types loaded (Terrain.json)");

    }

    //Load Structures.json
    private void LoadStructures()
    {

        //Read data
        string filePath = Application.streamingAssetsPath + "/Data/Structure.json";
        TerrainStructure[] e;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            e = JsonHelper.FromJson<TerrainStructure>(dataAsJson);
        }
        else
        {
            Debug.Log("Structure.json not found");
            return;
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

        //Read data
        string filePath = Application.streamingAssetsPath + "/Data/Biome.json";
        Biome[] e;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            e = JsonHelper.FromJson<Biome>(dataAsJson);
        }
        else
        {
            Debug.Log("Biome.json not found");
            return;
        }

        //Add to dictionary
        for (int i = 0; i < e.Length; i++)
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

            //Calculate centers of terrain power based on noise(used for painting gradient on map)
            item.terrainNoiseMiddles = new List<Pair<TerrainType, float>>();
            foreach (Biome.TypeSetting terrain in item.terrains)
            {
                //If minimum is 0 then the most powerful is at 0
                if (terrain.noiseMin == 0)
                {
                    item.terrainNoiseMiddles.Add(new Pair<TerrainType, float>(terrain.type, terrain.noiseMin));
                    continue;
                }
                //If maximum is 1 then the most powerful is at 1
                if (terrain.noiseMax == 1)
                {
                    item.terrainNoiseMiddles.Add(new Pair<TerrainType, float>(terrain.type, terrain.noiseMax));
                    continue;
                }
                //Most powerful at center of minimum and maximum
                item.terrainNoiseMiddles.Add(new Pair<TerrainType, float>(terrain.type, terrain.noiseMin + (terrain.noiseMax - terrain.noiseMin) / 2));
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

        //Load decals
        Texture2D[] decalsRaw = Resources.LoadAll<Texture2D>("StreamingAssets/Decals");

        //Load decals into a dictionary with PixelMatrix
        decals = new Dictionary<string, Graphics.PixelMatrix>();
        for (int i = 0; i < decalsRaw.Length; i++)
        {
            decals.Add(decalsRaw[i].name, new Graphics.PixelMatrix(decalsRaw[i]));
        }

        //Load Terrain JSON files
        LoadHeight();
        LoadTerrain();
        LoadStructures();
        LoadBiomes();
    }
}