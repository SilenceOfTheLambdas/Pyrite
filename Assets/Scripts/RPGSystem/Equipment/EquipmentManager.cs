using System;
using RPGSystem.Backend;
using RPGSystem.Item_Definitions;
using UnityEngine;

namespace RPGSystem.Equipment
{
    /// <summary>
    /// Manages the equipment of the player.
    /// </summary>
    public class EquipmentManager : MonoBehaviour
    {
        // TODO: Store currently equipped weapons, armour, etc.
        public WeaponStats equippedWeapon;
        public ArmourStats equippedHeadArmour;
        public ArmourStats equippedChestArmour;
        public ArmourStats equippedLegArmour;
        public ArmourStats equippedBootArmour;

        public void EquipItem(ItemStats itemToEquip, ItemTemplate.ItemType itemType)
        {
            if (itemType == ItemTemplate.ItemType.Weapon)
            {
                EquipWeapon(itemToEquip as WeaponStats);
            }

            if (itemType == ItemTemplate.ItemType.Armour)
            {
                EquipArmour(itemToEquip as ArmourStats);
            }
        }
        
        // TODO: Add methods to equip/unequip items.
        private void EquipWeapon(WeaponStats weaponToEquip)
        {
            if (equippedWeapon != null)
                equippedWeapon = weaponToEquip;
        }

        private void EquipArmour(ArmourStats armourToEquip)
        {
            switch (armourToEquip.ArmourType)
            {
                case ArmourTemplate.ArmourType.Head:
                    if (equippedHeadArmour != null)
                        equippedHeadArmour = armourToEquip;
                    break;
                case ArmourTemplate.ArmourType.Chest:
                    if (equippedChestArmour != null)
                        equippedChestArmour = armourToEquip;
                    break;
                case ArmourTemplate.ArmourType.Legs:
                    if (equippedLegArmour != null)
                        equippedLegArmour = armourToEquip;
                    break;
                case ArmourTemplate.ArmourType.Boots:
                    if (equippedLegArmour != null)
                        equippedBootArmour = armourToEquip;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        // TODO: Update UI Manager to display equipped items.
    }
}