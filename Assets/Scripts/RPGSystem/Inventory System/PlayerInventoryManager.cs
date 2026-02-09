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
        public List<InventoryItem> InventoryItems = new();

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
            Debug.Log($"Adding item to inventory at position {itemPosition}");
            InventoryItems.Add(item);
            item.SlotInfo.gameObject.transform.SetParent(gridItemsParent.transform);
            item.ItemIndex = itemPosition;
        }

        public void AddItemFromGround(InventoryItem item)
        {
            // Find the next empty slot and then add the item to the player's inventory
            var itemPosition = FindNextEmptySlot();
            InventoryItems.Add(item);
            item.ItemIndex = itemPosition;

            // Spawn the inventorySlotPrefab in the UI inventory grid
            var itemSlot = Instantiate(item.Stats.inventorySlotPrefab, gridItemsParent.transform);
            var inventorySlotInfo = itemSlot.GetComponent<InventorySlotInfo>();
            item.SlotInfo = inventorySlotInfo;
            inventorySlotInfo.Item = item;
        }

        public bool IsPlayerInventoryFull()
        {
            return InventoryItems.Count >= maximumInventorySize;
        }

        private int FindNextEmptySlot()
        {
            return InventoryItems.Count == 0 ? 0 : InventoryItems.Count;
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

       
        public void RemoveItemFromInventory(InventoryItem itemToRemove)
        {
            if (!InventoryItems.Contains(itemToRemove)) return;
            InventoryItems.Remove(itemToRemove);
            
            var subset = InventoryItems.Where(i => i.ItemIndex > itemToRemove.ItemIndex).ToList();
            foreach (var item in subset)
            {
                InventoryItems.Remove(item);
                item.ItemIndex -= 1;
                InventoryItems.Add(item);
            }
            InventoryItems.Sort((a, b) => a.ItemIndex.CompareTo(b.ItemIndex));
        }
    }
}