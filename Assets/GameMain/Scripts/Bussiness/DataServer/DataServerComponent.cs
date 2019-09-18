using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class DataServerComponent : GameFrameworkComponent, ICustomComponent
    {
        public HumanDataServer HumanServer
        {
            get;
            private set;
        }
        public void Init()
        {
            HumanServer = new HumanDataServer();
        }

        public void CreateHumans()
        {

        }

        public void Clear()
        {

        }

    }
}