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
            
            physicalDamage = Random.Range(weaponTemplate.baseWeaponStats.physicalDamage.min,
                weaponTemplate.baseWeaponStats.physicalDamage.max);
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
                
                generatedAffixes.Add(tempListOfPossibleAffixes[randomAffixIndex]);
                
                tempListOfPossibleAffixes.RemoveAt(randomAffixIndex);
            }
            tempListOfPossibleAffixes.Clear();
        }
    }
}