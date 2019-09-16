namespace IUV.SDN
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry
    {
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }

        public static FGUIComponent FGUIData
        {
            get;
            private set;
        }
        public static TimerComponent Timer
        {
            get;
            private set;
        }

        public static InputComponent Input
        {
            get;
            private set;
        }
        public static CameraComponent Camera
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            FGUIData = UnityGameFramework.Runtime.GameEntry.GetComponent<FGUIComponent>();
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            Timer = UnityGameFramework.Runtime.GameEntry.GetComponent<TimerComponent>();
            Input = UnityGameFramework.Runtime.GameEntry.GetComponent<InputComponent>();
            Camera = UnityGameFramework.Runtime.GameEntry.GetComponent<CameraComponent>();
        }
    }
}