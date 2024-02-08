using System;
using System.Linq;
using ReadyPlayerMe.AvatarCreator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AvatarListItem : MonoBehaviour
{
    [SerializeField]
    private AvatarElementButton[] buttonActions;
    public RawImage avatarImage;
    public UnityEvent onImageLoaded;

    /// <summary>
    /// Adds the listeners to the button's onClick event.
    /// </summary>
    /// <param name="avatarId">An avatar Id. This is needed to download the avatar image</param>
    /// <param name="actions">An operator, that has a list of AvatarListItem actions, that can be listened</param>
    public void SetupButton(string avatarId, params AvatarListItemAction[] actions)
    {
        foreach (AvatarListItemAction avatarElementAction in actions)
        {
            if (buttonActions.Any((buttonAction) => buttonAction.actionType == avatarElementAction.actionType))
            {
                var button = buttonActions.First((button => button.actionType == avatarElementAction.actionType)).button;
                button.onClick.AddListener(avatarElementAction.actionToPerform.Invoke);
            }
        }

        SetIcon(avatarId);
    }

    /// <summary>
    /// Sets the icon of the avatar component
    /// </summary>
    /// <param name="avatarId">ID of the avatar</param>
    private async void SetIcon(string avatarId)
    {
        if (avatarImage == null)
        {
            return;
        }
        var texture = await AvatarRenderHelper.GetPortrait(avatarId);

        avatarImage.texture = texture;
        onImageLoaded?.Invoke();
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
        public ButtonAction actionType;
        public Action actionToPerform;
    }
}
