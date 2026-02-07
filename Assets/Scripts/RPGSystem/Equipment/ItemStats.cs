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
        
        /// <summary>
        /// The prefab of the inventory slot this equipment will be placed in.
        /// </summary>
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
        
        public ItemRequirements itemRequirements;
        
        public enum EquipmentSlot
        {
            MainHand,
            OffHand,
            Head,
            Body,
            Legs,
            Feet
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

        /// <summary>
        /// Generates the item requirements needed for a player to equip this item,
        /// based on the provided template and the item's equipment level.
        /// </summary>
        /// <param name="templateItemRequirements">The template that contains base requirement values for level, strength, dexterity, and intelligence.</param>
        public void GenerateItemRequirements(ItemRequirements templateItemRequirements)
        {
            var template = templateItemRequirements;
            itemRequirements.playerLevelRequirement = equipmentLevel;
            itemRequirements.playerStrengthRequirement = (template.playerStrengthRequirement *= 1) + equipmentLevel;
            itemRequirements.playerDexterityRequirement = (template.playerDexterityRequirement *= 1) + equipmentLevel;
            itemRequirements.playerIntelligenceRequirement = (template.playerIntelligenceRequirement *= 1) + equipmentLevel;
        }
    }

    [Serializable]
    public struct ItemRequirements
    {
        [NonSerialized]
        public int playerLevelRequirement;
        public int playerStrengthRequirement;
        public int playerDexterityRequirement;
        public int playerIntelligenceRequirement;
    }
}