using System.Collections.Generic;
using System.ComponentModel;
using RPGSystem.Backend;
using UnityEngine;

namespace RPGSystem.Item_Definitions
{
    [CreateAssetMenu(fileName = "BaseWeaponTemplate", menuName = "Inventory/Items/New WeaponTemplate")]
    public class WeaponTemplate : ItemTemplate
    {
        [Header("Weapon Type")] public WeaponType weaponType;

        [Header("Weapon Equipped Model Prefab")]
        public GameObject equippedWeaponModelPrefab;

        [Header("BaseWeaponTemplate Baseline at level 1")]
        public BaseWeaponTemplate baseWeaponStats;

        public enum WeaponType
        {
            TwoHandedSword, // Two-handed sword
            OneHandedSword,
            Axe, // One-handed axe
            Dagger, // One-handed dagger
            Bow, // Two-handed bow
            Staff, // Two-handed staff
            Crossbow // Two-handed crossbow
        }

        [Header("Affixes")] [Description("Provide a list of possible affixes that could apply to this item.")]
        public List<Affix> possibleAffixes;
    }
}