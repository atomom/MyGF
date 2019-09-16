using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class MyGLoader : GLoader
    {
        protected override void LoadExternal()
        {
            GameEntry.FGUIData.LoadTexture(this.url, OnLoadSuccess, OnLoadFail);
        }

        protected override void FreeExternal(NTexture texture)
        {
            texture.refCount--;
        }

        void OnLoadSuccess(NTexture texture)
        {
            if (string.IsNullOrEmpty(this.url))
                return;

            this.onExternalLoadSuccess(texture);
        }

        void OnLoadFail(string error)
        {
            Log.Error("load " + this.url + " failed: " + error);
            this.onExternalLoadFailed();
        }
    }
}