using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace IUV.SDN
{
    public class ProcedureLogin : ProcedureBase
    {
        private bool m_Open = false;
        private bool m_StartTopo = false;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void StartTopo()
        {
            m_StartTopo = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            m_StartTopo = false;
            m_Open = false;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_Open == false)
            {
                m_Open = true;
                GameEntry.UI.OpenUIForm(UIFormId.LoginForm, this);
            }
            if (m_StartTopo)
            {
                m_StartTopo = false;
                m_Open = false;
                procedureOwner.SetData<VarInt>(Constant.SDNKey.NextSceneId, GameEntry.Config.GetInt(Constant.SDNKey.SceneTopo));
                procedureOwner.SetData<VarInt>(Constant.SDNKey.GameMode, (int) GameMode.Survival);
                procedureOwner.SetData<VarBool>(Constant.SDNKey.SaveMain, true);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }
        }
    }
}