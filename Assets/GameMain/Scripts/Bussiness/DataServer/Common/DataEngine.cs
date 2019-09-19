using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IUV.SDN
{

    public interface DataEngine
    {
        int CreateTable<T>();

        int DropTable<T>();

        int Delete<T>(T t);

        int Add<T>(T t);

        int BatchAdd<T>(List<T> datas);

        int BatchRemove<T>(List<T> datas);

        int BatchUpdate<T>(List<T> datas);
        List<T> Find<T>(Expression<Func<T, bool>> predicate, int i) where T : new();

        int Update<T>(T t);
    }
}