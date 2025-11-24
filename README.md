
### Pyrite — Unity RPG Prototype

Pyrite is a Unity-based RPG prototype. It includes a basic player controller with WASD movement using Unity's Input System, an inventory UI, and early RPG systems for items and equipment.

#### Tech stack
- Unity 6000.2.7f2
- C# 9.0 (Assembly-CSharp)
- .NET Framework 4.x (net471)

### Getting started
1. Open the project in Unity Hub using Unity 6000.2.7f2.
2. Open the scene: `Assets/Scenes/Prototyping.unity`.
3. Press Play.

### Controls
- Movement: WASD (camera-relative)
- Mouse: standard look/orbit depending on your camera rig

### Project structure (high-level)
- `Assets/Scripts/Player/` — Player controller and related scripts
- `Assets/Scripts/RPGSystem/` — Items, equipment, inventory logic
- `Assets/Scenes/` — Scenes (e.g., `Prototyping.unity`)
- `Assets/InputSystem_Actions.inputactions` — Input System actions
