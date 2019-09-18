using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class HumanDataServer : IDataServer<HumanData>
    {
        DBServer<HumanData> m_DB;
        public HumanDataServer()
        {
            m_DB = new DBServer<HumanData>();
        }

        public bool Add(HumanData t)
        {
            m_DB.Insert(t);
            GameEntry.Cache.HumanCache.Add(t);
            return true;
        }
        public HumanData Find(int id)
        {
            var ret = GameEntry.Cache.HumanCache.Get(id);
            if (ret == null)
            {
                ret = m_DB.Find(id);
                GameEntry.Cache.HumanCache.Add(ret);
            }
            return ret;
        }
        public HumanData Update(HumanData t)
        {
            m_DB.Update(t);
            GameEntry.Cache.HumanCache.Update(t);
            return t;
        }

        public bool Delete(int id)
        {
            m_DB.Delete(id);
            GameEntry.Cache.HumanCache.Remove(id);
            return true;
        }

        public void BatchAdd(List<HumanData> datas)
        {
            m_DB.BatchAdd(datas);
            foreach (var item in datas)
            {
                GameEntry.Cache.HumanCache.Add(item);
            }
        }

        public void BatchUpdate(List<HumanData> datas)
        {
            m_DB.BatchUpdate(datas);
            foreach (var item in datas)
            {
                GameEntry.Cache.HumanCache.Update(item);
            }
        }

        public void BatchRemove(List<int> datas)
        {
            m_DB.BatchDelete(datas);
            foreach (var item in datas)
            {
                GameEntry.Cache.HumanCache.Remove(item);
            }
        }

        List<HumanData> m_TmpList;
        public List<HumanData> Find(string sql)
        {
            m_TmpList.Clear();
            List<int> ids = m_DB.BatchFind(sql);
            foreach (var item in ids)
            {
                var ret = GameEntry.Cache.HumanCache.Get(item);
                m_TmpList.Add(ret);
            }
            return m_TmpList;
        }
    }
}