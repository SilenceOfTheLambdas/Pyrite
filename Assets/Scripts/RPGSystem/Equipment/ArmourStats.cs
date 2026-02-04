using System;
using System.Collections.Generic;
using RPGSystem.Backend;
using RPGSystem.Item_Definitions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPGSystem.Equipment
{
    [Serializable]
    public class ArmourStats : ItemBaseStats
    {
        [Header("Base Defenses")]
        public int physicalArmour; // Changed from protected to public for easier access
        public int magicalArmour;

        [Header("Resistances")]
        // Allows you to assign specific resistances (e.g. 50% Fire Res)
        public List<ElementalResistance> elementalResistances;

        [Header("Stat Bonuses")]
        public RpgManager.CorePlayerStats statBonuses;
        
        [Header("Armour Type")]
        public ArmourTemplate.ArmourType armourType;

        [Header("Affixes")]
        public List<ItemTemplate.Affix> generatedAffixes;

        [Serializable]
        public struct ElementalResistance
        {
            /// <summary>
            /// The type of damage this armour resists.
            /// </summary>
            public RpgManager.ElementalDamageType damageType;
            [Range(0, 100)] 
            public float resistancePercentage;
        }
        
        private ArmourTemplate _armourTemplate;

        /// <summary>
        /// Generates armour stats using a formula similar to WeaponStats:
        /// 1) Roll a baseline by armour slot, 2) scale by level and rarity, 3) generate affixes by rarity/tier,
        /// 4) apply affix effects to armour/resistances/stats.
        /// </summary>
        public void GenerateBaseArmourStats(ArmourTemplate.ArmourType typeOfArmourToGenerate)
        {
            // Get a random armour template from the database
            itemTemplate =
                ItemDatabase.Instance.GetRandomItemTemplateByType(ItemTemplate.ItemType.Armour) as ArmourTemplate;
            _armourTemplate = itemTemplate as ArmourTemplate;
            
            // Generate base item info
            GenerateBaseItemInfo(itemTemplate);
            
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

            // 1) Baseline by slot
            RollBaselineByArmourType();

            // 2) Scale by level and rarity
            ScaleArmourValues();

            // 3) Generate affixes from template/rarity
            generatedAffixes = new List<ItemTemplate.Affix>();
            var raritySettings = RpgManager.Instance.raritySettings
                .Find(e => e.rarity == equipmentRarity);
            // If rarity allows any affixes, generate them using the ArmourTemplate's possible affixes
            if (itemTemplate is ArmourTemplate armourTemplate &&
                raritySettings.rarityAffixBonusRange.max > 0)
            {
                GenerateAffixes(raritySettings.rarityAffixBonusRange.min,
                    raritySettings.rarityAffixBonusRange.max, armourTemplate);
            }

            // 4) Apply affixes
            ApplyAffixes();
        }

        private void InitializeElementalResistances()
        {
            if (elementalResistances == null)
                elementalResistances = new List<ElementalResistance>();
            elementalResistances.Clear();

            foreach (RpgManager.ElementalDamageType type in Enum.GetValues(typeof(RpgManager.ElementalDamageType)))
            {
                elementalResistances.Add(new ElementalResistance
                {
                    damageType = type,
                    resistancePercentage = 0
                });
            }
        }

        private void RollBaselineByArmourType()
        {
            // If the template provided a base, prefer that (optional)
            // if (itemTemplate is ArmourTemplate && _armourTemplate.baseArmourStats != null)
            // {
            //     // Copy baseline values from template (designer-driven)
            //     physicalArmour = _armourTemplate.baseArmourStats.physicalArmour;
            //     magicalArmour = _armourTemplate.baseArmourStats.magicalArmour;
            //     statBonuses = _armourTemplate.baseArmourStats.statBonuses;
            //     // Copy any default resistances defined on the template
            //     if (_armourTemplate.baseArmourStats.elementalResistances != null &&
            //         _armourTemplate.baseArmourStats.elementalResistances.Count > 0)
            //     {
            //         foreach (var res in _armourTemplate.baseArmourStats.elementalResistances)
            //         {
            //             SetResistance(res.damageType, res.resistancePercentage);
            //         }
            //     }
            //     // If template supplied non-zero, we're done rolling baselines.
            //     if (physicalArmour > 0 || magicalArmour > 0)
            //         return;
            // }

            // Derive baseline values from armour type
            switch (armourType)
            {
                case ArmourTemplate.ArmourType.Head:
                    physicalArmour = Random.Range(5, 9);     // 5-8
                    magicalArmour = Random.Range(3, 7);      // 3-6
                    break;
                case ArmourTemplate.ArmourType.Chest:
                    physicalArmour = Random.Range(12, 19);   // 12-18
                    magicalArmour = Random.Range(6, 11);     // 6-10
                    break;
                case ArmourTemplate.ArmourType.Legs:
                    physicalArmour = Random.Range(8, 15);    // 8-14
                    magicalArmour = Random.Range(4, 9);      // 4-8
                    break;
                case ArmourTemplate.ArmourType.Boots:
                    physicalArmour = Random.Range(4, 9);     // 4-8
                    magicalArmour = Random.Range(2, 6);      // 2-5
                    break;
            }
        }

        private void ScaleArmourValues()
        {
            var rarityMult = RpgManager.Instance.raritySettings
                .Find(e => e.rarity == equipmentRarity).rarityMultiplier;

            var scaledPhysical = physicalArmour * (1 + equipmentLevel * RpgManager.Instance.itemLevelFactor);
            var scaledMagical = magicalArmour * (1 + equipmentLevel * RpgManager.Instance.itemLevelFactor);

            physicalArmour = (int)(scaledPhysical * rarityMult);
            magicalArmour = (int)(scaledMagical * rarityMult);
        }

        private void GenerateAffixes(int rarityAffixBonusRangeMin, int rarityAffixBonusRangeMax, ArmourTemplate armourTemplate)
        {
            var randomNumberOfAffixes = Random.Range(rarityAffixBonusRangeMin, rarityAffixBonusRangeMax);
            var tempListOfPossibleAffixes = new List<ItemTemplate.Affix>();

            // Prepare affix values scaled to current item tier, similar to WeaponStats
            foreach (var possibleAffix in armourTemplate.possibleAffixes)
            {
                var affix = possibleAffix;
                switch (affix.Type)
                {
                    case ItemTemplate.Affix.AffixType.AddedStrength:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.strength,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.strength);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Affix.AffixType.AddedDexterity:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.dexterity,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.dexterity);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Affix.AffixType.AddedIntelligence:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.intelligence,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.intelligence);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Affix.AffixType.AddedHealth:
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.vitality,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.vitality);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Affix.AffixType.AddedArmour:
                        // Use vitality as a proxy for defensive scaling
                        affix.Value = Random.Range(
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.min.vitality,
                            RpgManager.Instance.itemTiers[RpgManager.Instance.currentItemTier - 1].tierStatsRange.max.vitality);
                        tempListOfPossibleAffixes.Add(affix);
                        break;
                    case ItemTemplate.Affix.AffixType.IncreasedFireResistance:
                    case ItemTemplate.Affix.AffixType.IncreasedIceResistance:
                    case ItemTemplate.Affix.AffixType.IncreasedLightningResistance:
                    case ItemTemplate.Affix.AffixType.IncreasedPoisonResistance:
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
                generatedAffixes.Add(tempListOfPossibleAffixes[randomAffixIndex]);
                tempListOfPossibleAffixes.RemoveAt(randomAffixIndex);
            }
            tempListOfPossibleAffixes.Clear();
        }

        private void ApplyAffixes()
        {
            if (generatedAffixes == null) return;

            foreach (var affix in generatedAffixes)
            {
                switch (affix.Type)
                {
                    case ItemTemplate.Affix.AffixType.AddedStrength:
                        statBonuses.strength += affix.Value;
                        break;
                    case ItemTemplate.Affix.AffixType.AddedDexterity:
                        statBonuses.dexterity += affix.Value;
                        break;
                    case ItemTemplate.Affix.AffixType.AddedIntelligence:
                        statBonuses.intelligence += affix.Value;
                        break;
                    case ItemTemplate.Affix.AffixType.AddedHealth:
                        statBonuses.vitality += affix.Value;
                        break;
                    case ItemTemplate.Affix.AffixType.AddedArmour:
                        // Flat armour bonus scaled a little by item level factor and tier
                        var flatArmour = (int)((RpgManager.Instance.currentItemTier * affix.Value) *
                                               RpgManager.Instance.itemLevelFactor);
                        physicalArmour += flatArmour;
                        magicalArmour += (int)(flatArmour * 0.6f); // magical slightly lower by default
                        break;
                    case ItemTemplate.Affix.AffixType.IncreasedFireResistance:
                        AddResistance(RpgManager.ElementalDamageType.Fire, affix.Value);
                        break;
                    case ItemTemplate.Affix.AffixType.IncreasedIceResistance:
                        AddResistance(RpgManager.ElementalDamageType.Ice, affix.Value);
                        break;
                    case ItemTemplate.Affix.AffixType.IncreasedLightningResistance:
                        AddResistance(RpgManager.ElementalDamageType.Lightning, affix.Value);
                        break;
                    case ItemTemplate.Affix.AffixType.IncreasedPoisonResistance:
                        AddResistance(RpgManager.ElementalDamageType.Poison, affix.Value);
                        break;
                }
            }
        }

        private void AddResistance(RpgManager.ElementalDamageType type, float amount)
        {
            for (int i = 0; i < elementalResistances.Count; i++)
            {
                if (elementalResistances[i].damageType != type) continue;
                var res = elementalResistances[i];
                res.resistancePercentage = Mathf.Clamp(res.resistancePercentage + amount, 0f, 100f);
                elementalResistances[i] = res;
                return;
            }
        }

        private void SetResistance(RpgManager.ElementalDamageType type, float amount)
        {
            for (int i = 0; i < elementalResistances.Count; i++)
            {
                if (elementalResistances[i].damageType != type) continue;
                var res = elementalResistances[i];
                res.resistancePercentage = Mathf.Clamp(amount, 0f, 100f);
                elementalResistances[i] = res;
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
            var physicalArmourText = "Physical Armour: " + physicalArmour;
            var magicalArmourText = "Magical Armour: " + magicalArmour;

            itemDescription += physicalArmourText + "\n"
                               + magicalArmourText + "\n";

            // Core stat bonuses (only show if positive)
            bool hasAnyCoreBonus = statBonuses.strength > 0 || statBonuses.dexterity > 0 ||
                                   statBonuses.intelligence > 0 || statBonuses.vitality > 0 ||
                                   statBonuses.magic > 0 || statBonuses.luck > 0;
            if (hasAnyCoreBonus)
            {
                if (statBonuses.strength > 0) itemDescription += $"Strength: +{statBonuses.strength}\n";
                if (statBonuses.dexterity > 0) itemDescription += $"Dexterity: +{statBonuses.dexterity}\n";
                if (statBonuses.intelligence > 0) itemDescription += $"Intelligence: +{statBonuses.intelligence}\n";
                if (statBonuses.vitality > 0) itemDescription += $"Vitality: +{statBonuses.vitality}\n";
                if (statBonuses.magic > 0) itemDescription += $"Magic: +{statBonuses.magic}\n";
                if (statBonuses.luck > 0) itemDescription += $"Luck: +{statBonuses.luck}\n";
            }

            // Elemental Resistances (only list > 0)
            if (elementalResistances != null)
            {
                foreach (var res in elementalResistances)
                {
                    if (res.resistancePercentage <= 0) continue;
                    itemDescription += $"<color=#F1F06F>{res.damageType} Resistance: <color=white>{res.resistancePercentage:F0}%\n";
                }
            }

            // Affixes (mirror weapon formatting)
            if (generatedAffixes != null && generatedAffixes.Count > 0)
            {
                itemDescription += "<color=grey><align=\"center\">___________________\n";
                foreach (var generatedAffix in generatedAffixes)
                {
                    switch (generatedAffix.Type)
                    {
                        case ItemTemplate.Affix.AffixType.AddedStrength:
                        case ItemTemplate.Affix.AffixType.AddedIntelligence:
                        case ItemTemplate.Affix.AffixType.AddedDexterity:
                        case ItemTemplate.Affix.AffixType.AddedHealth:
                        case ItemTemplate.Affix.AffixType.AddedElementalDamage:
                        case ItemTemplate.Affix.AffixType.AddedArmour:
                            itemDescription += $"Adds {generatedAffix.Value} to {generatedAffix.Type.ToString().Remove(0, 5)}\n";
                            break;
                        case ItemTemplate.Affix.AffixType.IncreasedPhysicalDamage:
                        case ItemTemplate.Affix.AffixType.IncreasedCritChance:
                        case ItemTemplate.Affix.AffixType.IncreasedFireResistance:
                        case ItemTemplate.Affix.AffixType.IncreasedIceResistance:
                        case ItemTemplate.Affix.AffixType.IncreasedLightningResistance:
                        case ItemTemplate.Affix.AffixType.IncreasedPoisonResistance:
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