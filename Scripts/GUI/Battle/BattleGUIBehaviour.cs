using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class BattleGUIBehaviour : MonoBehaviour
{
    public void Init()
    {
        //投降事件
        GameObject surrenderButton = GameObject.Find("SurrenderButton");
        surrenderButton.GetComponent<Button>().onClick.AddListener(OnSurrenderButtonClick);
    }

    private void OnSurrenderButtonClick()
    {
        BattleManager.Instance.HandleBattleEnd("Player");
    }
}
