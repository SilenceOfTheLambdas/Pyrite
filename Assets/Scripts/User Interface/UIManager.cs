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
        private GameObject _currentItemTooltipPanel;
        [SerializeField] private GameObject commonItemTooltipPanel;
        [SerializeField] private GameObject uncommonItemTooltipPanel;
        [SerializeField] private GameObject rareItemTooltipPanel;
        [SerializeField] private GameObject epicItemTooltipPanel;

        private TextMeshProUGUI _itemStatsName;
        private TextMeshProUGUI _itemStatsDescription;

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
                if (_currentItemTooltipPanel != null)
                    _currentItemTooltipPanel.SetActive(false);
            }
            
        }

        public void ShowItemTooltip(InventorySlotInfo slotInfo)
        {
            var itemPosition = slotInfo.itemPosition;
            var item = _playerInventoryManager.GetItemBySlotPosition(itemPosition);
            
            switch (item.stats.equipmentRarity)
            {
                case RpgManager.ItemRarity.Common:
                    _currentItemTooltipPanel = commonItemTooltipPanel;
                    break;
                case RpgManager.ItemRarity.Uncommon:
                    _currentItemTooltipPanel = uncommonItemTooltipPanel;
                    break;
                case RpgManager.ItemRarity.Rare:
                    _currentItemTooltipPanel = rareItemTooltipPanel;
                    break;
                case RpgManager.ItemRarity.Epic:
                    _currentItemTooltipPanel = epicItemTooltipPanel;
                    break;
                case RpgManager.ItemRarity.Unique:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _itemStatsDescription = _currentItemTooltipPanel.transform.Find("Item Stats").GetComponent<TextMeshProUGUI>();
            _itemStatsName = _currentItemTooltipPanel.transform.Find("Item Name").GetComponent<TextMeshProUGUI>();
            if (!_currentItemTooltipPanel.activeSelf)
                _currentItemTooltipPanel.SetActive(true);

            switch (item.stats.itemType)
            {
                case ItemTemplate.ItemType.Weapon:
                    var itemStats = item.stats as WeaponStats;
                    _itemStatsName.SetText(itemStats?.itemTemplate.itemName);
                    _itemStatsDescription.text = itemStats?.GenerateWeaponStatsDescription();
                    break;
                case ItemTemplate.ItemType.Armour:
                    var armourStats = item.stats as ArmourStats;
                    _itemStatsName.SetText(armourStats?.itemTemplate.itemName);
                    _itemStatsDescription.text = armourStats?.GenerateArmourStatsDescription();
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
            _currentItemTooltipPanel.SetActive(false);
        }
    }
}
