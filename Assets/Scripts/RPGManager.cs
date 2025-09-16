using System;
using System.Collections.Generic;
using UnityEngine;

public class RpgManager : MonoBehaviour
{
    #region Singleton

    public static RpgManager Instance { get; private set; }
        
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;
    }

    #endregion

    [Header("ItemTemplate Tier Settings")]
    public List<ItemTier> itemTiers;

    [Header("Rarity Settings")]
    public ItemRaritySettings[] raritySettings = new ItemRaritySettings[5];

    #region Structs and Enums

    [Serializable]
    public struct ItemRaritySettings
    {
        public ItemRarity rarity;
        public StatRange<int> rarityAffixBonusRange;
    }
    
    [Serializable]
    public struct ItemTier
    {
        [Header("Tier Settings")]
        [Tooltip("Assign a given tier to a range of levels.")] public int itemTier;
        public StatRange<int> tierLevelRange;
        
        /// <summary>
        /// This list specifies a range of how much a stat ON ITEMS will increase per tier.
        /// </summary>
        public StatRange<CorePlayerStats> tierStatsRange;
    }
    
    [Serializable]
    public struct StatRange<T>
    {
        public T min;
        public T max;
    }
    
    [Serializable]
    public struct ElementalDamage
    {
        public int fire;
        public int ice;
        public int lightning;
        public int poison;
    }
    
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Unique
    }

    [Serializable]
    public struct CorePlayerStats
    {
        public int strength;
        public int dexterity;
        public int intelligence;
        public int vitality;
        public int magic;
        public int luck;
    }

    #endregion
}