using System;
using Core;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class MainMenuBehaviour : MonoBehaviour
{
    public void Init()
    {
        //房间号输入框绑定事件
        GameObject roomNumberInput = GameObject.Find("RoomNumberInput");
        roomNumberInput.GetComponent<TMP_InputField>().onValueChanged.AddListener(OnRoomNumberInputChanged);
        //开始按键绑定事件
        GameObject startButton = GameObject.Find("StartButton");
        startButton.GetComponent<Button>().onClick.AddListener(OnStartButtonClick);
    }

    public void OnRoomNumberInputChanged(string value)
    {
        LocalDataManager.Instance.Set("roomNumber", value);
    }

    public void OnStartButtonClick()
    {
        //发起进入房间请求
        NinjaEnterMessage enterMessage = new NinjaEnterMessage(LocalDataManager.Instance.Get<string>("roomNumber"));
        NetWorkManager.Instance.Send(enterMessage);
        //阻塞接受消息
        NinjaRoomMessage roomMessage = NetWorkManager.Instance.Receive<NinjaRoomMessage>();
        if (roomMessage.IfP1 == 2)
        {
            LogUtils.Log("the room is full or maybe you've entered");
        }
        else
        {
            LocalDataManager.Instance.Set("ifP1", roomMessage.IfP1);
            
            GameApp.Instance.LoadBattleScene();
        }
    }
}
