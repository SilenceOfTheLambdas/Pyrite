using System;
using Player;
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

        private Tooltip _currentItemTooltip;
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
                Destroy(gameObject);
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
            
            switch (item.Stats.equipmentRarity)
            {
                case RpgManager.ItemRarity.Common:
                    _currentItemTooltipPanel = commonItemTooltipPanel;
                    _currentItemTooltip = commonItemTooltipPanel.GetComponent<Tooltip>();
                    break;
                case RpgManager.ItemRarity.Uncommon:
                    _currentItemTooltipPanel = uncommonItemTooltipPanel;
                    _currentItemTooltip = uncommonItemTooltipPanel.GetComponent<Tooltip>();
                    break;
                case RpgManager.ItemRarity.Rare:
                    _currentItemTooltipPanel = rareItemTooltipPanel;
                    _currentItemTooltip = rareItemTooltipPanel.GetComponent<Tooltip>();
                    break;
                case RpgManager.ItemRarity.Epic:
                    _currentItemTooltipPanel = epicItemTooltipPanel;
                    _currentItemTooltip = epicItemTooltipPanel.GetComponent<Tooltip>();
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

            switch (item.Stats.itemType)
            {
                case ItemTemplate.ItemType.Weapon:
                    var itemStats = item.Stats as WeaponStats;
                    _itemStatsName.SetText(itemStats?.equipmentName);
                    _itemStatsDescription.text = itemStats?.GenerateWeaponStatsDescription();
                    break;
                case ItemTemplate.ItemType.Armour:
                    var armourStats = item.Stats as ArmourStats;
                    _itemStatsName.SetText(armourStats?.equipmentName);
                    _itemStatsDescription.text = armourStats?.GenerateArmourStatsDescription();
                    break;
                case ItemTemplate.ItemType.Accessory:
                case ItemTemplate.ItemType.Potion:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            SetItemRequirementsText(item.Stats);
        }

        private void SetItemRequirementsText(ItemStats itemStats)
        {
            // Level Requirements
            if (itemStats.itemRequirements.playerLevelRequirement > PlayerRpgController.Instance.CurrentPlayerLevel)
            {
                _currentItemTooltip.itemLevelRequirementText.text = 
                    "Level: " + $"<color=red>{itemStats.itemRequirements.playerLevelRequirement}";
            }
            else
            {
                _currentItemTooltip.itemLevelRequirementText.text = 
                    "Level: " + itemStats.itemRequirements.playerLevelRequirement;
            }
            
            // Strength Requirements
            if (itemStats.itemRequirements.playerStrengthRequirement >
                PlayerRpgController.Instance.CurrentPlayerAttributes.strength)
            {
                _currentItemTooltip.itemStrengthRequirementText.text = 
                    "Strength: " + $"<color=red>{itemStats.itemRequirements.playerStrengthRequirement}";
            }
            else
            {
                _currentItemTooltip.itemStrengthRequirementText.text = 
                    "Strength: " + itemStats.itemRequirements.playerStrengthRequirement;
            }
            
            // Dexterity Requirements
            if (itemStats.itemRequirements.playerDexterityRequirement >
                PlayerRpgController.Instance.CurrentPlayerAttributes.dexterity)
            {
                _currentItemTooltip.itemDexterityRequirementText.text = 
                    "Dexterity: " + $"<color=red>{itemStats.itemRequirements.playerDexterityRequirement}";
            }
            else
            {
                _currentItemTooltip.itemDexterityRequirementText.text = 
                    "Dexterity: " + itemStats.itemRequirements.playerDexterityRequirement;
            }
            
            // Intelligence Requirements
            if (itemStats.itemRequirements.playerIntelligenceRequirement >
                PlayerRpgController.Instance.CurrentPlayerAttributes.intelligence)
            {
                _currentItemTooltip.itemIntelligenceRequirementText.text = 
                    "Intelligence: " + $"<color=red>{itemStats.itemRequirements.playerIntelligenceRequirement}";
            }
            else
            {
                _currentItemTooltip.itemIntelligenceRequirementText.text = 
                    "Intelligence: " + itemStats.itemRequirements.playerIntelligenceRequirement;
            }
        }
        
        public void HideItemTooltip()
        {
            _currentItemTooltipPanel.SetActive(false);
        }
    }
}
