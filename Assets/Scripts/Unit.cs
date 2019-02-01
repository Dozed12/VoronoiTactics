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

        //TODO These calculations can be done by each province on generation since map wont me moved in the 3D space
        //Saves some light calculations so not big deal but more readable

        //Assign
        this.province = province;
        this.map = map;

        //Province position        
        float axisAllignedX = (float)(province.center.X - map.mapData.settings.WIDTH / 2);
        float axisAllignedY = (float)(province.center.Y - map.mapData.settings.HEIGHT / 2);

        //Inverted
        int pixelsPerUnit = 100;
        Vector3 provincePos = new Vector3(axisAllignedY / pixelsPerUnit, axisAllignedX / pixelsPerUnit, -0.002f);

        //Transform to world space
        transform.position = map.transform.TransformPoint(provincePos);

        return this;

    }

}
