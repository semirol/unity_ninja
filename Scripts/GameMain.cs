
using Managers;

public class GameMain : UnitySingleton<GameMain>
{
    void Start()
    {
        //启动管理模块
        gameObject.AddComponent<ResourceManager>();
        ResourceManager.Instance.Init();
        gameObject.AddComponent<EventManager>();
        gameObject.AddComponent<LocalDataManager>();
        gameObject.AddComponent<NetWorkManager>();
        NetWorkManager.Instance.Init();
        gameObject.AddComponent<UIManager>();
        UIManager.Instance.Init();
        gameObject.AddComponent<MapManager>();
        gameObject.AddComponent<AudioManager>();
        //初始化游戏逻辑模块
        gameObject.AddComponent<GameApp>();
        //游戏逻辑入口
        GameApp.Instance.GameStart();
        //播放声音
        AudioManager.Instance.PlayBgm("MainMenu.wav");
    }
}
