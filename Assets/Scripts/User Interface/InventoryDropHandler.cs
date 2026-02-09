using RPGSystem.Equipment;
using RPGSystem.Inventory_System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace User_Interface
{
    public class InventoryDropHandler : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Transform inventoryParent;
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            var slotInfo = eventData.pointerDrag.GetComponent<InventorySlotInfo>();

            // We don't do anything if the player drops the item within the inventory grid
            if (eventData.pointerDrag.transform.parent == inventoryParent) return;
            
            // Check if inventory is NOT full
            if (!PlayerInventoryManager.Instance.IsPlayerInventoryFull())
            {
                eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = false;
                eventData.pointerDrag.transform.SetParent(inventoryParent);
                eventData.pointerDrag.transform.localPosition = Vector3.zero;
                eventData.pointerDrag.GetComponent<InventorySlotInfo>().EquipmentSlot.isSlotEmpty = true;
                
                // Un-equip item
                EquipmentManager.Instance.UnequipItemBySlot(slotInfo.Item.Stats.equipmentSlot);
                PlayerInventoryManager.Instance.AddItem(slotInfo.Item);
            }
        }
    }
}
