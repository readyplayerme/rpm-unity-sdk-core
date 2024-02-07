using System;
using ReadyPlayerMe.AvatarCreator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AvatarListItem : MonoBehaviour
{
    [SerializeField] private Button customizeAvatarButton;
    [SerializeField] private Button selectAvatarButton;
    [SerializeField] private Button deleteAvatarButton;
    public RawImage avatarImage;
    public UnityEvent onImageLoaded;

    /// <summary>
    /// Adds the listeners to the button's onClick event.
    /// </summary>
    /// <param name="customizeAvatar">A function to run when the customize button is clicked</param>
    /// <param name="selectAvatar">A function to run when the select button is clicked</param>
    /// <param name="deleteAvatar">A function to run when the delete button is clicked</param>
    public void AddListener(Action customizeAvatar, Action selectAvatar, Action deleteAvatar)
    {
        if (customizeAvatarButton != null)
        {
            customizeAvatarButton.onClick.AddListener(customizeAvatar.Invoke);
        }

        if (selectAvatarButton != null)
        {
            selectAvatarButton.onClick.AddListener(selectAvatar.Invoke);
        }

        if (deleteAvatarButton != null)
        {
            deleteAvatarButton.onClick.AddListener(deleteAvatar.Invoke);
        }
    }

    /// <summary>
    /// Sets the icon of the avatar component
    /// </summary>
    /// <param name="texture">The texture to be assigned to the RawImage component</param>
    /// <param name="sizeToParent">If true the icon will resize itself to fit inside the parent RectTransform</param>
    public async void SetIcon(string avatarId)
    {
        if (avatarImage == null)
        {
            return;
        }
        var texture = await AvatarRenderHelper.GetPortrait(avatarId);

        var avatarRectTransform = avatarImage.GetComponent<RectTransform>();

        var previousSize = avatarRectTransform.sizeDelta;
        avatarImage.texture = texture;
        avatarRectTransform.sizeDelta = previousSize;
        onImageLoaded?.Invoke();
    }
}
