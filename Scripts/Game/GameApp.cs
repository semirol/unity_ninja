
using System;
using Core;
using Managers;
using UnityEngine;
using Utils;

public class GameApp : UnitySingleton<GameApp>
{
    public void GameStart()
    {
        // todo 模拟登陆获取playerId
        LocalDataManager.Instance.Set("playerId", new System.Random().Next());
        
        InitMainCamera();
        LoadMainScene();
    }

    public void LoadMainScene()
    {
        UIManager.Instance.CleanBattleGUI();
        UIManager.Instance.LoadMainMenu();
        MapManager.Instance.Clean();
        // todo 场景切换资源管理到底应该怎么做，直接清空所有对象？
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("Enemy"));
    }

    public void LoadBattleScene()
    {
        //加载技能prefab到缓存
        ResourceManager.Instance.GetCachedAssetFromShortPath<GameObject>("Effects/Skills/EnergyWave/EnergyWave.prefab");
        ResourceManager.Instance.GetCachedAssetFromShortPath<GameObject>("Effects/Skills/Shield/Shield.prefab");
        ResourceManager.Instance.GetCachedAssetFromShortPath<GameObject>("Effects/Skills/Dart/Dart.prefab");
        ResourceManager.Instance.GetCachedAssetFromShortPath<GameObject>("Effects/Skills/Blink/Blink.prefab");
        ResourceManager.Instance.GetCachedAssetFromShortPath<GameObject>("Effects/Skills/Blink/AfterBlink.prefab");
        //加载战斗UI界面（摇杆、按键等）
        UIManager.Instance.CleanMainMenu();
        UIManager.Instance.LoadBattleGUI();
        //初始化输入控制（这个必须放在LoadBattleGUI后面，每次战斗开始都要重新初始化） // todo 使用装饰器在每个方法前自动init
        InputManager.Instance.Init();
        //初始化地图
        MapManager.Instance.LoadMap("Meta");
        //初始化玩家和对手
        InitPlayerAndEnemy();
        //发送ReadyMessage
        NinjaReadyMessage message = new NinjaReadyMessage();
        NetWorkManager.Instance.Send(message);
        //开始战斗，运行第一帧逻辑帧
        BattleManager.Instance.StartBattle();
    }

    public void InitMainCamera()
    {
        GameObject mainCamera = ResourceManager.Instance.InstantiateFromShortPath("MainCamera.prefab");
        mainCamera.AddComponent<MainCameraBehaviour>();
    }

    public void InitPlayerAndEnemy()
    {
        int ifP1 = LocalDataManager.Instance.Get<int>("ifP1");
        GameObject playerPrefab = ResourceManager.Instance.GetAssetFromShortPath<GameObject>("Characters/Player/Player.prefab");
        Transform playerTransform = GameObject.Find("Game").transform.Find(ifP1 == 1 ? "P1Position" : "P2Position").transform;
        GameObject player = InstantiateUtils.InstantiateBySameName(playerPrefab, playerTransform.position, playerTransform.rotation);
        PlayerBehaviour.Player = player.AddComponent<PlayerBehaviour>();
        // GameObject enemyPrefab = ResourceManager.Instance.GetAssetFromShortPath<GameObject>("Characters/Player/Enemy.prefab");
        Transform enemyTransform = GameObject.Find("Game").transform.Find(ifP1 == 1 ? "P2Position" : "P1Position").transform;
        GameObject enemy = Instantiate(playerPrefab, enemyTransform.position, enemyTransform.rotation);
        enemy.name = "Enemy";
        PlayerBehaviour.Enemy = enemy.AddComponent<PlayerBehaviour>();
    }

}
