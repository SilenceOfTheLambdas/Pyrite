using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem.Backend
{
    /// <summary>
    /// Represents the baseline stats of an armour item at level 1. This should be used to create armour base types e.g. leather armour.
    /// </summary>
    [Serializable]
    public class BaselineArmourStats
    {
        [Header("Base Defenses")] [SerializeField]
        public int physicalArmour;
        public int magicalArmour;

        [Header("Resistances")] [SerializeField]
        // Allows you to assign specific resistances (e.g. 50% Fire Res)
        public List<ElementalResistance> elementalResistances;

        [Header("Stat Bonuses")] [SerializeField]
        public RpgManager.CorePlayerStats statBonuses;

        [NonSerialized]
        public List<ItemTemplate.Postfix> GeneratedPostfixes;

        [Serializable]
        public struct ElementalResistance
        {
            /// <summary>
            /// The type of damage this armour resists.
            /// </summary>
            [SerializeField]
            public RpgManager.ElementalDamageType damageType;
            [Range(0, 100)] [SerializeField]
            public float resistancePercentage;
        }

        /// <summary>
        /// Creates a deep copy of this BaselineArmourStats instance.
        /// </summary>
        public BaselineArmourStats DeepCopy()
        {
            var copy = new BaselineArmourStats
            {
                physicalArmour = this.physicalArmour,
                magicalArmour = this.magicalArmour,
                statBonuses = this.statBonuses,
                elementalResistances = new List<ElementalResistance>()
            };

            if (this.elementalResistances != null)
            {
                foreach (var resistance in this.elementalResistances)
                {
                    copy.elementalResistances.Add(resistance);
                }
            }

            return copy;
        }
    }
}
