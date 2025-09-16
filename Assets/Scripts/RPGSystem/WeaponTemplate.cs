using System;

namespace RPGSystem
{
    [Serializable]
    public class WeaponTemplate
    {
        public RpgManager.StatRange<int> physicalDamage;
        public RpgManager.StatRange<RpgManager.ElementalDamage> elementalDamage;
        public float attackSpeed;
        public float criticalDamageMultiplier;
        public RpgManager.StatRange<float> criticalDamageChance;
    }
}