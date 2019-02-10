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
    public void OnRandomBattle(){
        MainUI.SetActive(false);
        BattleSetupUI.SetActive(true);
    }

    //========================================

    //Battle Setup UI Group TODO do missing
    public void OnBattle(){
        BattleSetupUI.SetActive(false);
        BattleUI.SetActive(true);
    }

}