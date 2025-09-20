using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

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

    private void Start()
    {
        Assert.IsNotNull(PlayerRpgController, "PlayerRpgController is not set.");
    }
    
    [Header("Current Item Tier of the Player")]
    public int currentItemTier;
    
    [Header("ItemTemplate Tier Settings")]
    public List<ItemTier> itemTiers;

    [Range(0f, 1f)]
    public float itemLevelFactor;

    [Header("Rarity Settings")]
    public List<ItemRaritySettings> raritySettings;
    
    [field: SerializeField] public PlayerRpgController PlayerRpgController { get; private set; }

    #region Structs and Enums

    [Serializable]
    public struct ItemRaritySettings
    {
        public ItemRarity rarity;
        public float rarityMultiplier;
        public StatRange<int> rarityAffixBonusRange;
    }
    
    [Serializable]
    public struct ItemTier
    {
        [Header("Tier Settings")]
        [Tooltip("Assign a given tier to a range of levels.")] 
        public int itemTier;
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
        public ElementalDamageType type;
        public float amount;
    }

    public enum ElementalDamageType
    {
        Fire,
        Ice,
        Lightning,
        Poison
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