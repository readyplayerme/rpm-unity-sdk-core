using UnityEngine;
using UnityEngine.EventSystems;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AvatarMouseRotation: MonoBehaviour
    {
        private const int MOUSE_BUTTON_INDEX = 0;
        
        [SerializeField] private float speed = 50;

        private IMouseInput mouseInput;
        private float lastPosX;
        private bool rotate;

        private void Awake()
        {
            mouseInput = GetComponent<IMouseInput>();
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject() && !rotate)
            {
                return;
            }
            
            if (mouseInput.GetButtonDown(MOUSE_BUTTON_INDEX))
            {
                lastPosX = Input.mousePosition.x;
                rotate = true;
            }

            if (mouseInput.GetButtonUp(MOUSE_BUTTON_INDEX))
            {
                rotate = false;
            }
            
            if (mouseInput.GetButtonPressed(MOUSE_BUTTON_INDEX))
            {
                transform.Rotate(Vector3.up, (lastPosX - Input.mousePosition.x) * (Time.deltaTime * speed));
                lastPosX = Input.mousePosition.x;
            }
        }
    }
}
