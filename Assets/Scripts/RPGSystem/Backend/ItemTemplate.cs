using System;
using UnityEngine;

namespace RPGSystem.Backend
{
    public class ItemTemplate : ScriptableObject
    {
        /// <summary>
        /// The main prefab for this item, this is the prefab that will be instantiated upon pickup.
        /// </summary>
        [Header("ItemTemplate Info")]
        public GameObject mainItemPrefab;

        /// <summary>
        /// This is the prefab model shown on the floor when spawned in to the world.
        /// </summary>
        public GameObject itemPickupPrefab;
        
        /// <summary>
        /// The name of the item
        /// </summary>
        public string itemName;
        
        /// <summary>
        /// The size in width and height of the item slot.
        /// </summary>
        public Vector2Int itemSlotSize;

        public ItemType itemType;
        
        /// <summary>
        /// The base type of this item.
        /// </summary>
        public enum ItemType {
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
                Strength,
                /// e.g +0-10 Intelligence
                Intelligence,
                /// e.g +0-10 Dexterity
                Dexterity,
                /// e.g +0-10 Vitality
                Vitality,
                /// e.g +0-10 Magic
                Magic,
                /// e.g +0-10 Strength
                Luck,
                /// e.g +0-10% extra physical damage
                PhysicalDamagePercentage,
                /// e.g +0-10% extra crit chance
                CritChancePercentage,
                /// e.g +0-10 Fire Damage
                WeaponElementalDamage,
                /// e.g +0-10 Armour
                Armour,
                /// e.g +0-10% Fire Resistance
                FireResistancePercentage,
                /// e.g +0-10% Ice Resistance
                IceResistancePercentage,
                /// e.g +0-10% Lightning Resistance
                LightningResistancePercentage,
                /// e.g +0-10% Poison Resistance
                PoisonResistancePercentage,
            }
        }
    }
}