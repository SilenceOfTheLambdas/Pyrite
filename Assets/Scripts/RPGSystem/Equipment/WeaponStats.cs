using System;
using System.Collections.Generic;
using RPGSystem.Backend;
using RPGSystem.Item_Definitions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPGSystem.Equipment
{
    /// <summary>
    /// The generated stats of a weapon equipment type.
    /// </summary>
    [Serializable]
    public abstract class WeaponStats : BaseItemInfo
    {
        /// <summary>
        /// The generated physical damage.
        /// </summary>
        public int physicalDamage;

        /// <summary>
        /// The type of elemental damage this weapon deals. A weapon can only ever do ONE type of elemental damage.
        /// </summary>
        public RpgManager.ElementalDamage elementalDamage;
        
        /// <summary>
        /// The generated attack speed.
        /// </summary>
        public float attackSpeed;
        
        /// <summary>
        /// The generated attack range.
        /// </summary>
        public float attackRange;
        
        /// <summary>
        /// A generated list of affixes that apply to this weapon, this may be empty.
        /// </summary>
        public List<ItemTemplate.Affix> generatedAffixes;

        /// <summary>
        /// Generates the stats for all weapon types.
        /// </summary>
        /// <param name="weaponTemplate">The weapon baseWeaponStats to use.</param>
        public void GenerateBaseWeaponStats(WeaponTemplate weaponTemplate)
        {
            GenerateBaseItemInfo(weaponTemplate);
            
            // Get the base values at level 1
            physicalDamage = Random.Range(weaponTemplate.baseWeaponStats.physicalDamage.min,
                weaponTemplate.baseWeaponStats.physicalDamage.max);
            GenerateAndAssignElementalDamage(weaponTemplate);
            attackSpeed = weaponTemplate.baseWeaponStats.attackSpeed;
            attackRange = weaponTemplate.baseWeaponStats.attackRange;
            
            // Generate affixes
            generatedAffixes = new List<ItemTemplate.Affix>();
            switch (equipmentRarity)
            {
                case RpgManager.ItemRarity.Common:
                    break;
                case RpgManager.ItemRarity.Uncommon:
                    GenerateAffixes(RpgManager.Instance.raritySettings[0].rarityAffixBonusRange.min,
                        RpgManager.Instance.raritySettings[0].rarityAffixBonusRange.max, weaponTemplate);
                    break;
                case RpgManager.ItemRarity.Rare:
                    GenerateAffixes(RpgManager.Instance.raritySettings[1].rarityAffixBonusRange.min,
                        RpgManager.Instance.raritySettings[1].rarityAffixBonusRange.max, weaponTemplate);
                    break;
                case RpgManager.ItemRarity.Epic:
                    GenerateAffixes(RpgManager.Instance.raritySettings[2].rarityAffixBonusRange.min,
                        RpgManager.Instance.raritySettings[2].rarityAffixBonusRange.max, weaponTemplate);
                    break;
                case RpgManager.ItemRarity.Unique:
                    GenerateAffixes(RpgManager.Instance.raritySettings[3].rarityAffixBonusRange.min,
                        RpgManager.Instance.raritySettings[3].rarityAffixBonusRange.max, weaponTemplate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // Scale the damage based on stats of the item, player and any affixes.
            ScalePhysicalDamage();
            ScaleElementalDamage();
            
            // Additional scaling is done after these, based on the player's attributes.
        }

        private void GenerateAndAssignElementalDamage(WeaponTemplate weaponTemplate)
        {
            var weaponTemplateElementalDamage = weaponTemplate.baseWeaponStats.elementalDamage;
            // First, check if this weapon template supports elemental damage
            if (weaponTemplateElementalDamage.min.amount > 0)
            {
                // If it does, then generate a random amount of damage based on the weapon template
                var damageAmount = Random.Range(weaponTemplateElementalDamage.min.amount,
                    weaponTemplateElementalDamage.max.amount);
                
                // Then assign the generated ElementalDamage of this weapon
                RpgManager.ElementalDamage generatedElementalDamage;
                generatedElementalDamage.type = weaponTemplateElementalDamage.min.type;
                generatedElementalDamage.amount = damageAmount;
                elementalDamage = generatedElementalDamage;
            }
        }
        
        private void ScalePhysicalDamage()
        {
            var scaledStat = physicalDamage * (1 + equipmentLevel * RpgManager.Instance.itemLevelFactor);
            var finalBase = scaledStat * RpgManager.Instance.raritySettings
                .Find(e => e.rarity == equipmentRarity).rarityMultiplier;
            physicalDamage = (int) finalBase;
        }

        private void ScaleElementalDamage()
        {
            if (!(elementalDamage.amount > 0)) return;
            
            // Setting the baseline
            var scaledStat = elementalDamage.amount * (1 + equipmentLevel * RpgManager.Instance.itemLevelFactor);
            var finalBase = scaledStat * RpgManager.Instance.raritySettings
                .Find(e => e.rarity == equipmentRarity).rarityMultiplier;
            elementalDamage.amount = (int) finalBase;
                
            // Now adjusting for any affixes that deal elemental damage.
            foreach (var affix in generatedAffixes)
            {
                if (affix.Type == ItemTemplate.Affix.AffixType.FireDamage)
                {
                    if (elementalDamage.type == RpgManager.ElementalDamageType.Fire)
                    {
                        Debug.Log("Trying to increase fire damage further.");
                        var tierStatIncrease = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange
                                .min.magic,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange
                                .max.magic);
                        elementalDamage.amount += (RpgManager.Instance.currentItemTier * tierStatIncrease) *
                                                  RpgManager.Instance.itemLevelFactor;
                    }
                }
            }
        }

        /// <summary>
        /// Generates a random number of affixes to apply to this weapon based on the rarity. Common items will have no affixes.
        /// </summary>
        /// <param name="rarityAffixBonusRangeMin">The minimum number of affixes that could apply to this item.</param>
        /// <param name="rarityAffixBonusRangeMax">The maximum number of affixes that could apply to this item.</param>
        /// <param name="weaponTemplate">A reference to the weapon template to use.</param>
        private void GenerateAffixes(int rarityAffixBonusRangeMin, int rarityAffixBonusRangeMax, WeaponTemplate weaponTemplate)
        {
            // Choose a random number of affixes at the start
            var randomNumberOfAffixes = Random.Range(rarityAffixBonusRangeMin, rarityAffixBonusRangeMax);
            var tempListOfPossibleAffixes = new List<ItemTemplate.Affix>(weaponTemplate.possibleAffixes);
            for (var i = 0; i < randomNumberOfAffixes; i++)
            {
                var randomAffixIndex = Random.Range(0, tempListOfPossibleAffixes.Count);
                
                // Here we are checking to see of we have any elemental damage that applies to this weapon.
                // If we do NOT have any elemental damage, then we can't apply any affixes that deal elemental damage.
                switch (elementalDamage.amount)
                {
                    case 0 when tempListOfPossibleAffixes[i].Type 
                                == ItemTemplate.Affix.AffixType.FireDamage:
                    case 0 when tempListOfPossibleAffixes[i].Type 
                                == ItemTemplate.Affix.AffixType.IceDamage:
                    case 0 when tempListOfPossibleAffixes[i].Type 
                                == ItemTemplate.Affix.AffixType.PoisonDamage:
                    case 0 when tempListOfPossibleAffixes[i].Type 
                                == ItemTemplate.Affix.AffixType.LightningDamage:
                        continue;
                    default:
                        generatedAffixes.Add(tempListOfPossibleAffixes[randomAffixIndex]);
                        tempListOfPossibleAffixes.RemoveAt(randomAffixIndex);
                        break;
                }
            }
            tempListOfPossibleAffixes.Clear();
        }
    }
}