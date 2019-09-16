using System;
using FairyGUI;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class FAniView : IDisposable
    {
        GameObject aniSet;
        GGraph holder;

        public FAniView(GComponent com)
        {
            holder = com.GetChild("grap") as GGraph;
        }

        public void Show(string type, string subtype = "")
        {
            holder.visible = true;
            var anipath = "UI/UIAnims/Ani/" + type + "-" + subtype;
            var path = AssetUtility.GetPrefabAsset(anipath);
            GameEntry.Resource.LoadAsset(path, typeof(GameObject), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    aniSet = asset as GameObject;
                    GameObject go = GameObject.Instantiate(aniSet);
                    GoWrapper wrapper = new GoWrapper(go);
                    holder.SetNativeObject(wrapper);
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error(path);
                }));
        }

        public void Hide()
        {
            holder.visible = false;
        }
        public void Dispose()
        {
            holder.SetNativeObject(null);
        }
    }
}