using System;
using UnityEngine;

namespace RPGSystem.Equipment
{
    /// <summary>
    /// This class is used to store the base stats of an item.
    /// </summary>
    [Serializable]
    public abstract class BaseItemInfo : MonoBehaviour
    {
        /// <summary>
        /// The name of this equipment.
        /// </summary>
        public string equipmentName;
        
        /// <summary>
        /// The generated rarity of this equipment.
        /// </summary>
        public RpgManager.ItemRarity equipmentRarity;
        
        /// <summary>
        /// The generated level of this equipment.
        /// </summary>
        public int equipmentLevel;
        
        /// <summary>
        /// Does the player have this item equipped?
        /// </summary>
        public bool isEquipped;

        /// <summary>
        /// If the player has this equipped, this is the slot it is equipped in.
        /// </summary>
        public EquipmentSlot equipmentSlot;
        
        public enum EquipmentSlot
        {
            MainHand,
            OffHand,
            Head,
            Body,
            Legs,
            Feet,
        }
    }
}