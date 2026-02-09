using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.Assertions;

public class RpgManager : MonoBehaviour
{
    #region Singleton

    public static RpgManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;
    }

    #endregion

    private void Start()
    {
        Assert.IsNotNull(PlayerRpgController, "PlayerRpgController is not set.");

        // Making sure the rarity drop chance total is 100%.
        var itemRarityDropChanceTotal = raritySettings.Sum(raritySetting => raritySetting.rarityDropChance);
        // Assert.AreEqual(100f, itemRarityDropChanceTotal, "Rarity drop chance total must be 100%.");
    }

    [Header("Current Item Tier of the Player")]
    public int currentItemTier;

    [Header("ItemTemplate Tier Settings")] public List<ItemTier> itemTiers;

    [Range(0f, 1f)] public float itemLevelFactor;

    [Header("Rarity Settings")] public List<ItemRaritySettings> raritySettings;

    [field: SerializeField] public PlayerRpgController PlayerRpgController { get; private set; }

    #region Structs and Enums

    [Serializable]
    public struct ItemRaritySettings
    {
        public ItemRarity rarity;
        [Range(0f, 100f)] public float rarityDropChance;
        public float rarityMultiplier;
        public StatRange<int> rarityAffixBonusRange;
    }

    [Serializable]
    public struct ItemTier
    {
        [Header("Tier Settings")] [Tooltip("Assign a given tier to a range of levels.")]
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
    public struct ElementalDamage : IEquatable<ElementalDamage>
    {
        public ElementalDamageType type;

        [Tooltip("Set to 0 for elemental damage to NOT be applied.")]
        public float amount;

        public bool Equals(ElementalDamage other)
        {
            return type == other.type;
        }

        public override bool Equals(object obj)
        {
            return obj is ElementalDamage other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)type, amount);
        }
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
        
        public static CorePlayerStats operator +(CorePlayerStats a, CorePlayerStats b)
        {
            return new CorePlayerStats
            {
                strength = a.strength + b.strength,
                dexterity = a.dexterity + b.dexterity,
                intelligence = a.intelligence + b.intelligence,
                vitality = a.vitality + b.vitality,
                magic = a.magic + b.magic,
                luck = a.luck + b.luck
            };
        }
        
        public static CorePlayerStats operator -(CorePlayerStats a, CorePlayerStats b)
        {
            return new CorePlayerStats
            {
                strength = a.strength - b.strength,
                dexterity = a.dexterity - b.dexterity,
                intelligence = a.intelligence - b.intelligence,
                vitality = a.vitality - b.vitality,
                magic = a.magic - b.magic,
                luck = a.luck - b.luck
            };
        }
    }

    public enum AttributeType
    {
        Strength,
        Dexterity,
        Intelligence,
        Vitality,
        Magic,
        Luck
    }

    #endregion
}