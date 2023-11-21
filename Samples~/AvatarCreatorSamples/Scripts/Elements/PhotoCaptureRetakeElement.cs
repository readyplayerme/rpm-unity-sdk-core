using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(PhotoCaptureElement))]
public class PhotoCaptureRetakeElement : MonoBehaviour
{
    [Header("Retake Buttons")]
    [SerializeField, Tooltip("Button for confirming the chosen photo")] private Button confirmPhotoButton;
    [SerializeField, Tooltip("Button for retaking the photo")] private Button retakePhotoButton;
    
    [Space(5)]
    [Header("Retake Events")]
    public UnityEvent<Texture2D> onPhotoCaptureConfirmed;

    private PhotoCaptureElement photoCaptureElement;
    private Texture2D selectedTexture;

    private void Awake()
    {
        photoCaptureElement = GetComponent<PhotoCaptureElement>();
        photoCaptureElement.onPhotoCaptured.AddListener(OnPhotoCaptured);
        retakePhotoButton.onClick.AddListener(RetakePhoto);
        confirmPhotoButton.onClick.AddListener(ConfirmPhoto);
    }
    
    private void OnDestroy()
    {
        photoCaptureElement.onPhotoCaptured.RemoveListener(OnPhotoCaptured);
        retakePhotoButton.onClick.RemoveListener(RetakePhoto);
        confirmPhotoButton.onClick.RemoveListener(ConfirmPhoto);
    }
    
    private void OnPhotoCaptured(Texture2D texture)
    {
        selectedTexture = texture;

        retakePhotoButton.gameObject.SetActive(true);
        confirmPhotoButton.gameObject.SetActive(true);
        photoCaptureElement.takePhotoButton.gameObject.SetActive(false);
        
        photoCaptureElement.StopCamera();
    }

    private void RetakePhoto()
    {
        retakePhotoButton.gameObject.SetActive(false);
        confirmPhotoButton.gameObject.SetActive(false);
        photoCaptureElement.takePhotoButton.gameObject.SetActive(true);

        photoCaptureElement.StartCamera();
    }
    
    private void ConfirmPhoto()
    {
        onPhotoCaptureConfirmed?.Invoke(selectedTexture);
    }
}
