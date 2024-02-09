using System;
using ReadyPlayerMe.AvatarCreator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserAvatarElement : MonoBehaviour
{
    [SerializeField]
    private AvatarElementButton[] buttonActions;
    public RawImage AvatarImage;
    public UnityEvent OnImageLoaded;

    /// <summary>
    /// Adds the listeners to the button's onClick event.
    /// </summary>
    /// <param name="avatarId">An avatar Id. This is needed to download the avatar image</param>
    /// <param name="action">An operator, that has a list of AvatarListItemAction actions, that can be listened</param>
    public void SetupButton(string avatarId, Action<AvatarListItemAction> action)
    {
        foreach (AvatarElementButton avatarElementAction in buttonActions)
        {
            avatarElementAction.button.onClick.AddListener(() => action?.Invoke(new AvatarListItemAction()
            {
                ActionType = avatarElementAction.actionType,
                AvatarId = avatarId
            }));
        }

        SetIcon(avatarId);
    }

    /// <summary>
    /// Sets the icon of the avatar component
    /// </summary>
    /// <param name="avatarId">ID of the avatar</param>
    private async void SetIcon(string avatarId)
    {
        if (AvatarImage == null)
        {
            return;
        }
        var texture = await AvatarRenderHelper.GetPortrait(avatarId);

        AvatarImage.texture = texture;
        OnImageLoaded?.Invoke();
    }

    public enum ButtonAction
    {
        Delete,
        Select,
        Customize
    };
    [Serializable]
    public struct AvatarElementButton
    {
        public Button button;
        public ButtonAction actionType;
    }
    public struct AvatarListItemAction
    {
        public ButtonAction ActionType;
        public string AvatarId;
    }
}
