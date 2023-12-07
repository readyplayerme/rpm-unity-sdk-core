using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AvatarPreviewElement : MonoBehaviour
    {
        [SerializeField] private AvatarRotator avatarRotator;
        [Tooltip("If true the avatar will face directly at the camera"), SerializeField] private bool resetRotationOnPreview = true;
        private GameObject avatar;

        public void PreviewAvatar(GameObject newAvatar)
        {
            if (avatar != null)
            {
                Destroy(avatar);
            }

            avatar = newAvatar;
            avatar.transform.SetParent(avatarRotator.transform);
            avatar.transform.localPosition = Vector3.zero;
            avatar.transform.localRotation = Quaternion.identity;
            if (resetRotationOnPreview)
            {
                avatarRotator.ResetRotation();
            }
        }
    }
}
