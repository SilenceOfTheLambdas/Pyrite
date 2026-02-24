using System.Collections.Generic;
using System.ComponentModel;
using EditorAttributes;
using RPGSystem.Backend;
using UnityEngine;

namespace RPGSystem.Item_Definitions
{
    [CreateAssetMenu(fileName = "BaseWeaponTemplate", menuName = "Inventory/Items/New WeaponTemplate")]
    public class WeaponTemplate : ItemTemplate
    {
        [AssetPreview(64f, 64f), Required] public GameObject equippedWeaponModelPrefab;

        [Title("Weapon Stats")]
        public WeaponType weaponType;
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

        [Title("Possible Suffixes"), DataTable(true)] [Description("Provide a list of possible affixes that could apply to this item.")]
        public List<Suffix> possibleSuffixes;
    }
}