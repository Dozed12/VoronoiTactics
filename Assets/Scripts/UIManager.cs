using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //Major UIs
    public GameObject BattleSetupUI;
    public GameObject BattleUI;
    public GameObject MainUI;

    //========================================

    //Main UI Group TODO do missing
    public void OnRandomBattle()
    {
        MainUI.SetActive(false);
        BattleSetupUI.SetActive(true);
    }

    //========================================

    public GameObject MapPreview;

    //Battle Setup UI Group TODO do missing
    public void OnGenerate()
    {
        //Generate
        Map.Generate();

        //Change preview image
        Sprite sprite = Sprite.Create(Map.mapData.mapModes["Map"], new Rect(0, 0, Map.mapData.settings.WIDTH, Map.mapData.settings.HEIGHT), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect);
        MapPreview.GetComponent<Image>().sprite = sprite;
    }

    //Battle Setup UI Group TODO do missing
    public void OnBattle()
    {
        BattleSetupUI.SetActive(false);
        BattleUI.SetActive(true);
    }

    //========================================

    public Map Map;

}