using UnityEngine;
using UnityEngine.InputSystem;

namespace User_Interface
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private InputActionReference toggleInventoryAction;
        [SerializeField] private GameObject inventoryPanel;
        
        private void Awake()
        {
            if (Instance != null)
                Destroy(this.gameObject);
            Instance = this;
        }

        private void Update()
        {
            if (toggleInventoryAction.action.triggered)
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}
