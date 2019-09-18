using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class HumanDataLogic
    {
        public HumanData MyHumanData
        {
            get;
            set;
        }

        public List<HumanData> CreateInArea()
        {
            List<HumanData> ret = new List<HumanData>();
            GameEntry.DataServer.HumanServer.BatchAdd(ret);
            return ret;
        }

        public HumanData CreateMyRole(HumanData data)
        {
            GameEntry.DataServer.HumanServer.Add(data);
            MyHumanData = data;
            return MyHumanData;
        }
    }
}