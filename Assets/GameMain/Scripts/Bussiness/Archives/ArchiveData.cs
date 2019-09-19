using UnityEngine;

namespace IUV.SDN
{
    public class ArchiveData : BaseData
    {
        [SerializeField]
        private int m_Time = 0;

        /// <summary>
        /// 实体编号。
        /// </summary>
        public int Time
        {
            get
            {
                return m_Time;
            }
            set
            {
                m_Time = value;
            }
        }
    }
}