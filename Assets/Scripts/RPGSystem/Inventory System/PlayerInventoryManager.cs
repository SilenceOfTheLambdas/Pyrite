using System.Collections.Generic;
using System.Linq;
using RPGSystem.Equipment;
using UnityEngine;

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
            if (inventoryItems.Count >= maximumInventorySize)
                return;
            
            inventoryItems.Add(FindNextEmptySlot(), item);
            Instantiate(item.stats.itemTemplate.inventorySlotPrefab, gridItemsParent.transform);
        }
        
        private Vector2 FindNextEmptySlot()
        {
            if (inventoryItems.Count == 0)
                return Vector2.zero;

            return inventoryItems.Last().Key + Vector2.one;
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
