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

        public static EquipmentManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            Instance = this;
        }

        public void EquipItem(ItemStats itemToEquip)
        {
            if (itemToEquip.equipmentSlot == ItemStats.EquipmentSlot.MainHand) EquipWeapon(itemToEquip as WeaponStats);

            if (itemToEquip.equipmentSlot is ItemStats.EquipmentSlot.Body
                or ItemStats.EquipmentSlot.Legs or ItemStats.EquipmentSlot.Feet
                or ItemStats.EquipmentSlot.Gauntlets or ItemStats.EquipmentSlot.Head
                or ItemStats.EquipmentSlot.Ring1 or ItemStats.EquipmentSlot.Ring2)
                EquipArmour(itemToEquip as ArmourStats);

            itemToEquip.isEquipped = true;
        }

        private void EquipWeapon(WeaponStats weaponToEquip)
        {
            equippedWeapon = weaponToEquip;
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}