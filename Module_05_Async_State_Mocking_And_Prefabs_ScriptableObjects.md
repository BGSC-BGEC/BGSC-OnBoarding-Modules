# Module 5: Cloning Factories & Living Data - React State/Fetching, Mock Deadlock Breakers, Prefabs & ScriptableObjects

> **[Curriculum Master Hub](./00_Curriculum_Master_Hub.md)** | **[PR & Submission Guide](./PR_And_Submission_Guide.md)** | **[Module 5 Submissions (`submissions/Module_05/`)](./submissions/Module_05/README.md)**

- **Sprint Dates:** Nov 17 – Nov 22, 2026 | **Estimated Workload:** 4 to 5 Hours Max
- **Deadline 1 (Soft Checkpoint):** Nov 22, 2026 | **Deadline 2 (Final Verification):** Nov 25, 2026
- **Prerequisite:** [Module 4 (React Components / Input & Raycasting)](./Module_04_React_AI_Frontend_And_Input_Raycasting.md) | **Track Options:** Heavy Load or Maintenance Load

## Previously on BGSC Onboarding...

In **Module 4**, you built the interactive surface of your apps and games:

- **AppDev Crew:** You built clean React components using `props` and `.map()`, learned how to pair-program with AI without shipping broken code, and checked your Core Web Vitals. **The problem right now?** Your React UI is basically a museum painting! If a user clicks a button or if new data arrives from the server, your variables change in memory, *but React doesn't redraw the screen* because regular variables don't trigger re-renders!
- **GameDev Crew:** You mastered Unity's New Input System, `OnTriggerEnter`, and `Physics.Raycast` lasers. **The problem right now?** If you want 50 bullets in an FPS or 40 cards in a Card Game deck, are you really going to manually drag-and-drop 50 individual spheres into the Hierarchy before pressing Play? And worse: if you change a card's Mana cost, do you have to edit 40 copies by hand? Enter **Prefabs**, **Object Pooling**, and **`ScriptableObjects`**.

## 1. Why Are We Learning This? ("The Why")

- **AppDev — Why React Hooks (`useState`, `useEffect`), Axios Interceptors, and the Deadlock Rule?**
  1. **State Drives the UI (`UI = f(state)`):** In React, you never manually mutate the DOM (`document.getElementById`). You update **State (`useState`)**, and React automatically calculates the minimal DOM update.
  2. **Handling the Real World (`useEffect` + Loading/Error States):** Campus Wi-Fi drops. Servers return `500 Internal Server Error`. A beginner assumes `fetch()` always succeeds in 0ms; an engineer tracks `isLoading`, `error`, and `data` states so the user sees a smooth skeleton loader instead of a blank white crash screen!
  3. **The Deadlock Rule in Action ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)):** What if your Backend teammate has 3 midsem exams this week and hasn't finished the `/api/posts` endpoint? **You never wait!** You build an API client layer with a toggleable **Mock Data Adapter** so Frontend ships on time and flips one environment variable when Backend goes live ([PlateMate_Frontend PR #1](https://github.com/Nikunj1310/PlateMate_Frontend/pull/1)).
