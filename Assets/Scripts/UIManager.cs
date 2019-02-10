using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //Major UIs
    public GameObject battleSetupUI;
    public GameObject battleUI;
    public GameObject mainUI;
    
    //========================================

    //Main UI Group TODO do missing
    public void OnRandomBattle(){
        mainUI.SetActive(false);
        battleSetupUI.SetActive(true);
    }

}