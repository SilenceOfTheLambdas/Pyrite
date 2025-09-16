using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using RPGSystem.Backend;
using UnityEngine;

namespace RPGSystem.Item_Definitions
{
    [CreateAssetMenu(fileName = "WeaponTemplate", menuName = "Inventory/Items/New WeaponTemplate")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class WeaponTemplate : ItemTemplate
    {
        [Header("WeaponTemplate Baseline at level 1")]
        public RPGSystem.WeaponTemplate template;
        
        public enum WeaponType
        {
            Sword, // One-handed sword
            Axe, // One-handed axe
            Dagger, // One-handed dagger
            Bow, // Two-handed bow
            Lance, // Two-handed lance
            Staff, // Two-handed staff
            Crossbow // Two-handed crossbow
        }

        [Header("Affixes")] [Description("Provide a list of possible affixes that could apply to this item.")]
        public List<Affix> possibleAffixes;
    }
}