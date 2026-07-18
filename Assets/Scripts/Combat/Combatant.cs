using System;
using UnityEngine;

namespace Combat
{
    public class Combatant : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] private Faction faction;

        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth = 100f;
        
        public Faction Faction => faction;
        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        public bool IsAlive => currentHealth > 0f;

        public event Action<Combatant> OnDeath;
        public event Action<Combatant, DamageInfo> OnDamageTaken;
        public event Action<Combatant, float> OnHealed;

        private void Awake()
        {
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }

        public void SetMaxHealth(float value, bool healToFull = true)
        {
            maxHealth = Mathf.Max(1f, value);

            currentHealth = healToFull ? maxHealth : Mathf.Clamp(currentHealth, 0f, maxHealth);
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (!IsAlive) return;

            DamageInfo resolveDamage = DamageResolver.Resolve(damageInfo, this);
            currentHealth -= resolveDamage.FinalDamage;
            currentHealth = Mathf.Max(0f, currentHealth);
            
            OnDamageTaken?.Invoke(this, resolveDamage);

            if (currentHealth <= 0f)
                Die();
        }

        public void Heal(float amount)
        {
            if (!IsAlive) return;

            float healAmount = Mathf.Max(0f, amount);
            currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
            
            OnHealed?.Invoke(this, healAmount);
        }

        private void Die()
        {
            OnDeath?.Invoke(this);
        }
    }
}