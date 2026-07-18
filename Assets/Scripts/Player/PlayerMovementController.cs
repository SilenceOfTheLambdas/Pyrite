using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputActionReference jumpInputAction;
        [SerializeField] private MouseLookController mouseLookController;
        [SerializeField] private Transform movementCamera;
        
        [Header("Properties")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationLerpSpeed = 12f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float gravity = -9.81f;

        private Vector3 _movementBasisForward;
        private Vector3 _movementBasisRight;
        private bool _hasMovementBasis;
        private float _verticalVelocity = 0f;
        private Vector3 _airHorizontalVelocity = Vector3.zero;
        
        private CharacterController _characterController;
        private Camera _camera;
        private Animator _animator;
        private InputAction _moveAction;
        private Vector3 _lastPosition;
        public InputActionReference moveInputAction;

        private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
        
        private void Awake()
        {
            _camera = Camera.main;
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            
            if (movementCamera == null && _camera != null)
                movementCamera = _camera.transform;
        }

        private void Start()
        {
            if (_animator == null) Debug.LogError("Could not find Animator component attached to player game object!");
            if (_characterController == null)
                Debug.LogError("Could not find CharacterController component attached to player game object!");

            // Cache the action from the reference if provided via inspector
            if (moveInputAction != null)
                _moveAction = moveInputAction.action != null
                    ? moveInputAction.action
                    : moveInputAction.asset.FindActionMap("Player").FindAction("Move");
        }

        private void OnEnable()
        {
            _moveAction?.Enable();
            jumpInputAction.action.Enable();
        }

        private void OnDisable()
        {
            _moveAction?.Disable();
            jumpInputAction.action.Disable();
        }

        private void Update()
        {
            Vector2 moveInput = _moveAction.ReadValue<Vector2>();
            
            bool isMouseLooking = mouseLookController != null && mouseLookController.IsCameraLooking;
            
            HandleMovement(moveInput, isMouseLooking);
        }
        
        /// <summary>
        /// Handles movement based on the input and camera state.
        /// </summary>
        /// <param name="moveInput"></param>
        /// <param name="isMouseLooking"></param>
        private void HandleMovement(Vector2 moveInput, bool isMouseLooking)
        {
            if (_characterController == null) return;
            
            bool isGrounded = _characterController.isGrounded;

            Vector3 moveDirection = Vector3.zero;
            
            if (moveInput.sqrMagnitude >= 0.001f)
            {
                moveDirection = isMouseLooking
                    ? GetPlayerRelativeMoveDirection(moveInput)
                    : GetCameraRelativeMoveDirection(moveInput);
            
                if (moveDirection.sqrMagnitude > 1f)
                    moveDirection.Normalize();
            
            
                if (!isMouseLooking && moveDirection.sqrMagnitude > 0.001f)
                    FaceMovementDirection(moveDirection);
            }

            Vector3 horizontalMovement;

            if (isGrounded)
            {
                horizontalMovement = moveDirection * moveSpeed;
                
                if (moveDirection.sqrMagnitude >= 0.001f && !isMouseLooking)
                    FaceMovementDirection(moveDirection);
            }
            else
            {
                horizontalMovement = _airHorizontalVelocity;
            }

            if (isGrounded && _verticalVelocity < 0f)
                _verticalVelocity = -2f;
            
            if (isGrounded && jumpInputAction.action.WasPressedThisFrame())
            {
                _airHorizontalVelocity = horizontalMovement;
                _verticalVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }
            
            _verticalVelocity += gravity * Time.deltaTime;
            
            Vector3 verticalMovement = Vector3.up * _verticalVelocity;

            _characterController.Move((horizontalMovement + verticalMovement) * Time.deltaTime);
        }

        private Vector3 GetPlayerRelativeMoveDirection(Vector2 moveInput)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            
            return forward * moveInput.y + right * moveInput.x;
        }

        private Vector3 GetCameraRelativeMoveDirection(Vector2 moveInput)
        {
            if (movementCamera == null)
                return new Vector3(moveInput.x, 0f, moveInput.y);
            
            Vector3 forward = movementCamera.forward;
            Vector3 right = movementCamera.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            
            return forward * moveInput.y + right * moveInput.x;
        }

        private void FaceMovementDirection(Vector3 moveDirection)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                rotationLerpSpeed * Time.deltaTime
                );
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, 0.01f);
        }
    }
}