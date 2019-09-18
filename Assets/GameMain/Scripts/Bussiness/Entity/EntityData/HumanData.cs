using UnityEngine;

namespace IUV.SDN
{
    public class HumanData : TargetableObjectData
    {
        [SerializeField]
        private int m_MaxHP = 0;
        public HumanData(CampType camp) : base(camp) { }
        public override int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }

        public int Age
        {
            get;
            set;
        }

        public int m_MaxAge = 0;
        public int MaxAge
        {
            get
            {
                return m_MaxAge;
            }
        }
    }
}