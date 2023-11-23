using UnityEngine;
using UnityEngine.Events;

public class PhotoCaptureConfirmElement : MonoBehaviour
{
    [Space(5)]
    [Header("Events")]
    public UnityEvent<Texture2D> onPhotoCaptureConfirmed;
    
    private Texture2D selectedTexture;
    
    public void SetTexture(Texture2D texture)
    {
        selectedTexture = texture;
    }

    public void ConfirmPhoto()
    {
        onPhotoCaptureConfirmed?.Invoke(selectedTexture);
    }
}
