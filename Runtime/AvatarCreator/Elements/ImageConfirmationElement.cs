using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImageConfirmationElement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RawImage imageTextureTarget;
    
    [Header("Events")]
    public UnityEvent<Texture2D> onImageConfirmed;

    public void SetTexture(Texture2D texture)
    {
        imageTextureTarget.texture = texture;
    }

    public void ConfirmPhoto()
    {
        var texture = new Texture2D(imageTextureTarget.texture.width, imageTextureTarget.texture.height, TextureFormat.ARGB32, false);
        
        onImageConfirmed?.Invoke(texture);
    }
}
