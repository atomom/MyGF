using UnityEngine;

namespace IUV.SDN
{
    public static partial class Constant
    {
        /// <summary>
        /// 层。
        /// </summary>
        public static class Layer
        {
            public const string DefaultLayerName = "Default";
            public static readonly int DefaultLayerId = LayerMask.NameToLayer(DefaultLayerName);

            public const string UILayerName = "UI";
            public static readonly int UILayerId = LayerMask.NameToLayer(UILayerName);

            public const string WalkLayerName = "Walkable";
            public static readonly int WalkLayerId = LayerMask.NameToLayer(WalkLayerName);

            public static readonly int TouchAble = 1 << WalkLayerId | 1 << DefaultLayerId;
        }
    }
}