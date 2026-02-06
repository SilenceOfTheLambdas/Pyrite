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
                    var weaponTemplate = _itemTemplate as WeaponTemplate;
                    if (_itemTemplate is null)
                    {
                        Debug.LogError("Unable to get random weapon template!");
                        return;
                    }
                    var weaponStats = GenerateWeaponStatsType(weaponTemplate);
                    weaponStats.inventorySlotPrefab = weaponTemplate.inventorySlotPrefab;
                    weaponStats.GenerateItemNameTypeAndLevel(weaponTemplate, itemRarity);
                    weaponStats.GenerateBaseWeaponStats(weaponTemplate);

                    _playerInventoryManager.AddItem(new InventoryItem(weaponStats, 1));
                    Destroy(gameObject);
                    break;
                case ItemTemplate.ItemType.Armour:
                    if (_itemTemplate is not ArmourTemplate armourTemplate)
                    {
                        Debug.LogError("Unable to get an armour template!");
                        return;
                    }
                    var armourStats = GenerateArmourStatsType(armourTemplate);
                    armourStats.inventorySlotPrefab = armourTemplate.inventorySlotPrefab;
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

        private WeaponStats GenerateWeaponStatsType(WeaponTemplate weaponTemplate)
        {
            switch (weaponTemplate.weaponType)
            {
                case WeaponTemplate.WeaponType.Two_Handed_Sword:
                    break;
                case WeaponTemplate.WeaponType.One_Handed_Sword:
                    var oneHandedSwordStats = gameObject.AddComponent<WeaponStats>();
                    return oneHandedSwordStats;
                case WeaponTemplate.WeaponType.Axe:
                case WeaponTemplate.WeaponType.Dagger:
                case WeaponTemplate.WeaponType.Bow:
                case WeaponTemplate.WeaponType.Staff:
                case WeaponTemplate.WeaponType.Crossbow:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.LogError("Unable to find the weapon type from the weapon template!");
            return null;
        }

        private ArmourStats GenerateArmourStatsType(ArmourTemplate armourTemplate)
        {
            switch (armourTemplate.armourType)
            {
                case ArmourTemplate.ArmourType.Head:
                case ArmourTemplate.ArmourType.Chest:
                case ArmourTemplate.ArmourType.Legs:
                case ArmourTemplate.ArmourType.Boots:
                    var armourStats = gameObject.AddComponent<ArmourStats>();
                    armourStats.GeneratedArmourStats = armourTemplate.baselineArmourStats;
                    return armourStats;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Debug.LogError("Unable to find the armour type from the armour template!");
            return null;
        }
    }
}
