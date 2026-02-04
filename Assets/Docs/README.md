### Pyrite Project Documentation

#### Overview
This Unity project implements an RPG-style item and inventory system with procedurally generated weapon stats, a player controller, and an in-game UI for inventory and tooltips.

Core subsystems:
- Backend data definitions for items, tiers, rarity, and elemental damage.
- Item templates and runtime stat generation (weapons currently implemented).
- Inventory management and UI display/tooltip.
- Player controllers for movement and RPG attributes.
- World pickups that convert in-scene items into inventory entries.

Related source paths (under `Assets/Scripts`):
- Backend: `RPGManager.cs`, `RPGSystem/Backend/*`
- Item definitions and equipment: `RPGSystem/Item Definitions/*`, `RPGSystem/Equipment/*`
- Inventory: `RPGSystem/Inventory System/*`
- UI: `User Interface/*`
- Player: `Player/*`

For diagrams, see `Assets/Docs/diagrams.md`.

---

### Backend and Data Definitions

#### `RpgManager` (MonoBehaviour)
Singleton holding global RPG configuration and enums.

- Lifecycle:
  - `Awake()`: sets `Instance`.
  - `Start()`: validates `PlayerRpgController`; comment hints at validating rarity totals.
- Key members:
  - `int currentItemTier`: Current item tier affecting scaling/affixes.
  - `List<ItemTier> itemTiers`: Tier configuration, including stat ranges for affix generation.
  - `float itemLevelFactor`: Level-based scaling factor for item stats.
  - `List<ItemRaritySettings> raritySettings`: Rarity drop chances, multipliers, and affix count ranges.
  - `PlayerRpgController PlayerRpgController { get; private set; }`: Player stats reference.
- Structs/Enums used across systems:
  - `ItemRarity { Common, Uncommon, Rare, Epic, Unique }`
  - `ElementalDamageType { Fire, Ice, Lightning, Poison }`
  - `ElementalDamage { ElementalDamageType type, float amount }`
  - `StatRange<T> { T min, T max }`
  - `ItemTier { int itemTier, StatRange<int> tierLevelRange, StatRange<CorePlayerStats> tierStatsRange }`
  - `ItemRaritySettings { ItemRarity rarity, float rarityDropChance, float rarityMultiplier, StatRange<int> rarityAffixBonusRange }`
  - `CorePlayerStats { strength, dexterity, intelligence, vitality, magic, luck }`

Usage: Central configuration used by item generation (`ItemBaseStats`, `WeaponStats`) and UI.

#### `ItemTemplate` (ScriptableObject)
Base template for all item types.

- Fields: `mainItemPrefab`, `itemPickupPrefab`, `itemName`, `inventorySlotPrefab`, `ItemType { Weapon, Armour, Accessory, Potion }`.
- Nested `Affix { AffixType Type, int Value }` with types like `IncreasedPhysicalDamage`, `IncreasedCritChance`, `AddedElementalDamage`, and attributes/resistances.

Usage: Extended by concrete templates, e.g., `WeaponTemplate`.

#### `WeaponTemplate` (ItemTemplate)
Weapon-specific template data.

- Fields: `WeaponType`, `BaseWeaponTemplate baseWeaponStats`, `List<Affix> possibleAffixes`.

#### `BaseWeaponTemplate` (Serializable)
Baseline stat ranges for a weapon type at level 1.

- Fields: `StatRange<int> physicalDamage`, `StatRange<ElementalDamage> elementalDamage`, `attackSpeed`, `attackRange`, `criticalDamageMultiplier`, `StatRange<float> criticalDamageChance`.

---

### Equipment and Item Stats

#### `ItemBaseStats` (MonoBehaviour, abstract)
Common base for all item stats.

- Fields: `equipmentName`, `equipmentRarity`, `equipmentLevel`, `isEquipped`, `itemType`, `equipmentSlot`, hidden `itemTemplate`.
- Method: `GenerateBaseItemInfo(ItemTemplate)` copies base info from template, rolls rarity by weighted chances, and rolls `equipmentLevel` using `PlayerRpgController.CurrentPlayerLevel` and rarity.

#### `WeaponStats` (ItemBaseStats, abstract)
Runtime-generated stats for weapon items.

- Fields: `physicalDamage`, `elementalDamage`, `attackSpeed`, `attackRange`, `critMultiplier`, `criticalDamageChance`, `generatedAffixes`.
- Methods:
  - `GenerateBaseWeaponStats(WeaponTemplate)`: main generator; seeds baseline, rolls affixes based on rarity settings, then scales damage.
  - `GenerateAndAssignElementalDamage(WeaponTemplate)`: assigns base elemental damage if template supports it.
  - `ScalePhysicalDamage()`: applies level and rarity scaling to physical damage.
  - `ScaleElementalDamage()`: scales elemental damage and adds contributions from elemental affixes.
  - `GenerateAffixes(int min, int max, WeaponTemplate)`: picks and rolls affixes using current item tier’s stat ranges; enforces a single elemental-damage affix.
  - `GenerateWeaponStatsDescription()`: returns a textual description for UI tooltips.

