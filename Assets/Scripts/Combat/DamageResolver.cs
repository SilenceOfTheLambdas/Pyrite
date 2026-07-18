
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Combat
{
    public static class DamageResolver
    {
        public static DamageInfo Resolve(DamageInfo damageInfo, Combatant targetCombatant)
        {
            float finalDamage = damageInfo.PhysicalDamage;

            if (damageInfo.CanCrit && Random.value <= damageInfo.CritChance)
            {
                damageInfo.WasCritical = true;
                finalDamage *= damageInfo.CritMultiplier;
            }

            finalDamage += ResolveElementalDamage(damageInfo.ElementalDamage, targetCombatant);
            finalDamage = Mathf.Max(0f, finalDamage);
            
            damageInfo.FinalDamage = finalDamage;
            return damageInfo;
        }

        public static float ResolveElementalDamage(RpgManager.ElementalDamage elementalDamage,
            Combatant targetCombatant)
        {
            if (elementalDamage.amount == 0f) return 0f;
            float totalElementalDamage = 0f;

            
            float resistancePercentage = GetResistancePercentage(targetCombatant, elementalDamage.type);
            float resistanceMultiplier = Mathf.Clamp01(1f - resistancePercentage / 100f);

            totalElementalDamage += elementalDamage.amount * resistanceMultiplier;
            

            return totalElementalDamage;
        }

        public static float GetResistancePercentage(Combatant targetCombatant,
            RpgManager.ElementalDamageType damageType)
        {
            PlayerRpgController playerRpgController = targetCombatant.GetComponent<PlayerRpgController>();

            if (playerRpgController == null || playerRpgController.currentElementalResistances == null)
                return 0f;

            foreach (var resistance in playerRpgController.currentElementalResistances)
            {
                if (resistance.damageType == damageType)
                    return resistance.resistancePercentage;
            }

            return 0f;
        }
    }
}