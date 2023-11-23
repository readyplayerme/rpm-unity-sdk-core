using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhotoCaptureElement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RawImage cameraTextureTarget;

    [Space(5)]
    [Header("Events")]
    public UnityEvent<Texture2D> onPhotoCaptured;

    private WebCamTexture cameraTexture;

    private void Awake()
    {
        InitializeCamera();
    }
    
    public void StartCamera()
    {
        if (cameraTexture != null && !cameraTexture.isPlaying)
        {
            cameraTexture.Play();
        }
    }

    public void StopCamera()
    {
        if (cameraTexture != null && cameraTexture.isPlaying)
        {
            cameraTexture.Stop();
        }
    }
    
    public void TakePhoto()
    {
        if (cameraTexture == null || !cameraTexture.isPlaying)
            return;

        var texture = new Texture2D(cameraTextureTarget.texture.width, cameraTextureTarget.texture.height, TextureFormat.ARGB32, false);
        texture.SetPixels(cameraTexture.GetPixels());
        texture.Apply();

        StopCamera();
        
        onPhotoCaptured?.Invoke(texture);
    }

    private void InitializeCamera()
    {
        var webCamDevice = GetWebCamDevice();

        SetupPhotoBoothTexture(webCamDevice?.name);
        StartCamera();
    }

    private void SetupPhotoBoothTexture(string textureName)
    {
        var size = cameraTextureTarget.rectTransform.sizeDelta;
        cameraTexture = new WebCamTexture(textureName, (int) size.x, (int) size.y);
        cameraTextureTarget.color = Color.white;
        cameraTextureTarget.texture = cameraTexture;
    }

    private static WebCamDevice? GetWebCamDevice()
    {
        var devices = WebCamTexture.devices;

        if (devices.Length == 0)
            return null;

        var webCamDevice = devices.FirstOrDefault(device => device.isFrontFacing);
        
        return webCamDevice.Equals(default(WebCamDevice)) ? devices[0] : webCamDevice;
    }
}