#### `OneHandedSwordStats` (WeaponStats)
Concrete weapon stat component.

- Fields: `WeaponTemplate weaponTemplate`.
- Lifecycle: `Start()` sets `itemTemplate` and calls `GenerateBaseWeaponStats`.

---

### Inventory System

#### `InventoryItem`
Data wrapper for an inventory entry.

- Fields: `ItemBaseStats stats`, `int itemCount`.
- Methods: constructor and `UpdateItemCount(int)`.

#### `InventorySlotInfo` (MonoBehaviour)
Holds grid coordinates for a UI slot: `Vector2 itemPosition` with `(0,0)` as top-left.

#### `PlayerInventoryManager` (MonoBehaviour)
Maintains the player inventory and gold, and spawns UI slots.

- Fields: `Dictionary<Vector2, InventoryItem> inventoryItems`, `int maximumInventorySize = 42`, `GameObject gridItemsParent`, `int CurrentPlayerGold { get; private set; }`.
- Methods:
  - `AddItem(InventoryItem)`: adds to next empty slot, instantiates `item.stats.itemTemplate.inventorySlotPrefab`, and sets `InventorySlotInfo.itemPosition`.
  - `GetItemBySlotPosition(Vector2)`: lookup.
  - `AddPlayerGold(int)`, `RemovePlayerGold(int)`.

---

### UI

#### `UIManager` (MonoBehaviour)
Singleton controlling inventory visibility and hover tooltips.

- Fields: `InputActionReference toggleInventoryAction`, `GameObject inventoryPanel`, rarity-specific tooltip panels, internal text fields, and a cached `PlayerInventoryManager`.
- Lifecycle: `Awake()` singleton init; `Start()` locates `PlayerInventoryManager` on the `Player`-tagged object.
- `Update()`: toggles `inventoryPanel` when input action triggers; hides current tooltip.
- Methods:
  - `ShowItemTooltip(InventorySlotInfo)`: fetches item from inventory, selects tooltip panel by `equipmentRarity`, activates it, and fills stats. Currently implements `ItemType.Weapon` via `WeaponStats.GenerateWeaponStatsDescription()`.
  - `HideItemTooltip()`: hides the active tooltip panel.

#### `ItemSlotHover` (MonoBehaviour)
Forwards pointer enter/exit events to `UIManager` to show/hide tooltips.

---

### Player

#### `PlayerMovementController` (MonoBehaviour)
WASD/analog movement via the new Input System and `NavMeshAgent`.

- Initializes references (`Camera`, `NavMeshAgent`, `Animator`), resolves input action, and updates movement + animation parameters.
- `MovePlayer(Vector2)`: camera-relative movement with rotation slerp.

#### `PlayerRpgController` (MonoBehaviour)
Holds the player’s level, exp, health, and attributes (`PlayerStats`). Defaults are set in `Awake()`.

#### `PickupObject` (MonoBehaviour)
On trigger with the player, converts a world item into an `InventoryItem` and adds it to inventory. Weapon pickups wrap the attached `OneHandedSwordStats`.

---

### System Flow
1. A weapon prefab has `OneHandedSwordStats` with a `WeaponTemplate`.
2. The player collides with a `PickupObject` → an `InventoryItem` is created and added via `PlayerInventoryManager.AddItem`.
3. The inventory spawns a UI slot (`inventorySlotPrefab`) and stores its grid position (`InventorySlotInfo`).
4. Hovering a slot triggers `ItemSlotHover` → `UIManager.ShowItemTooltip` → panel selection by rarity and description fill from `WeaponStats`.
5. Inventory visibility toggled by `toggleInventoryAction`.

---

### Adding new Item Templates
1. Right-click in the project panel and go to 'Create > Inventory > Items' and choose a desired item type.
2. Assign the values for the new item template, making sure to create a new GameObject for the inventory slot prefab and an object representing the item pickup prefab.
4. Add the new item template to the `ItemDatabase` script attached to the `RPGManager` game object.

---

### Adding new *Types* Items
1. Create a new `ItemTemplate` and extend `ItemBaseStats` with a new concrete subclass.
2. Create a method to generate the item stats making sure to call `GenerateBaseItemInfo(ItemTemplate)`.
   3. ItemTemplate is randomly selected from the list of itemTemplates in `ItemDatabase`.
4. Create a new ScriptableObject that derives from `ItemTemplate`, making sure to add the CreateAsset menu tag.
5. Add the new ItemTemplate to the `ItemDatabase`.

---

### Notes and Suggestions
- Tooltip UI: `UIManager` expects a child named `Item Stats` (TMPUGUI) under each tooltip panel.
- Inventory grid: `FindNextEmptySlot()` uses a fixed width heuristic (`x > 5`); consider making grid width configurable.
- Affixes and tiers: Ensure `currentItemTier` is 1-based and within range of `itemTiers`.
- Extend tooltip for `Armour`, `Accessory`, `Potion` in `UIManager` when implemented.
- Consider integrating `PlayerRpgController.CurrentPlayerAttributes` into damage scaling.
