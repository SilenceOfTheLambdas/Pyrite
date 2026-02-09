using RPGSystem.Equipment;

namespace RPGSystem.Inventory_System
{
    public class InventoryItem
    {
        public ItemStats Stats;
        public InventorySlotInfo SlotInfo;
        public int ItemIndex;

        public InventoryItem(ItemStats itemStats)
        {
            Stats = itemStats;
        }
    }
}