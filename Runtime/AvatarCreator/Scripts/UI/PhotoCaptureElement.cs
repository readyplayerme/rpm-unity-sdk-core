using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif
using UnityEngine.Events;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    public class PhotoCaptureElement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private RawImage cameraTextureTarget;
        [SerializeField] private bool initializeOnEnable = true;

        [Space(5)]
        [Header("Events")]
        public UnityEvent<Texture2D> onPhotoCaptured;
        
        private WebCamTexture cameraTexture;
        private bool isInitialized;

        private int videoRotationAngle;
        private bool videoVerticallyMirrored;

        private CancellationTokenSource ctxSource;

        private void OnEnable()
        {
            if (initializeOnEnable)
            {
                StartCamera();
            }
        }

        private void OnDisable()
        {
            ctxSource?.Cancel();
            StopCamera();
        }

        public async void StartCamera()
        {
            await GetPermission();

            if (!isInitialized)
            {
                InitializeCamera();
            }
            
            if (cameraTexture != null && !cameraTexture.isPlaying)
            {
                cameraTexture.Play();

                var currentRotation = cameraTextureTarget.transform.rotation;
                cameraTextureTarget.transform.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, cameraTexture.videoRotationAngle);

                if (!cameraTexture.videoVerticallyMirrored)
                {
                    cameraTextureTarget.transform.localScale = new Vector3(-1, 1, 1);
                }
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

            onPhotoCaptured?.Invoke(texture);
        }

        private async Task GetPermission()
        {
            ctxSource?.Cancel();
            ctxSource = new CancellationTokenSource();

#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
            }

            while (!Permission.HasUserAuthorizedPermission(Permission.Camera) && !ctxSource.IsCancellationRequested)
            {
                await Task.Yield();
            }

#elif UNITY_IOS
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                var async = Application.RequestUserAuthorization(UserAuthorization.Microphone);
                while (!async.isDone && !ctxSource.IsCancellationRequested)
                {
                    await Task.Yield();
                }
            }
#endif

        }

        private void InitializeCamera()
        {
            var webCamDevice = GetWebCamDevice();
            SetupPhotoBoothTexture(webCamDevice?.name);
            isInitialized = true;
        }

        private void SetupPhotoBoothTexture(string textureName)
        {
            var size = cameraTextureTarget.rectTransform.sizeDelta;
            cameraTexture = new WebCamTexture(textureName, (int) size.x, (int) size.y);
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
}
