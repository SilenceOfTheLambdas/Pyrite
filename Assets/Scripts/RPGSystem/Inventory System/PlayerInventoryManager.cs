using System;
using System.Collections.Generic;
using System.Linq;
using RPGSystem.Equipment;
using UnityEngine;

namespace RPGSystem.Inventory_System
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        public static PlayerInventoryManager Instance;

        /// <summary>
        /// A list of all the items in the inventory.
        /// Key = position of the item in inventory.
        /// Value = the Inventory Item itself.
        /// </summary>
        public Dictionary<int, InventoryItem> InventoryItems = new();

        [SerializeField] private int maximumInventorySize = 24;
        [SerializeField] private GameObject gridItemsParent;

        public int CurrentPlayerGold { private set; get; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            Instance = this;
        }

        public void AddItem(InventoryItem item)
        {
            // Find the next empty slot and then add the item to the player's inventory
            var itemPosition = FindNextEmptySlot();
            item.itemIndex = itemPosition;
            InventoryItems.Add(itemPosition, item);

            // Spawn the inventorySlotPrefab in the UI inventory grid
            var itemSlot = Instantiate(item.Stats.inventorySlotPrefab, gridItemsParent.transform);
            var inventorySlotInfo = itemSlot.GetComponent<InventorySlotInfo>();
            inventorySlotInfo.Item = item;
        }

        public bool IsPlayerInventoryFull()
        {
            return InventoryItems.Count >= maximumInventorySize;
        }

        private int FindNextEmptySlot()
        {
            if (InventoryItems.Count == 0 ) return 0;
            return InventoryItems.Count + 1;
        }

        public ItemStats GetEquippedItemBySlot(ItemStats.EquipmentSlot equipmentSlot)
        {
            var playerEquipmentManager = EquipmentManager.Instance;
            return equipmentSlot switch
            {
                ItemStats.EquipmentSlot.MainHand => playerEquipmentManager.equippedWeapon,
                ItemStats.EquipmentSlot.Head => playerEquipmentManager.equippedHeadArmour,
                ItemStats.EquipmentSlot.Body => playerEquipmentManager.equippedChestArmour,
                ItemStats.EquipmentSlot.Gauntlets => playerEquipmentManager.equippedGauntlets,
                ItemStats.EquipmentSlot.Legs => playerEquipmentManager.equippedLegArmour,
                ItemStats.EquipmentSlot.Feet => playerEquipmentManager.equippedBootArmour,
                ItemStats.EquipmentSlot.Ring1 => playerEquipmentManager.equippedRing1,
                ItemStats.EquipmentSlot.Ring2 => playerEquipmentManager.equippedRing2,
                _ => throw new ArgumentOutOfRangeException(nameof(equipmentSlot), equipmentSlot, null)
            };
        }

        public void RemoveItemFromInventory(int itemIndex)
        {
            if (!InventoryItems.ContainsKey(itemIndex)) return;
            
            InventoryItems.Remove(itemIndex);
            
            // Shift all items down
            var itemsToShift = InventoryItems.Where(kvp => kvp.Key >= itemIndex).ToList();
            foreach (var valuePair in itemsToShift)
            {
                InventoryItems.Remove(valuePair.Key);
                InventoryItems[valuePair.Key - 1] = valuePair.Value;
                InventoryItems[valuePair.Key - 1].itemIndex = valuePair.Key - 1;
            }
        }

        public void AddPlayerGold(int amount)
        {
            CurrentPlayerGold += amount;
        }

        public void RemovePlayerGold(int amount)
        {
            if (CurrentPlayerGold <= 0)
                return;
            CurrentPlayerGold -= amount;
        }
    }
}