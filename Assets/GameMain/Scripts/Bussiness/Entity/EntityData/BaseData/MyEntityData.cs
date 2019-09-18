using UnityEngine;

namespace IUV.SDN
{
    public abstract class MyEntityData : EntityData
    {

        [SerializeField]
        private string m_Name = null;
        /// <summary>
        /// 角色名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
        public MyEntityData() : base()
        {

        }
    }
}