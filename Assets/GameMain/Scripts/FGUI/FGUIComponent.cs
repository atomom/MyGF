using System;
using System.Collections.Generic;
using FairyGUI;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
namespace IUV.SDN
{
    public class FGUIComponent : GameFrameworkComponent
    {
        float nTime;
        Dictionary<string, object> mFGUIBytesDic = new Dictionary<string, object>();
        Dictionary<string, NTexture> mPool = new Dictionary<string, NTexture>();
        public delegate void LoadCompleteCallback(NTexture texture);
        public delegate void LoadErrorCallback(string error);
        public const int POOL_CHECK_TIME = 30;
        public const int MAX_POOL_SIZE = 100;
        public void LoadTexture(string url,
            LoadCompleteCallback onSuccess,
            LoadErrorCallback onFail)
        {
            NTexture texture;
            if (mPool.TryGetValue(url, out texture))
            {
                texture.refCount++;
                onSuccess(texture);
                return;
            }

            GameEntry.Resource.LoadAsset(url, typeof(Texture2D), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    texture = new NTexture(asset as Texture2D);
                    texture.refCount++;
                    onSuccess(texture);
                },

                (assetName, status, errorMessage, userData) =>
                {
                    onFail(errorMessage);
                }));
        }

        void Update()
        {
            if (Time.time - nTime > POOL_CHECK_TIME)
            {
                nTime = Time.time;
                FreeIdleIcons();
            }
        }
        void FreeIdleIcons()
        {
            int cnt = mPool.Count;
            if (cnt > MAX_POOL_SIZE)
            {
                List<string> toRemove = null;

                var e = mPool.GetEnumerator();
                while (e.MoveNext())
                {
                    var de = e.Current;
                    string key = de.Key;
                    NTexture texture = de.Value;
                    if (texture.refCount == 0)
                    {
                        if (toRemove == null)
                            toRemove = new List<string>();
                        toRemove.Add(key);
                        texture.Dispose();
                        cnt--;
                        if (cnt <= MAX_POOL_SIZE / 2)
                            break;
                    }
                }
                if (toRemove != null)
                {
                    for (int i = 0, n = toRemove.Count; i < n; ++i)
                    {
                        var key = toRemove[i];
                        mPool.Remove(key);
                    }
                }
            }
        }
        public void ClearFGui()
        {
            mFGUIBytesDic.Clear();
        }

        public object GetFGui(string name)
        {
            object ret;
            mFGUIBytesDic.TryGetValue(name, out ret);
            return ret;
        }

        private void AddFGui(string name, object asset)
        {
            object ret;
            if (mFGUIBytesDic.TryGetValue(name, out ret))
            {
                mFGUIBytesDic[name] = asset;
            }
            else
            {
                mFGUIBytesDic.Add(name, asset);
            }
        }

        Dictionary<string, Action<string, int>> mSuccess = new Dictionary<string, Action<string, int>>();
        public void LoadFGuiModel(string name, Action<string> before, Action<string, int> success)
        {
            mSuccess[name] = success;
            LoadFGuiFile(name, "fui", 0, before, (name1, i) =>
            {
                LoadAtlas(name1, -1);
            }, mSuccess[name]);
        }

        void LoadAtlas(string name, int i)
        {
            string t = "atlas0";
            LoadFGuiFile(name, t, i + 1, null, LoadAtlas, mSuccess[name]);
        }

        void LoadFGuiFile(string name, string type, int i, Action<string> before, Action<string, int> success, Action<string, int> failed)
        {
            string filename = null;
            if (type == "fui")
            {
                filename = AssetUtility.GetFGUIBytesAsset(name);
            }
            else
            {
                filename = AssetUtility.GetFGUIAtlasAsset(name, type, i);
            }

            if (before != null)
            {
                before(filename);
            }
            if (type == "fui")
            {
                if (GetFGui(AssetUtility.GetFGUIBytesAsset(name)) != null)
                {
                    if (success != null)
                    {
                        success(name, i);
                    }

                    return;
                }
            }
            else
            {
                if (GetFGui(AssetUtility.GetFGUIAtlasAsset(name, type, i)) != null)
                {

                    if (success != null)
                    {
                        success(name, i);
                    }

                    return;
                }
            }

            GameEntry.Resource.LoadAsset(filename, Constant.AssetPriority.FGUIAsset, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    if (type == "fui")
                    {
                        AddFGui(AssetUtility.GetFGUIBytesAsset(name), asset);
                    }
                    else
                    {
                        AddFGui(AssetUtility.GetFGUIAtlasAsset(name, type, i), asset);
                    }

                    if (success != null)
                    {
                        success(name, i);
                    }
                    Log.Info("Load fuiAsset '{0}' OK.", filename);
                },

                (assetName, status, errorMessage, userData) =>
                {
                    if (failed != null)
                    {
                        failed(name, i);
                    }
                    if (type == "fui")
                    {
                        Log.Error("Can not load fuiAsset '{0}' from '{1}' with error message '{2}'.", filename, assetName, errorMessage);
                    }
                }));
        }

        public object LoadResource(string name, string extension, System.Type type, out DestroyMethod destroyMethod)
        {
            object ret = GameEntry.FGUIData.GetFGui(name + extension);
            destroyMethod = DestroyMethod.Unload;
            if (!name.EndsWith("!a") && ret == null)
            {
                Log.Error("Load fuiAsset '{0}{1}' failed.", name, extension);
            }
            return ret;
        }

        public void AddFGUIPackage(string name)
        {
            UIPackage.AddPackage(AssetUtility.GetFGUIPackage(name), LoadResource);
        }
    }
}