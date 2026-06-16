using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Combatinitiator : MonoBehaviour
{
    public static event Action OnCombatStarted;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            // Initiate combat
            OnCombatStarted?.Invoke();
    }
}