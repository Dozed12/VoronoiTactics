using System.Collections;
using System.Collections.Generic;

/* 
    TODO Terrain Model

    - User picks TerrainBiome upon map generation
    - Biome affects possible weathers
    - Weather can be picked at start but can change
    - Weather is wind, rain and temperature
    - Each Biome has several terrain types possible with different densities
    - Use Noise to distribute terrains
    - Assign height values as a secondary feature
    - HIGH_MOUNTAINS completly replaces the terrain as their own terrain type
    - Overlap with special terrain types (Village, Outskirts, City) (probably using noise too)

    Biomes:
    https://www.researchgate.net/profile/Rudi_Van_Aarde/publication/274288653/figure/fig1/AS:458704500858882@1486375083230/World-map-of-coverage-of-14-terrestrial-biomes-The-14-terrestrial-biomes-adapted-from.png

*/

//Biomes to choose, on MapData
public enum Biome
{
    TUNDRA,
    TAIGA,
    WOODLAND,
    DESERT,
    TEMPERATE_FOREST,
    TROPICAL_FOREST,
    TEMPERATE_GRASSLAND,
    TROPICAL_GRASSLAND,
    FLOODED_GRASSLAND
}

//Height types, on ProvinceData
public enum TerrainHeight
{
    PLAIN,
    HILLS,
    LOW_MOUNTAINS,
    HIGH_MOUNTAINS
}

//TODO Terrains possible for biomes, on ProvinceData
//Can be overlapped with special terrain types
public enum TerrainType
{
    DESERT,
    SEMIDESERT,
    BARREN,
    WOODS,
    FOREST,
    DENSE_FOREST,
    JUNGLE,
    GRASSLAND,
    SHRUBLAND,
    SWAMP,
    STEPPE,
    SAVANNA,
}

//Special terrain that overlaps other
public enum TerrainSpecial{
    FARMLAND,
    VILLAGE,
    OUTSKIRTS,
    CITY
}

public class Data
{
    
}