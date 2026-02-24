using System;
using EditorAttributes;
using RPGSystem.Equipment;
using UnityEngine;

namespace RPGSystem.Backend
{
    public class ItemTemplate : ScriptableObject
    {
        [Title("Item Information")]
        public string itemName;
        public ItemType itemType;
        [SerializeField, Required] public GameObject inventorySlotPrefab;
        [AssetPreview(64f, 64f), Required] public GameObject itemPickupPrefab;
        
        [Title("Item Requirements"), DataTable(true)]
        public ItemRequirements baselineItemRequirements;

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

        private void Awake()
        {
            // Check to make sure required properties are set
            if (itemName == null || inventorySlotPrefab == null || itemPickupPrefab == null)
                Debug.LogError("Item template missing required properties!");
        }

        [Serializable]
        public struct Suffix
        {
            [field: SerializeField] public SuffixType Type { get; set; }
            [field: SerializeField] public int Value { get; set; }

            public enum SuffixType
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
                AddedElementalDamageToWeapon,

                /// e.g +0-10 Armour
                AddedArmour,

                /// e.g +0-10% Fire Resistance
                IncreasedFireResistance,

                /// e.g +0-10% Ice Resistance
                IncreasedIceResistance,

                /// e.g +0-10% Lightning Resistance
                IncreasedLightningResistance,

                /// e.g +0-10% Poison Resistance
                IncreasedPoisonResistance
            }
        }
    }
}