- **GameDev — Why Prefabs, Object Pooling & `ScriptableObjects` Are Mandatory for FPS & Card Games?**
  1. **Prefabs (`Instantiate` / `Destroy`):** A Prefab is a reusable stamp stored in your `Assets/` folder. Need to spawn an enemy, a muzzle flash, or a drawn card at runtime? One line: `Instantiate(cardPrefab, spawnPos, Quaternion.identity)`.
  2. **Why `ScriptableObjects` Save Your RAM and Sanity ([Unity ScriptableObject Manual](https://docs.unity3d.com/6000.6/Documentation/Manual/class-ScriptableObject.html)):** Imagine spawning 100 Goblin cards or 100 FPS Rifles. If each `MonoBehaviour` stores its own copy of `cardDescription`, `baseDamage`, `artworkSprite`, and `manaCost`, Unity duplicates that identical data 100 times in RAM! A **`ScriptableObject`** is a data-only asset that lives in your project folder—all 100 spawned prefabs point to *one* shared `ScriptableObject` in memory (**The Flyweight Pattern** from [Game Programming Patterns](https://gameprogrammingpatterns.com/contents.html))!

## 2. Track A: Application Development - `useState`, `useEffect`, Async Fetching & Mock Fallbacks

### What You Need to Understand

#### A. `useState` & `useEffect` Without Infinite Loops

- **`const [items, setItems] = useState([])`:** Declares reactive state. Calling `setItems(newData)` tells React to re-render the component with the new data.
- **`useEffect(() => { ... }, [dependencies])`:** Runs side effects (like fetching from your Express API) *after* the component renders.
  - **CRITICAL TRAP:** Never call `setItems()` inside `useEffect` without a dependency array `[]`, or every state update will trigger a re-render, which triggers `useEffect` again, DDOSing your own backend 1,000 times per second!

```jsx
// src/components/FoodInventoryFeed.jsx
import { useState, useEffect } from "react";

// Deadlock Rule Fallback: Used automatically if Backend server isn't live yet!
const MOCK_INVENTORY = [{ id: 101, title: "4 Extra Paneer Wraps (ANC)", donor: "Nikunj", servings: 4 },
  { id: 102, title: "Unopened Cold Coffee Pack", donor: "Aarav", servings: 2 }
];

export default function FoodInventoryFeed() {
  const [posts, setPosts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let isMounted = true; // Prevents setting state if user navigates away mid-fetch!

    const fetchInventory = async () => {
      setIsLoading(true);
      setError(null);
      try {
        const response = await fetch("http://localhost:3002/api/posts");
        if (!response.ok) throw new Error(`HTTP Error ${response.status}`);
        const json = await response.json();
        if (isMounted) setPosts(json.data);
      } catch (err) {
        console.warn("Backend unreachable — activating Deadlock Rule Mock Fallback:", err.message);
        if (isMounted) {
          setPosts(MOCK_INVENTORY);
          setError("Live API offline — showing local mock dataset.");
        }
      } finally {
        if (isMounted) setIsLoading(false);
      }
    };

    fetchInventory();
    return () => { isMounted = false; };
  }, []); // Empty array [] means: run ONCE when component mounts!

  if (isLoading) return <p>Loading campus inventory...</p>;

  return (
    <section>
      {error && <div className="warning-banner">{error}</div>}
      <ul>
        {posts.map((post) => (
          <li key={post.id}>
            <strong>{post.title}</strong> — Donated by {post.donor} ({post.servings} servings)
          </li>
        ))}
      </ul>
    </section>
  );
}
```

### Curated Top-Rated Learning Resources (AppDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [React Official Docs: Adding Interactivity & Managing State](https://react.dev/learn) | Explains `useState`, state as a snapshot, and `useEffect` lifecycle synchronization with interactive sandboxes. | Complete *State: A Component's Memory* and *Synchronizing with Effects*. | 90 mins |
| [PlateMate_Frontend PR #1 Architecture Reference](https://github.com/Nikunj1310/PlateMate_Frontend/pull/1) | Real club production codebase showing centralized Axios clients, JWT header injection, and mock-to-live API replacement. | Review the *API Client Architecture* and *Design Choices* sections of the PR description. | 30 mins |
| [The Modern JavaScript Tutorial: Fetch & Async Error Handling](https://javascript.info/) | Deep, practical coverage of `fetch()`, HTTP status handling, and aborting stale requests. | Read *Network Requests: Fetch* and *Error Handling with async/await*. | 45 mins |

## 3. Track B: Game Development - Prefabs, Object Pooling & `ScriptableObject` Architecture

### What You Need to Understand

#### A. Data-Driven Design with `ScriptableObject` (Card Decks & FPS Loadouts)

Look at how one single `ScriptableObject` script lets a Game Designer create 100 new **Cards** or **FPS Weapons** just by right-clicking in the Unity Project window (`Create -> BGSC -> CardOrWeaponData`)—without touching a single line of C# code!

```csharp
using UnityEngine;

// [CreateAssetMenu] adds a right-click menu item inside the Unity Editor!
[CreateAssetMenu(fileName = "NewItemData", menuName = "BGSC/CardOrWeaponData")]
public class ItemDataSO : ScriptableObject
{
    [Header("Shared Static Data (Lives once in RAM!)")]
    public string itemName;
    [TextArea] public string description;
    public int manaOrAmmoCost = 1;
    public int powerOrDamage = 25;
    public Color themeColor = Color.cyan;
}
```

#### B. Runtime Prefab Instantiation & Pickup/Card Logic

Now we attach a lightweight `MonoBehaviour` to our **Prefab** that reads from `ItemDataSO` and notifies the player when picked up or played:

```csharp
using UnityEngine;

public class CollectiblePrefab : MonoBehaviour
{
    [Header("Drag any ItemDataSO asset here!")]
    public ItemDataSO itemData;

    private Renderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<Renderer>();
        if (itemData != null && _meshRenderer != null)
        {
            // Automatically tint the 3D prefab based on its ScriptableObject data!
            _meshRenderer.material.color = itemData.themeColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && itemData != null)
        {
            Debug.Log($"[PICKUP] Collected '{itemData.itemName}' (+{itemData.powerOrDamage} Power)! Description: {itemData.description}");
            // Destroy this spawned prefab instance after pickup
            Destroy(gameObject);
        }
    }
}
```

> **Pro Tip for FPS Builders (Object Pooling):** Calling `Instantiate()` and `Destroy()` 30 times a second for machine-gun bullets triggers C#'s Garbage Collector and causes frame-rate stutters! As covered in [Game Programming Patterns: Object Pool](https://gameprogrammingpatterns.com/contents.html), FPS games pre-spawn 50 inactive bullets at `Start()` and toggle `gameObject.SetActive(true/false)` to recycle them with zero memory lag.

### Curated Top-Rated Learning Resources (GameDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Unity Manual: ScriptableObject Data Assets](https://docs.unity3d.com/6000.6/Documentation/Manual/class-ScriptableObject.html) & [Scripting API](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/ScriptableObject.html) | Official Unity documentation explaining how `ScriptableObject` assets serialize shared data for cards, items, and weapons. | Read the full manual page and create 3 custom `.asset` files using `[CreateAssetMenu]`. | 60 mins |
| [Unity Learn: Junior Programmer Pathway (Prefabs & Spawning)](https://learn.unity.com/pathway/junior-programmer) | Step-by-step exercises on creating Prefabs, spawning waves with `Instantiate()`, and cleaning up objects with `Destroy()`. | Complete *Unit 2: Basic Gameplay (Prefabs & Spawn Manager)*. | 60 mins |
| [Game Programming Patterns: Flyweight & Object Pool](https://gameprogrammingpatterns.com/contents.html) | Explains the exact computer science patterns behind `ScriptableObjects` (Flyweight) and bullet recycling (Object Pool). | Read the *Object Pool* chapter summary (25 mins). | 30 mins |

## 4. Step-by-Step Sprint Action Plan ("What To Do")

### Phase 1 & 2 - **DEADLINE 1 (Soft Checkpoint: Nov 22, 2026)**

- **AppDev D1 Deliverable:** Push a React component using `useState` and `useEffect` that fetches JSON from an endpoint (or triggers your Deadlock Rule Mock Fallback) and logs/renders the raw state. Open `[WIP][Module 5 - D1]` PR.
- **GameDev D1 Deliverable:** Push a C# spawner script that dynamically spawns (`Instantiate`) a Prefab at runtime and defines a basic `ScriptableObject` class. Open `[WIP][Module 5 - D1]` PR.

### Phase 3 & 4 - **DEADLINE 2 (Final Verification: Nov 25, 2026)**

- **AppDev D2 Final Deliverable:**
  - **Heavy Load:** A dynamic React list/dashboard view that fetches data asynchronously via `fetch` or Axios on mount, renders loading skeletons/spinners, handles network failures gracefully with a user-friendly error banner + **Deadlock Rule Mock Data Fallback**, and includes client-side search/filter state ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Dynamic React list view with `useState`, `useEffect`, loading state, and error handler.
- **GameDev D2 Final Deliverable:**
  - **Heavy Load:** A reusable Prefab & `ScriptableObject` system featuring at least **3 distinct `.asset` data instances** (e.g., 3 different Card types with Mana/Effect stats OR 3 different FPS Weapon/Pickup types) that dynamically configure spawned Prefabs at runtime and modify player stats upon collection/activation ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** 1 Prefab spawner + 2 `ScriptableObject` assets modifying player state on `OnTriggerEnter`.

## 5. Cross-Domain Peer Audit & Submission Portal

- **Reviewer Checklist:**
  - [ ] **AppDev:** `useEffect` has a proper dependency array (zero infinite network requests in Chrome Network tab); loading and error states are visibly rendered.
  - [ ] **GameDev:** Changing a stat inside a `ScriptableObject` asset in the Inspector immediately updates the behavior/appearance of spawned Prefabs without changing C# code.
- **Submit Your Pull Request Here:** Place your deliverable inside [`submissions/Module_05/<Track>/<your-github-handle>/`](./submissions/Module_05/README.md), open your PR following the **[BGSC PR & Submission Guide](./PR_And_Submission_Guide.md)**, and log your PR URL in the [Official Submission GForm](https://forms.gle/c5s8DZsGgcoepuVRA).
