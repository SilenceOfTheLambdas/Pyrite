using System;
using System.Collections.Generic;
using System.ComponentModel;
using RPGSystem.Backend;
using UnityEngine;

namespace RPGSystem.Item_Definitions
{
    [CreateAssetMenu(fileName = "BaseWeaponTemplate", menuName = "Inventory/Items/New ArmourTemplate")]
    public class ArmourTemplate : ItemTemplate
    {
        public ArmourType armourType;
        
        /// <summary>
        /// Baseline stats for this armour type.
        /// </summary>
        public BaselineArmourStats baselineArmourStats;
        
        [Serializable]
        public enum ArmourType
        {
            Head,
            Chest,
            Legs,
            Boots
        }
        
        [Header("Affixes")] [Description("Provide a list of possible affixes that could apply to this item.")]
        public List<Postfix> possibleAffixes;
    }
}