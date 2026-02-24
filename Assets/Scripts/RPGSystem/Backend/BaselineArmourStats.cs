using System;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using Void = EditorAttributes.Void;

namespace RPGSystem.Backend
{
    /// <summary>
    /// Represents the baseline stats of an armour item at level 1. This should be used to create armour base types e.g. leather armour.
    /// </summary>
    [Serializable]
    public class BaselineArmourStats
    {
        [SerializeField, HorizontalGroup(drawInBox: true,nameof(physicalArmour), nameof(magicalArmour))]
        private Void armourGroupHolder;
        [HideProperty, HideInInspector] public int physicalArmour;
        [HideProperty, HideInInspector] public int magicalArmour;

        [SerializeField] [DataTable(true)]
        public RpgManager.CorePlayerStats statBonuses;

        [NonSerialized] public List<ItemTemplate.Suffix> GeneratedAffixes;
        
        public static BaselineArmourStats operator +(BaselineArmourStats a, BaselineArmourStats b)
        {
            var combined = new BaselineArmourStats
            {
                physicalArmour = a.physicalArmour + b.physicalArmour,
                magicalArmour = a.magicalArmour + b.magicalArmour,
                statBonuses = a.statBonuses + b.statBonuses
            };

            return combined;
        }
        
        public static BaselineArmourStats operator -(BaselineArmourStats a, BaselineArmourStats b)
        {
            var combined = new BaselineArmourStats
            {
                physicalArmour = a.physicalArmour - b.physicalArmour,
                magicalArmour = a.magicalArmour - b.magicalArmour,
                statBonuses = a.statBonuses - b.statBonuses
            };

            return combined;
        }

        /// <summary>
        /// Creates a deep copy of this BaselineArmourStats instance.
        /// </summary>
        public BaselineArmourStats DeepCopy()
        {
            var copy = new BaselineArmourStats
            {
                physicalArmour = physicalArmour,
                magicalArmour = magicalArmour,
                statBonuses = statBonuses
            };

            return copy;
        }
        
        [Serializable]
        public struct ElementalResistance : IEquatable<ElementalResistance>
        {
            /// <summary>
            /// The type of damage this armour resists.
            /// </summary>
            [SerializeField] public RpgManager.ElementalDamageType damageType;

            [Range(0, 100)] [SerializeField] public float resistancePercentage;
            
            public static ElementalResistance operator +(ElementalResistance a, ElementalResistance b)
            {
                if (a.damageType == b.damageType)
                    return new ElementalResistance
                    {
                        // damageType = a.damageType,
                        resistancePercentage = a.resistancePercentage + b.resistancePercentage
                    };
                throw new InvalidOperationException("Cannot add two ElementalResistances of different damage types.");
            }

            public bool Equals(ElementalResistance other)
            {
                return damageType == other.damageType;
            }

            public override bool Equals(object obj)
            {
                return obj is ElementalResistance other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine((int)damageType, resistancePercentage);
            }
        }
    }
}