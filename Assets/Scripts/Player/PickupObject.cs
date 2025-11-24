using System;
using RPGSystem.Backend;
using RPGSystem.Equipment;
using RPGSystem.Equipment.Swords;
using RPGSystem.Inventory_System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player
{
    public class PickupObject : MonoBehaviour
    {
        public ItemTemplate.ItemType itemType;
        private PlayerInventoryManager _playerInventoryManager;

        private void Start()
        {
            _playerInventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>();
            Assert.IsNotNull(_playerInventoryManager, "Player needs to have a PlayerInventoryManager component attached.");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                switch (itemType)
                {
                    case ItemTemplate.ItemType.Weapon:
                        _playerInventoryManager.AddItem(new InventoryItem(GetComponent<OneHandedSwordStats>(), 1));
                        Destroy(gameObject);
                        break;
                    case ItemTemplate.ItemType.Armour:
                        var armourStats = GetComponent<ArmourStats>();
                        armourStats.GenerateBaseArmourStats(armourStats.armourType);
                        var inventoryItem = new InventoryItem(armourStats, 1)
                        {
                            stats = GetComponent<ArmourStats>()
                        };
                        _playerInventoryManager.AddItem(inventoryItem);
                        Destroy(gameObject);
                        break;
                    case ItemTemplate.ItemType.Accessory:
                        break;
                    case ItemTemplate.ItemType.Potion:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
