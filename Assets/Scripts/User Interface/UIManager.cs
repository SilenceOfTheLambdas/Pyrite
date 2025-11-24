using System;
using RPGSystem.Backend;
using RPGSystem.Equipment;
using RPGSystem.Inventory_System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace User_Interface
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private InputActionReference toggleInventoryAction;
        [SerializeField] private GameObject inventoryPanel;
        /// <summary>
        /// Displayed when hovering over an item in the inventory.
        /// </summary>
        [SerializeField] private GameObject itemTooltipPanel;
        [SerializeField] private GameObject commonItemTooltipPanel;
        [SerializeField] private GameObject uncommonItemTooltipPanel;
        [SerializeField] private GameObject rareItemTooltipPanel;
        [SerializeField] private GameObject epicItemTooltipPanel;

        [SerializeField] private TextMeshProUGUI itemStatsName;
        [SerializeField] private TextMeshProUGUI itemStatsDescription;

        private PlayerInventoryManager _playerInventoryManager;
        
        private void Awake()
        {
            if (Instance != null)
                Destroy(this.gameObject);
            Instance = this;
        }

        private void Start()
        {
            _playerInventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>();
            Assert.IsNotNull(_playerInventoryManager,
                "Player needs to have a PlayerInventoryManager component attached.");
        }

        private void Update()
        {
            if (toggleInventoryAction.action.triggered)
            {
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                itemTooltipPanel.SetActive(false);
            }
            
        }

        public void ShowItemTooltip(InventorySlotInfo slotInfo)
        {
            var itemPosition = slotInfo.itemPosition;
            var item = _playerInventoryManager.GetItemBySlotPosition(itemPosition);
            
            switch (item.stats.equipmentRarity)
            {
                case RpgManager.ItemRarity.Common:
                    itemTooltipPanel = commonItemTooltipPanel;
                    break;
                case RpgManager.ItemRarity.Uncommon:
                    itemTooltipPanel = uncommonItemTooltipPanel;
                    break;
                case RpgManager.ItemRarity.Rare:
                    itemTooltipPanel = rareItemTooltipPanel;
                    break;
                case RpgManager.ItemRarity.Epic:
                    itemTooltipPanel = epicItemTooltipPanel;
                    break;
                case RpgManager.ItemRarity.Unique:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            itemStatsDescription = itemTooltipPanel.transform.Find("Item Stats").GetComponent<TextMeshProUGUI>();
            if (!itemTooltipPanel.activeSelf)
                itemTooltipPanel.SetActive(true);

            switch (item.stats.itemType)
            {
                case ItemTemplate.ItemType.Weapon:
                    var itemStats = item.stats as WeaponStats;
                    itemStatsDescription.text = itemStats?.GenerateWeaponStatsDescription();
                    break;
                case ItemTemplate.ItemType.Armour:
                    break;
                case ItemTemplate.ItemType.Accessory:
                    break;
                case ItemTemplate.ItemType.Potion:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void HideItemTooltip()
        {
            itemTooltipPanel.SetActive(false);
        }
    }
}
