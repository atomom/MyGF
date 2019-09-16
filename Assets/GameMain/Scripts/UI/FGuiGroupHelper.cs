using FairyGUI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    /// <summary>
    /// uGUI 界面组辅助器。
    /// </summary>
    public class FGUIGroupHelper : UIGroupHelperBase
    {
        public const int DepthFactor = 10000;

        private int m_Depth = 0;

        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public override void SetDepth(int depth)
        {
            m_Depth = depth;
        }

        private void Awake()
        {

        }

        private void Start()
        {

        }
    }
}