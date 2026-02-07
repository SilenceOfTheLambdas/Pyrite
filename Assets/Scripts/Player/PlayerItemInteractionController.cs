using RPGSystem.Inventory_System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using World;

namespace Player
{
    public class PlayerItemInteractionController : MonoBehaviour
    {
        [SerializeField] private InputActionReference interactInputAction;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (interactInputAction.action.triggered)
            {
                if (!CameraController.TryGetClickedObject(
                        _camera,
                        LayerMask.GetMask("Interactable"),
                        out var interactableObject,
                        20f)) return;
                
                if (interactableObject.CompareTag("LootContainer"))
                {
                    var lootContainer = interactableObject.GetComponent<LootContainer>();
                    lootContainer.DropLootAroundContainer();
                }

                if (interactableObject.CompareTag("ItemPickup"))
                {
                    if (!PlayerInventoryManager.Instance.IsPlayerInventoryFull())
                    {
                        var pickupObject = interactableObject.GetComponent<PickupObject>();
                        pickupObject.PickupDroppedItemFromLootContainer();
                    }
                }
            }
        }
    }
}
