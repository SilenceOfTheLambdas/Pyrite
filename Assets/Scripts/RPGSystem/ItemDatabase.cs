using System.Collections.Generic;
using RPGSystem.Backend;
using RPGSystem.Item_Definitions;
using UnityEngine;

namespace RPGSystem
{
    public class ItemDatabase : MonoBehaviour
    {
        public static ItemDatabase Instance;

        public List<ItemTemplate> itemTemplates;
        
        private void Awake()
        {
            if ( Instance != null && Instance != this )
                Destroy( this.gameObject );
            Instance = this;
        }


        /// <summary>
        /// Attempts to find any WeaponTemplates in the ItemDatabase.
        /// </summary>
        /// <returns>All WeaponTemplates if any are found.</returns>
        public List<WeaponTemplate> GetAllWeaponTemplates()
        {
            var weaponTemplates = new List<WeaponTemplate>();
            foreach (var itemTemplate in itemTemplates)
            {
                if (itemTemplate.itemType == ItemTemplate.ItemType.Weapon)
                    weaponTemplates.Add(itemTemplate as WeaponTemplate);
            }

            return weaponTemplates;
        }

        private List<ArmourTemplate> GetAllArmourTemplates()
        {
            var armourTemplates = new List<ArmourTemplate>();
            foreach (var itemTemplate in itemTemplates)
            {
                if (itemTemplate.itemType == ItemTemplate.ItemType.Armour)
                    armourTemplates.Add(itemTemplate as ArmourTemplate);
            }
            
            return armourTemplates;
        }

        /// <summary>
        /// Attempts to get a random item baseWeaponStats of a given type.
        /// </summary>
        /// <param name="itemType">The type of item baseWeaponStats to look for.</param>
        /// <returns>Returns null if none were found, otherwise it returns a random item baseWeaponStats.</returns>
        public ItemTemplate GetRandomItemTemplateByType(ItemTemplate.ItemType itemType)
        {
            if (itemType == ItemTemplate.ItemType.Weapon)
            {
                var weaponsList = GetAllWeaponTemplates();
                return weaponsList[Random.Range(0, weaponsList.Count)];
            }

            if (itemType == ItemTemplate.ItemType.Armour)
            {
                var armourList = GetAllArmourTemplates();
                return armourList[Random.Range(0, armourList.Count)];
            }

            return null;
        }
        
        /// <summary>
        /// Attempts to get a random item baseWeaponStats.
        /// </summary>
        /// <returns>A random ItemTemplate or null if nothing is found.</returns>
        public ItemTemplate GetRandomItemTemplate() => itemTemplates[Random.Range(0, itemTemplates.Count)];
        
        
    }
}
