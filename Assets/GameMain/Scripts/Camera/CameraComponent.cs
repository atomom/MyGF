using System;
using System.Collections.Generic;
using FairyGUI;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Camera")]
    public class CameraComponent : GameFrameworkComponent, ICustomComponent
    {
        private Dictionary<CameraEffectType, ICameraEffect> m_CameraEffects = new Dictionary<CameraEffectType, ICameraEffect>();
        private List<ICameraBehaviour> m_CameraBehaviour = new List<ICameraBehaviour>();

        public Camera MainCamera { get; set; }
        public Camera UICamera { get; set; }
        public Transform Pivot { get; set; }

        [SerializeField]
        private Vector3 m_PivotOffset = new Vector3(0, 2, 0);
        [SerializeField]
        private Vector3 m_CameraOffset = new Vector3(0, 0, -4);

        private CameraEffectType curEffectType = CameraEffectType.ScreenFade;
        private float fadeTime = 1.5f;
        private HighlightingEffect highlighting;

        public void Init()
        {
            //获取设置当前屏幕分辩率
            Resolution[] resolutions = Screen.resolutions;
            //设置当前分辨率
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);

            Screen.fullScreen = true; //设置成全屏

            QualitySettings.SetQualityLevel(4);

            Pivot = transform.Find("Pivot");
            if (Pivot == null)
            {
                Pivot = new GameObject("Pivot").transform;
                Pivot.parent = transform;
                Pivot.localPosition = m_PivotOffset;
            }

            CreateMainCamera();

            InitUICamera();

            InitCameraEffect();

            InitCameraBehaviour();

            InitOtherAction();
        }

        public void Clear()
        {
            highlighting = null;
            m_CameraEffects.Clear();
            m_CameraBehaviour.Clear();
            GameObject.Destroy(Pivot.gameObject);
        }

        /// <summary>
        /// 创建相机
        /// </summary>
        /// <param name="name">相机名字</param>
        /// <returns></returns>
        public Camera CreateCamera(string name)
        {
            GameObject go = new GameObject(name);
            go.transform.parent = Pivot;
            go.transform.localPosition = m_CameraOffset;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            return go.AddComponent<Camera>();
        }

        /// <summary>
        /// 重载主相机
        /// </summary>
        public void ResetMainCamera()
        {
            if (MainCamera == null)
            {
                return;
            }
            MainCamera.fieldOfView = 60;
            MainCamera.renderingPath = RenderingPath.Forward;
            MainCamera.depth = Constant.Depth.MainCameraDepth;

            MainCamera.transform.parent = Pivot;
            // MainCamera.transform.localPosition = m_CameraOffset;
            // MainCamera.transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// 设置相机结点位置
        /// </summary>
        public void SetCameraRigPos(Vector3 pos)
        {
            transform.position = pos;
        }

        /// <summary>
        /// 显示相机特效
        /// </summary>
        /// <param name="effectType">特效类型</param>
        public void ShowEffect(CameraEffectType effectType)
        {
            if (effectType == CameraEffectType.Deafault)
            {
                HideAllEffect();
                return;
            }

            ICameraEffect cameraEffect;
            m_CameraEffects.TryGetValue(effectType, out cameraEffect);
            if (cameraEffect == null)
            {
                Log.Error("Can no find cameraEffect by type:", effectType);
                return;
            }

            cameraEffect.Show();
        }

        /// <summary>
        /// 隐藏相机特效
        /// </summary>
        /// <param name="effectType">特效类型</param>
        public void HideEffect(CameraEffectType effectType)
        {
            ICameraEffect cameraEffect;
            m_CameraEffects.TryGetValue(effectType, out cameraEffect);
            if (cameraEffect == null)
            {
                Log.Error("Can no find cameraEffect by type:", effectType);
                return;
            }

            cameraEffect.Hide();
        }

        /// <summary>
        /// 隐藏所有相机特效
        /// </summary>
        public void HideAllEffect()
        {
            foreach (KeyValuePair<CameraEffectType, ICameraEffect> effect in m_CameraEffects)
            {
                effect.Value.Hide();
            }
        }

        /// <summary>
        /// 淡入相机特效
        /// </summary>
        /// <param name="effectType">特效类型</param>
        /// <param name="fadeTime">淡入时间</param>
        /// <param name="callback">回调</param>
        public void FadeInEffect(CameraEffectType effectType, float fadeTime, Action callback = null)
        {
            ICameraEffect cameraEffect;
            m_CameraEffects.TryGetValue(effectType, out cameraEffect);
            if (cameraEffect == null)
            {
                Log.Error("Can no find cameraEffect by type:", effectType);
                return;
            }

            cameraEffect.FadeIn(fadeTime, callback);
        }

        /// <summary>
        /// 淡出相机特效
        /// </summary>
        /// <param name="effectType">特效类型</param>
        /// <param name="fadeTime">淡出时间</param>
        /// <param name="callback">回调</param>
        public void FadeOutEffect(CameraEffectType effectType, float fadeTime, Action callback = null)
        {
            ICameraEffect cameraEffect;
            m_CameraEffects.TryGetValue(effectType, out cameraEffect);
            if (cameraEffect == null)
            {
                Log.Error("Can no find cameraEffect by type:", effectType);
                return;
            }

            cameraEffect.FadeOut(fadeTime, callback);
        }

        /// <summary>
        /// 切换相机行为
        /// </summary>
        /// <param name="behaviourType">行为类型</param>
        public void SwitchCameraBehaviour(CameraBehaviourType behaviourType)
        {
            foreach (var cameraBehaviour in m_CameraBehaviour)
            {
                if (cameraBehaviour.Type == behaviourType)
                {
                    cameraBehaviour.Enable();
                }
                else
                {
                    cameraBehaviour.Disable();
                }
            }
        }

        private void InitCameraEffect()
        {
            ICameraEffect blurMovieEffect = MainCamera.gameObject.GetOrAddComponent<BlurMovieEffect>();
            m_CameraEffects.Add(CameraEffectType.BlurMovie, blurMovieEffect);

            ICameraEffect blurRadialEffect = MainCamera.gameObject.GetOrAddComponent<BlurRadialEffect>();
            m_CameraEffects.Add(CameraEffectType.BlurRadial, blurRadialEffect);

            ICameraEffect waterDropEffect = MainCamera.gameObject.GetOrAddComponent<WaterDropEffect>();
            m_CameraEffects.Add(CameraEffectType.WaterDrop, waterDropEffect);

            ICameraEffect screenGray = MainCamera.gameObject.GetOrAddComponent<ScreenGrayEffect>();
            m_CameraEffects.Add(CameraEffectType.ScreenGray, screenGray);

            ICameraEffect oilPaint = MainCamera.gameObject.GetOrAddComponent<OilPaintEffect>();
            m_CameraEffects.Add(CameraEffectType.OilPaint, oilPaint);

            ICameraEffect screenFade = MainCamera.gameObject.GetOrAddComponent<ScreenFadeEffect>();
            m_CameraEffects.Add(CameraEffectType.ScreenFade, screenFade);

            ICameraEffect screenShake = MainCamera.gameObject.GetOrAddComponent<ScreenShakeEffect>();
            m_CameraEffects.Add(CameraEffectType.ScreenShake, screenShake);

            HideAllEffect();
        }

        private void CreateMainCamera()
        {
            MainCamera = Camera.main;
            ResetMainCamera();
        }

        private void InitCameraBehaviour()
        {
            ICameraBehaviour freeLook = gameObject.GetOrAddComponent<CameraFreeLook>();
            m_CameraBehaviour.Add(freeLook);

            ICameraBehaviour lockLook = gameObject.GetOrAddComponent<CameraLockLook>();
            m_CameraBehaviour.Add(lockLook);

            //ICameraBehaviour smartLook = gameObject.GetOrAddComponent<CameraSmartLook>();
            //m_CameraBehaviour.Add(smartLook);

            SwitchCameraBehaviour(CameraBehaviourType.Default);
        }

        private void InitUICamera()
        {
            UICamera = GameObject.FindObjectOfType<StageCamera>()?.GetComponent<Camera>();
            if (UICamera == null)
            {
                Log.Error("Please Add UI Camera To The Scene.");
                return;
            }

            UICamera.depth = Constant.Depth.UICameraDepth;
        }

        private void InitOtherAction()
        {
            highlighting = MainCamera.gameObject.GetOrAddComponent<HighlightingEffect>();
            MainCamera.enabled = false;
            MainCamera.enabled = true;
        }

        public void EnableHighlight(bool highlight = false)
        {
            if (highlighting != null)
            {
                highlighting.enabled = highlight;
            }
        }

        void Update()
        {

        }

        public void FollowTarget(Transform target)
        {
            MainCamera.transform.rotation = target.transform.rotation;
            MainCamera.transform.position = target.transform.position;
        }
    }
}