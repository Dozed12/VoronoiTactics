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
    public int[] color;
    //Height Impact (Actual value used when comparing province height)
    public int heightImpact;
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

    [System.Serializable]
    public struct Decal
    {
        //File
        public string name;
        //Reach of decal in cell seperations (1 = max(cell width, cell height))
        public float reach;
        //Number to spawn
        public int number;
        //Randomize rotation
        public bool rotate;
        //Chance of each decal appearing (default is 1)
        public float chance;
    }

    //Name of Type
    public string name;
    //Simplified color
    public int[] color;
    //Decals to use
    public Decal[] decals;
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
    public Dictionary<TerrainType, float> terrainNoiseMiddles;
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

    public Dictionary<string, Graphics.PixelMatrix[]> decals;

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

        //Add
        for (int i = 0; i < e.Length; i++)
        {
            //Add to dictionary
            heights.Add(e[i].name, e[i]);
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

        //Add
        for (int i = 0; i < e.Length; i++)
        {
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
            item.terrainNoiseMiddles = new Dictionary<TerrainType, float>();
            foreach (Biome.TypeSetting terrain in item.terrains)
            {
                //If minimum is 0 then the most powerful is at 0
                if (terrain.noiseMin == 0)
                {
                    item.terrainNoiseMiddles.Add(terrain.type, terrain.noiseMin);
                    continue;
                }
                //If maximum is 1 then the most powerful is at 1
                if (terrain.noiseMax == 1)
                {
                    item.terrainNoiseMiddles.Add(terrain.type, terrain.noiseMax);
                    continue;
                }
                //Most powerful at center of minimum and maximum
                item.terrainNoiseMiddles.Add(terrain.type, terrain.noiseMin + (terrain.noiseMax - terrain.noiseMin) / 2);
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

        //TODO Do this in seperate function(probably similar versions will be needed)
        //Load decals
        string[] filePaths = Directory.GetFiles(@Application.streamingAssetsPath + "/Decals", "*.png", SearchOption.TopDirectoryOnly);
        Texture2D[] decalsRaw = new Texture2D[filePaths.Length];
        for (int i = 0; i < filePaths.Length; i++)
        {
            //Get file
            WWW www = new WWW("file://" + filePaths[i]);
            decalsRaw[i] = www.texture;
            //Index of last '\'
            int l = filePaths[i].LastIndexOf('\\');
            //Get name of file (with extension)
            decalsRaw[i].name = filePaths[i].Substring(l + 1);
        }

        //Load decals into a dictionary with PixelMatrix
        decals = new Dictionary<string, Graphics.PixelMatrix[]>();
        for (int i = 0; i < decalsRaw.Length; i++)
        {
            //Rotation variants
            //TODO only process rotated versions if required in JSON
            //(very small optimization, also maybe not ideal if the same decal is used in a rotated and not rotated context)
            Graphics.PixelMatrix[] decalsArray = new Graphics.PixelMatrix[4];
            decalsArray[0] = new Graphics.PixelMatrix(decalsRaw[i]);
            decalsArray[1] = Graphics.RotateSq(decalsArray[0], 1);
            decalsArray[2] = Graphics.RotateSq(decalsArray[0], 2);
            decalsArray[3] = Graphics.RotateSq(decalsArray[0], 3);
            decals.Add(decalsRaw[i].name, decalsArray);
        }

        //Load Terrain JSON files
        LoadHeight();
        LoadTerrain();
        LoadStructures();
        LoadBiomes();
    }
}