using System.Collections.Generic;

namespace Managers
{
    public class EventManager : UnitySingleton<EventManager>
    {
        public delegate void EventHandler(string eventName, object udata);

        private Dictionary<string, EventHandler> _dic = new Dictionary<string, EventHandler>();

        public void AddListener(string eventName, EventHandler handler)
        {
            if (_dic.ContainsKey(eventName))
            {
                _dic[eventName] += handler;
            }
            else
            {
                _dic.Add(eventName, handler);
            }
        }

        public void RemoveListener(string eventName, EventHandler handler)
        {
            if (!_dic.ContainsKey(eventName))
            {
                return;
            }

            _dic[eventName] -= handler;

            if (_dic[eventName] == null)
            {
                _dic.Remove(eventName);
            }
        }

        public void Emit(string eventName, object udata)
        {
            if (!_dic.ContainsKey(eventName))
            {
                return;
            }

            _dic[eventName](eventName, udata);
        }
    }
}