using RPGSystem.Equipment;
using RPGSystem.Inventory_System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace User_Interface
{
    public class EquipmentSlot : MonoBehaviour, IDropHandler
    {
        public bool isSlotEmpty = true;
        public ItemStats.EquipmentSlot equipmentSlot;
        private InventoryItem _equippedItem;

        private bool EquipItem(InventoryItem itemToEquip)
        {
            isSlotEmpty = false;
            EquipmentManager.Instance.EquipItem(itemToEquip.Stats);
            itemToEquip.SlotInfo.transform.SetParent(transform);
            itemToEquip.SlotInfo.transform.localPosition = Vector3.zero;
            return true;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var slotInfo = eventData.pointerDrag.GetComponent<InventorySlotInfo>();
            if (isSlotEmpty)
            {
                // Check if the player meets the requirements to equip this item
                if (!slotInfo.Item.Stats.CheckItemRequirements())
                {
                    return;
                }
                
                eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = false;
                if (slotInfo.Item.Stats.equipmentSlot != equipmentSlot) return;
                
                if (EquipItem(slotInfo.Item))
                {
                    slotInfo.EquipmentSlot = this;
                    eventData.pointerDrag.transform.SetParent(transform);
                    eventData.pointerDrag.transform.localPosition = Vector3.zero;
                    PlayerInventoryManager.Instance.RemoveItemFromInventory(slotInfo.Item);
                    slotInfo.Item.ItemIndex = -1; // mark the item as not being in the inventory any more
                    _equippedItem = slotInfo.Item;
                }
            }
            else
            {
                // Check if the player meets the requirements to equip this item
                if (!slotInfo.Item.Stats.CheckItemRequirements())
                {
                    return;
                }
                if (slotInfo.Item.Stats.equipmentSlot != equipmentSlot) return;
                
                // Get rid of the equipped item in the Equipment Slot
                EquipmentManager.Instance.UnequipItemBySlot(_equippedItem.Stats.equipmentSlot);
                
                if (EquipItem(slotInfo.Item))
                {
                    slotInfo.EquipmentSlot = this;
                    eventData.pointerDrag.transform.localPosition = Vector3.zero;
                    PlayerInventoryManager.Instance.RemoveItemFromInventory(slotInfo.Item);
                }
                
                // Then add the item back to the inventory
                PlayerInventoryManager.Instance.AddItem(_equippedItem);
                eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
                _equippedItem = slotInfo.Item;
            }
        }
    }
}
