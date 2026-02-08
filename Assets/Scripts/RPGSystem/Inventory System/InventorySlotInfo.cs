using UnityEngine;

namespace RPGSystem.Inventory_System
{
    public class InventorySlotInfo : MonoBehaviour
    {
        /// <summary>
        /// The position (x,y) if this item in the inventory.
        /// E.g. (0,0) is the top left corner of the inventory.
        /// </summary>
        public Vector2 itemPosition;

        public InventoryItem item;
    }
}