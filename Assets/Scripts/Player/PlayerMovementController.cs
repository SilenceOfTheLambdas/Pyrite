using Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private MouseLookController mouseLookController;
        [SerializeField] private Transform movementCamera;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationLerpSpeed = 12f;

        private Vector3 _movementBasisForward;
        private Vector3 _movementBasisRight;
        private bool _hasMovementBasis;
        
        private void Awake()
        {
            _camera = Camera.main;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            
            if (movementCamera == null && _camera != null)
                movementCamera = _camera.transform;
        }

        private void Start()
        {
            if (_navMeshAgent == null) Debug.LogError("Could not find NavMeshAgent attached to player game object!");
            if (_animator == null) Debug.LogError("Could not find Animator component attached to player game object!");

            // Use manual rotation so we can face the move direction smoothly
            _navMeshAgent.updateRotation = false;

            // Cache the action from the reference if provided via inspector
            if (moveInputAction != null)
                _moveAction = moveInputAction.action != null
                    ? moveInputAction.action
                    : moveInputAction.asset.FindActionMap("Player").FindAction("Move");
        }

        private void OnEnable()
        {
            _moveAction?.Enable();
        }

        private void OnDisable()
        {
            _moveAction?.Disable();
        }

        private void Update()
        {
            Vector2 moveInput = _moveAction.ReadValue<Vector2>();
            
            bool isMouseLooking = mouseLookController != null && mouseLookController.IsCameraLooking;
            
            HandleMovement(moveInput, isMouseLooking);
        }
        
        private void HandleMovement(Vector2 moveInput, bool isMouseLooking)
        {
            if (moveInput.sqrMagnitude < 0.001f)
                return;


            Vector3 moveDirection = isMouseLooking
                ? GetPlayerRelativeMoveDirection(moveInput)
                : GetCameraRelativeMoveDirection(moveInput);

            if (moveDirection.sqrMagnitude < 0.001f)
                return;
            
            if (moveDirection.sqrMagnitude > 1f)
                moveDirection.Normalize();
            
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            
            if (!isMouseLooking)
                FaceMovementDirection(moveDirection);
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
        
        private NavMeshAgent _navMeshAgent;
        private Camera _camera;
        private Animator _animator;
        private InputAction _moveAction;
        private Vector3 _lastPosition;
        public InputActionReference moveInputAction;

        private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    }
}