using System;
using RPGSystem.Item_Definitions;
using UnityEngine;

namespace RPGSystem.Equipment
{
    /// <summary>
    /// Manages the equipment of the player.
    /// </summary>
    public class EquipmentManager : MonoBehaviour
    {
        public WeaponStats equippedWeapon;
        public ArmourStats equippedHeadArmour;
        public ArmourStats equippedChestArmour;
        public ArmourStats equippedLegArmour;
        public ArmourStats equippedBootArmour;
        public ArmourStats equippedGauntlets;
        public ArmourStats equippedRing1;
        public ArmourStats equippedRing2;

        [Header("Weapon Attachment Point")] [SerializeField]
        private Transform rightHandAttachmentPoint;

        public static EquipmentManager Instance { get; private set; }
        
        public event Action<WeaponStats> OnWeaponEquipped;
        public event Action<WeaponStats> OnWeaponUnequipped;
        
        public event Action<ArmourStats> OnArmourEquipped;
        public event Action<ArmourStats> OnArmourUnequipped;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            Instance = this;
        }

        public void EquipItem(ItemStats itemToEquip)
        {
            if (itemToEquip.equipmentSlot == ItemStats.EquipmentSlot.MainHand)
            {
                EquipWeapon(itemToEquip as WeaponStats);
                OnWeaponEquipped?.Invoke(itemToEquip as WeaponStats);
            }

            if (itemToEquip.equipmentSlot is ItemStats.EquipmentSlot.Body
                or ItemStats.EquipmentSlot.Legs or ItemStats.EquipmentSlot.Feet
                or ItemStats.EquipmentSlot.Gauntlets or ItemStats.EquipmentSlot.Head
                or ItemStats.EquipmentSlot.Ring1 or ItemStats.EquipmentSlot.Ring2)
            {
                EquipArmour(itemToEquip as ArmourStats);
                OnArmourEquipped?.Invoke(itemToEquip as ArmourStats);
            }
            
            itemToEquip.isEquipped = true;
        }

        public void UnequipItemBySlot(ItemStats.EquipmentSlot itemToUnequip)
        {
            switch (itemToUnequip)
            {
                case ItemStats.EquipmentSlot.MainHand:
                    equippedWeapon.isEquipped = false;
                    OnWeaponUnequipped?.Invoke(equippedWeapon);
                    equippedWeapon = null;
                    if (rightHandAttachmentPoint.childCount > 0)
                        Destroy(rightHandAttachmentPoint.GetChild(0).gameObject);
                    break;
                case ItemStats.EquipmentSlot.Head:
                    equippedHeadArmour.isEquipped = false;
                    OnArmourUnequipped?.Invoke(equippedHeadArmour);
                    equippedHeadArmour = null;
                    break;
                case ItemStats.EquipmentSlot.Body:
                    equippedChestArmour.isEquipped = false;
                    OnArmourUnequipped?.Invoke(equippedChestArmour);
                    equippedChestArmour = null;
                    break;
                case ItemStats.EquipmentSlot.Legs:
                    equippedLegArmour.isEquipped = false;
                    OnArmourUnequipped?.Invoke(equippedLegArmour);
                    equippedLegArmour = null;
                    break;
                case ItemStats.EquipmentSlot.Feet:
                    equippedBootArmour.isEquipped = false;
                    OnArmourUnequipped?.Invoke(equippedBootArmour);
                    equippedBootArmour = null;
                    break;
                case ItemStats.EquipmentSlot.Gauntlets:
                    equippedGauntlets.isEquipped = false;
                    OnArmourUnequipped?.Invoke(equippedGauntlets);
                    equippedGauntlets = null;
                    break;
                case ItemStats.EquipmentSlot.Ring1:
                    equippedRing1.isEquipped = false;
                    OnArmourUnequipped?.Invoke(equippedRing1);
                    equippedRing1 = null;
                    break;
                case ItemStats.EquipmentSlot.Ring2:
                    equippedRing2.isEquipped = false;
                    OnArmourUnequipped?.Invoke(equippedRing2);
                    equippedRing2 = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EquipWeapon(WeaponStats weaponToEquip)
        {
            equippedWeapon = weaponToEquip;
            // Show the equipped weapon on the character
            var weaponTemplate = ItemDatabase.Instance.GetTemplateByName(weaponToEquip.equipmentName) as WeaponTemplate;
            if (weaponTemplate == null)
            {
                Debug.LogError($"Unable to find weapon template with given name: {weaponToEquip.equipmentName}!");
                return;
            }
            // Spawn the weapon model
           Instantiate(weaponTemplate.equippedWeaponModelPrefab, rightHandAttachmentPoint);
        }

        private void EquipArmour(ArmourStats armourToEquip)
        {
            switch (armourToEquip.ArmourType)
            {
                case ArmourTemplate.ArmourType.Head:
                    equippedHeadArmour = armourToEquip;
                    break;
                case ArmourTemplate.ArmourType.Chest:
                    equippedChestArmour = armourToEquip;
                    break;
                case ArmourTemplate.ArmourType.Legs:
                    equippedLegArmour = armourToEquip;
                    break;
                case ArmourTemplate.ArmourType.Boots:
                    equippedBootArmour = armourToEquip;
                    break;
                case ArmourTemplate.ArmourType.Gauntlets:
                    equippedGauntlets = armourToEquip;
                    break;
                case ArmourTemplate.ArmourType.Ring:
                    equippedRing1 = armourToEquip;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}