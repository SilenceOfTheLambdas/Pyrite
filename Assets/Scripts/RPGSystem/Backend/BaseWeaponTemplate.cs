using System;

namespace RPGSystem.Backend
{
    [Serializable]
    public class BaseWeaponTemplate
    {
        public RpgManager.StatRange<int> physicalDamage;
        public RpgManager.StatRange<RpgManager.ElementalDamage> elementalDamage;
        public float attackSpeed;
        public float attackRange;
        public float criticalDamageMultiplier;
        public RpgManager.StatRange<float> criticalDamageChance;
    }
}