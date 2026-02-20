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
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            Instance = this;
        }


        /// <summary>
        /// Attempts to find any WeaponTemplates in the ItemDatabase.
        /// </summary>
        /// <returns>All WeaponTemplates if any are found.</returns>
        private List<WeaponTemplate> GetAllWeaponTemplates()
        {
            var weaponTemplates = new List<WeaponTemplate>();
            foreach (var itemTemplate in itemTemplates)
                if (itemTemplate.itemType == ItemTemplate.ItemType.Weapon)
                    weaponTemplates.Add(itemTemplate as WeaponTemplate);

            return weaponTemplates;
        }

        private List<ArmourTemplate> GetAllArmourTemplates()
        {
            var armourTemplates = new List<ArmourTemplate>();
            foreach (var itemTemplate in itemTemplates)
                if (itemTemplate.itemType == ItemTemplate.ItemType.Armour)
                    armourTemplates.Add(itemTemplate as ArmourTemplate);

            return armourTemplates;
        }
        
        /// <summary>
        /// Attempts to find an ItemTemplate with a given name.
        /// </summary>
        /// <param name="itemName">Case-sensitive string representing the name of the item template to look up.</param>
        /// <returns>Null if no entry in the list was found</returns>
        public ItemTemplate GetTemplateByName(string itemName)
        {
            return itemTemplates.Find(itemTemplate => itemTemplate.itemName == itemName);
        }

        /// <summary>
        /// Attempts to get a random item baseWeaponStats.
        /// </summary>
        /// <returns>A random ItemTemplate or null if nothing is found.</returns>
        public ItemTemplate GetRandomItemTemplate()
        {
            return itemTemplates[Random.Range(0, itemTemplates.Count)];
        }
    }
}