using Managers;
using UnityEngine;

namespace Utils
{
    public class LogUtils
    {
        public static void Log(object message, Object context = null)
        {
            try
            {
                UIManager.Instance.Log((string) message);
            }
            catch
            {
                // ignored
            }

            if (context == null)
            {
                Debug.Log(message);
            }
            else
            {
                Debug.Log(message, context);
            }
        }
    }
}