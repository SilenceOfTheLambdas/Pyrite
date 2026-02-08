using System;
using RPGSystem;
using RPGSystem.Backend;
using RPGSystem.Equipment;
using RPGSystem.Equipment.Swords;
using RPGSystem.Inventory_System;
using RPGSystem.Item_Definitions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player
{
    public class PickupObject : MonoBehaviour
    {
        public ItemTemplate.ItemType itemType;
        private PlayerInventoryManager _playerInventoryManager;
        public RpgManager.ItemRarity itemRarity;
        private ItemTemplate _itemTemplate;

        private void Start()
        {
            _playerInventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>();
            Assert.IsNotNull(_playerInventoryManager,
                "Player needs to have a PlayerInventoryManager component attached.");
        }

        public void SetItemRarityAndTemplate(ItemTemplate template, RpgManager.ItemRarity rarity)
        {
            _itemTemplate = template;
            itemRarity = rarity;
        }

        public void PickupDroppedItemFromLootContainer()
        {
            switch (itemType)
            {
                case ItemTemplate.ItemType.Weapon:
                    if (_itemTemplate is not WeaponTemplate weaponTemplate)
                    {
                        Debug.LogError("Unable to get random weapon template!");
                        return;
                    }

                    var weaponStats = new WeaponStats
                    {
                        inventorySlotPrefab = weaponTemplate.inventorySlotPrefab
                    };
                    weaponStats.GenerateItemNameTypeAndLevel(weaponTemplate, itemRarity);
                    weaponStats.GenerateWeaponStats(weaponTemplate);

                    _playerInventoryManager.AddItem(new InventoryItem(weaponStats, 1));
                    Destroy(gameObject); // Destroy pickup object
                    break;
                case ItemTemplate.ItemType.Armour:
                    if (_itemTemplate is not ArmourTemplate armourTemplate)
                    {
                        Debug.LogError("Unable to get an armour template!");
                        return;
                    }

                    var armourStats = new ArmourStats(armourTemplate.baselineArmourStats)
                    {
                        GeneratedArmourStats = armourTemplate.baselineArmourStats.DeepCopy(),
                        inventorySlotPrefab = armourTemplate.inventorySlotPrefab
                    };
                    armourStats.GenerateItemNameTypeAndLevel(armourTemplate, itemRarity);
                    armourStats.GenerateArmourStats(armourTemplate!.armourType, armourTemplate);

                    _playerInventoryManager.AddItem(new InventoryItem(armourStats, 1));
                    Destroy(gameObject);
                    break;
                case ItemTemplate.ItemType.Accessory:
                case ItemTemplate.ItemType.Potion:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}