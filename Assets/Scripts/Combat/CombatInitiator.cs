using System;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class CombatInitiator : MonoBehaviour
    {
        public static event Action OnCombatStarted;

        private void Awake()
        {
            SphereCollider sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            
            Rigidbody body = GetComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // If the player enters the enemies area, engage combat
            if (!other.CompareTag("Player"))
                return;
            
            CombatManager.Instance.SetCurrentTarget(gameObject);
            // Initiate combat
            OnCombatStarted?.Invoke();
        }
    }
}