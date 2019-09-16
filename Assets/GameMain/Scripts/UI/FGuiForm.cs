using System;
using System.Collections;
using FairyGUI;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public abstract class FGUIForm : UIFormLogic
    {
        public virtual string FileName
        {
            get;
            set;
        }
        public virtual string ModelName
        {
            get;
            set;
        }

        public const int DepthFactor = 100;
        private const float FadeTime = 0.3f;

        private static Font s_MainFont = null;
        public GComponent GComponent
        {
            get;
            private set;
        }

        public int OriginalDepth
        {
            get;
            private set;
        }

        public int Depth
        {
            get
            {
                return GComponent.sortingOrder;
            }
        }

        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();

            if (ignoreFade)
            {
                GameEntry.UI.CloseUIForm(this);
            }
            else
            {
                if ((gameObject as GameObject) != null && gameObject.activeSelf)
                {
                    StartCoroutine(CloseCo(FadeTime));
                }
            }
        }

        public void PlayUISound(int uiSoundId)
        {
            GameEntry.Sound.PlayUISound(uiSoundId);
        }

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            s_MainFont = mainFont;

            GameObject go = new GameObject();
            go.AddComponent<Text>().font = mainFont;
            Destroy(go);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
            AddFGUIPackage(FileName);

            var go = UIPackage.CreateObject(FileName, ModelName);
            GComponent = go.asCom;
            GComponent.fairyBatching = true;
            GComponent.MakeFullScreen();
            GRoot.inst.AddChild(GComponent);
            OriginalDepth = GComponent.sortingOrder;
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);
            ForceRefresh(userData);
            GComponent.visible = true;
            GComponent.alpha = 0f;
            StopAllCoroutines();

            if ((gameObject as GameObject) != null && gameObject.activeSelf)
            {
                StartCoroutine(GComponent.FadeToAlpha(1f, FadeTime));
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            GComponent.visible = false;
            base.OnClose(isShutdown, userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnPause()
#else
        protected internal override void OnPause()
#endif
        {
            GComponent.visible = false;
            base.OnPause();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnResume()
#else
        protected internal override void OnResume()
#endif
        {
            base.OnResume();
            GComponent.visible = true;
            GComponent.alpha = 0f;
            StopAllCoroutines();

            if ((gameObject as GameObject) != null && gameObject.activeInHierarchy)
            {
                StartCoroutine(GComponent.FadeToAlpha(1f, FadeTime));
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnCover()
#else
        protected internal override void OnCover()
#endif
        {
            base.OnCover();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnReveal()
#else
        protected internal override void OnReveal()
#endif
        {
            base.OnReveal();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnRefocus(object userData)
#else
        protected internal override void OnRefocus(object userData)
#endif
        {
            base.OnRefocus(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
        /// <summary>
        /// ????
        /// </summary>
        /// <param name="userData"></param>
        public virtual void ForceRefresh(object userData)
        {

        }
#if UNITY_2017_3_OR_NEWER
        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#else
        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#endif
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth = FGUIGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            GComponent.sortingOrder += deltaDepth;
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return null;
            GameEntry.UI.CloseUIForm(this);
        }

        public void AddFGUIPackage(string name)
        {
            GameEntry.FGUIData.AddFGUIPackage(name);
        }
        protected virtual void OnDestroy()
        {
            GRoot.inst.RemoveChild(GComponent, true);
            GComponent = null;
        }
    }
}