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
        public Dictionary<Vector2, InventoryItem> InventoryItems = new();

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
            InventoryItems.Add(itemPosition, item);

            // Spawn the inventorySlotPrefab in the UI inventory grid
            var itemSlot = Instantiate(item.Stats.inventorySlotPrefab, gridItemsParent.transform);
            var inventorySlotInfo = itemSlot.GetComponent<InventorySlotInfo>();
            inventorySlotInfo.itemPosition = itemPosition;
            inventorySlotInfo.item = item;
        }

        public bool IsPlayerInventoryFull()
        {
            return InventoryItems.Count >= maximumInventorySize;
        }

        private Vector2 FindNextEmptySlot()
        {
            if (InventoryItems.Count == 0)
                return Vector2.zero;

            if (InventoryItems.Last().Key.x > 5)
                return new Vector2(InventoryItems.Last().Key.x + 1, 0);

            return new Vector2(InventoryItems.Last().Key.x, InventoryItems.Last().Key.y + 1);
        }

        public ItemStats GetItemBySlotPosition(Vector2 position)
        {
            return InventoryItems[position].Stats;
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

        public void RemoveItemFromInventory(Vector2 position)
        {
            InventoryItems.Remove(position);
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