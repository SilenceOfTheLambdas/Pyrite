using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    public class CameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private MouseLookController mouseLookController;
        
        [Header("Input")]
        [SerializeField] private InputActionReference lookDeltaAction;
        [SerializeField] private InputActionReference zoomInputAction;
        
        [Header("Camera")]
        [SerializeField] private float minCameraDistance = 3f;
        [SerializeField] private float maxCameraDistance = 15f;
        [SerializeField] private float offsetX = 0f;
        [SerializeField] private float offsetY = 5f;
        [SerializeField] private float offsetZ = -10f;
        [SerializeField] private float lookSensitivity = 0.15f;
        [SerializeField] private float cameraFollowSpeed = 5f;
        [SerializeField] private float cameraZoomSpeed = 2f;

        [Header("Pitch")]
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 70f;

        [Header("Player Rotation")]
        [SerializeField] private float playerTurnSpeed = 20f;
        
        /// <summary>
        /// Rotates around the player horizontally
        /// </summary>
        private float _cameraYaw;
        /// <summary>
        /// Rotates around the player vertically (up/down)
        /// </summary>
        private float _cameraPitch;
        
        private float _currentDistance = 8f;

        private void OnEnable()
        {
            lookDeltaAction.action.Enable();
            zoomInputAction.action.Enable();
        }

        private void OnDisable()
        {
            lookDeltaAction.action.Disable();
            zoomInputAction.action.Disable();
        }

        private void Start()
        {
            if (playerCamera == null) playerCamera = Camera.main;
            if (playerTransform != null) _cameraYaw = playerTransform.eulerAngles.y;
        }

        private void LateUpdate()
        {
            if (playerTransform == null || cameraTarget == null || playerCamera == null)
                return;

            HandleLookInput();
            HandleZoom();
            HandlePlayerRotation();
            UpdateCameraPosition();
        }

        private void HandleLookInput()
        {
            if (mouseLookController == null || !mouseLookController.IsLooking) return;
            
            Vector2 lookDelta = lookDeltaAction.action.ReadValue<Vector2>();
            _cameraYaw += lookDelta.x * lookSensitivity;
            _cameraPitch -= lookDelta.y * lookSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, minPitch, maxPitch);
        }

        private void HandleZoom()
        {
            float zoomInput = zoomInputAction.action.ReadValue<float>();
            
            if (Mathf.Approximately(zoomInput, 0f)) return;
            
            _currentDistance -= zoomInput * cameraZoomSpeed;
            _currentDistance = Mathf.Clamp(_currentDistance, minCameraDistance, maxCameraDistance);
        }

        private void HandlePlayerRotation()
        {
            if (mouseLookController == null || !mouseLookController.IsMouseLooking) return;
            if (playerTransform == null) return;
            
            Quaternion targetRotation = Quaternion.Euler(0f, _cameraYaw, 0f);
            playerTransform.rotation = Quaternion.Slerp(
                playerTransform.rotation,
                targetRotation,
                playerTurnSpeed * Time.deltaTime);
        }

        private void UpdateCameraPosition()
        {
            Quaternion cameraRotation = Quaternion.Euler(_cameraPitch, _cameraYaw, 0f);
            Vector3 desiredPosition = cameraTarget.position + cameraRotation * new Vector3(0f, 0f, -_currentDistance);

            playerCamera.transform.position = Vector3.Lerp(
                playerCamera.transform.position,
                desiredPosition,
                cameraFollowSpeed * Time.deltaTime);
            
            playerCamera.transform.LookAt(cameraTarget.position);
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

            if (Physics.Raycast(ray, out _, 1000f, LayerMask.GetMask("Interactable"))) return true;

            return false;
        }
    }
}