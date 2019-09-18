using System;
using System.Collections.Generic;
using FairyGUI;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace IUV.SDN
{
    public class ProcedurePreload : ProcedureBase
    {
        string[] datas = new string[] { "DRMusic", "DRScene", "DRSound", "DRUIForm", "DRUISound" };
        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            m_LoadedFlag.Clear();

            PreloadResources();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            IEnumerator<bool> iter = m_LoadedFlag.Values.GetEnumerator();
            while (iter.MoveNext())
            {
                if (!iter.Current)
                {
                    return;
                }
            }
            //Config是读取的配置文件 DefaultConfig.txt
            procedureOwner.SetData<VarInt>(Constant.CommonKey.NextSceneId, GameEntry.Config.GetInt(Constant.CommonKey.SceneLogin));
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        private void PreloadResources()
        {
            // Preload configs
            LoadConfig("DefaultConfig");

            // Preload data tables
            for (int i = 0; i < datas.Length; i++)
            {
                LoadDataTable(datas[i]);
            }

            // Preload dictionaries
            LoadDictionary("Default");

            UIConfig.defaultFont = "";
            // Preload fonts
            LoadFont("MainFont");

            LoadFGui();
        }

        private void LoadFGui()
        {
            UIObjectFactory.SetLoaderExtension(typeof(MyGLoader));

            GRoot.inst.SetContentScaleFactor(designResolutionX: Constant.UI.Width, designResolutionY: Constant.UI.Height, screenMatchMode: UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
            GameEntry.FGUIData.ClearFGui();

            var fguiModels = new string[] { };
            for (int i = 0, n = fguiModels.Length; i < n; ++i)
            {
                var fguimodel = fguiModels[i];
                GameEntry.FGUIData.LoadFGuiModel(fguimodel,
                    (fuiname) =>
                    {
                        m_LoadedFlag.Add(fguimodel, false);
                    },
                    (name, k) =>
                    {
                        GameEntry.FGUIData.AddFGUIPackage(fguimodel);
                        m_LoadedFlag[fguimodel] = true;
                    });
            }
        }

        private void LoadConfig(string configName)
        {
            m_LoadedFlag.Add(string.Format("Config.{0}", configName), false);
            GameEntry.Config.LoadConfig(configName, LoadType.Text, this);
        }

        private void LoadDataTable(string dataTableName)
        {
            m_LoadedFlag.Add(string.Format("DataTable.{0}", dataTableName), false);
            GameEntry.DataTable.LoadDataTable(dataTableName, LoadType.Text, this);
        }

        private void LoadDictionary(string dictionaryName)
        {
            m_LoadedFlag.Add(string.Format("Dictionary.{0}", dictionaryName), false);
            GameEntry.Localization.LoadDictionary(dictionaryName, this);
        }

        private void LoadFont(string fontName)
        {
            m_LoadedFlag.Add(string.Format("Font.{0}", fontName), false);
            GameEntry.Resource.LoadAsset(AssetUtility.GetFontAsset(fontName), Constant.AssetPriority.FontAsset, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Font font = (Font) asset;
                    m_LoadedFlag[string.Format("Font.{0}", fontName)] = true;
                    FGUIForm.SetMainFont(font);

                    FontManager.RegisterFont(FontManager.GetFont(fontName, font), fontName);
                    if (UIConfig.defaultFont == "")
                    {
                        UIConfig.defaultFont += fontName;
                    }
                    else
                    {
                        UIConfig.defaultFont += "," + fontName;
                    }

                    Log.Info("Load font '{0}' OK.", UIConfig.defaultFont);
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", fontName, assetName, errorMessage);
                }));
        }

        private void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {
            LoadConfigSuccessEventArgs ne = (LoadConfigSuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[string.Format("Config.{0}", ne.ConfigName)] = true;
            Log.Info("Load config '{0}' OK.", ne.ConfigName);
        }

        private void OnLoadConfigFailure(object sender, GameEventArgs e)
        {
            LoadConfigFailureEventArgs ne = (LoadConfigFailureEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'.", ne.ConfigName, ne.ConfigAssetName, ne.ErrorMessage);
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[string.Format("DataTable.{0}", ne.DataTableName)] = true;
            Log.Info("Load data table '{0}' OK.", ne.DataTableName);
        }

        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableName, ne.DataTableAssetName, ne.ErrorMessage);
        }

        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[string.Format("Dictionary.{0}", ne.DictionaryName)] = true;
            Log.Info("Load dictionary '{0}' OK.", ne.DictionaryName);
        }

        private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryName, ne.DictionaryAssetName, ne.ErrorMessage);
        }
    }
}