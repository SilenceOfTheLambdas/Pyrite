using System;
using System.Collections.Generic;
using RPGSystem;
using RPGSystem.Backend;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    public class LootContainer : MonoBehaviour
    {
        [SerializeField] private Transform itemDropStartingLocation;
        [SerializeField] private int itemDropPositionSpacing;
        
        [SerializeField]
        private RpgManager.StatRange<int> numberOfItemsToDropRange;
        private int _numberOfItemsToDrop;

        private bool _hasBeenUsed = false;
        
        /// <summary>
        /// Container rarity sets the rarity of ALL items that are dropped.
        /// </summary>
        [NonSerialized]
        public RpgManager.ItemRarity ContainerRarity;

        private readonly List<ItemTemplate> _generatedLootItemsToDrop = new();

        private void Start()
        {
            // Randomly assign the number of items to drop
            _numberOfItemsToDrop = Random.Range(numberOfItemsToDropRange.min, numberOfItemsToDropRange.max + 1);
            
            // Randomly assign container rarity
            RandomlyAssignContainerRarity();
            
            // We want to generate the loot for this container at the start for performance reasons
            GenerateLootItemsToDrop();
        }

        private void RandomlyAssignContainerRarity()
        {
            var randomWeight = Random.Range(0, 100);
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[0].rarityDropChance)
                ContainerRarity = RpgManager.ItemRarity.Common;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[1].rarityDropChance)
                ContainerRarity = RpgManager.ItemRarity.Uncommon;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[2].rarityDropChance)
                ContainerRarity = RpgManager.ItemRarity.Rare;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[3].rarityDropChance)
                ContainerRarity = RpgManager.ItemRarity.Epic;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[4].rarityDropChance)
                ContainerRarity = RpgManager.ItemRarity.Unique;
        }

        private void GenerateLootItemsToDrop()
        {
            for (var i = 0; i <= _numberOfItemsToDrop; i++)
            {
                _generatedLootItemsToDrop.Add(ItemDatabase.Instance.GetRandomItemTemplate());
            }
        }

        /// <summary>
        /// Spawns loot items around the container. The method iterates through the generated loot items
        /// and creates item pickup objects at defined positions around the container starting location.
        /// </summary>
        /// <remarks>
        /// The drop positions are determined using the container's starting location and a spacing offset.
        /// Each instantiated item uses the corresponding item's pickup prefab.
        /// </remarks>
        public void DropLootAroundContainer()
        {
            if (_hasBeenUsed) return;
            var lastItemPosition = itemDropStartingLocation.position;
            foreach (var itemTemplate in _generatedLootItemsToDrop)
            {
                var dropLocation = lastItemPosition + new Vector3(
                    Random.insideUnitCircle.x * itemDropPositionSpacing,
                    0.5f,
                    Random.insideUnitCircle.y * itemDropPositionSpacing);
                lastItemPosition = dropLocation;
                Instantiate(itemTemplate.itemPickupPrefab, dropLocation, Quaternion.identity);
            }

            _hasBeenUsed = true;
        }
    }
}
