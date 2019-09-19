using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    /// <summary>
    /// 人类数据服务
    /// </summary>
    public class BaseDataServer<T> : IDataServer<T> where T : BaseData, new()
    {
        public DataEngine DB
        {
            get;
            set;
        }

        public int Delete(T t)
        {
            return DB.Delete<T>(t);
        }

        public int Add(T t)
        {
            return DB.Add<T>(t);
        }

        public int BatchAdd(List<T> datas)
        {
            return DB.BatchAdd<T>(datas);
        }

        public int BatchRemove(List<T> datas)
        {
            return DB.BatchRemove<T>(datas);
        }

        public int BatchUpdate(List<T> datas)
        {
            return DB.BatchUpdate<T>(datas);
        }

        public List<T> Find(Expression<Func<T, bool>> predicate, int i)
        {
            return DB.Find<T>(predicate, i);
        }

        public int Update(T t)
        {
            return DB.Update<T>(t);
        }
    }
}