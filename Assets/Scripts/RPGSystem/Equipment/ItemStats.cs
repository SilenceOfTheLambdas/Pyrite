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
    public abstract class ItemStats : MonoBehaviour
    {
        /// <summary>
        /// The name of this equipment.
        /// </summary>
        public string equipmentName;
        
        public GameObject inventorySlotPrefab;
        
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
        public void GenerateItemNameTypeAndLevel(ItemTemplate itemTemplate)
        {
            equipmentName = itemTemplate.itemName;
            itemType = itemTemplate.itemType;
            
            // generate a random number between 0 and 100 and then check if it is less than the rarity drop chance
            var randomWeight = Random.Range(0, 100);
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[0].rarityDropChance)
                equipmentRarity = RpgManager.ItemRarity.Common;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[1].rarityDropChance)
                equipmentRarity = RpgManager.ItemRarity.Uncommon;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[2].rarityDropChance)
                equipmentRarity = RpgManager.ItemRarity.Rare;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[3].rarityDropChance)
                equipmentRarity = RpgManager.ItemRarity.Epic;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[4].rarityDropChance)
                equipmentRarity = RpgManager.ItemRarity.Unique;
            
            var maxLevel = RpgManager.Instance.PlayerRpgController.CurrentPlayerLevel + (int)equipmentRarity;
            equipmentLevel = Random.Range(RpgManager.Instance.PlayerRpgController.CurrentPlayerLevel, maxLevel);
        }
        
        public void GenerateItemNameTypeAndLevel(ItemTemplate itemTemplate, RpgManager.ItemRarity itemRarity)
        {
            equipmentName = itemTemplate.itemName;
            itemType = itemTemplate.itemType;
            equipmentRarity = itemRarity;
            
            var maxLevel = RpgManager.Instance.PlayerRpgController.CurrentPlayerLevel + (int)equipmentRarity;
            equipmentLevel = Random.Range(RpgManager.Instance.PlayerRpgController.CurrentPlayerLevel, maxLevel);
        }
    }
}