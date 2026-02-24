using System;
using EditorAttributes;
using Player;
using RPGSystem.Backend;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPGSystem.Equipment
{
    /// <summary>
    /// This class is used to store the base stats of an item.
    /// </summary>
    [Serializable]
    public abstract class ItemStats
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

        public EquipmentSlot equipmentSlot;

        public ItemRequirements itemRequirements;

        public enum EquipmentSlot
        {
            MainHand,
            Head,
            Body,
            Gauntlets,
            Legs,
            Feet,
            Ring1,
            Ring2
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
            // Base formula: BaseRequirement + (ItemLevel × Multiplier × RarityFactor)
            var template = templateItemRequirements;
            var rarityMultiplier = RpgManager.Instance.raritySettings.Find(e => e.rarity == equipmentRarity)
                .rarityMultiplier;
            itemRequirements.playerLevelRequirement = equipmentLevel;

            itemRequirements.playerStrengthRequirement =
                Mathf.RoundToInt(template.playerStrengthRequirement + (equipmentLevel * 0.5f * rarityMultiplier));
            itemRequirements.playerDexterityRequirement = 
                Mathf.RoundToInt(template.playerDexterityRequirement + (equipmentLevel * 0.5f * rarityMultiplier));
            itemRequirements.playerIntelligenceRequirement =
                Mathf.RoundToInt(template.playerIntelligenceRequirement + (equipmentLevel * 0.5f * rarityMultiplier));
        }
        
        /// <summary>
        /// Checks if the player meets the requirements to use the item.
        /// </summary>
        /// <returns>
        /// True if the player's level and attributes satisfy the item's requirements, otherwise false.
        /// </returns>
        public bool CheckItemRequirements()
        {
            var player = PlayerRpgController.Instance;
            
            return player.CurrentPlayerLevel >= itemRequirements.playerLevelRequirement
                   && player.currentPlayerAttributes.strength >= itemRequirements.playerStrengthRequirement
                   && player.currentPlayerAttributes.dexterity >= itemRequirements.playerDexterityRequirement
                   && player.currentPlayerAttributes.intelligence >= itemRequirements.playerIntelligenceRequirement;
        }
    }

    [Serializable]
    public struct ItemRequirements
    {
        public int playerStrengthRequirement;
        public int playerDexterityRequirement;
        public int playerIntelligenceRequirement;
        [NonSerialized] public int playerLevelRequirement;
    }
}