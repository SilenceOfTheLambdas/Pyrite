using RPGSystem.Inventory_System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace User_Interface
{
    public class ItemSlotDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private CanvasGroup _canvasGroup;
        private Vector2 _originalItemPosition;
        private Transform _originalParent;
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
            _originalItemPosition = transform.position;
            _originalParent = transform.parent;

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

            // If our parent is the same, we weren't dropped into a slot.
            // Or check if we were dropped over nothing
            if (transform.parent == _originalParent || eventData.pointerDrag == null)
            {
                transform.position = _originalItemPosition;
            }
        }
    }
}
