using System;
using System.Collections.Generic;
using EditorAttributes;
using RPGSystem.Backend;
using RPGSystem.Equipment;
using UnityEngine;
using Void = EditorAttributes.Void;

namespace Player
{
    public class PlayerRpgController : MonoBehaviour
    {
        #region Properties

        [TabGroup(nameof(healthXpLevel), nameof(damage), nameof(armour), nameof(attributes))]
        [SerializeField] private Void groupHolder;
        
        [VerticalGroup(nameof(CurrentPlayerLevel), nameof(CurrentPlayerExp), nameof(CurrentPlayerHealth), nameof(PlayerMaxHealth))]
        [SerializeField, HideInInspector] private Void healthXpLevel;
        [field: SerializeField, HideProperty] public int CurrentPlayerLevel { get; private set; }
        [field: SerializeField, HideProperty] public int CurrentPlayerExp { get; private set; }
        [field: SerializeField, HideProperty] public int CurrentPlayerHealth { get; private set; }
        [field: SerializeField, HideProperty] public int PlayerMaxHealth { get; private set; }
        
        // Player Damage Properties
        [VerticalGroup(nameof(currentPhysicalDamage), nameof(currentElementalDamage), nameof(currentAttackSpeed), 
            nameof(currentAttackRange), nameof(currentCriticalChance), nameof(currentCriticalDamageMultiplier))]
        [SerializeField, HideInInspector] private Void damage;
        [Title("Player Damage Stats")]
        [HideProperty] public float currentPhysicalDamage;
        [HideProperty] public List<RpgManager.ElementalDamage> currentElementalDamage;
        [HideProperty] public float currentAttackSpeed;
        [HideProperty] public float currentAttackRange;
        [HideProperty] public float currentCriticalChance;
        [HideProperty] public float currentCriticalDamageMultiplier;
        
        
        [VerticalGroup(nameof(totalPlayerArmourStats), nameof(currentElementalResistances))]
        [SerializeField, HideInInspector] private Void armour;
        [Title("Player Armour Stats")]
        [HideProperty] public BaselineArmourStats totalPlayerArmourStats;
        [HideProperty, DataTable(true)] public List<BaselineArmourStats.ElementalResistance> currentElementalResistances;
        
        [VerticalGroup(nameof(currentPlayerAttributes))]
        [SerializeField, HideInInspector] private Void attributes;
        [Title("Player Attributes")]
        [HideProperty, DataTable(true)] public PlayerStats currentPlayerAttributes;
        
        #endregion
        public static PlayerRpgController Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;

            // Setting player starting stats.
            CurrentPlayerExp = 0;
            CurrentPlayerHealth = 100;
            PlayerMaxHealth = 100;
            
            // Initialise the player's total armour stats with 0 values for all stats and 0% resistance for all elemental damage types.
            currentElementalResistances = new List<BaselineArmourStats.ElementalResistance>
            {
                new()
                {
                    damageType = RpgManager.ElementalDamageType.Fire,
                    resistancePercentage = 0
                },
                new()
                {
                    damageType = RpgManager.ElementalDamageType.Ice,
                    resistancePercentage = 0
                },
                new()
                {
                    damageType = RpgManager.ElementalDamageType.Lightning,
                    resistancePercentage = 0
                },
                new()
                {
                    damageType = RpgManager.ElementalDamageType.Poison,
                    resistancePercentage = 0
                }
            };
        }

        private void Start()
        {
            EquipmentManager.Instance.OnWeaponEquipped += IncreasePlayerStats;
            EquipmentManager.Instance.OnArmourEquipped += IncreasePlayerStats;
            
            EquipmentManager.Instance.OnWeaponUnequipped += DecreasePlayerStats;
            EquipmentManager.Instance.OnArmourUnequipped += DecreasePlayerStats;
        }
        
