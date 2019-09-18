using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class DataLocalComponent : GameFrameworkComponent, ICustomComponent
    {
        public HumanDataLogic HumanLogic
        {
            get;
            private set;
        }
        public void Clear()
        {

        }

        public void Init()
        {
            HumanLogic = new HumanDataLogic();
        }
    }
}