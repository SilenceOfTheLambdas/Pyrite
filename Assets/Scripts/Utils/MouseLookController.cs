using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Utils
{
    public class MouseLookController : MonoBehaviour
    {
        [SerializeField] private InputActionReference mouseLookAction;
        [SerializeField] private InputActionReference cameraLookAction;

        public bool IsMouseLooking => _isMouseLooking;
        public bool IsCameraLooking => _isCameraLooking;
        public bool IsLooking => _isMouseLooking || _isCameraLooking;
        
        private bool _isMouseLooking;
        private bool _isCameraLooking;

        private void OnEnable()
        {
            mouseLookAction.action.Enable();
            cameraLookAction.action.Enable();

            mouseLookAction.action.started += OnMouseLookStarted;
            mouseLookAction.action.canceled += OnMouseLookCanceled;

            cameraLookAction.action.started += OnCameraLookStarted;
            cameraLookAction.action.canceled += OnCameraLookCanceled;
        }
        
        private void OnDisable()
        {
            mouseLookAction.action.started -= OnMouseLookStarted;
            mouseLookAction.action.canceled -= OnMouseLookCanceled;

            cameraLookAction.action.started -= OnCameraLookStarted;
            cameraLookAction.action.canceled -= OnCameraLookCanceled;
            
            mouseLookAction.action.Disable();
            cameraLookAction.action.Disable();

            SetCursorLookMode(false);
        }
        
        private void OnMouseLookStarted(InputAction.CallbackContext context)
        {
            if (IsPointerOverUi()) return;
            _isMouseLooking = true;
            SetCursorLookMode(true);
        }

        private void OnMouseLookCanceled(InputAction.CallbackContext context)
        {
            _isMouseLooking = false;

            if (!_isCameraLooking)
                SetCursorLookMode(false);
        }

        private void OnCameraLookStarted(InputAction.CallbackContext context)
        {
            if (IsPointerOverUi()) return;

            _isCameraLooking = true;
            SetCursorLookMode(true);
        }
        
        private void OnCameraLookCanceled(InputAction.CallbackContext context)
        {
            _isCameraLooking = false;

            if (!_isMouseLooking)
                SetCursorLookMode(false);
        }

        private static bool IsPointerOverUi()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        private static void SetCursorLookMode(bool isLooking)
        {
            Cursor.visible = !isLooking;
            Cursor.lockState = isLooking ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}