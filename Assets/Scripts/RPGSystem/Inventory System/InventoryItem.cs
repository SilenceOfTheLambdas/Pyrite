using RPGSystem.Equipment;

namespace RPGSystem.Inventory_System
{
    public class InventoryItem
    {
        public ItemStats Stats;
        public int itemCount;

        public InventoryItem(ItemStats itemStats, int itemCount)
        {
            Stats = itemStats;
            this.itemCount = itemCount;
        }

        private void UpdateItemCount(int newItemCount)
        {
            itemCount = newItemCount;
        }
    }
}