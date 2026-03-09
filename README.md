# RecycleRight 

An interactive **XR waste sorting game** built with Unity where players learn to sort trash into the correct recycling bins. Players grab waste items using XR hand controllers and place them into the appropriate bins (Paper, Plastic, Glass) while receiving real-time audio and visual feedback.

## Table of Contents

- [About](#about)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [XR Interaction Simulator](#xr-interaction-simulator)
- [Asset Credits & Licenses](#asset-credits--licenses)

---

## About

**RecycleRight** is a VR-based educational experience designed to teach players proper waste sorting. The game features:

- **XR Grab Interactions** — Pick up trash items using VR controllers
- **Smart Bin Validation** — Key-lock system ensures only the correct waste type is accepted per bin
- **Visual Feedback** — Outline highlights (yellow on hover, green on correct, red on wrong placement)
- **Audio Feedback** — Sound effects for correct, wrong, and completion events
- **Score Tracking** — Per-bin score UI with animated updates

| Detail          | Value                                  |
| --------------- | -------------------------------------- |
| **Engine**      | Unity 6                                |
| **Render Pipeline** | Universal Render Pipeline (URP)   |
| **XR Toolkit**  | XR Interaction Toolkit 3.3.1           |
| **XR Runtime**  | OpenXR 1.16.1                          |
| **Platform**    | Android XR                             |
| **Company**     | Abhiwan                                |

---

## Project Structure

> **Only the `Assets/_Project/` folder (and Modeling) is version-controlled.** All other Assets folders are third-party assets or Unity-generated content and should not be modified directly.

```
Assets/_Project/
├── Dev/
│   ├── Audio/                  # Sound effects (Clap, Right, Wrong)
│   ├── Prefab/                 # Game prefabs (Trash items, UI)
│   ├── Scene/                  # Main scene — RecycleRight.unity
│   ├── Scripts/                # Game logic
│   │   ├── GameManager/        #   Central game orchestrator
│   │   ├── TrashbinManager/    #   Bin validation & scoring
│   │   ├── TrashManager/       #   Trash item interactions & feedback
│   │   └── Utilities/          #   Trigger helpers
│   └── SocketInteractors/      # Custom XR socket system
│       ├── Scripts/            #   Lock/key, grid sockets, infinite interactable
│       ├── Models/
│       ├── Materials/
│       ├── Prefabs/
│       └── SocketKeys/         #   ScriptableObject keys per waste type
│
└── Modeling/
    ├── Trash/                  # 3D models for waste items
    │   ├── Glass Bottle/
    │   ├── Paper_Trash/
    │   └── Plastic Bottle/
    └── Trash_Can/              # Trash can model (SM_TrashCan.fbx) + textures
```

---

## Getting Started

### Prerequisites

- **Unity 6** (or compatible version)
- **XR Interaction Toolkit** 3.3.1 (installed via Package Manager)
- **OpenXR Plugin** 1.16.1
- **DOTween** (included in `Assets/Plugins/Demigiant`)

### Opening the Project

1. Clone or download this repository.
2. Open the project folder in Unity Hub.
3. Open the main scene: `Assets/_Project/Dev/Scene/RecycleRight.unity`.
4. Ensure XR Plugin Management is configured for your target device under **Edit → Project Settings → XR Plug-in Management**.

### Building

- Target platform: **Android**
- Minimum SDK: **32**
- Ensure OpenXR is selected as the active XR runtime.

---

## Architecture: Socket Correctness & Filtering

The project uses a **Key-Lock system** built on top of XR Interaction Toolkit's socket interactors to ensure only the correct waste type can be accepted by each bin.

**How it works:**

1. **Key** — A `ScriptableObject` asset representing a waste category (e.g., Paper, Plastic, Glass). Each key is a unique identifier stored in `SocketKeys/`.
2. **Keychain** — A `MonoBehaviour` attached to each trash prefab. It holds one or more `Key` assets that identify the trash type (tracked by instance ID for fast lookup).
3. **Lock** — A serializable validator attached to each bin's socket interactor. It defines which `Key`(s) the socket accepts.
4. **XRLockSocketInteractor** — A custom socket interactor that extends Unity's `XRSocketInteractor`. Before allowing an object to snap in, it checks whether the object's `Keychain` contains the required `Key` defined by the socket's `Lock`. If the key doesn't match, the interaction is rejected.
5. **XRGridSocketInteractor** — Extends the socket system to support a 2D grid layout, allowing a single bin to hold multiple items in organized positions.

**Flow:**
```
Player grabs trash → Places in bin → Socket checks Lock vs Keychain
  ├─ Key matches    → Accept: score increments, green outline, success audio
  └─ Key mismatch   → Reject: red outline, wrong audio, item returns to original position
```

---

## XR Interaction Simulator

> **The XR Controller Simulator by Unity is very useful for testing XR interactions without a physical headset, but it can be complex to navigate initially.**

To test the project without a VR headset, Unity provides the **XR Device Simulator** (included with XR Interaction Toolkit samples):

### Quick Controls Reference

**Navigation & Mode Switching:**

| Action                   | Key                                           |
| ------------------------ | --------------------------------------------- |
| **Cycle Mode**           | `Tab` — switch between FPS (HMD + Controllers) and Controller/Hand mode |
| **Toggle Left Device**   | `[` (press twice to cycle Controller ↔ Hand)  |
| **Toggle Right Device**  | `]` (press twice to cycle Controller ↔ Hand)  |
| **Toggle HMD Only**      | `H`                                           |
| **Move**                 | `W` / `A` / `S` / `D`                        |
| **Rotate**               | Arrow keys or Right-click + Mouse Move        |
| **Scroll Translate**     | Mouse Scroll Wheel (forward/backward)         |
| **Reset Position**       | `R`                                           |

**Controller Inputs** (while manipulating controllers):

| Action                   | Key        |
| ------------------------ | ---------- |
| **Trigger**              | `T`        |
| **Grip**                 | `G`        |
| **Primary Button**       | `1`        |
| **Secondary Button**     | `2`        |
| **Menu**                 | `M`        |
| **Primary 2D Axis**      | `I` / `J` / `K` / `L` |

> Hold `Shift` to target the **left** device instead of the right when pressing controller/hand hotkeys.

**Useful extras:** `` ` `` (backtick) cycles quick-action modes, `Space` performs the active quick-action.

### If the Simulator Is Not Working

The simulator is already added to the project. If it's not responding:

1. Go to **Edit → Project Settings → XR Interaction Toolkit**.
2. Make sure the **Use XR Device Simulator in scenes** option is enabled.
3. Re-enter Play Mode.

> **Tip:** While the simulator is powerful for rapid iteration, some interactions (especially two-handed) feel more natural on an actual headset. Use the simulator for quick functional testing and validate final behaviour on a real device.

---

## Asset Credits & Licenses

This project uses the following third-party assets. Please review each asset's license before redistributing.

| Asset | Source | License / Link |
| ----- | ------ | -------------- |
| **Park** (Environment) | *Unity Asset Store* | [Environment](https://assetstore.unity.com/packages/3d/environments/urban/low-poly-park-61922) |
| **Waste Bin Lowpoly** | *Unity Asset Store* | [Wastebin](https://assetstore.unity.com/packages/3d/props/waste-bin-73303) |
| **Quick Outline** | *Unity Asset Store* |[OutLine](https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488) |
| **DOTween** (HOTween v2) | Demigiant | [http://dotween.demigiant.com/](http://dotween.demigiant.com/) — Free license |
| **Sketchfab 3D Models** | *Sketchfab* | [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/) |
| — `----` | *Sketchfab* | [Link](https://sketchfab.com/3d-models/plastic-water-bottle-731efe2635c9472c9c1e4fdb1f8fbd13) |
| — `----` | *Sketchfab* |[Link](https://sketchfab.com/3d-models/plastic-bottle-daa6ea3a66c3418e80b025c015626642) |
| — `----` | *Sketchfab* |[Link](https://sketchfab.com/3d-models/broken-glass-bottle-weapon-73ce3ffb171f47d5b3943a779b670249) |

| **XR Interaction Toolkit Samples** | Unity Technologies | Included with XR Interaction Toolkit package |
| **TextMesh Pro** | Unity Technologies | Included with Unity |
