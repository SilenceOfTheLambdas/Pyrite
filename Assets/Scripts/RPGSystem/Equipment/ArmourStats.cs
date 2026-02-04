using System;
using System.Collections.Generic;
using RPGSystem.Backend;
using RPGSystem.Item_Definitions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPGSystem.Equipment
{
    public class ArmourStats : ItemBaseStats
    {
        /// <summary>
        /// The generated armour stats based on the baseline stats given in the item template.
        /// </summary>
        [NonSerialized]
        public BaselineArmourStats GeneratedArmourStats;
        
        public ArmourStats(BaselineArmourStats baselineArmourStats) => GeneratedArmourStats = baselineArmourStats;

        [NonSerialized]
        public ArmourTemplate.ArmourType ArmourType;
        
        /// <summary>
        /// Generates armour stats using a formula similar to WeaponStats:
        /// 1) Roll a baseline by armour slot, 2) scale by level and rarity, 3) generate affixes by rarity/tier,
        /// 4) apply affix effects to armour/resistances/stats.
        /// </summary>
        public void GenerateBaseArmourStats(ArmourTemplate.ArmourType typeOfArmourToGenerate, ArmourTemplate armourTemplate)
        {
            ArmourType = typeOfArmourToGenerate;
            // Generate base item info
            GenerateBaseItemInfo(armourTemplate);
            
            // Map equipment slot from armour type
            switch (typeOfArmourToGenerate)
            {
                case ArmourTemplate.ArmourType.Head:
                    equipmentSlot = EquipmentSlot.Head;
                    break;
                case ArmourTemplate.ArmourType.Chest:
                    equipmentSlot = EquipmentSlot.Body;
                    break;
                case ArmourTemplate.ArmourType.Legs:
                    equipmentSlot = EquipmentSlot.Legs;
                    break;
                case ArmourTemplate.ArmourType.Boots:
                    equipmentSlot = EquipmentSlot.Feet;
                    break;
            }

            // Initialize resistances for all elements at 0 so we can safely add to them.
            InitializeElementalResistances();

            // 1) Scale by level and rarity
            ScaleArmourValues();

            // 2) Generate affixes from template/rarity
            GeneratedArmourStats.generatedPostfixes = new List<ItemTemplate.Postfix>();
            var raritySettings = RpgManager.Instance.raritySettings
                .Find(e => e.rarity == equipmentRarity);
            // If rarity allows any affixes, generate them using the ArmourTemplate's possible affixes
            if (raritySettings.rarityAffixBonusRange.max > 0)
            {
                GenerateAffixes(raritySettings.rarityAffixBonusRange.min,
                    raritySettings.rarityAffixBonusRange.max, armourTemplate);
            }

            // 3) Apply post-fixes
            ApplyPostfixes();
        }

        private void InitializeElementalResistances()
        {
            if (GeneratedArmourStats.elementalResistances == null)
                GeneratedArmourStats. elementalResistances = new List<BaselineArmourStats.ElementalResistance>();
            GeneratedArmourStats.elementalResistances.Clear();

            foreach (RpgManager.ElementalDamageType type in Enum.GetValues(typeof(RpgManager.ElementalDamageType)))
            {
                GeneratedArmourStats.elementalResistances.Add(new BaselineArmourStats.ElementalResistance
                {
                    damageType = type,
                    resistancePercentage = 0
                });
            }
        }

        private void ScaleArmourValues()
        {
            var rarityMult = RpgManager.Instance.raritySettings
                .Find(e => e.rarity == equipmentRarity).rarityMultiplier;

            var scaledPhysical = GeneratedArmourStats.physicalArmour * (1 + equipmentLevel * RpgManager.Instance.itemLevelFactor);
            var scaledMagical = GeneratedArmourStats.magicalArmour * (1 + equipmentLevel * RpgManager.Instance.itemLevelFactor);

            GeneratedArmourStats.physicalArmour = (int)(scaledPhysical * rarityMult);
            GeneratedArmourStats.magicalArmour = (int)(scaledMagical * rarityMult);
        }

        private void GenerateAffixes(int rarityAffixBonusRangeMin, int rarityAffixBonusRangeMax, ArmourTemplate armourTemplate)
        {
            var randomNumberOfAffixes = Random.Range(rarityAffixBonusRangeMin, rarityAffixBonusRangeMax);
            var tempListOfPossibleAffixes = new List<ItemTemplate.Postfix>();

            // Prepare affix values scaled to current item tier, similar to WeaponStats
            foreach (var possibleAffix in armourTemplate.possibleAffixes)
            {
                var affix = possibleAffix;
                switch (affix.Type)
                {
                    case ItemTemplate.Postfix.PostfixType.AddedStrength:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.strength,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.strength);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Postfix.PostfixType.AddedDexterity:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.dexterity,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.dexterity);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Postfix.PostfixType.AddedIntelligence:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.intelligence,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.intelligence);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Postfix.PostfixType.AddedHealth:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.vitality,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.vitality);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Postfix.PostfixType.AddedArmour:
                        // Use vitality as a proxy for defensive scaling
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.vitality,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.vitality);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Postfix.PostfixType.IncreasedFireResistance:
                    case ItemTemplate.Postfix.PostfixType.IncreasedIceResistance:
                    case ItemTemplate.Postfix.PostfixType.IncreasedLightningResistance:
                    case ItemTemplate.Postfix.PostfixType.IncreasedPoisonResistance:
                        // Use magic stat to scale resistance affixes
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.magic,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.magic);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                }
            }

            for (var i = 0; i < randomNumberOfAffixes && tempListOfPossibleAffixes.Count > 0; i++)
            {
                var randomAffixIndex = Random.Range(0, tempListOfPossibleAffixes.Count);
                GeneratedArmourStats.generatedPostfixes.Add(tempListOfPossibleAffixes[randomAffixIndex]);
                tempListOfPossibleAffixes.RemoveAt(randomAffixIndex);
            }
            tempListOfPossibleAffixes.Clear();
        }

        private void ApplyPostfixes()
        {
            if (GeneratedArmourStats.generatedPostfixes == null) return;

            foreach (var affix in GeneratedArmourStats.generatedPostfixes)
            {
                switch (affix.Type)
                {
                    case ItemTemplate.Postfix.PostfixType.AddedStrength:
                        GeneratedArmourStats.statBonuses.strength += affix.Value;
                        break;
                    case ItemTemplate.Postfix.PostfixType.AddedDexterity:
                        GeneratedArmourStats.statBonuses.dexterity += affix.Value;
                        break;
                    case ItemTemplate.Postfix.PostfixType.AddedIntelligence:
                        GeneratedArmourStats.statBonuses.intelligence += affix.Value;
                        break;
                    case ItemTemplate.Postfix.PostfixType.AddedHealth:
                        GeneratedArmourStats.statBonuses.vitality += affix.Value;
                        break;
                    case ItemTemplate.Postfix.PostfixType.AddedArmour:
                        // Flat armour bonus scaled a little by item level factor and tier
                        var flatArmour = (int)((RpgManager.Instance.currentItemTier * affix.Value) *
                                               RpgManager.Instance.itemLevelFactor);
                        GeneratedArmourStats.physicalArmour += flatArmour;
                        GeneratedArmourStats.magicalArmour += (int)(flatArmour * 0.6f); // magical slightly lower by default
                        break;
                    case ItemTemplate.Postfix.PostfixType.IncreasedFireResistance:
                        AddResistance(RpgManager.ElementalDamageType.Fire, affix.Value);
                        break;
                    case ItemTemplate.Postfix.PostfixType.IncreasedIceResistance:
                        AddResistance(RpgManager.ElementalDamageType.Ice, affix.Value);
                        break;
                    case ItemTemplate.Postfix.PostfixType.IncreasedLightningResistance:
                        AddResistance(RpgManager.ElementalDamageType.Lightning, affix.Value);
                        break;
                    case ItemTemplate.Postfix.PostfixType.IncreasedPoisonResistance:
                        AddResistance(RpgManager.ElementalDamageType.Poison, affix.Value);
                        break;
                }
            }
        }

        private void AddResistance(RpgManager.ElementalDamageType type, float amount)
        {
            for (int i = 0; i < GeneratedArmourStats.elementalResistances.Count; i++)
            {
                if (GeneratedArmourStats.elementalResistances[i].damageType != type) continue;
                var res = GeneratedArmourStats.elementalResistances[i];
                res.resistancePercentage = Mathf.Clamp(res.resistancePercentage + amount, 0f, 100f);
                GeneratedArmourStats.elementalResistances[i] = res;
                return;
            }
        }

        private void SetResistance(RpgManager.ElementalDamageType type, float amount)
        {
            for (int i = 0; i < GeneratedArmourStats.elementalResistances.Count; i++)
            {
                if (GeneratedArmourStats.elementalResistances[i].damageType != type) continue;
                var res = GeneratedArmourStats.elementalResistances[i];
                res.resistancePercentage = Mathf.Clamp(amount, 0f, 100f);
                GeneratedArmourStats.elementalResistances[i] = res;
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
            var magicalArmourText = "Magical Armour: " + GeneratedArmourStats.magicalArmour;

            itemDescription += physicalArmourText + "\n"
                               + magicalArmourText + "\n";

            // Core stat bonuses (only show if positive)
            bool hasAnyCoreBonus = GeneratedArmourStats.statBonuses.strength > 0 || GeneratedArmourStats.statBonuses.dexterity > 0 ||
                                   GeneratedArmourStats.statBonuses.intelligence > 0 || GeneratedArmourStats.statBonuses.vitality > 0 ||
                                   GeneratedArmourStats.statBonuses.magic > 0 || GeneratedArmourStats.statBonuses.luck > 0;
            if (hasAnyCoreBonus)
            {
                if (GeneratedArmourStats.statBonuses.strength > 0) itemDescription += $"Strength: +{GeneratedArmourStats.statBonuses.strength}\n";
                if (GeneratedArmourStats.statBonuses.dexterity > 0) itemDescription += $"Dexterity: +{GeneratedArmourStats.statBonuses.dexterity}\n";
                if (GeneratedArmourStats.statBonuses.intelligence > 0) itemDescription += $"Intelligence: +{GeneratedArmourStats.statBonuses.intelligence}\n";
                if (GeneratedArmourStats.statBonuses.vitality > 0) itemDescription += $"Vitality: +{GeneratedArmourStats.statBonuses.vitality}\n";
                if (GeneratedArmourStats.statBonuses.magic > 0) itemDescription += $"Magic: +{GeneratedArmourStats.statBonuses.magic}\n";
                if (GeneratedArmourStats.statBonuses.luck > 0) itemDescription += $"Luck: +{GeneratedArmourStats.statBonuses.luck}\n";
            }

            // Elemental Resistances (only list > 0)
            if (GeneratedArmourStats.elementalResistances != null)
            {
                foreach (var res in GeneratedArmourStats.elementalResistances)
                {
                    if (res.resistancePercentage <= 0) continue;
                    itemDescription += $"<color=#F1F06F>{res.damageType} Resistance: <color=white>{res.resistancePercentage:F0}%\n";
                }
            }

            // Affixes (mirror weapon formatting)
            if (GeneratedArmourStats.generatedPostfixes != null && GeneratedArmourStats.generatedPostfixes.Count > 0)
            {
                itemDescription += "<color=grey><align=\"center\">___________________\n";
                foreach (var generatedAffix in GeneratedArmourStats.generatedPostfixes)
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
            }

            return itemDescription;
        }
    }
}