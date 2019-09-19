using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SQLite4Unity3d;

namespace IUV.SDN
{
    public class SqliteEngine : DataEngine
    {
        DataService m_Sqlite;
        public SqliteEngine(string url)
        {
            m_Sqlite = new DataService(url);
            Connection = m_Sqlite.Connection;
        }
        SQLiteConnection Connection;

        public int CreateTable<T>()
        {
            var ret = Connection.CreateTable<T>();
            return ret;
        }

        public int DropTable<T>()
        {
            var ret = Connection.DropTable<T>();
            return ret;
        }
        public int Delete<T>(T t)
        {
            var i = Connection.Delete(t);
            return i;
        }

        public int Add<T>(T t)
        {
            var i = Connection.Insert(t);
            return i;
        }

        public int BatchAdd<T>(List<T> datas)
        {
            var ret = Connection.InsertAll(datas);
            return ret;
        }

        public int BatchRemove<T>(List<T> datas)
        {
            var ret = Connection.CreateCommand("");
            var t = ret.ExecuteNonQuery();
            return t;
        }

        public int BatchUpdate<T>(List<T> datas)
        {
            var ret = Connection.UpdateAll(datas);
            return ret;
        }
        public List<T> Find<T>(Expression<Func<T, bool>> predicate, int i) where T : new()
        {
            var ret = Connection.Table<T>().Where(predicate).Take(i).ToList();
            return ret;
        }

        public int Update<T>(T t)
        {
            var ret = Connection.Update(t);
            return ret;
        }
    }
}