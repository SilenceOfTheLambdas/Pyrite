using RPGSystem.Equipment;

namespace RPGSystem.Inventory_System
{
    public class InventoryItem
    {
        public ItemBaseStats Stats;
        public int itemCount;

        public InventoryItem(ItemBaseStats itemBaseStats, int itemCount)
        {
            Stats = itemBaseStats;
            this.itemCount = itemCount;
        }

        private void UpdateItemCount(int newItemCount)
        {
            itemCount = newItemCount;
        }
    }
}