using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class CacheServer<T> where T : MyEntityData
    {
        private Dictionary<int, T> m_Data = new Dictionary<int, T>();

        public bool Add(T data)
        {
            m_Data.Add(data.Id, data);
            return true;
        }

        public T Get(int id)
        {
            T ret;
            m_Data.TryGetValue(id, out ret);
            return ret;
        }

        public bool Remove(int id)
        {
            T ret;
            if (m_Data.TryGetValue(id, out ret))
            {
                m_Data.Remove(id);
            }
            return true;
        }

        public bool Update(T t)
        {
            T ret;
            if (m_Data.TryGetValue(t.Id, out ret))
            {
                m_Data[t.Id] = t;
            }
            return true;
        }
        public void Clear()
        {
            m_Data.Clear();
        }
    }
}