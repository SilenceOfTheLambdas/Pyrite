using RPGSystem.Equipment;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField] private InputActionReference attackInputAction;
        public bool playerIsAttacking;

        private void OnEnable()
        {
            attackInputAction.action.Enable();
        }

        private void OnDisable()
        {
            attackInputAction.action.Disable();
        }
    }
}
