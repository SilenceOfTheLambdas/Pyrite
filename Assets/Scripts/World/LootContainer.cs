using System.Collections.Generic;
using Player;
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

        [SerializeField] private RpgManager.StatRange<int> numberOfItemsToDropRange;
        private int _numberOfItemsToDrop;

        private bool _hasBeenUsed;

        [SerializeField] private bool overrideContainerRarity;
        public RpgManager.ItemRarity containerRarity;

        private readonly List<ItemTemplate> _generatedLootItemsToDrop = new();

        private void Start()
        {
            // Randomly assign the number of items to drop
            _numberOfItemsToDrop = Random.Range(numberOfItemsToDropRange.min, numberOfItemsToDropRange.max + 1);

            if (!overrideContainerRarity)
                // Randomly assign container rarity if we do not override the rarity
                RandomlyAssignContainerRarity();

            // We want to generate the loot for this container at the start for performance reasons
            GenerateLootItemsToDrop();
        }

        private void RandomlyAssignContainerRarity()
        {
            var randomWeight = Random.Range(0, 100);
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[0].rarityDropChance)
                containerRarity = RpgManager.ItemRarity.Common;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[1].rarityDropChance)
                containerRarity = RpgManager.ItemRarity.Uncommon;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[2].rarityDropChance)
                containerRarity = RpgManager.ItemRarity.Rare;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[3].rarityDropChance)
                containerRarity = RpgManager.ItemRarity.Epic;
            if (randomWeight >= 100f - RpgManager.Instance.raritySettings[4].rarityDropChance)
                containerRarity = RpgManager.ItemRarity.Unique;
        }

        private void GenerateLootItemsToDrop()
        {
            for (var i = 0; i <= _numberOfItemsToDrop; i++)
                _generatedLootItemsToDrop.Add(ItemDatabase.Instance.GetRandomItemTemplate());
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

            foreach (var itemTemplate in _generatedLootItemsToDrop)
            {
                // Calculate X and Z position
                var randomCircle = Random.insideUnitCircle * itemDropPositionSpacing;
                var spawnPosition = itemDropStartingLocation.position + new Vector3(randomCircle.x, 5f, randomCircle.y);
                
                // Raycast down to find the floor
                if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Walkable")))
                {
                    // Spawn slightly above the ground
                    var finalDropLocation = hit.point + new Vector3(0, 0.5f, 0);
                    var item = Instantiate(itemTemplate.itemPickupPrefab, finalDropLocation, Quaternion.identity);
                    item.GetComponent<PickupObject>().SetItemRarityAndTemplate(itemTemplate, containerRarity);
                }
            }

            _hasBeenUsed = true;
        }
    }
}