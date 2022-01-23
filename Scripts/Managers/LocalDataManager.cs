using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LocalDataManager : UnitySingleton<LocalDataManager>
    {
        private static Dictionary<string, object> _dic = new Dictionary<string, object>();
        
        public void Set(string key, object value)
        {
            _dic[key] = value;
        }

        public T Get<T>(string key)
        {
            return (T)_dic[key];
        }

        public T GetOrDefault<T>(string key, T value)
        {
            return (T)_dic.GetValueOrDefault(key, value);
        }
    }
}