using System;
using System.Collections.Generic;
using RPGSystem.Backend;
using RPGSystem.Item_Definitions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPGSystem.Equipment
{
    [Serializable]
    public class ArmourStats : ItemStats
    {
        /// <summary>
        /// The generated armour stats based on the baseline stats given in the item template.
        /// </summary>
        [NonSerialized] public BaselineArmourStats GeneratedArmourStats;
        [NonSerialized] public List<BaselineArmourStats.ElementalResistance> ElementalResistances;

        public ArmourStats(BaselineArmourStats baselineArmourStats)
        {
            GeneratedArmourStats = baselineArmourStats;
        }

        [NonSerialized] public ArmourTemplate.ArmourType ArmourType;

        /// <summary>
        /// Generates armour stats using a formula similar to WeaponStats:
        /// 1) Roll a baseline by armour slot, 2) scale by level and rarity, 3) generate affixes by rarity/tier,
        /// 4) apply affix effects to armour/resistances/stats.
        /// </summary>
        public void GenerateArmourStats(ArmourTemplate.ArmourType typeOfArmourToGenerate, ArmourTemplate armourTemplate)
        {
            ArmourType = typeOfArmourToGenerate;

            // Map equipment slot from armour type
            equipmentSlot = typeOfArmourToGenerate switch
            {
                ArmourTemplate.ArmourType.Head => EquipmentSlot.Head,
                ArmourTemplate.ArmourType.Chest => EquipmentSlot.Body,
                ArmourTemplate.ArmourType.Legs => EquipmentSlot.Legs,
                ArmourTemplate.ArmourType.Boots => EquipmentSlot.Feet,
                ArmourTemplate.ArmourType.Gauntlets => EquipmentSlot.Gauntlets,
                _ => equipmentSlot
            };

            // Initialise resistances for all elements at 0 so we can safely add to them.
            InitializeElementalResistances();

            // 1) Scale by level and rarity
            ScaleArmourValues();

            // 2) Generate affixes from template/rarity
            GeneratedArmourStats.GeneratedAffixes = new List<ItemTemplate.Affix>();
            var raritySettings = RpgManager.Instance.raritySettings
                .Find(e => e.rarity == equipmentRarity);

            // If rarity allows any affixes, generate them using the ArmourTemplate's possible affixes
            if (raritySettings.rarityAffixBonusRange.min > 0)
                GenerateAffixes(raritySettings.rarityAffixBonusRange.min,
                    raritySettings.rarityAffixBonusRange.max, armourTemplate);

            // 3) Apply post-fixes
            ApplyPostfixes();

            // 4) Finally, generate the requirements to equip this item
            GenerateItemRequirements(armourTemplate.baselineItemRequirements);
        }

        private void InitializeElementalResistances()
        {
            if (ElementalResistances == null)
                ElementalResistances = new List<BaselineArmourStats.ElementalResistance>();
            ElementalResistances.Clear();

            foreach (RpgManager.ElementalDamageType type in Enum.GetValues(typeof(RpgManager.ElementalDamageType)))
                ElementalResistances.Add(new BaselineArmourStats.ElementalResistance
                {
                    damageType = type,
                    resistancePercentage = 0
                });
        }

        private void ScaleArmourValues()
        {
            var rarityMult = RpgManager.Instance.raritySettings
                .Find(e => e.rarity == equipmentRarity).rarityMultiplier;

            var scaledPhysical = GeneratedArmourStats.physicalArmour *
                                 (1 + equipmentLevel * RpgManager.Instance.itemLevelFactor);
            var scaledMagical = GeneratedArmourStats.magicalArmour *
                                (1 + equipmentLevel * RpgManager.Instance.itemLevelFactor);

            GeneratedArmourStats.physicalArmour = (int)(scaledPhysical * rarityMult);
            GeneratedArmourStats.magicalArmour = (int)(scaledMagical * rarityMult);
        }

        private void GenerateAffixes(int rarityAffixBonusRangeMin, int rarityAffixBonusRangeMax,
            ArmourTemplate armourTemplate)
        {
            var randomNumberOfAffixes = Random.Range(rarityAffixBonusRangeMin, rarityAffixBonusRangeMax);
            var tempListOfPossibleAffixes = new List<ItemTemplate.Affix>();

            // Prepare postfix values scaled to the current item tier, similar to WeaponStats
            foreach (var possibleAffix in armourTemplate.possiblePostfixes)
            {
                var affix = possibleAffix;
                switch (affix.Type)
                {
                    case ItemTemplate.Affix.PostfixType.AddedStrength:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min
                                .strength,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max
                                .strength);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedDexterity:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min
                                .dexterity,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max
                                .dexterity);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedIntelligence:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min
                                .intelligence,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max
                                .intelligence);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedHealth:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.strength,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.strength);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedArmour:
                        // Use vitality as a proxy for defensive scaling
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min
                                .strength * (int)equipmentRarity,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max
                                .strength * (int)equipmentRarity);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedFireResistance:
                    case ItemTemplate.Affix.PostfixType.IncreasedIceResistance:
                    case ItemTemplate.Affix.PostfixType.IncreasedLightningResistance:
                    case ItemTemplate.Affix.PostfixType.IncreasedPoisonResistance:
                        // Use magic stat to scale resistance affixes
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.intelligence * (int)equipmentRarity,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.intelligence * (int)equipmentRarity);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                }
            }

            for (var i = 0; i < randomNumberOfAffixes && tempListOfPossibleAffixes.Count > 0; i++)
            {
                var randomAffixIndex = Random.Range(0, tempListOfPossibleAffixes.Count);
                GeneratedArmourStats.GeneratedAffixes.Add(tempListOfPossibleAffixes[randomAffixIndex]);
                tempListOfPossibleAffixes.RemoveAt(randomAffixIndex);
            }

            tempListOfPossibleAffixes.Clear();
        }

        private void ApplyPostfixes()
        {
            if (GeneratedArmourStats.GeneratedAffixes == null) return;

            foreach (var affix in GeneratedArmourStats.GeneratedAffixes)
                switch (affix.Type)
                {
                    case ItemTemplate.Affix.PostfixType.AddedStrength:
                        GeneratedArmourStats.statBonuses.strength += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedDexterity:
                        GeneratedArmourStats.statBonuses.dexterity += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedIntelligence:
                        GeneratedArmourStats.statBonuses.intelligence += affix.Value;
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedHealth:
                        GeneratedArmourStats.statBonuses.strength += 
                            affix.Value + (int)(RpgManager.Instance.currentItemTier * affix.Value *
                            RpgManager.Instance.itemLevelFactor);
                        break;
                    case ItemTemplate.Affix.PostfixType.AddedArmour:
                        // Flat armour bonus scaled a little by item level factor and tier
                        var flatArmour = (int)(RpgManager.Instance.currentItemTier * affix.Value *
                                               RpgManager.Instance.itemLevelFactor);
                        GeneratedArmourStats.physicalArmour += flatArmour;
                        GeneratedArmourStats.magicalArmour +=
                            (int)(flatArmour * 0.6f); // magical slightly lower by default
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedFireResistance:
                        AddResistance(RpgManager.ElementalDamageType.Fire, affix.Value);
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedIceResistance:
                        AddResistance(RpgManager.ElementalDamageType.Ice, affix.Value);
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedLightningResistance:
                        AddResistance(RpgManager.ElementalDamageType.Lightning, affix.Value);
                        break;
                    case ItemTemplate.Affix.PostfixType.IncreasedPoisonResistance:
                        AddResistance(RpgManager.ElementalDamageType.Poison, affix.Value);
                        break;
                }
        }

        private void AddResistance(RpgManager.ElementalDamageType type, float amount)
        {
            for (var i = 0; i < ElementalResistances.Count; i++)
            {
                if (ElementalResistances[i].damageType != type) continue;
                var res = ElementalResistances[i];
                res.resistancePercentage = Mathf.Clamp(res.resistancePercentage + amount, 0f, 100f);
                ElementalResistances[i] = res;
                return;
            }
        }

        private void SetResistance(RpgManager.ElementalDamageType type, float amount)
        {
            for (var i = 0; i < ElementalResistances.Count; i++)
            {
                if (ElementalResistances[i].damageType != type) continue;
                var res = ElementalResistances[i];
                res.resistancePercentage = Mathf.Clamp(amount, 0f, 100f);
                ElementalResistances[i] = res;
                return;
            }
        }

        /// <summary>
        /// Builds a user-facing tooltip string describing the armour stats, similar to GenerateWeaponStatsDescription().
        /// </summary>
        /// <returns>Formatted description string.</returns>
        public string GenerateArmourStatsDescription()
        {
            var itemDescription = "";

            // Base armour values and weight
            var physicalArmourText = "Physical Armour: " + GeneratedArmourStats.physicalArmour;
            itemDescription += physicalArmourText + "\n";

            if (GeneratedArmourStats.magicalArmour > 0)
            {
                var magicalArmourText = "Magical Armour: " + GeneratedArmourStats.magicalArmour;
                itemDescription += magicalArmourText + "\n";
            }

            // Core stat bonuses (only show if positive)
            var hasAnyCoreBonus = GeneratedArmourStats.statBonuses.strength > 0 ||
                                  GeneratedArmourStats.statBonuses.dexterity > 0 ||
                                  GeneratedArmourStats.statBonuses.intelligence > 0;
            if (hasAnyCoreBonus)
            {
                if (GeneratedArmourStats.statBonuses.strength > 0)
                    itemDescription += $"{GeneratedArmourStats.statBonuses.strength} Added to Strength\n";
                if (GeneratedArmourStats.statBonuses.dexterity > 0)
                    itemDescription += $"{GeneratedArmourStats.statBonuses.dexterity} Added to Dexterity\n";
                if (GeneratedArmourStats.statBonuses.intelligence > 0)
                    itemDescription += $"{GeneratedArmourStats.statBonuses.intelligence} Added To Intelligence\n";
            }

            // Elemental Resistances (only list > 0)
            if (ElementalResistances != null)
                foreach (var res in ElementalResistances)
                {
                    if (res.resistancePercentage <= 0) continue;
                    itemDescription +=
                        $"<color=#F1F06F>{res.damageType} Resistance: <color=white>{res.resistancePercentage:F0}%\n";
                }

            // Affixes (mirror weapon formatting)
            if (GeneratedArmourStats.GeneratedAffixes != null && GeneratedArmourStats.GeneratedAffixes.Count > 0)
            {
                itemDescription += "<color=grey><align=\"center\">________\n<align=\"left\"><size=60%>\n";
                foreach (var generatedAffix in GeneratedArmourStats.GeneratedAffixes)
                    switch (generatedAffix.Type)
                    {
                        case ItemTemplate.Affix.PostfixType.AddedStrength:
                        case ItemTemplate.Affix.PostfixType.AddedIntelligence:
                        case ItemTemplate.Affix.PostfixType.AddedDexterity:
                        case ItemTemplate.Affix.PostfixType.AddedHealth:
                        case ItemTemplate.Affix.PostfixType.AddedElementalDamageToWeapon:
                        case ItemTemplate.Affix.PostfixType.AddedArmour:
                            itemDescription +=
                                $"Adds {generatedAffix.Value} to {generatedAffix.Type.ToString().Remove(0, 5)}\n";
                            break;
                        case ItemTemplate.Affix.PostfixType.IncreasedPhysicalDamage:
                        case ItemTemplate.Affix.PostfixType.IncreasedCritChance:
                        case ItemTemplate.Affix.PostfixType.IncreasedFireResistance:
                        case ItemTemplate.Affix.PostfixType.IncreasedIceResistance:
                        case ItemTemplate.Affix.PostfixType.IncreasedLightningResistance:
                        case ItemTemplate.Affix.PostfixType.IncreasedPoisonResistance:
                            itemDescription +=
                                $"Increased {generatedAffix.Type.ToString().Remove(0, 9)} by {generatedAffix.Value}%\n";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }

            return itemDescription;
        }

        // public ArmourStats DeepCopy()
        // {
        //     var copy = (ArmourStats)MemberwiseClone();
        //     copy.GeneratedArmourStats = copy.GeneratedArmourStats.DeepCopy();
        //     return copy;
        // }
    }
}