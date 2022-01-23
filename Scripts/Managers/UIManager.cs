using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Managers
{
    public class UIManager : UnitySingleton<UIManager>
    {
        private GameObject _logBarPrefab;
        private GameObject _currentCanvas;

        public void Init()
        {
            _logBarPrefab = ResourceManager.Instance.GetAssetFromShortPath<GameObject>("GUI/LogBar/LogBar.prefab");
        }
        
        public void LoadMainMenu()
        {
            GameObject mainMenu = ResourceManager.Instance.InstantiateFromShortPath("GUI/MainMenu/MainMenu.prefab");
            mainMenu.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            mainMenu.GetComponent<Canvas>().worldCamera = Camera.main;
            _currentCanvas = mainMenu;
            MainMenuBehaviour mainMenuBehaviour = mainMenu.AddComponent<MainMenuBehaviour>();
            mainMenuBehaviour.Init();
        }

        public void LoadBattleGUI()
        {
            GameObject battleGUI = ResourceManager.Instance.InstantiateFromShortPath("GUI/Battle/BattleGUI.prefab");
            battleGUI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            battleGUI.GetComponent<Canvas>().worldCamera = Camera.main;
            _currentCanvas = battleGUI;
            BattleGUIBehaviour battleGUIBehaviour = battleGUI.AddComponent<BattleGUIBehaviour>();
            battleGUIBehaviour.Init();
        }
        
        public void CleanMainMenu()
        {
            Destroy(GameObject.Find("MainMenu"));
        }

        public void CleanBattleGUI()
        {
            Destroy(GameObject.Find("BattleGUI"));
        }

        //通过UI显示的方式打印可见用户信息
        public void Log(string msg)
        {
            GameObject logBar = Instantiate(_logBarPrefab, _currentCanvas.transform, false);
            TMP_Text text = logBar.GetComponentInChildren<TMP_Text>();
            text.SetText(msg);
            Destroy(logBar, 2);
        }
    }
}