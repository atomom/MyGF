using System.Collections.Generic;

namespace IUV.SDN
{
    public class DBServer<T> where T : BaseData
    {
        public void Insert(T t)
        {
            t.Id = GameEntry.Entity.GenerateSerialId();
        }

        public T Find(int id)
        {
            T ret = null;
            return ret;
        }

        public void Update(T t)
        {

        }

        public void Delete(int id)
        {

        }

        public void BatchAdd(List<T> datas)
        {

        }

        public void BatchUpdate(List<T> datas)
        {

        }

        public void BatchDelete(List<int> ids)
        {

        }

        List<int> temp = new List<int>();
        public List<int> BatchFind(string sql)
        {
            return temp;
        }
    }
}