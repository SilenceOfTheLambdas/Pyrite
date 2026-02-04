using System;
using UnityEngine;

namespace RPGSystem.Backend
{
    public class ItemTemplate : ScriptableObject
    {
        /// <summary>
        /// The name of the item
        /// </summary>
        public string itemName;

        public GameObject inventorySlotPrefab;

        public ItemType itemType;

        /// <summary>
        /// The base type of this item.
        /// </summary>
        public enum ItemType
        {
            Weapon,
            Armour,
            Accessory,
            Potion
        }

        [Serializable]
        public struct Affix
        {
            [field: SerializeField] public AffixType Type { get; set; }
            [field: SerializeField] public int Value { get; set; }
            public enum AffixType
            {
                /// e.g +0-10 Strength
                AddedStrength,
                /// e.g +0-10 Intelligence
                AddedIntelligence,
                /// e.g +0-10 Dexterity
                AddedDexterity,
                /// e.g +0-10 Vitality
                AddedHealth,
                /// e.g +0-10% extra physical damage
                IncreasedPhysicalDamage,
                /// e.g +0-10% extra crit chance
                IncreasedCritChance,
                /// e.g +0-10 Fire Damage
                AddedElementalDamage,
                /// e.g +0-10 Armour
                AddedArmour,
                /// e.g +0-10% Fire Resistance
                IncreasedFireResistance,
                /// e.g +0-10% Ice Resistance
                IncreasedIceResistance,
                /// e.g +0-10% Lightning Resistance
                IncreasedLightningResistance,
                /// e.g +0-10% Poison Resistance
                IncreasedPoisonResistance,
            }
        }
    }
}