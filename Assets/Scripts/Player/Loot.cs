using System;
using RPGSystem.Backend;
using RPGSystem.Equipment;
using RPGSystem.Inventory_System;
using RPGSystem.Item_Definitions;
using UnityEngine;

namespace Player
{
    public class Loot
    {
        private PlayerInventoryManager _playerInventoryManager;
        private readonly RpgManager.ItemRarity _itemRarity;
        private readonly ItemTemplate _itemTemplate;

        public Loot(ItemTemplate itemTemplate, RpgManager.ItemRarity itemRarity)
        {
            _itemTemplate = itemTemplate;
            _itemRarity = itemRarity;
        }

        /// <summary>
        /// Generates the stats for a loot item based on its loot type.
        /// Determines the item type (e.g. weapon, armour) of the loot
        /// and creates the corresponding stats object, initialising its properties.
        /// </summary>
        /// <returns>
        /// An instance of <c>ItemStats</c> corresponding to the generated loot stats based on the loot type.
        /// Returns <c>null</c> if the item type is unsupported or if there is an error during generation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the loot item type does not match any of the known types.
        /// </exception>
        public ItemStats GenerateLootStatsBasedOnLootType()
        {
            ItemStats itemStats = null;
            switch (_itemTemplate.itemType)
            {
                case ItemTemplate.ItemType.Weapon:
                    if (_itemTemplate is not WeaponTemplate weaponTemplate)
                    {
                        Debug.LogError("Unable to get random weapon template!");
                        break;
                    }

                    var weaponStats = new WeaponStats
                    {
                        inventorySlotPrefab = weaponTemplate.inventorySlotPrefab
                    };
                    weaponStats.GenerateItemNameTypeAndLevel(weaponTemplate, _itemRarity);
                    weaponStats.GenerateWeaponStats(weaponTemplate);

                    itemStats = weaponStats;
                    break;
                case ItemTemplate.ItemType.Armour:
                    if (_itemTemplate is not ArmourTemplate armourTemplate)
                    {
                        Debug.LogError("Unable to get an armour template!");
                        break;
                    }

                    var armourStats = new ArmourStats(armourTemplate.baselineArmourStats)
                    {
                        GeneratedArmourStats = armourTemplate.baselineArmourStats.DeepCopy(),
                        inventorySlotPrefab = armourTemplate.inventorySlotPrefab
                    };
                    armourStats.GenerateItemNameTypeAndLevel(armourTemplate, _itemRarity);
                    armourStats.GenerateArmourStats(armourTemplate!.armourType, armourTemplate);

                    itemStats = armourStats;
                    break;
                case ItemTemplate.ItemType.Accessory:
                case ItemTemplate.ItemType.Potion:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return itemStats;
        }
    }
}