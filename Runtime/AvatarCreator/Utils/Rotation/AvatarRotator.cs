using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AvatarRotator : MonoBehaviour
    {
        private const string TAG = nameof(AvatarRotator);
        
        [SerializeField] private GameObject avatar;
        [SerializeField] private float speed = 50;
        [SerializeField] private bool rememberLastRotation = true;

        private IAvatarRotatorInput avatarRotatorInput;
        private Quaternion lastRotation;

        private void Awake()
        {
            avatarRotatorInput = GetComponent<IAvatarRotatorInput>();
            if (avatar == null)
            {
                SDKLogger.LogWarning(TAG, $"Avatar object not set for {gameObject.name}.");
            }
        }

        public void SetAvatar(GameObject newAvatar)
        {
            avatar = newAvatar;
            if (rememberLastRotation && avatar != null)
            {
                avatar.transform.rotation = lastRotation;
            }
        }

        private void Update()
        {
            if (avatar == null || avatarRotatorInput == null || !avatarRotatorInput.IsInputDetected())
            {
                return;
            }

            var rotationAmount = avatarRotatorInput.GetRotationAmount();
            avatar.transform.Rotate(Vector3.up, rotationAmount * Time.deltaTime * speed);
            lastRotation = avatar.transform.rotation;
        }
    }
}
