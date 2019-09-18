using UnityEngine;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class MyHuman : TargetableObjectView
    {
        [SerializeField]
        private HumanData m_MyHumanData = null;

        private Vector3 m_TargetPosition = Vector3.zero;

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

            m_MyHumanData = userData as HumanData;
            if (m_MyHumanData == null)
            {
                Log.Error("My Human data is invalid.");
                return;
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }
    }
}