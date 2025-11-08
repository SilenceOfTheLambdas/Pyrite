using RPGSystem.Backend;
using RPGSystem.Equipment;

namespace RPGSystem.Inventory_System
{
    public class InventoryItem
    {
        public ItemBaseStats stats;
        public int itemCount;

        public InventoryItem(ItemBaseStats itemBaseStats, int itemCount)
        {
            stats = itemBaseStats;
            this.itemCount = itemCount;
        }

        private void UpdateItemCount(int newItemCount)
        {
            itemCount = newItemCount;
        }
    }
}