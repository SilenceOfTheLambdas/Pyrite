using RPGSystem.Inventory_System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace User_Interface
{
    public class ItemSlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            UIManager.Instance.ShowItemTooltip(GetComponent<InventorySlotInfo>().Item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.HideItemTooltip();
        }
    }
}