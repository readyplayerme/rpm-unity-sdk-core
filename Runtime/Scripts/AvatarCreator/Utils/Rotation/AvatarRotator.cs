using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AvatarRotator : MonoBehaviour
    {
        private const string TAG = nameof(AvatarRotator);
        [SerializeField] private float speed = 50;

        private IAvatarRotatorInput avatarRotatorInput;

        private void Awake()
        {
            avatarRotatorInput = GetComponent<IAvatarRotatorInput>();
        }

        public void ResetRotation()
        {
            transform.rotation = Quaternion.identity;
        }

        private void Update()
        {
            if (avatarRotatorInput == null || !avatarRotatorInput.IsInputDetected())
            {
                return;
            }

            var rotationAmount = avatarRotatorInput.GetRotationAmount();
            transform.Rotate(Vector3.up, rotationAmount * Time.deltaTime * speed);
        }
    }
}
