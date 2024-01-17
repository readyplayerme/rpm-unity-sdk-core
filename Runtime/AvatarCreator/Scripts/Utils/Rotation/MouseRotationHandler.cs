using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class MouseRotationHandler : MonoBehaviour, IAvatarRotatorInput
    {
        private const int MOUSE_BUTTON_INDEX = 0;
        private float lastPosX;
        private bool rotate;

        public bool IsInputDetected()
        {
            if (Input.GetMouseButtonDown(MOUSE_BUTTON_INDEX))
            {
                lastPosX = Input.mousePosition.x;
                rotate = true;
            }
            else if (Input.GetMouseButtonUp(MOUSE_BUTTON_INDEX))
            {
                rotate = false;
            }

            return rotate;
        }

        public float GetRotationAmount()
        {
            var rotationAmount = lastPosX - Input.mousePosition.x;
            lastPosX = Input.mousePosition.x;
            return rotationAmount;
        }
    }
}
