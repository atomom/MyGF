using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IUV.SDN
{
    public interface IDataServer<T> where T : BaseData
    {

        int Delete(T t);

        int Add(T t);

        int BatchAdd(List<T> datas);

        int BatchRemove(List<T> datas);

        int BatchUpdate(List<T> datas);
        List<T> Find(Expression<Func<T, bool>> predicate, int i);

        int Update(T t);
    }
}