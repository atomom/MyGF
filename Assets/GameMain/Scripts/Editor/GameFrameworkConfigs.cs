using GameFramework;
using UnityEngine;
using UnityGameFramework.Editor;
using UnityGameFramework.Editor.AssetBundleTools;

namespace IUV.SDN.Editor
{
    public static class GameFrameworkConfigs
    {
        [BuildSettingsConfigPath]
        public static string BuildSettingsConfig = GameFramework.Utility.Path.GetCombinePath(Application.dataPath, "GameMain/Configs/BuildSettings.xml");

        [AssetBundleBuilderConfigPath]
        public static string AssetBundleBuilderConfig = GameFramework.Utility.Path.GetCombinePath(Application.dataPath, "GameMain/Configs/AssetBundleBuilder.xml");

        [AssetBundleEditorConfigPath]
        public static string AssetBundleEditorConfig = GameFramework.Utility.Path.GetCombinePath(Application.dataPath, "GameMain/Configs/AssetBundleEditor.xml");

        [AssetBundleCollectionConfigPath]
        public static string AssetBundleCollectionConfig = GameFramework.Utility.Path.GetCombinePath(Application.dataPath, "GameMain/Configs/AssetBundleCollection.xml");
    }
}
