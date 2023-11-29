using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class MouseInput: MonoBehaviour, IMouseInput
    {
        public bool GetButtonDown(int index)
        {
            return Input.GetMouseButtonDown(index);
        }

        public bool GetButtonUp(int index)
        {
            return Input.GetMouseButtonUp(index);
        }

        public bool GetButtonPressed(int index)
        {
            return Input.GetMouseButton(index);
        }
    }
}
