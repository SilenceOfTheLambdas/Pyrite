using RPGSystem.Equipment;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField] private InputActionReference attackInputAction;
        public bool playerIsAttacking;
        private Animator _animator;
        private bool _readyForFollowUpAttack;
        private PlayerMovementController _playerMovementController;
        
        private static readonly int SwordSlash = Animator.StringToHash("SwordSlash");
        private static readonly int InwardSlash = Animator.StringToHash("InwardSlash");
        private static readonly int InterruptAttack = Animator.StringToHash("InterruptAttack");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerMovementController = GetComponent<PlayerMovementController>();
            if (_animator == null) Debug.LogError("Player is missing an Animator component!");
            if (attackInputAction != null)
            {
                attackInputAction.action.performed += PerformAttack;
            }
            else Debug.LogError("Attack Input action must be assigned!");
        }

        private void PerformAttack(InputAction.CallbackContext ctx)
        {
            if (!EquipmentManager.Instance.HasWeaponEquipped) return;
            if (_readyForFollowUpAttack) return;
            // Check if the mouse pointer is hovered over a UI object
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            playerIsAttacking = true;
            _animator.SetTrigger(SwordSlash);
        }

        private void Update()
        {
            if (_readyForFollowUpAttack && attackInputAction.action.triggered)
            {
                _animator.SetTrigger(InwardSlash);
            }
            
            // Check if any movement inputs were activated
            if (_playerMovementController.moveInputAction.action.IsPressed())
            {
                _readyForFollowUpAttack = false;
                _animator.SetTrigger(InterruptAttack);
            } else _animator.ResetTrigger(InterruptAttack);
        }

        private void OnEnable()
        {
            attackInputAction.action.Enable();
        }

        private void OnDisable()
        {
            attackInputAction.action.Disable();
        }

        public void StartFollowupAttackTimer() => _readyForFollowUpAttack = true;

        public void EndFollowupAttackTimer()
        {
            _readyForFollowUpAttack = false;
        }

        public void SetPlayerIdleState()
        {
            playerIsAttacking = false;
        }
    }
}
