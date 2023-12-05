using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class CameraFocuser : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Vector3 faceViewPoint = new Vector3(0f, 1.66f, 0.85f);
        [SerializeField] private Vector3 bodyViewPoint = new Vector3(0f, 1.4f, 2.6f);
        [SerializeField] private float defaultDuration = 1f;

        private Vector3 targetPosition;
        private Vector3 startPosition;
        private float transitionTime;
        private float duration;
        private bool isTransitioning;

        private void LateUpdate()
        {
            if (!isTransitioning) return;
            transitionTime += Time.deltaTime;
            if (transitionTime < duration)
            {
                var t = Mathf.SmoothStep(0.0f, 1.0f, transitionTime / duration);
                cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            }
            else
            {
                cameraTransform.position = targetPosition;
                isTransitioning = false;
            }
        }

        public void FocusOnFace()
        {
            StartTransition(faceViewPoint, defaultDuration);
        }

        public void FocusOnBody()
        {
            StartTransition(bodyViewPoint, defaultDuration);
        }

        public void StopTransition()
        {
            isTransitioning = false;
        }

        private void StartTransition(Vector3 newTargetLocalPosition, float transitionDuration)
        {
            startPosition = cameraTransform.position;
            targetPosition = transform.TransformPoint(newTargetLocalPosition);
            duration = transitionDuration;
            transitionTime = 0f;
            isTransitioning = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + faceViewPoint, 0.1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + bodyViewPoint, 0.1f);
        }
    }
}
