using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class CacheServerComponent : GameFrameworkComponent, ICustomComponent
    {
        public CacheServer<HumanData> HumanCache
        {
            get;
            private set;
        }

        public void Init()
        {
            HumanCache = new CacheServer<HumanData>();
        }
        public void Clear()
        {
            HumanCache = null;
        }
    }
}