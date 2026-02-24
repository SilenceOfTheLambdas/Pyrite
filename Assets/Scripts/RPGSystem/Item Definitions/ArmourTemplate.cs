using System;
using System.Collections.Generic;
using System.ComponentModel;
using EditorAttributes;
using RPGSystem.Backend;
using UnityEngine;

namespace RPGSystem.Item_Definitions
{
    [CreateAssetMenu(fileName = "BaseWeaponTemplate", menuName = "Inventory/Items/New ArmourTemplate")]
    public class ArmourTemplate : ItemTemplate
    {
        [Title("Armour Stats")]
        public ArmourType armourType;

        /// <summary>
        /// Baseline stats for this armour type.
        /// </summary>
        [Space]
        public BaselineArmourStats baselineArmourStats;

        [Title("Possible Suffixes")] [DataTable(true, false)]
        [Description("Provide a list of possible suffixes that could apply to this item.")]
        public List<Suffix> possibleSuffixes;

        [Serializable]
        public enum ArmourType
        {
            Head,
            Chest,
            Gauntlets,
            Legs,
            Boots,
            Ring
        }
    }
}