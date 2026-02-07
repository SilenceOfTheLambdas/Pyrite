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
    public class WeaponStats : ItemStats
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

        [Header("Crit Stats")]
        public float critMultiplier;

        public float criticalDamageChance;

        /// <summary>
        /// A generated list of affixes that apply to this weapon, this may be empty.
        /// </summary>
        [Header("Affixes")] 
        public List<ItemTemplate.Postfix> generatedAffixes = new();

        /// <summary>
        /// Generates the stats for all weapon types.
        /// </summary>
        /// <param name="weaponTemplate">The weapon baseWeaponStats to use.</param>
        public void GenerateWeaponStats(WeaponTemplate weaponTemplate)
        {
            // Get the base values at level 1
            physicalDamage = Random.Range(weaponTemplate.baseWeaponStats.physicalDamage.min,
                weaponTemplate.baseWeaponStats.physicalDamage.max);
            
            // 1) First, we generate any elemental damage that may apply to this weapon
            GenerateAndAssignElementalDamage(weaponTemplate);
            
            // 2) Then we assign basic stats like attack speed, range, crit multiplier and critical damage chance.
            // These values are not scaled.
            attackSpeed = weaponTemplate.baseWeaponStats.attackSpeed;
            attackRange = weaponTemplate.baseWeaponStats.attackRange;
            critMultiplier = weaponTemplate.baseWeaponStats.criticalDamageMultiplier;
            criticalDamageChance = Random.Range(weaponTemplate.baseWeaponStats.criticalDamageChance.min,
                weaponTemplate.baseWeaponStats.criticalDamageChance.max);
            
            // 3) This is where any Postfixes are applied
            generatedAffixes = new List<ItemTemplate.Postfix>();
            switch (equipmentRarity)
            {
                case RpgManager.ItemRarity.Common: // No Postfixes applied
                    break;
                case RpgManager.ItemRarity.Uncommon:
                    GenerateAffixes(RpgManager.Instance.raritySettings[1].rarityAffixBonusRange.min,
                        RpgManager.Instance.raritySettings[1].rarityAffixBonusRange.max, weaponTemplate);
                    break;
                case RpgManager.ItemRarity.Rare:
                    GenerateAffixes(RpgManager.Instance.raritySettings[2].rarityAffixBonusRange.min,
                        RpgManager.Instance.raritySettings[2].rarityAffixBonusRange.max, weaponTemplate);
                    break;
                case RpgManager.ItemRarity.Epic:
                    GenerateAffixes(RpgManager.Instance.raritySettings[3].rarityAffixBonusRange.min,
                        RpgManager.Instance.raritySettings[3].rarityAffixBonusRange.max, weaponTemplate);
                    break;
                case RpgManager.ItemRarity.Unique:
                    GenerateAffixes(RpgManager.Instance.raritySettings[4].rarityAffixBonusRange.min,
                        RpgManager.Instance.raritySettings[4].rarityAffixBonusRange.max, weaponTemplate);
                    break;
                default:
                    Debug.LogError("Invalid Rarity");
                    break;
            }
            
            // 4) Now, we scale the physical and elemental damage after postfixes are generated as some values
            // are scaled by the generated postfixes.
            ScalePhysicalDamage();
            ScaleElementalDamage();
            
            // 5) Finally, generate the weapon equip requirements
            GenerateItemRequirements(weaponTemplate.baselineItemRequirements);
        }

        /// <summary>
        /// Here we generate any elemental damage based on the weapon template. This is done
        /// before any affixes are applied and only if the minimum amount of elemental damage is greater than 0.
        /// </summary>
        /// <param name="weaponTemplate">The weapon template scriptable object.</param>
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
            physicalDamage = (int)finalBase;
        }

        /// <summary>
        /// Applies extra elemental damage to a weapon based on the stats of the item, player tier and any affixes.
        /// </summary>
        private void ScaleElementalDamage()
        {
            // Setting the baseline
            var scaledStat = elementalDamage.amount * (1 + equipmentLevel * RpgManager.Instance.itemLevelFactor);
            var finalBase = scaledStat * RpgManager.Instance.raritySettings
                .Find(e => e.rarity == equipmentRarity).rarityMultiplier;
            elementalDamage.amount = (int)finalBase;

            // Now adjusting for any affixes that deal elemental damage.
            foreach (var affix in generatedAffixes)
            {
                if (affix.Type == ItemTemplate.Postfix.PostfixType.AddedElementalDamage)
                {
                    var tierStatIncrease = affix.Value;
                    elementalDamage.amount += (RpgManager.Instance.currentItemTier * tierStatIncrease) *
                                              RpgManager.Instance.itemLevelFactor;
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
            var tempListOfPossibleAffixes = new List<ItemTemplate.Postfix>();

            #region Generating Postfix Values based on Level Tier

            var hasGeneratedElementalDamage = false;
            foreach (var possibleAffix in weaponTemplate.possibleAffixes)
            {
                if (possibleAffix.Type == ItemTemplate.Postfix.PostfixType.IncreasedPhysicalDamage)
                {
                    var generatedPossibleAffix = possibleAffix;
                    generatedPossibleAffix.Type = possibleAffix.Type;
                    generatedPossibleAffix.Value = Random.Range(RpgManager.Instance
                        .itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange
                        .min.strength, RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1]
                        .tierStatsRange
                        .max.strength);
                    tempListOfPossibleAffixes.Add(generatedPossibleAffix);
                }

                if (possibleAffix.Type is ItemTemplate.Postfix.PostfixType.IncreasedCritChance
                    or ItemTemplate.Postfix.PostfixType.AddedDexterity)
                {
                    var generatedPossibleAffix = possibleAffix;
                    generatedPossibleAffix.Type = possibleAffix.Type;
                    generatedPossibleAffix.Value = Random.Range(RpgManager.Instance
                        .itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange
                        .min.dexterity, RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1]
                        .tierStatsRange
                        .max.dexterity);
                    tempListOfPossibleAffixes.Add(generatedPossibleAffix);
                }

                // We only want to generate one elemental damage affix.
                if (possibleAffix.Type is ItemTemplate.Postfix.PostfixType.AddedElementalDamage
                    && !hasGeneratedElementalDamage)
                {
                    var generatedPossibleAffix = possibleAffix;
                    generatedPossibleAffix.Type = possibleAffix.Type;
                    generatedPossibleAffix.Value = Random.Range(RpgManager.Instance
                            .itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange
                            .min.magic, RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1]
                            .tierStatsRange.max.magic);
                    tempListOfPossibleAffixes.Add(generatedPossibleAffix);
                    hasGeneratedElementalDamage = true;
                }
            }
            #endregion

            #region Actually assigning affixes to this generated weapon randomly


            for (var i = 0; i < randomNumberOfAffixes; i++)
            {
                var randomAffixIndex = Random.Range(0, tempListOfPossibleAffixes.Count);
                if (tempListOfPossibleAffixes.Count > 0)
                {
                    // Check if we have an elemental damage affix.
                    if (tempListOfPossibleAffixes[randomAffixIndex].Type ==
                        ItemTemplate.Postfix.PostfixType.AddedElementalDamage)
                    {
                        // If so, we choose a random element type to apply to the weapon
                        elementalDamage.type = SelectRandomElementalDamageType();
                    }

                    generatedAffixes.Add(tempListOfPossibleAffixes[randomAffixIndex]);
                    tempListOfPossibleAffixes.RemoveAt(randomAffixIndex);
                }
            }
            tempListOfPossibleAffixes.Clear();

            #endregion
        }

        /// <summary>
        /// Selects a random elemental damage type.
        /// </summary>
        /// <returns>A randomly chosen ElementalDamageType.</returns>
        private RpgManager.ElementalDamageType SelectRandomElementalDamageType()
        {
            var randomElementIndex = Random.Range(0, Enum.GetValues(typeof(RpgManager.ElementalDamageType))
                .Length);
            return (RpgManager.ElementalDamageType)randomElementIndex;
        }

        public string GenerateWeaponStatsDescription()
        {
            string itemDescription = "";

            var physicalDamageText = "Physical Dmg: " + physicalDamage;
            var attackSpeedText = $"Attack Speed: {attackSpeed:F1}";
            var attackRangeText = "Range: " + attackRange;
            var criticalDamageChanceText = $"Crit Chance: {criticalDamageChance:F1}%";
            var critMultiplierText = "Crit Multi: x" + critMultiplier;

            itemDescription += physicalDamageText + "\n"
                + attackSpeedText + "\n"
                + attackRangeText + "\n"
                + criticalDamageChanceText + "\n"
                + critMultiplierText + "\n";

            // Elemental Damage
            if (elementalDamage.amount > 0)
            {
                itemDescription += $"<color=#F1F06F>{elementalDamage.type} damage: ";
                itemDescription += $"<color=white>{elementalDamage.amount:F1}\n";
            }

            // Affixes
            if (generatedAffixes.Count <= 0) return itemDescription;
            itemDescription += "<color=grey><align=\"center\">________\n<align=\"left\"><size=60%>\n";
            foreach (var generatedAffix in generatedAffixes)
            {
                switch (generatedAffix.Type)
                {
                    case ItemTemplate.Postfix.PostfixType.AddedStrength:
                    case ItemTemplate.Postfix.PostfixType.AddedIntelligence:
                    case ItemTemplate.Postfix.PostfixType.AddedDexterity:
                    case ItemTemplate.Postfix.PostfixType.AddedHealth:
                    case ItemTemplate.Postfix.PostfixType.AddedElementalDamage:
                    case ItemTemplate.Postfix.PostfixType.AddedArmour:
                        itemDescription += $"Adds {generatedAffix.Value} to {generatedAffix.Type.ToString().Remove(0, 5)}\n";
                        break;
                    case ItemTemplate.Postfix.PostfixType.IncreasedPhysicalDamage:
                    case ItemTemplate.Postfix.PostfixType.IncreasedCritChance:
                    case ItemTemplate.Postfix.PostfixType.IncreasedFireResistance:
                    case ItemTemplate.Postfix.PostfixType.IncreasedIceResistance:
                    case ItemTemplate.Postfix.PostfixType.IncreasedLightningResistance:
                    case ItemTemplate.Postfix.PostfixType.IncreasedPoisonResistance:
                        itemDescription += $"Increased {generatedAffix.Type.ToString().Remove(0, 9)} by {generatedAffix.Value}%\n";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return itemDescription;
        }
    }
}