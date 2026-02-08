using RPGSystem.Inventory_System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace User_Interface
{
    public class ItemSlotDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private CanvasGroup _canvasGroup;
        private Vector2 _originalItemPosition;
        private InventorySlotInfo _inventorySlotInfo;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _inventorySlotInfo = GetComponent<InventorySlotInfo>();
            _canvasGroup.blocksRaycasts = true;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalItemPosition = _inventorySlotInfo.itemPosition;

            if (_canvasGroup != null)
            {
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.alpha = 0.6f;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.alpha = 1f;
            }
        }
    }
}
