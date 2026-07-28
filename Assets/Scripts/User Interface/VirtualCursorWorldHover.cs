using UnityEngine;

namespace User_Interface
{
    public class VirtualCursorWorldHover : MonoBehaviour
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
            if (virtualCursorUIInput != null && virtualCursorUIInput.IsPointerOverUI())
            {
                CursorManager.Instance.ResetCursor();
                return;
            }

            Ray ray = worldCamera.ScreenPointToRay(CursorManager.Instance.VirtualPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayers))
            {
                CursorManager.Instance.SetCursor(CursorManager.CursorType.Interact);
            }
            else
            {
                CursorManager.Instance.ResetCursor();
            }
        }
    }
}
