using RPGSystem.Equipment;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils;

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
        }

        private void PerformAttack(int AttackAnimationName)
        {
            if (!EquipmentManager.Instance.HasWeaponEquipped) return;
            // Check if the mouse pointer is hovered over a UI object
            if (EventSystem.current.IsPointerOverGameObject() || CameraController.IsMouseOverInteractable(Camera.main)) return;
            playerIsAttacking = true;
            _animator.SetTrigger(AttackAnimationName);
        }

        private void Update()
        {
            // Handle continuous / Held Attack
            if (attackInputAction.action.IsPressed())
            {
                if (_readyForFollowUpAttack)
                    PerformAttack(InwardSlash);
                else if (!playerIsAttacking)
                    PerformAttack(SwordSlash);
            }
            
            // Check if any movement inputs were activated
            if (_playerMovementController.moveInputAction.action.triggered)
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
            _readyForFollowUpAttack = false;
        }
    }
}
