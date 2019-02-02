using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{

}

public class Unit : MonoBehaviour
{

    //Temporary Test Sprite size
    int width = 50;
    int height = 50;

    //Unit data
    UnitData data;

    //Province where unit is
    ProvinceData province;

    //Sprite used(Currently a test sprite)
    Sprite sprite;

    //Map where unit is placed
    Map map;

    // Start is called before the first frame update
    void Start()
    {
        //Test Sprite
        Graphics.PixelMatrix pixels = new Graphics.PixelMatrix(width, height, Color.white);
        Texture2D tex = new Texture2D(width, height);
        tex.SetPixels(pixels.pixels);
        sprite = Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
        this.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    //Place on Map
    public Unit PlaceOnMap(Map map, ProvinceData province)
    {

        //Assign
        this.province = province;
        this.map = map;

        //TODO These calculations can be done by each province on generation since map wont me moved in the 3D space
        //Saves some light calculations so not big deal but more readable

        //World coordinates from province
        transform.position = map.MapToWorld(new Vector2((float)province.center.X, (float)province.center.Y));

        return this;

    }

}
