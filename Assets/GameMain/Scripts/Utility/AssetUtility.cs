using GameFramework;

namespace IUV.SDN
{
    public static class AssetUtility
    {
        public static string GetConfigAsset(string assetName, LoadType loadType)
        {
            return string.Format("Assets/GameMain/Configs/{0}.{1}", assetName, loadType == LoadType.Text ? "txt" : "bytes");
        }

        public static string GetDataTableAsset(string assetName, LoadType loadType)
        {
            return string.Format("Assets/GameMain/DataTables/{0}.{1}", assetName, loadType == LoadType.Text ? "csv" : "bytes");
        }

        public static string GetDictionaryAsset(string assetName, LoadType loadType)
        {
            return string.Format("Assets/GameMain/Localization/{0}/Dictionaries/{1}.{2}", GameEntry.Localization.Language.ToString(), assetName, loadType == LoadType.Text ? "xml" : "bytes");
        }
        public static string GetConfigAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Configs/{0}.txt", assetName);
        }

        public static string GetDataTableAsset(string assetName)
        {
            return string.Format("Assets/GameMain/DataTables/{0}.csv", assetName);
        }

        public static string GetDictionaryAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Localization/{0}/Dictionaries/{1}.xml", GameEntry.Localization.Language.ToString(), assetName);
        }

        public static string GetFontAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Localization/{0}/Fonts/{1}.ttf", GameEntry.Localization.Language.ToString(), assetName);
        }

        public static string GetSceneAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Scenes/{0}.unity", assetName);
        }

        public static string GetMusicAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Art/Music/{0}.mp3", assetName);
        }

        public static string GetSoundAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Art/Sounds/{0}.wav", assetName);
        }

        public static string GetEntityAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Art/Entities/{0}.prefab", assetName);
        }
        public static string GetPrefabAsset(string assetPath)
        {
            return string.Format("Assets/GameMain/{0}.prefab", assetPath);
        }

        public static string GetUIFormAsset(string assetName)
        {
            return string.Format("Assets/GameMain/UI/UIForms/{0}.prefab", assetName);
        }

        public static string GetUISoundAsset(string assetName)
        {
            return string.Format("Assets/GameMain/UI/UISounds/{0}.wav", assetName);
        }

        public static string GetFGUIBytesAsset(string name)
        {
            return string.Format("{0}_fui.bytes", GetFGUIPackage(name));
        }
        public static string GetFGUIAtlasAsset(string name, string ext, int i)
        {
            if (i == 0)
            {
                return string.Format("{0}_{1}.png", GetFGUIPackage(name), ext);
            }
            return string.Format("{0}_{1}_{2}.png", GetFGUIPackage(name), ext, i);
        }
        public static string GetFGUIPackage(string name)
        {
            return string.Format("Assets/GameMain/UI/FGUI/{0}", name);
        }
        public static string GetIcon(string name)
        {
            return string.Format("Assets/GameMain/UI/UISprites/Icons/{0}.png", name);
        }
        public static string GetTexture(string name)
        {
            return string.Format("Assets/GameMain/UI/UITexture/{0}.png", name);
        }
    }
}