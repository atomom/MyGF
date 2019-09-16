using System;
using UnityEngine;

namespace IUV.SDN
{
    public abstract class CameraEffectBase : MonoBehaviour, ICameraEffect
    {
        public abstract void Init();

        public abstract void Show();

        public abstract void Hide();

        public abstract void FadeIn(float fadeTime, Action callback = null);

        public abstract void FadeOut(float fadeTime, Action callback = null);

        public abstract void Dispose();
    }
}