        private void IncreasePlayerStats(WeaponStats weaponStats)
        {
            currentPhysicalDamage = weaponStats.physicalDamage;
            currentElementalDamage.Add(weaponStats.elementalDamage);
            currentAttackSpeed = weaponStats.attackSpeed;
            currentAttackRange = weaponStats.attackRange;
            currentCriticalChance = weaponStats.criticalDamageChance;
            currentCriticalDamageMultiplier = weaponStats.critMultiplier;

            if (weaponStats.generatedAffixes == null) return;
            foreach (var affix in weaponStats.generatedAffixes)
            {
                switch (affix.Type)
                {
                    case ItemTemplate.Suffix.SuffixType.AddedStrength:
                        currentPlayerAttributes.strength += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedIntelligence:
                        currentPlayerAttributes.intelligence += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedDexterity:
                        currentPlayerAttributes.dexterity += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedHealth:
                        PlayerMaxHealth += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedPhysicalDamage:
                        currentPhysicalDamage += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedCritChance:
                        currentCriticalChance += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedElementalDamageToWeapon:
                    case ItemTemplate.Suffix.SuffixType.AddedArmour:
                    case ItemTemplate.Suffix.SuffixType.IncreasedFireResistance:
                    case ItemTemplate.Suffix.SuffixType.IncreasedIceResistance:
                    case ItemTemplate.Suffix.SuffixType.IncreasedLightningResistance:
                    case ItemTemplate.Suffix.SuffixType.IncreasedPoisonResistance:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void IncreasePlayerStats(ArmourStats armourStats)
        {
            var armourStatsCopy = armourStats.GeneratedArmourStats.DeepCopy();
            totalPlayerArmourStats += armourStatsCopy;

            if (armourStatsCopy.GeneratedAffixes == null) return;
            foreach (var affix in armourStatsCopy.GeneratedAffixes)
            {
                switch (affix.Type)
                {
                    case ItemTemplate.Suffix.SuffixType.AddedStrength:
                        currentPlayerAttributes.strength += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedIntelligence:
                        currentPlayerAttributes.intelligence += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedDexterity:
                        currentPlayerAttributes.dexterity += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedHealth:
                        PlayerMaxHealth += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedPhysicalDamage:
                        currentPhysicalDamage += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedCritChance:
                        currentCriticalChance += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedElementalDamageToWeapon:
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedArmour:
                        totalPlayerArmourStats.physicalArmour += affix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedFireResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Fire, affix.Value);
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedIceResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Ice, affix.Value);
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedLightningResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Lightning, affix.Value);
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedPoisonResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Poison, affix.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void DecreasePlayerStats(WeaponStats weaponStats)
        {
            currentPhysicalDamage -= weaponStats.physicalDamage;
            currentElementalDamage.Remove(weaponStats.elementalDamage);
            currentAttackSpeed -= weaponStats.attackSpeed;
            currentAttackRange -= weaponStats.attackRange;
            currentCriticalChance -= weaponStats.criticalDamageChance;
            currentCriticalDamageMultiplier -= weaponStats.critMultiplier;

            foreach (var postfix in weaponStats.generatedAffixes)
            {
                switch (postfix.Type)
                {
                    case ItemTemplate.Suffix.SuffixType.AddedStrength:
                        currentPlayerAttributes.strength -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedIntelligence:
                        currentPlayerAttributes.intelligence -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedDexterity:
                        currentPlayerAttributes.dexterity -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedHealth:
                        PlayerMaxHealth -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedPhysicalDamage:
                        currentPhysicalDamage -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedCritChance:
                        currentCriticalChance -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedElementalDamageToWeapon:
                    case ItemTemplate.Suffix.SuffixType.AddedArmour:
                    case ItemTemplate.Suffix.SuffixType.IncreasedFireResistance:
                    case ItemTemplate.Suffix.SuffixType.IncreasedIceResistance:
                    case ItemTemplate.Suffix.SuffixType.IncreasedLightningResistance:
                    case ItemTemplate.Suffix.SuffixType.IncreasedPoisonResistance:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void DecreasePlayerStats(ArmourStats armourStats)
        {
            var armourStatsCopy = armourStats.GeneratedArmourStats.DeepCopy();
            totalPlayerArmourStats -= armourStatsCopy;

            if (armourStatsCopy.GeneratedAffixes == null) return;
            foreach (var postfix in armourStatsCopy.GeneratedAffixes)
            {
                switch (postfix.Type)
                {
                    case ItemTemplate.Suffix.SuffixType.AddedStrength:
                        currentPlayerAttributes.strength -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedIntelligence:
                        currentPlayerAttributes.intelligence -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedDexterity:
                        currentPlayerAttributes.dexterity -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedHealth:
                        PlayerMaxHealth -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedPhysicalDamage:
                        currentPhysicalDamage -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedCritChance:
                        currentCriticalChance -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedElementalDamageToWeapon:
                        break;
                    case ItemTemplate.Suffix.SuffixType.AddedArmour:
                        totalPlayerArmourStats.physicalArmour -= postfix.Value;
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedFireResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Fire, -postfix.Value);
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedIceResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Ice, -postfix.Value);
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedLightningResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Lightning, -postfix.Value);
                        break;
                    case ItemTemplate.Suffix.SuffixType.IncreasedPoisonResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Poison, -postfix.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void IncreaseElementalResistance(RpgManager.ElementalDamageType damageType, float resistanceIncrease)
        {
            var resistance = currentElementalResistances.Find(r => r.damageType == damageType);
            resistance.resistancePercentage += resistanceIncrease;
        }

        [Serializable]
        public struct PlayerStats
        {
            public int strength;
            public int intelligence;
            public int dexterity;
            public int vitality;
            public int magic;
            public int luck;
        }
    }
}