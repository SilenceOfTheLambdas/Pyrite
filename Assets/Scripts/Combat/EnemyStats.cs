using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class EnemyStats : MonoBehaviour
    {
        [Header("Health")] 
        public float maxHealth;

        [Header("Movement")]
        public float moveSpeed = 3.5f;
        public float rotationSpeed = 12f;

        [Header("Detection")]
        public float aggroRadius = 8f;
        public float attackRange = 2f;
        public float leashRadius = 20f;

        [Header("Attack")]
        public float physicalDamage = 8f;
        public RpgManager.ElementalDamage elementalDamage;
        public float attackCooldown = 1.5f;
        public float criticalChance = 0.05f;
        public float criticalDamageMultiplier = 1.5f;
    }
}