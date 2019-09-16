using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IUV.SDN
{
    public enum MoveAction
    {
        Left,
        Right,
        UP,
        Down,
        LRotate,
        RRotate,
        Speed
    }
    /// <summary>
    /// 移动数据
    /// </summary>
    public class MoveInfo
    {
        public KeyCode key;
        /// <summary>
        /// 当前移动方向
        /// </summary>
        public MoveAction dir;
        public bool isMouseDown;

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var o = obj as MoveInfo;
            return o.dir == dir && o.key == key;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return dir.GetHashCode() * 1000 + key.GetHashCode();
        }

        public override string ToString()
        {
            return Utility.ToJson(this);
        }

    }
    /// <summary>
    /// 在机房中的第一人称视角下，用户移动
    /// </summary>
    public class PlayerMove : MonoBehaviour
    {
        public CharacterController Controller
        {
            get;
            set;
        }
        public NavMeshAgent NavAgent
        {
            get;
            set;
        }
        public Camera MainCamera
        {
            get;
            set;
        }

        [Tooltip("用户移动速度")]
        public float MoveSpeed = 1f; //移动速度
        [Tooltip("用户左右旋转视角的速度")]
        private float AngleSpeed = 0;
        [Tooltip("碰撞距离检测，用户与障碍物，障碍物默认是设置到第8层")]
        public float spinLength = 4;
        [Tooltip("摄像机到跟随目标的距离")]
        public float distance = 0.5f;
        public GameObject head;
        public float los = 5f; //视距，在指定的视距内显示热点效果
        public Transform mouseFlg;
        Vector3 orgPos;
        Vector3 orgQua;
        MoveAction[] move1 = new MoveAction[] { MoveAction.Left, MoveAction.Right, MoveAction.UP, MoveAction.Down };
        KeyCode[][] keys1 = { new KeyCode[] { KeyCode.A, KeyCode.LeftArrow }, new KeyCode[] { KeyCode.D, KeyCode.RightArrow }, new KeyCode[] { KeyCode.W, KeyCode.UpArrow }, new KeyCode[] { KeyCode.S, KeyCode.DownArrow } };
        List<MoveInfo> infos1 = new List<MoveInfo>();

        MoveAction[] move2 = new MoveAction[] { MoveAction.LRotate, MoveAction.RRotate, MoveAction.Speed };
        KeyCode[][] keys2 = { new KeyCode[] { KeyCode.Q }, new KeyCode[] { KeyCode.E }, new KeyCode[] { KeyCode.LeftShift } };
        List<MoveInfo> infos2 = new List<MoveInfo>();

        //避开障碍物 逻辑处理
        void Awake()
        {
            Controller = GetComponent<CharacterController>();
            orgPos = transform.localPosition;
            orgQua = transform.localRotation.eulerAngles;
            NavAgent = GetComponent<NavMeshAgent>();
            Init(move1, keys1, infos1);
            Init(move2, keys2, infos2);
        }

        void Init(MoveAction[] moves, KeyCode[][] keys, List<MoveInfo> infos)
        {
            infos.Clear();
            for (int i = 0, c = moves.Length; i < c; ++i)
            {
                var dir = moves[i];
                var k2 = keys[i];
                foreach (var k in k2)
                {
                    var m = new MoveInfo();
                    m.key = k;
                    m.dir = dir;
                    infos.Add(m);
                }
            }
        }
        /// <summary>
        /// 初始化设置，每次进入3D场景漫游都会初始化调用此函数
        /// </summary>
        void OnEnable()
        {
            foreach (var item in infos1)
            {
                item.isMouseDown = false;
            }
            if (mouseFlg != null)
            {
                mouseFlg.gameObject.SetActive(false);
            }
        }
        void Update()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            CheckUserKeyBoradDown();
        }
        Vector3 targetPosition;
        MoveInfo tmpm = new MoveInfo();
        MoveInfo tmp1;
        /// <summary>
        /// 检测用户是否按下按键
        /// </summary>
        void CheckUserKeyBoradDown()
        {
            if (MainCamera == null)
            {
                return;
            }
            delta_rotation_x = 0;
            delta_rotation_y = 0;
            FollowTargetByMousePosition();
            Vector3 position = transform.localPosition;

            Vector3 local = transform.localPosition;
            if (AngleSpeed == 0)
            {
                AngleSpeed = transform.localRotation.eulerAngles.y;
            }
            MoveSpeed = 1;
            GetKey(move2, keys2, infos2);
            var rotate = infos2[0];
            if (rotate.isMouseDown)
            {
                switch (rotate.dir)
                {
                    case MoveAction.LRotate:
                        delta_rotation_x = -RotateSpeed;
                        break;
                    case MoveAction.RRotate:
                        delta_rotation_x = RotateSpeed;
                        break;
                    case MoveAction.Speed:
                        MoveSpeed = 2;
                        break;
                }
            }
            transform.Rotate(0, delta_rotation_x, 0, Space.World);

            GetKey(move1, keys1, infos1);
            var move = infos1[0];
            if (move.isMouseDown)
            {
                if (mouseFlg != null)
                {
                    mouseFlg.gameObject.SetActive(false);
                }

                if (NavAgent.enabled)
                    NavAgent.isStopped = true;

                position.y = local.y;

                switch (move.dir)
                {
                    case MoveAction.UP:
                        Controller.Move(MoveSpeed * Time.deltaTime * transform.forward);
                        break;
                    case MoveAction.Down:
                        Controller.Move(-MoveSpeed * Time.deltaTime * transform.forward);
                        break;
                    case MoveAction.Left:
                        Controller.Move(-MoveSpeed * Time.deltaTime * transform.right);
                        break;
                    case MoveAction.Right:
                        Controller.Move(MoveSpeed * Time.deltaTime * transform.right);
                        break;
                }
            }

            GameEntry.Camera.FollowTarget(head.transform);

            //判断鼠标效
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                RaycastHit[] raycastHits = new RaycastHit[2];
                if (Physics.RaycastNonAlloc(ray, raycastHits, 1000, Constant.Layer.TouchAble) > 0)
                {
                    hit = raycastHits[0];
                    var dis = 1000f;
                    foreach (var h in raycastHits)
                    {
                        if (h.transform != null)
                        {
                            if (h.distance < dis)
                            {
                                dis = h.distance;
                                hit = h;
                            }
                        }
                    }
                    if (hit.transform.gameObject.layer != Constant.Layer.WalkLayerId)
                    {
                        return;
                    }
                    targetPosition = hit.point;
                    if (mouseFlg != null)
                    {

                        transform.rotation = head.transform.rotation;

                        head.transform.localRotation = Quaternion.identity;

                        MainCamera.transform.rotation = head.transform.rotation;

                        mouseFlg.gameObject.SetActive(true);
                        mouseFlg.position = targetPosition;
                        NavAgent.enabled = true;
                        NavAgent.isStopped = false;
                        NavAgent.destination = targetPosition;
                    }
                }
            }
            float des = Vector3.Distance(transform.position, targetPosition);
            if (mouseFlg != null && mouseFlg.gameObject.activeSelf && des < 1f)
            {
                mouseFlg.gameObject.SetActive(false);
            }
        }

        private void GetKey(MoveAction[] moves, KeyCode[][] keys, List<MoveInfo> infos)
        {
            for (int i = 0, c = moves.Length; i < c; ++i)
            {
                var dir = moves[i];
                var k2 = keys[i];
                foreach (var k in k2)
                {
                    tmpm.dir = dir;
                    tmpm.key = k;
                    var idx = infos.IndexOf(tmpm);
                    if (idx == -1 || idx >= infos.Count)
                    {
                        break;
                    }
                    var m = infos[idx];
                    if (Input.GetKeyDown(k))
                    {
                        m.isMouseDown = true;
                        infos.RemoveAt(idx);
                        infos.Insert(0, m);
                    }
                    if (Input.GetKeyUp(k))
                    {
                        m.isMouseDown = false;
                        infos.RemoveAt(idx);
                        infos.Add(m);
                    }
                }
            }
        }

        bool isAllowRotate = false;
        float delta_rotation_x, delta_rotation_y;
        float RotateSpeed = 1; //镜头旋转速率
        /// <summary>
        /// 让目标（camera）跟随鼠标旋转,鼠标移动到什么位置则camera旋转到鼠标对应的方向
        /// </summary>
        void FollowTargetByMousePosition()
        {
            if (Input.GetMouseButtonDown(1))
            {
                isAllowRotate = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                isAllowRotate = false;
            }
            if (isAllowRotate)
            {
                //目标跟随鼠标旋转
                delta_rotation_x = Input.GetAxis("Mouse X") * RotateSpeed;
                delta_rotation_y = -Input.GetAxis("Mouse Y") * RotateSpeed;
                head.transform.Rotate(0, delta_rotation_x, 0, Space.World);
                head.transform.Rotate(delta_rotation_y, 0, 0);
            }
        }
    }
}