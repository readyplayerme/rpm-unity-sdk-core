using ReadyPlayerMe.Core;
using UnityEditor;
using UnityEngine;

public static class EditorAssetLoader 
{
    private const string SETTINGS_SAVE_FOLDER = "Ready Player Me/Resources/Settings";
    private const string SETTINGS_ASSET_NAME = "ReadyPlayerMeSettings.asset";
    private const string AVATAR_LOADER_ASSET_NAME = "AvatarLoaderSettings.asset";

    private const string CONFIG_SAVE_FOLDER = "Ready Player Me/Resources/Configurations";

    public static readonly string DefaultReadyPlayerMeSettingsPath = $"Packages/com.readyplayerme.core/Settings/{SETTINGS_ASSET_NAME}";
    public static readonly string DefaultAvatarLoaderSettingsPath = $"Packages/com.readyplayerme.core/Settings/{AVATAR_LOADER_ASSET_NAME}";
    
    private static readonly string[] DefaultConfigNames = {  "Avatar Config Medium", "Avatar Config Low", "Avatar Config High" };

    public static void CreateSettingsAssets()
    {
        DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{SETTINGS_SAVE_FOLDER}");
        CreateAvatarConfigAssets();
        CreateAvatarLoaderSettings();
        CreateReadyPlayerMeSettings();
    }

    private static void CreateReadyPlayerMeSettings()
    {
        var defaultSettings = AssetDatabase.LoadAssetAtPath<ReadyPlayerMeSettings>(DefaultReadyPlayerMeSettingsPath);
        ReadyPlayerMeSettings newSettings = ScriptableObject.CreateInstance<ReadyPlayerMeSettings>();
        newSettings.partnerSubdomain = defaultSettings.partnerSubdomain;
        var loaderSettings = AvatarLoaderSettings.LoadSettings();
        newSettings.avatarLoaderSettings = loaderSettings;

        AssetDatabase.CreateAsset(newSettings, $"Assets/{SETTINGS_SAVE_FOLDER}/{SETTINGS_ASSET_NAME}");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateAvatarLoaderSettings()
    {
        var defaultSettings = AssetDatabase.LoadAssetAtPath<AvatarLoaderSettings>(DefaultAvatarLoaderSettingsPath);
        var newSettings = ScriptableObject.CreateInstance<AvatarLoaderSettings>();
        newSettings.AvatarConfig = null;
        newSettings.AvatarCachingEnabled = defaultSettings.AvatarCachingEnabled;

        AssetDatabase.CreateAsset(newSettings, $"Assets/{SETTINGS_SAVE_FOLDER}/{AVATAR_LOADER_ASSET_NAME}");
        AssetDatabase.SaveAssets();
    }
    
    public static void CreateAvatarConfigAssets()
    {
        DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{CONFIG_SAVE_FOLDER}");
        foreach (var configName in DefaultConfigNames)
        {
#if DISABLE_AUTO_INSTALLER
            var defaultConfig = AssetDatabase.LoadAssetAtPath<AvatarConfig>($"Assets/Ready Player Me/Core/Configurations/{configName}.asset");
#else
            var defaultConfig = AssetDatabase.LoadAssetAtPath<AvatarConfig>($"Packages/com.readyplayerme.core/Configurations/{configName}.asset");
#endif
            var newSettings = ScriptableObject.CreateInstance<AvatarConfig>();
            newSettings.Pose = defaultConfig.Pose;
            newSettings.MeshLod = defaultConfig.MeshLod;
            newSettings.TextureAtlas = defaultConfig.TextureAtlas;
            newSettings.MorphTargets = defaultConfig.MorphTargets;
            newSettings.UseHands = defaultConfig.UseHands;
            newSettings.TextureSizeLimit = defaultConfig.TextureSizeLimit;
            AssetDatabase.CreateAsset(newSettings, $"Assets/{CONFIG_SAVE_FOLDER}/{configName}.asset");
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
