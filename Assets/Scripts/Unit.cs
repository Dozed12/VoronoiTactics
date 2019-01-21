using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{

}

public class Unit : MonoBehaviour
{

    int width = 50;
    int height = 50;
    UnitData data;
    ProvinceData province;
    Sprite sprite;
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

    // Update is called once per frame
    void Update()
    {

    }

    //Place on Map
    public Unit PlaceOnMap(Map map, ProvinceData province)
    {

        //Assign
        this.province = province;
        this.map = map;

        //Province position
        int pixelsPerUnit = 100;
        int axisAllignedX = (int)(province.center.X - map.mapData.settings.WIDTH / 2);
        int axisAllignedY = (int)(province.center.Y - map.mapData.settings.HEIGHT / 2);
        Vector3 provincePos = new Vector3(axisAllignedX / pixelsPerUnit, axisAllignedY / pixelsPerUnit, -0.002f);

        //Transform to world space
        transform.position = map.transform.TransformPoint(provincePos);

        return this;

    }

}
