using UnityEngine;
using UnityEngine.UIElements;

namespace Utils
{
    public class InstantiateUtils
    {
        public static GameObject InstantiateBySameName(GameObject original)
        {
            GameObject o = Object.Instantiate(original);
            o.name = original.name;
            return o;
        }
        
        public static GameObject InstantiateBySameName(GameObject original, Vector3 position, Quaternion rotation)
        {
            GameObject o = Object.Instantiate(original, position, rotation);
            o.name = original.name;
            return o;
        }
    }
}