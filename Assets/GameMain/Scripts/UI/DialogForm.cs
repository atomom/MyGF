using System;
using FairyGUI;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class DialogForm : FGUIForm
    {
        public DialogParams UserDialogParams;
        public string Title
        {
            get;
            set;
        }
        public GComponent SubCom
        {
            get;
            private set;
        }
        public object UserData
        {
            get;
            set;
        }
        protected GGroup Group;
        private Vector3 mOldPos;
        public GTextField TitleLabel;

        private Controller modeCtl;
        private int Mode = 2;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            UserDialogParams = userData as DialogParams;
            if (UserDialogParams != null)
            {
                FileName = UserDialogParams.FileName == null ? FileName : UserDialogParams.FileName;
                ModelName = UserDialogParams.ModelName == null? ModelName : UserDialogParams.ModelName;
            }
            FileName = string.IsNullOrEmpty(FileName) ? "topo" : FileName;
            ModelName = string.IsNullOrEmpty(ModelName) ? "DialogShow" : ModelName;
            base.OnInit(userData);

            Group = GComponent.GetChild("Group").asGroup;

            SubCom = GComponent.GetChild("Dialog").asCom;
            modeCtl = SubCom.GetController("Mode");

        }

        private void AddButtonClick()
        {
            SubCom.GetChild("Exit").onClick.Add(OnExit);
            SubCom.GetChild("Cancel").onClick.Add(OnExit);
            SubCom.GetChild("Ok").onClick.Add(OnOk);
            SubCom.GetChild("Ok1").onClick.Add(OnOk);
        }

#if UNITY_2017_3_OR_NEWER
        protected virtual void OnOk()
#else
        protected internal virtual void OnOk()
#endif
        {
            if (UserDialogParams.OnClickConfirm != null)
            {
                UserDialogParams.OnClickConfirm(UserData);
            }
            this.Close();
        }
#if UNITY_2017_3_OR_NEWER
        protected virtual void OnExit()
#else
        protected internal virtual void OnExit()
#endif
        {
            if (UserDialogParams.OnClickCancel != null)
            {
                UserDialogParams.OnClickCancel(UserData);
            }
            this.Close();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);
        }

        public override void ForceRefresh(object userData)
        {
            UserDialogParams = userData as DialogParams;
            Title = UserDialogParams.Title == null ? Title : UserDialogParams.Title;
            Mode = UserDialogParams.Mode == 0 ? 2 : UserDialogParams.Mode;
            Mode = Mathf.Min(Mathf.Max(1, Mode), 2);
            Title = string.IsNullOrEmpty(Title) ? "" : Title;

            modeCtl.SetSelectedIndex(Mode - 1);
            TitleLabel = SubCom.GetChild("title").asTextField;
            TitleLabel.text = Title;

            AddButtonClick();

            var label = GComponent.GetChild("label");
            if (label != null)
            {
                label.text = UserDialogParams.Message;
            }

            UserData = UserDialogParams.UserData;
            if (UserData != null && UserData is object[])
            {
                var objs = UserData as object[];
                if (objs.Length >= 4)
                {
                    Vector3 pos = (Vector3) objs[1] + new Vector3(150, 0); //当前的l位置
                    Vector2 size = (Vector2) objs[2]; //限定范围宽度
                    Vector2 gPos = (Vector2) objs[3]; //当前所在范围内的 g位置
                    mOldPos = GComponent.GlobalToLocal(gPos) + new Vector2(150, 0); //当前所在的本地的老位置
                    ShowPos(pos, size);
                }
            }
        }

        void ShowPos(Vector3 pos, Vector2 size)
        {
            var r = size.x - pos.x;
            var b = size.y - pos.y;
            Group.position = mOldPos;
            if (r < Group.width)
            {
                Group.position = Group.position - new Vector3(Group.width + 150, 0, 0);
            }
            if (b < Group.height)
            {
                Group.position = Group.position - new Vector3(0, Group.height - b, 0);
            }
        }
        /// <summary>
        /// 实时刷新 鼠标移动时，未画完的线条
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
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