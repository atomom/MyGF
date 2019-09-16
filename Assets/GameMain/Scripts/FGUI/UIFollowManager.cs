using FairyGUI;
using UnityEngine;

namespace IUV.SDN
{
    public class UIFollowManager
    {
        private GObject _rect;
        private GComponent _agent;
        public GComponent Source
        {
            get;
            set;
        }

        private static UIFollowManager _inst;
        public static UIFollowManager inst
        {
            get
            {
                if (_inst == null)
                    _inst = new UIFollowManager();
                return _inst;
            }
        }

        public UIFollowManager()
        {
            _agent = (GComponent) UIObjectFactory.NewObject(ObjectType.Component);
            _agent.gameObjectName = "UIFollowAgent";
            GRoot.inst.onRightClick.Add(End);
            _agent.SetHome(GRoot.inst);
            _agent.touchable = false; //important
            _agent.draggable = true;
            _agent.SetSize(0, 0);
            _agent.SetPivot(0.5f, 0.5f, true);
            _agent.sortingOrder = int.MaxValue;
        }
        public Vector2 _cha = Vector2.zero;
        public void StartMove(GComponent source, GObject rect, Vector2 cha)
        {
            _cha = cha;
            _rect = rect;
            End();
            if (_agent.parent == null)
            {
                Source = source;
                _agent.AddChild(Source);
                GRoot.inst.AddChild(_agent);
            }
        }
        public void FollowMouse()
        {
            if (Source != null)
            {
                var screenPos = Input.mousePosition;
                //原点位置转换
                screenPos.y = Screen.height - screenPos.y;

                Vector2 pt = GRoot.inst.GlobalToLocal(screenPos) + _cha;
                _agent.SetXY(Mathf.RoundToInt(pt.x), Mathf.RoundToInt(pt.y));
            }
        }
        public void End()
        {
            if (_agent.parent != null)
            {
                _agent.RemoveChild(Source, true);
                GRoot.inst.RemoveChild(_agent);
                Source = null;
            }
        }
    }
}