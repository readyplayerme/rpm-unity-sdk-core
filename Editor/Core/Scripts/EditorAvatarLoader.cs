using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Loader;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

public class EditorAvatarLoader
{
    private const string TAG = nameof(EditorAvatarLoader);

    private readonly bool avatarCachingEnabled;

    /// Scriptable Object Avatar API request parameters configuration
    public AvatarConfig AvatarConfig;

    /// Importer to use to import glTF
    public IImporter Importer;

    private string avatarUrl;
    private OperationExecutor<AvatarContext> executor;
    private float startTime;

    public Action<AvatarContext> OnCompleted;

    /// <summary>
    /// This class constructor is used to any required fields.
    /// </summary>
    /// <param name="useDefaultGLTFDeferAgent">Use default defer agent</param>
    public EditorAvatarLoader()
    {
        AvatarLoaderSettings loaderSettings = AvatarLoaderSettings.LoadSettings();
        Importer = new GltFastAvatarImporter();
        AvatarConfig = loaderSettings.AvatarConfig != null ? loaderSettings.AvatarConfig : null;
    }

    /// Set the timeout for download requests
    public int Timeout { get; set; } = 20;

    /// <summary>
    /// Runs through the process of loading the avatar and creating a game object via the <c>OperationExecutor</c>.
    /// </summary>
    /// <param name="url">The URL to the avatars .glb file.</param>
    public async Task<AvatarContext> Load(string url)
    {
        var context = new AvatarContext();
        context.Url = url;
        context.AvatarCachingEnabled = false;
        context.AvatarConfig = AvatarConfig;
        context.ParametersHash = AvatarCache.GetAvatarConfigurationHash(AvatarConfig);

        // process url
        var urlProcessor = new UrlProcessor();
        context = await urlProcessor.Execute(context, CancellationToken.None);
        // get metadata
        var metadataDownloader = new MetadataDownloader();
        context = await metadataDownloader.Execute(context, CancellationToken.None);
        //download avatar into asset folder
        context.AvatarUri.LocalModelPath = await DownloadAvatarModel(context.AvatarUri);
        if (string.IsNullOrEmpty(context.AvatarUri.LocalModelPath))
        {
            Debug.LogError($"Failed to download avatar model from {context.AvatarUri.ModelUrl}");
            return null;
        }
        // import model
        context.Bytes = await File.ReadAllBytesAsync(context.AvatarUri.LocalModelPath);
        context = await Importer.Execute(context, CancellationToken.None);
        // Process the avatar
        var avatarProcessor = new AvatarProcessor();
        context = await avatarProcessor.Execute(context, CancellationToken.None);

        var avatar = (GameObject) context.Data;
        avatar.SetActive(true);

        var avatarData = avatar.AddComponent<AvatarData>();
        avatarData.AvatarId = avatar.name;
        avatarData.AvatarMetadata = context.Metadata;
        OnCompleted?.Invoke(context);
        return context;
    }

    private static async Task<string> DownloadAvatarModel(AvatarUri avatarUri)
    {
        var folderPath = Path.Combine(Application.dataPath, $"Ready Player Me/Avatars/{avatarUri.Guid}");
        // Ensure the folder exists
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Create the full file path
        var fullPath = Path.Combine(folderPath, avatarUri.Guid + ".glb");

        // Start the download
        using (UnityWebRequest request = UnityWebRequest.Get(avatarUri.ModelUrl))
        {
            Debug.Log($"Downloading {avatarUri.ModelUrl}...");
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield(); // Await completion of the web request
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Write the downloaded data to the file
                await File.WriteAllBytesAsync(fullPath, request.downloadHandler.data);
                Debug.Log($"File saved to: {fullPath}");

                // Refresh the AssetDatabase to recognize the new file
                AssetDatabase.Refresh();
                Debug.Log("AssetDatabase refreshed.");
                return fullPath;
            }
            Debug.LogError($"Failed to download file: {request.error}");
            return null;
        }
    }

}
