### Class Diagrams (Mermaid)

Copy any diagram block into a Mermaid-enabled viewer (e.g., VSCode with Mermaid extension, Mermaid Live Editor) to render.

---

#### Core System
```mermaid
classDiagram
    class RpgManager {
      <<Singleton>>
      int currentItemTier
      List~ItemTier~ itemTiers
      float itemLevelFactor
      List~ItemRaritySettings~ raritySettings
      PlayerRpgController PlayerRpgController
      --
      +enum ItemRarity
      +enum ElementalDamageType
      +struct ElementalDamage
      +struct StatRange<T>
      +struct ItemTier
      +struct ItemRaritySettings
      +struct CorePlayerStats
    }

    class PlayerRpgController {
      +int CurrentPlayerLevel
      +int CurrentPlayerExp
      +int CurrentPlayerHealth
      +int PlayerMaxHealth
      +PlayerStats CurrentPlayerAttributes
    }

    RpgManager --> PlayerRpgController : references
```

---

#### Item Templates and Equipment
```mermaid
classDiagram
    class ItemTemplate {
      +GameObject mainItemPrefab
      +GameObject itemPickupPrefab
      +string itemName
      +GameObject inventorySlotPrefab
      +ItemType itemType
      +struct Affix
    }
    class WeaponTemplate {
      +WeaponType weaponType
      +BaseWeaponTemplate baseWeaponStats
      +List~Affix~ possibleAffixes
    }
    class BaseWeaponTemplate {
      +StatRange~int~ physicalDamage
      +StatRange~ElementalDamage~ elementalDamage
      +float attackSpeed
      +float attackRange
      +float criticalDamageMultiplier
      +StatRange~float~ criticalDamageChance
    }
    class ItemBaseStats {
      +string equipmentName
      +ItemRarity equipmentRarity
      +int equipmentLevel
      +bool isEquipped
      +ItemType itemType
      +EquipmentSlot equipmentSlot
      #ItemTemplate itemTemplate
      +GenerateBaseItemInfo(ItemTemplate)
    }
    class WeaponStats {
      +int physicalDamage
      +ElementalDamage elementalDamage
      +float attackSpeed
      +float attackRange
      +float critMultiplier
      +float criticalDamageChance
      +List~Affix~ generatedAffixes
      +GenerateBaseWeaponStats(WeaponTemplate)
      -GenerateAndAssignElementalDamage(WeaponTemplate)
      -ScalePhysicalDamage()
      -ScaleElementalDamage()
      -GenerateAffixes(min,max,WeaponTemplate)
      +GenerateWeaponStatsDescription() string
    }
    class OneHandedSwordStats {
      +WeaponTemplate weaponTemplate
      +Start()
    }

    ItemTemplate <|-- WeaponTemplate
    WeaponTemplate --> BaseWeaponTemplate
    ItemBaseStats <|-- WeaponStats
    WeaponStats <|-- OneHandedSwordStats
    WeaponStats ..> WeaponTemplate
    ItemBaseStats ..> ItemTemplate
    ItemBaseStats ..> RpgManager
```

---

#### Inventory and UI
```mermaid
classDiagram
    class PlayerInventoryManager {
      -Dictionary~Vector2, InventoryItem~ inventoryItems
      -int maximumInventorySize
      -GameObject gridItemsParent
      +int CurrentPlayerGold
      +AddItem(InventoryItem)
      -FindNextEmptySlot() Vector2
      +GetItemBySlotPosition(Vector2) InventoryItem
      +AddPlayerGold(int)
      +RemovePlayerGold(int)
    }
    class InventoryItem {
      +ItemBaseStats stats
      +int itemCount
      +InventoryItem(ItemBaseStats,int)
    }
    class InventorySlotInfo {
      +Vector2 itemPosition
    }
    class UIManager {
      <<Singleton>>
      -InputActionReference toggleInventoryAction
      -GameObject inventoryPanel
      -GameObject commonItemTooltipPanel
      -GameObject uncommonItemTooltipPanel
      -GameObject rareItemTooltipPanel
      -GameObject epicItemTooltipPanel
      -TextMeshProUGUI _itemStatsDescription
      -PlayerInventoryManager _playerInventoryManager
      +ShowItemTooltip(InventorySlotInfo)
      +HideItemTooltip()
    }
    class ItemSlotHover {
      +OnPointerEnter(PointerEventData)
      +OnPointerExit(PointerEventData)
    }

    PlayerInventoryManager o--> InventoryItem
    InventoryItem --> ItemBaseStats
    UIManager --> PlayerInventoryManager : queries
    UIManager ..> WeaponStats : uses description
    ItemSlotHover --> UIManager : calls
    InventorySlotInfo <.. UIManager : passed in
```

---

#### Player and World Interaction
```mermaid
classDiagram
    class PlayerMovementController {
      -NavMeshAgent _navMeshAgent
      -Camera _camera
      -Animator _animator
      -InputAction _moveAction
      +Update()
      -MovePlayer(Vector2)
    }
    class PickupObject {
      +ItemType itemType
      -PlayerInventoryManager playerInventoryManager
      +OnTriggerEnter(Collider)
    }

    PickupObject --> PlayerInventoryManager : adds item
    PlayerMovementController ..> InputAction
    PlayerMovementController ..> NavMeshAgent
    PlayerMovementController ..> Animator
```
