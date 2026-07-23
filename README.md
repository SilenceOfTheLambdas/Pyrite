# Pyrite — Unity Gameplay Prototype

Pyrite is a Unity gameplay prototype focused on **gameplay system engineering**:
- ScriptableObject-based, data-driven item templates
- Inventory + pickup pipeline
- Generated item stats/rarity/prefix naming
- In-world item labels for better UX
- Basic enemy behaviour using a simple **state machine**

## Demo / What to play
**Goal:** validate the end-to-end loop of *world item → pickup → generated item → inventory UI*, and verify basic enemy behaviour transitions.

### How to run
1. Open Unity **6.4.5**
2. Open the project
3. Open scene: **Prototyping** (Demo)
4. Click **Play**

### Controls
- Move (WoW-style): **WASD**
- Jump: **Space**
- Move camera rotation / player rotation: **Left Mouse Button + Right Mouse Button**
- Interact / Pickup: **Left Mouse Button**
- Open Inventory UI: **Tab**
- Skill Bar 1 (slot 1–5): **1–5**
- Skill Bar 2 (slot 1–5): **Ctrl + 1–5**

## What to look for (quick walkthrough)
1. Move around using **WASD** and rotate the view with **mouse buttons**.
2. Approach a chest item and press **Left Mouse Button (Interact)**.
3. Confirm the pickup:
    - item is generated (rarity/prefix/stats as applicable)
    - inventory UI updates
    - in-world item label/visual feedback behaves as expected
4. Validate enemy behaviour:
    - enemies transition between their current **state machine** behaviours:
      - idle when the player is out of aggro range of the enemy
      - when player is within aggro range, chase the player
      - once in within the attack radius, start attacking the player
      - if the player moves too far from the enemy, the enemy will return to its original position and remain idle

## Implemented gameplay systems
This section highlights the parts that map directly to gameplay programming work.

### Data-driven items (ScriptableObjects)
- Item templates are defined via **ScriptableObjects**
- Item stats/rolls are generated from template parameters (including rarity/prefix-style generation where applicable)
- Supports extending item variety without rewriting gameplay logic

### Inventory + pickup pipeline
- World interaction (pickup) feeds into an inventory add flow
- Inventory UI reflects the underlying inventory state changes
- Item generation and storage are structured so the systems can evolve toward persistence/equip/use pipelines

### In-world item labels (UX plus engineering glue)
- World-space labels present item information (e.g. name/rarity styling)
- Label presentation is integrated with the pickup/interaction flow for consistent player feedback

### Enemy AI (simple state machine)
- Enemies use a **state machine** for behaviour transitions
- Designed for straightforward tuning/iteration now, with room to extend beyond basic states later

### Gameplay + UI coordination
- Core gameplay systems drive UI updates for inventory and interaction feedback
- Code is organised to keep responsibilities separated (items/inventory/enemy behaviour)

## Architecture overview (high level)
Pyrite’s gameplay loops are structured as pipelines:
- **World Interaction** → **Pickup/Interact** → **Item Generation** → **Inventory Update** (+ UI/label feedback)
- **Enemy State Machine** → **Behaviour execution** (movement/attacks/actions depending on state)

This approach keeps systems extensible by adding new item types, item effects, and additional enemy states.

## Repository layout (recommended entry points)
Start by exploring:
- `Assets/…/Scripts/Inventory/` (inventory UI + logic)
- `Assets/…/Scripts/Items/` (item templates + generation)
- `Assets/…/Scripts/Enemies/` (state machine + enemy behaviour)
- `Assets/…/Scenes/Prototyping.unity` (demo wiring)

## Known limitations / Next improvements
- Save/Load for inventory and generated item state
- Equip/use pipeline (so item effects change gameplay, not just inventory contents)
- Expand enemy state machine and add more tunable behaviours
- Add a small debug UI to speed up iteration (spawn enemies/loot, clear inventory, etc.)

## Development notes
Design notes, experiments, and implementation logs:
- `Project_Log.md`