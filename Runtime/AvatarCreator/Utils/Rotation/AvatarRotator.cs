using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AvatarRotator : MonoBehaviour
    {
        [SerializeField] private GameObject avatar;
        [SerializeField] private float speed = 50;
        [SerializeField] private bool rememberLastRotation = true;

        private IAvatarRotatorInput avatarRotatorInput;
        private Quaternion lastRotation;

        private void Awake()
        {
            avatarRotatorInput = GetComponent<IAvatarRotatorInput>();
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
