using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private InputAction zoomInputAction;
        [SerializeField] private float offsetX = 0f;
        [SerializeField] private float offsetY = 5f;
        [SerializeField] private float offsetZ = -10f;
        [SerializeField] private float maxCameraDistance = 15f;
        [SerializeField] private float minCameraDistance = 3f;
        [SerializeField] private float cameraFollowSpeed = 5f;
        [SerializeField] private float cameraZoomSpeed = 2f;

        private Camera _camera;
        private float _currentDistance = 8f;

        private void OnEnable()
        {
            zoomInputAction.Enable();
        }

        private void OnDisable()
        {
            zoomInputAction.Disable();
        }

        private void Start()
        {
            _camera = Camera.main;
            // Initialize the current distance based on the initial offset
            _currentDistance = Mathf.Sqrt(offsetX * offsetX + offsetY * offsetY + offsetZ * offsetZ);
            _currentDistance = Mathf.Clamp(_currentDistance, minCameraDistance, maxCameraDistance);
            _currentDistance = maxCameraDistance;
        }

        private void Update()
        {
            if (cameraTarget == null) return;

            // Handle zoom input
            var zoomInput = zoomInputAction.ReadValue<float>();
            _currentDistance += zoomInput * cameraZoomSpeed * Time.deltaTime;
            _currentDistance = Mathf.Clamp(_currentDistance, minCameraDistance, maxCameraDistance);

            // Calculate the desired position with the offset, maintaining the camera's own rotation
            var offset = new Vector3(offsetX, offsetY, offsetZ).normalized * _currentDistance;
            var desiredPosition = cameraTarget.position + offset;

            // Smoothly move the camera to the desired position
            _camera.transform.position = Vector3.Lerp(_camera.transform.position,
                desiredPosition, cameraFollowSpeed * Time.deltaTime);
        }

        public static bool TryGetClickedObject(
            Camera camera,
            LayerMask layerMask,
            out GameObject clickedObject,
            float maxDistance = 100f)
        {
            clickedObject = null;
            if (camera is null) return false;

            var mousePosition = Mouse.current.position.ReadValue();
            var ray = camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var hit, 20f, LayerMask.GetMask("Interactable"),
                    QueryTriggerInteraction.Ignore))
            {
                clickedObject = hit.collider.gameObject;
                return true;
            }

            return false;
        }

        public static bool IsMouseOverInteractable(Camera camera)
        {
            var mousePosition = Mouse.current.position.ReadValue();
            var ray = camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out _, 1000f, LayerMask.GetMask("Interactable"),
                    QueryTriggerInteraction.Ignore))
            {
                return true;
            }

            return false;
        }
    }
}