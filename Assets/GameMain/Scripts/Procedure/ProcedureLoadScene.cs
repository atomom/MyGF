using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace IUV.SDN
{
    public class ProcedureLoadScene : ProcedureBase
    {
        private bool m_Open = false;
        private bool m_Next = false;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void Next()
        {
            m_Next = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            m_Next = false;
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
                int m_SceneId = procedureOwner.GetData<VarInt>(Constant.CommonKey.NextSceneId).Value;
                IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
                DRScene drScene = dtScene.GetDataRow(m_SceneId);
                if (drScene == null)
                {
                    Log.Warning("Can not load scene '{0}' from data table.", m_SceneId.ToString());
                    return;
                }
                var formId = drScene.FormId;
                if (formId > -1)
                {
                    GameEntry.UI.OpenUIForm(formId, this);
                }
                else
                {
                    var nextSceneId = drScene.NextSceneId;
                    procedureOwner.SetData<VarInt>(Constant.CommonKey.NextSceneId, nextSceneId);
                    ChangeState<ProcedureChangeScene>(procedureOwner);
                }
            }
            if (m_Next)
            {
                m_Next = false;
                m_Open = false;
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