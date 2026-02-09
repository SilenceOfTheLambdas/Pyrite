using System;
using System.Collections.Generic;
using RPGSystem.Backend;
using RPGSystem.Equipment;
using UnityEngine;

namespace Player
{
    public class PlayerRpgController : MonoBehaviour
    {
        #region Properties

        [field: SerializeField]
        public int CurrentPlayerLevel { get; private set; }
        public int CurrentPlayerExp { get; private set; }
        public int CurrentPlayerHealth { get; private set; }
        public int PlayerMaxHealth { get; private set; }
        
        // Player Damage Properties
        public float currentPhysicalDamage;
        public List<RpgManager.ElementalDamage> currentElementalDamage;
        public float currentAttackSpeed;
        public float currentAttackRange;
        public float currentCriticalChance;
        public float currentCriticalDamageMultiplier;
        
        // Player Armour Properties
        public BaselineArmourStats totalPlayerArmourStats;
        public PlayerStats currentPlayerAttributes;
        public List<BaselineArmourStats.ElementalResistance> currentElementalResistances;
        
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
                    case ItemTemplate.Affix.PostfixType.AddedStrength:
                        currentPlayerAttributes.strength += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedIntelligence:
                        currentPlayerAttributes.intelligence += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedDexterity:
                        currentPlayerAttributes.dexterity += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedHealth:
                        PlayerMaxHealth += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedPhysicalDamage:
                        currentPhysicalDamage += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedCritChance:
                        currentCriticalChance += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedElementalDamageToWeapon:
                    case ItemTemplate.Affix.PostfixType.AddedArmour:
                    case ItemTemplate.Affix.PostfixType.IncreasedFireResistance:
                    case ItemTemplate.Affix.PostfixType.IncreasedIceResistance:
                    case ItemTemplate.Affix.PostfixType.IncreasedLightningResistance:
                    case ItemTemplate.Affix.PostfixType.IncreasedPoisonResistance:
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
                    case ItemTemplate.Affix.PostfixType.AddedStrength:
                        currentPlayerAttributes.strength += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedIntelligence:
                        currentPlayerAttributes.intelligence += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedDexterity:
                        currentPlayerAttributes.dexterity += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedHealth:
                        PlayerMaxHealth += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedPhysicalDamage:
                        currentPhysicalDamage += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedCritChance:
                        currentCriticalChance += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedElementalDamageToWeapon:
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedArmour:
                        totalPlayerArmourStats.physicalArmour += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedFireResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Fire, affix.Value);
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedIceResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Ice, affix.Value);
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedLightningResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Lightning, affix.Value);
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedPoisonResistance:
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
                    case ItemTemplate.Affix.PostfixType.AddedStrength:
                        currentPlayerAttributes.strength -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedIntelligence:
                        currentPlayerAttributes.intelligence -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedDexterity:
                        currentPlayerAttributes.dexterity -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedHealth:
                        PlayerMaxHealth -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedPhysicalDamage:
                        currentPhysicalDamage -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedCritChance:
                        currentCriticalChance -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedElementalDamageToWeapon:
                    case ItemTemplate.Affix.PostfixType.AddedArmour:
                    case ItemTemplate.Affix.PostfixType.IncreasedFireResistance:
                    case ItemTemplate.Affix.PostfixType.IncreasedIceResistance:
                    case ItemTemplate.Affix.PostfixType.IncreasedLightningResistance:
                    case ItemTemplate.Affix.PostfixType.IncreasedPoisonResistance:
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
                    case ItemTemplate.Affix.PostfixType.AddedStrength:
                        currentPlayerAttributes.strength -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedIntelligence:
                        currentPlayerAttributes.intelligence -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedDexterity:
                        currentPlayerAttributes.dexterity -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedHealth:
                        PlayerMaxHealth -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedPhysicalDamage:
                        currentPhysicalDamage -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedCritChance:
                        currentCriticalChance -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedElementalDamageToWeapon:
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedArmour:
                        totalPlayerArmourStats.physicalArmour -= postfix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedFireResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Fire, -postfix.Value);
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedIceResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Ice, -postfix.Value);
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedLightningResistance:
                        IncreaseElementalResistance(RpgManager.ElementalDamageType.Lightning, -postfix.Value);
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedPoisonResistance:
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