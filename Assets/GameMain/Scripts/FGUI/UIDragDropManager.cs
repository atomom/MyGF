using FairyGUI;
using UnityEngine;

namespace IUV.SDN
{
    public class UIDragDropManager
    {
        private GComponent _agent;
        private GObject _source;
        private object _sourceData;

        private static UIDragDropManager _inst;
        public static UIDragDropManager inst
        {
            get
            {
                if (_inst == null)
                    _inst = new UIDragDropManager();
                return _inst;
            }
        }

        public UIDragDropManager()
        {
            _agent = (GComponent) UIObjectFactory.NewObject(ObjectType.Component);
            _agent.gameObjectName = "UIDragDropAgent";
            _agent.SetHome(GRoot.inst);
            _agent.touchable = false; //important
            _agent.draggable = true;
            _agent.SetSize(0, 0);
            _agent.SetPivot(0.5f, 0.5f, true);
            _agent.sortingOrder = int.MaxValue;
            _agent.onDragEnd.Add(__dragEnd);
        }

        /// <summary>
        /// Loader object for real dragging.
        /// 用于实际拖动的Loader对象。你可以根据实际情况设置loader的大小，对齐等。
        /// </summary>
        public GComponent dragAgent
        {
            get { return _agent; }
        }

        /// <summary>
        /// Is dragging?
        /// 返回当前是否正在拖动。
        /// </summary>
        public bool dragging
        {
            get { return _agent.parent != null; }
        }

        /// <summary>
        /// Start dragging.
        /// 开始拖动。
        /// </summary>
        /// <param name="source">Source object. This is the object which initiated the dragging.</param>
        /// <param name="icon">Icon to be used as the dragging sign.</param>
        /// <param name="sourceData">Custom data. You can get it in the onDrop event data.</param>
        /// <param name="touchPointID">Copy the touchId from InputEvent to here, if has one.</param>
        public void StartDrag(GObject source, object sourceData, Vector2 cha, int touchPointID = -1)
        {
            if (_agent.parent == null)
            {
                _source = source;
                _agent.AddChild(_source);
                GRoot.inst.AddChild(_agent);
                _sourceData = sourceData;
                FollowMouse(cha);
                _agent.StartDrag(touchPointID);
            }
        }
        void FollowMouse(Vector2 cha)
        {
            if (_source != null)
            {
                var screenPos = Input.mousePosition;
                //原点位置转换
                screenPos.y = Screen.height - screenPos.y;
                Vector2 pt = GRoot.inst.GlobalToLocal(screenPos) + cha;
                _agent.SetXY(Mathf.RoundToInt(pt.x), Mathf.RoundToInt(pt.y));
            }
        }
        /// <summary>
        /// Cancel dragging.
        /// 取消拖动。
        /// </summary>
        public void Cancel()
        {
            if (_agent.parent != null)
            {
                _agent.StopDrag();
                _agent.RemoveChild(_source, true);
                GRoot.inst.RemoveChild(_agent);
                _source = null;
                _sourceData = null;
            }
        }

        private void __dragEnd(EventContext evt)
        {
            if (_agent.parent == null) //cancelled
                return;

            var source = _source;

            object sourceData = _sourceData;
            GObject obj = GRoot.inst.touchTarget;
            while (obj != null)
            {
                if (obj is GComponent)
                {
                    if (!((GComponent) obj).onDrop.isEmpty)
                    {
                        obj.RequestFocus();
                        ((GComponent) obj).onDrop.Call(sourceData, source);
                        break;
                    }
                }

                obj = obj.parent;
            }

            _agent.RemoveChild(_source, true);
            GRoot.inst.RemoveChild(_agent);
            _source = null;
            _sourceData = null;
        }

    }
}