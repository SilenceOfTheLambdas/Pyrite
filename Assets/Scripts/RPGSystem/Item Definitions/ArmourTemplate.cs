using System;
using System.Collections.Generic;
using System.ComponentModel;
using RPGSystem.Backend;
using RPGSystem.Equipment;
using UnityEngine;

namespace RPGSystem.Item_Definitions
{
    [CreateAssetMenu(fileName = "BaseWeaponTemplate", menuName = "Inventory/Items/new BaseArmourTemplate")]
    public class ArmourTemplate : ItemTemplate
    {
        public ArmourType armourType;
        // public ArmourStats baseArmourStats;
        
        [Serializable]
        public enum ArmourType
        {
            Head,
            Chest,
            Legs,
            Boots
        }
        
        [Header("Affixes")] [Description("Provide a list of possible affixes that could apply to this item.")]
        public List<Affix> possibleAffixes;
    }
}