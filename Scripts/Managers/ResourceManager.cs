using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utils;

namespace Managers
{
    public class ResourceManager : UnitySingleton<ResourceManager>
    {
        private Dictionary<string, Object> _cache;
        public void Init()
        {
            Addressables.InitializeAsync();
            _cache = new Dictionary<string, Object>();
        }

        public T GetCachedAssetFromPath<T>(string path) where T : Object
        {
            if (_cache.ContainsKey(path))
            {
                return _cache[path] as T;
            }
            T result = GetAssetFromPath<T>(path);
            _cache[path] = result;
            return result;
        }

        public T GetCachedAssetFromShortPath<T>(string path) where T : Object
        {
            return GetCachedAssetFromPath<T>(FromShortPath(path));
        }
        
        public T GetAssetFromPath<T>(string path) where T : Object
        {
            AsyncOperationHandle<T> asyncOperationHandle = Addressables.LoadAssetAsync<T>(path);
            return asyncOperationHandle.WaitForCompletion();
        }
        
        public T GetAssetFromShortPath<T>(string path) where T : UnityEngine.Object
        {
            return GetAssetFromPath<T>(FromShortPath(path));
        }
        

        public GameObject InstantiateFromPath(string path)
        {
            GameObject prefab = GetAssetFromPath<GameObject>(path);
            GameObject o = InstantiateUtils.InstantiateBySameName(prefab);
            return o;
        }

        public GameObject InstantiateFromShortPath(string path)
        {
            return InstantiateFromPath(FromShortPath(path));
        }

        public static string FromShortPath(string path)
        {
            return "Assets/AssetsPackage/" + path;
        }
    }
}