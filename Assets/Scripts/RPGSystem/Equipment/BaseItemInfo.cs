using System;
using RPGSystem.Backend;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPGSystem.Equipment
{
    /// <summary>
    /// This class is used to store the base stats of an item.
    /// </summary>
    [Serializable]
    public abstract class BaseItemInfo : MonoBehaviour
    {
        /// <summary>
        /// The name of this equipment.
        /// </summary>
        public string equipmentName;
        
        /// <summary>
        /// The generated rarity of this equipment.
        /// </summary>
        public RpgManager.ItemRarity equipmentRarity;
        
        /// <summary>
        /// The generated level of this equipment.
        /// </summary>
        public int equipmentLevel;
        
        /// <summary>
        /// Does the player have this item equipped?
        /// </summary>
        public bool isEquipped;

        public ItemTemplate.ItemType itemType;
        
        /// <summary>
        /// If the player has this equipped, this is the slot it is equipped in.
        /// </summary>
        public EquipmentSlot equipmentSlot;
        
        public enum EquipmentSlot
        {
            MainHand,
            OffHand,
            Head,
            Body,
            Legs,
            Feet,
        }

        /// <summary>
        /// Generates and sets this item base stat. Such as; name, rarity, level, etc.
        /// </summary>
        /// <param name="itemTemplate">The baseWeaponStats to use</param>
        public void GenerateBaseItemInfo(ItemTemplate itemTemplate)
        {
            equipmentName = itemTemplate.itemName;
            itemType = itemTemplate.itemType;
            
            // TODO: Make rarities weighted!
            equipmentRarity = (RpgManager.ItemRarity) Random.Range(0, Enum.GetNames(typeof(RpgManager.ItemRarity)).Length);
            
            var maxLevel = RpgManager.Instance.PlayerRpgController.CurrentPlayerLevel + (int)equipmentRarity;
            equipmentLevel = Random.Range(RpgManager.Instance.PlayerRpgController.CurrentPlayerLevel, maxLevel);
        }

        public virtual void GenerateStats(ItemTemplate itemTemplate)
        {
        }
    }
}