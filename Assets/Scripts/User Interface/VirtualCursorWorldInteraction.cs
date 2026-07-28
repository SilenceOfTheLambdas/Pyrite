using UnityEngine;
using UnityEngine.InputSystem;

namespace User_Interface
{
    public class VirtualCursorWorldInteraction : MonoBehaviour
    {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private LayerMask interactableLayers;
        [SerializeField] private float maxDistance = 100f;
        [SerializeField] private VirtualCursorUiInput virtualCursorUIInput;

        private void Awake()
        {
            if (worldCamera == null)
                worldCamera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current == null)
                return;

            if (!Mouse.current.leftButton.wasPressedThisFrame)
                return;

            if (virtualCursorUIInput != null && virtualCursorUIInput.IsPointerOverUI())
                return;

            Ray ray = worldCamera.ScreenPointToRay(CursorManager.Instance.VirtualPosition);

            if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayers))
                return;

            GameObject clickedObject = hit.collider.gameObject;

            Debug.Log($"Clicked world object with virtual cursor: {clickedObject.name}");
        }
    }
}
