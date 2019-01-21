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
}
