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

        private bool EquipItem(InventoryItem itemToEquip)
        {
            isSlotEmpty = false;
            EquipmentManager.Instance.EquipItem(itemToEquip.Stats);
            return true;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (isSlotEmpty)
            {
                var slotInfo = eventData.pointerDrag.GetComponent<InventorySlotInfo>();
                eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = false;
                if (slotInfo.item.Stats.equipmentSlot != equipmentSlot) return;

                if (EquipItem(slotInfo.item))
                {
                    eventData.pointerDrag.transform.SetParent(transform);
                    eventData.pointerDrag.transform.localPosition = Vector3.zero;
                    PlayerInventoryManager.Instance.RemoveItemFromInventory(slotInfo.itemPosition);
                }
            }
        }
    }
}
