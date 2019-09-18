//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    /// <summary>
    /// 可作为目标的实体类。
    /// </summary>
    public abstract class TargetableObjectView : EntityViewLogic
    {
        [SerializeField]
        private TargetableObjectData m_TargetableObjectData = null;

        public bool IsDead
        {
            get
            {
                return m_TargetableObjectData.HP <= 0;
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_TargetableObjectData = userData as TargetableObjectData;
            if (m_TargetableObjectData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
        }

        protected virtual void OnDead(EntityViewLogic attacker)
        {
            GameEntry.Entity.HideEntity(this);
        }

    }
}