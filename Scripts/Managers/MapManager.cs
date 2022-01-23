using UnityEngine;

namespace Managers
{
    public class MapManager : UnitySingleton<MapManager>
    {
        public void LoadMap(string mapName)
        {
            ResourceManager.Instance.InstantiateFromShortPath("Maps/"+mapName+"/Map.prefab");
            ResourceManager.Instance.InstantiateFromShortPath("Maps/"+mapName+"/Game.prefab");
        }

        public void Clean()
        {
            Destroy(GameObject.Find("Map"));
            Destroy(GameObject.Find("Game"));
        }
    }
}