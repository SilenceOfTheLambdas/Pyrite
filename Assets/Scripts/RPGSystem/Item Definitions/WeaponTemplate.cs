using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using RPGSystem.Backend;
using UnityEngine;

namespace RPGSystem.Item_Definitions
{
    [CreateAssetMenu(fileName = "BaseWeaponTemplate", menuName = "Inventory/Items/New BaseWeaponTemplate")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class WeaponTemplate : ItemTemplate
    {
        [Header("Weapon Type")]
        public WeaponType weaponType;
        [Header("BaseWeaponTemplate Baseline at level 1")]
        public BaseWeaponTemplate baseWeaponStats;
        
        public enum WeaponType
        {
            Two_Handed_Sword, // Two-handed sword
            One_Handed_Sword,
            Axe, // One-handed axe
            Dagger, // One-handed dagger
            Bow, // Two-handed bow
            Staff, // Two-handed staff
            Crossbow // Two-handed crossbow
        }

        [Header("Affixes")] [Description("Provide a list of possible affixes that could apply to this item.")]
        public List<Postfix> possibleAffixes;
    }
}