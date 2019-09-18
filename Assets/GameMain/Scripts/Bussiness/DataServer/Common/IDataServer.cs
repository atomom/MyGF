using System.Collections.Generic;

namespace IUV.SDN
{
    public interface IDataServer<T>
    {
        bool Add(T t);
        T Find(int id);
        T Update(T t);

        bool Delete(int id);

        void BatchAdd(List<T> datas);

        void BatchUpdate(List<T> datas);

        void BatchRemove(List<int> datas);

        List<T> Find(string sql);
    }
}