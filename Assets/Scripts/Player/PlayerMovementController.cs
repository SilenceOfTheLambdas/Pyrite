using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private void Start()
        {
            _camera = Camera.main;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            Assert.IsNotNull(_navMeshAgent, "Could not find NavMeshAgent attached to player game object.");
            Assert.IsNotNull(_animator, "Player needs to have an Animator component attached.");
        }

        private void Update()
        {
            #region Player Movement

            // Check if the player is activating the movement button and the destination is not too close.
            if (Mouse.current.leftButton.isPressed 
                && Vector3.Distance(transform.position, GetMouseWorldPosition()) >= 1f)
            {
                MovePlayer();
            }
            
            // Update Animation
            _animator.SetFloat(MovementSpeed, _navMeshAgent.desiredVelocity.magnitude);

            #endregion
        }

        private void MovePlayer()
        {
            var targetPosition = GetMouseWorldPosition();
            _navMeshAgent.SetDestination(targetPosition);
        }

        private Vector3 GetMouseWorldPosition()
        {
            if (_camera != null)
            {
                // Ignore clicks that start over UI
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return transform.position;
                
                var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Walkable")))
                {
                    if (hit.collider.gameObject.CompareTag("Walkable"))
                    {
                        return hit.point;
                    }
                }
            }
            // Return a player's current position if we do not find a valid world point.
            return transform.position;
        }

        private NavMeshAgent _navMeshAgent;
        private Camera _camera;
        private Animator _animator;
        
        private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    }
}
