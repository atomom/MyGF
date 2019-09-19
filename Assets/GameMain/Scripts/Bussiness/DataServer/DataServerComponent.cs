using System;
using SQLite4Unity3d;
using UnityGameFramework.Runtime;
using static IUV.SDN.Constant;

namespace IUV.SDN
{
    /// <summary>
    /// 数据服务管理汇总
    /// </summary>
    public class DataServerComponent : GameFrameworkComponent, ICustomComponent
    {
        public DataEngine engine;
        public Action action;
        public HumanDataServer HumanServer
        {
            get;
            private set;
        }

        public ArchivesDataServer ArchivesServer
        {
            get;
            private set;
        }
        public void Init()
        {
            engine = new SqliteEngine("");
            ArchivesServer = new ArchivesDataServer();
            ArchivesServer.DB = engine;
            HumanServer = new HumanDataServer();
            HumanServer.DB = engine;
        }

        public void CreateHumans()
        {

        }

        public void Clear() { }

    }
}