using System.IO;
using ReadyPlayerMe.Core;
using UnityEditor;
using UnityEngine;

public static class EditorAssetLoader 
{
    private static readonly string SettingsSaveFolder = "Ready Player Me/Resources/Settings";
    private const string SETTINGS_ASSET_NAME = "ReadyPlayerMeSettings.asset";
    private const string AVATAR_LOADER_ASSET_NAME = "AvatarLoaderSettings.asset";

    private const string CONFIG_SAVE_FOLDER = "Ready Player Me/Resources/Avatar Configurations";
    
#if !DISABLE_AUTO_INSTALLER

    public static readonly string ReadyPlayerMeAssetPath = $"Packages/com.readyplayerme.core/Settings/{SETTINGS_ASSET_NAME}";
    public static readonly string AvatarLoaderAssetPath = $"Packages/com.readyplayerme.core/Settings/{AVATAR_LOADER_ASSET_NAME}";
    public const string CONFIG_ASSET_FOLDER = "Packages/com.readyplayerme.core/Resources";

#else
    private static readonly string ReadyPlayerMeAssetPath = $"Assets/Ready Player Me/Core/Settings/{SETTINGS_ASSET_NAME}";
    private static readonly string AvatarLoaderAssetPath = $"Assets/Ready Player Me/Core/Settings/{AVATAR_LOADER_ASSET_NAME}";
    private const string CONFIG_ASSET_FOLDER = "Assets/Ready Player Me/Core/Resources";
#endif

    private static readonly string[] ConfigNames = {  "Avatar Config Medium", "Avatar Config Low", "Avatar Config High" };

    
    public static ReadyPlayerMeSettings LoadReadyPlayerMeSettings()
    {
        var absolutePath = $"{Application.dataPath}/{SettingsSaveFolder}/{SETTINGS_ASSET_NAME}";
        if (File.Exists(absolutePath))
        {
            return AssetDatabase.LoadAssetAtPath<ReadyPlayerMeSettings>(ReadyPlayerMeAssetPath);
        }
        return CreateReadyPlayerMeSettings();
    }
    
    private static ReadyPlayerMeSettings CreateReadyPlayerMeSettings()
    {
        var defaultSettings = AssetDatabase.LoadAssetAtPath<ReadyPlayerMeSettings>(ReadyPlayerMeAssetPath);
        ReadyPlayerMeSettings newSettings = ScriptableObject.CreateInstance<ReadyPlayerMeSettings>();
        newSettings.partnerSubdomain = defaultSettings.partnerSubdomain;
        var loaderSettings = LoadAvatarLoaderSettings();
        newSettings.AvatarLoaderSettings = loaderSettings;
        
        DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{SettingsSaveFolder}");
        
        AssetDatabase.CreateAsset(newSettings, $"Assets/{SettingsSaveFolder}/{SETTINGS_ASSET_NAME}");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return newSettings;
    }
    
    public static AvatarLoaderSettings LoadAvatarLoaderSettings()
    {
        var absolutePath = $"{Application.dataPath}/{SettingsSaveFolder}/{SETTINGS_ASSET_NAME}";
        if (File.Exists(absolutePath))
        {
            return AssetDatabase.LoadAssetAtPath<AvatarLoaderSettings>(AvatarLoaderAssetPath);
        }
        return CreateAvatarLoaderSettings();
    }
    
    private static AvatarLoaderSettings CreateAvatarLoaderSettings()
    {
        var defaultSettings = AssetDatabase.LoadAssetAtPath<AvatarLoaderSettings>(AvatarLoaderAssetPath);
        var newSettings = ScriptableObject.CreateInstance<AvatarLoaderSettings>();
        newSettings.AvatarConfig = defaultSettings.AvatarConfig;
        newSettings.AvatarCachingEnabled = defaultSettings.AvatarCachingEnabled;
        
        DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{SettingsSaveFolder}");
        
        AssetDatabase.CreateAsset(newSettings, $"Assets/{SettingsSaveFolder}/{AVATAR_LOADER_ASSET_NAME}");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return newSettings;
    }
    
    public static void CreateAvatarConfigAssets()
    {
        foreach (var configName in ConfigNames)
        {
            var absolutePath = $"{Application.dataPath}/{CONFIG_SAVE_FOLDER}/{configName}";
            if (File.Exists(absolutePath))
            {
                return;
            }
            DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{CONFIG_SAVE_FOLDER}");
            
            var defaultSettings = AssetDatabase.LoadAssetAtPath<AvatarConfig>($"{CONFIG_ASSET_FOLDER}/{configName}.asset");
            var newSettings = CopyAvatarConfig(defaultSettings);
            
            AssetDatabase.CreateAsset(newSettings, $"Assets/{CONFIG_SAVE_FOLDER}/{configName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private static AvatarConfig CopyAvatarConfig(AvatarConfig sourceConfig)
    {
        var newSettings = ScriptableObject.CreateInstance<AvatarConfig>();
        newSettings.Pose = sourceConfig.Pose;
        newSettings.MeshLod = sourceConfig.MeshLod;
        newSettings.MorphTargets = sourceConfig.MorphTargets;
        newSettings.UseHands = sourceConfig.UseHands;
        newSettings.TextureSizeLimit = sourceConfig.TextureSizeLimit;
        return newSettings;
    }
}
