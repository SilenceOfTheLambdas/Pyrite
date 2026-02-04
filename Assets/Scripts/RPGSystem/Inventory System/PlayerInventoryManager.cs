using System;
using System.Collections.Generic;
using System.Linq;
using RPGSystem.Backend;
using RPGSystem.Equipment;
using RPGSystem.Equipment.Swords;
using UnityEngine;
using User_Interface;

namespace RPGSystem.Inventory_System
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        /// <summary>
        /// A list of all the items in the inventory.
        /// Key = position of the item in inventory.
        /// Value = the Inventory Item itself.
        /// </summary>
        private Dictionary<Vector2, InventoryItem> inventoryItems = new();
        [SerializeField] private int maximumInventorySize = 42;
        [SerializeField] private GameObject gridItemsParent;

        public int CurrentPlayerGold { private set; get; }

        public void AddItem(InventoryItem item)
        {
            // TODO: Handle full inventory better.
            if (inventoryItems.Count >= maximumInventorySize)
                return;
            var itemPosition = FindNextEmptySlot();
            inventoryItems.Add(itemPosition, item);
            var itemSlot = Instantiate(item.stats.itemTemplate.inventorySlotPrefab, gridItemsParent.transform);
            itemSlot.GetComponent<InventorySlotInfo>().itemPosition = itemPosition;
        }

        private Vector2 FindNextEmptySlot()
        {
            if (inventoryItems.Count == 0)
                return Vector2.zero;

            if (inventoryItems.Last().Key.x > 5)
                return new Vector2(inventoryItems.Last().Key.x + 1, 0);

            return new Vector2(inventoryItems.Last().Key.x, inventoryItems.Last().Key.y + 1);
        }

        public InventoryItem GetItemBySlotPosition(Vector2 position)
        {
            return inventoryItems[position];
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
