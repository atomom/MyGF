//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace IUV.SDN
{
    [Serializable]
    public abstract class AccessoryObjectData : MyEntityData
    {
        [SerializeField]
        private int m_OwnerId = 0;
        private TargetableObjectData m_OwnerData;

        public AccessoryObjectData(int ownerId) : base()
        {
            m_OwnerId = ownerId;
        }

        /// <summary>
        /// 拥有者编号。
        /// </summary>
        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }

        /// <summary>
        /// 拥有者阵营。
        /// </summary>
        public CampType OwnerCamp
        {
            get
            {
                return m_OwnerData.Camp;
            }
        }
    }
}