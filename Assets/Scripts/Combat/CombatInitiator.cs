using System;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class CombatInitiator : MonoBehaviour
    {
        public static event Action OnCombatStarted;

        private void OnTriggerEnter(Collider other)
        {
            // If the player enters the enemies area, engage combat
            if (other.CompareTag("Player"))
                // Initiate combat
                OnCombatStarted?.Invoke();
        }
    }
}