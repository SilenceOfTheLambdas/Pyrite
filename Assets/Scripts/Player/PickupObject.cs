using System;
using RPGSystem.Backend;
using RPGSystem.Equipment.Swords;
using RPGSystem.Inventory_System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player
{
    public class PickupObject : MonoBehaviour
    {
        public ItemTemplate.ItemType itemType;
        private PlayerInventoryManager playerInventoryManager;

        private void Start()
        {
            playerInventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>();
            Assert.IsNotNull(playerInventoryManager, "Player needs to have a PlayerInventoryManager component attached.");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                switch (itemType)
                {
                    case ItemTemplate.ItemType.Weapon:
                        playerInventoryManager.AddItem(new InventoryItem(GetComponent<OneHandedSwordStats>(), 1));
                        Destroy(gameObject);
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
        }
    }
}
