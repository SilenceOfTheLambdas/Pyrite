using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private void Awake()
        {
            _camera = Camera.main;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Assert.IsNotNull(_navMeshAgent, "Could not find NavMeshAgent attached to player game object.");
            Assert.IsNotNull(_animator, "Player needs to have an Animator component attached.");

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
            #region Player Movement

            if (_moveAction != null)
            {
                var input = _moveAction.ReadValue<Vector2>();
                MovePlayer(input);
            }

            // Update Animation based on current movement velocity
            var speed = (transform.position - _lastPosition).magnitude / Time.deltaTime;
            _animator.SetFloat(MovementSpeed, speed, 0.1f, Time.deltaTime);
            _lastPosition = transform.position;

            #endregion
        }

        private void MovePlayer(Vector2 input)
        {
            // Convert input (WASD) to a world-space direction relative to the camera
            var camForward = Vector3.forward;
            var camRight = Vector3.right;
            if (_camera != null)
            {
                camForward = _camera.transform.forward;
                camForward.y = 0f;
                camForward.Normalize();
                camRight = _camera.transform.right;
                camRight.y = 0f;
                camRight.Normalize();
            }

            var moveDir = camForward * input.y + camRight * input.x;

            // Apply movement
            if (moveDir.sqrMagnitude > 0.0001f)
            {
                var displacement = moveDir.normalized * moveSpeed * Time.deltaTime;
                _navMeshAgent.Move(displacement);

                // Smoothly rotate to face movement direction
                var targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRot, rotationLerpSpeed * Time.deltaTime);
            }
        }

        private NavMeshAgent _navMeshAgent;
        private Camera _camera;
        private Animator _animator;
        private InputAction _moveAction;
        private Vector3 _lastPosition;
        [SerializeField] private InputActionReference moveInputAction;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationLerpSpeed = 12f;

        private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    }
}