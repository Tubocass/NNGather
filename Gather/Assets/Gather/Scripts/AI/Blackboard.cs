using System.Collections.Generic;

namespace Gather.AI
{
    [System.Serializable]
    public class Blackboard
    {
        private Dictionary<string, object> data = new Dictionary<string, object>();

        public void SetValue<T>(string key, T value)
        {
            if (data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }
        }

        public T GetValue<T>(string key)
        {
            object value;
            if (data.TryGetValue(key, out value) && value is T)
            {
                return (T)value;
            }
            return default(T);
        }

        public bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }

        public void Clear()
        {
            data.Clear();
        }
    }
}