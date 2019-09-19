using System;
using UnityEngine;

namespace IUV.SDN
{
    [Serializable]
    public class BaseData
    {
        public BaseData()
        {

        }

        [SerializeField]
        private int m_Id = 0;

        /// <summary>
        /// 实体编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                m_Id = value;
            }
        }

        [SerializeField]
        private int m_Name = 0;

        public int Name
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
    }
}