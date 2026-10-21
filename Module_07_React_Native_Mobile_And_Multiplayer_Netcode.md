# Module 7: Going Mobile & Going Multiplayer - React Native Architecture & Unity Netcode Foundations

> **[Curriculum Master Hub](./00_Curriculum_Master_Hub.md)** / **[PR & Submission Guide](./PR_And_Submission_Guide.md)** / **[Module 7 Submissions (`submissions/Module_07/`)](./submissions/Module_07/README.md)**

- **Sprint Dates:** Jan 06 – Jan 11, 2027 / **Estimated Workload:** 4 to 5 Hours Max
- **Deadline 1 (Soft Checkpoint):** Jan 11, 2027 / **Deadline 2 (Final Verification):** Jan 14, 2027
- **Prerequisite:** [Modules 1–6 (Full-Stack Web / Single-Player Unity Loop)](./Module_06_FullStack_CRUD_And_Game_Loop_Canvas_UI.md) / **Track Options:** Heavy Load or Maintenance Load

## Previously on BGSC Onboarding...

Happy New Year, Crew! Before the winter break, in **Module 6**, you shipped your first complete end-to-end applications:

- **AppDev Crew:** You wired a React web frontend to an authenticated Express + PostgreSQL backend. **Now, welcome to the real world of college students:** nobody opens a desktop laptop browser while walking to the Night Canteen or standing on the football field during a [BGSC League match](https://drive.google.com/open?id=1qMlqk_bqyzjaoteGGh6axxkbwbe9r0kDmYAlLfNJrco). They pull out their phones! Today, you take your React skills and bring them to **iOS and Android** using **React Native & Expo**.
- **GameDev Crew:** You built a complete single-player FPS/Card mini-game loop with Canvas HUDs and `SceneManager`. **Now comes the ultimate level-up:** playing against bots gets old fast. Whether you want to build a **5v5 Tactical FPS**, a **1v1 Multiplayer Card Battler**, or a **Co-op Arena**, two different computers have to agree on where every player, bullet, and card is in real time! Welcome to **Multiplayer Networking & Unity Netcode for GameObjects**.

## 1. Why Are We Learning This? ("The Why")

- **AppDev — Why React Native Instead of Writing Separate Swift (iOS) and Kotlin (Android) Apps?**
  - Maintaining two completely separate codebases for iOS and Android doubles your bug count and halves your team speed. **React Native** ([reactnative.dev](https://reactnative.dev/docs/getting-started)) lets you use the exact same React hooks (`useState`, `useEffect`), props, and Axios API client you already mastered in Modules 4–6, while compiling UI primitives (`<View>`, `<Text>`, `<Pressable>`, `<FlatList>`) straight into real native iOS and Android views!
- **GameDev — Why Multiplayer Requires an Authoritative Server (and Never Trusting the Client!):**
  - **Rule #1 of Multiplayer Game Backends:** *The Client is in the hands of the enemy.* If Player A's computer is allowed to tell Player B's computer, *"Hey, I just teleported behind you with 999,999 HP and drew 5 Ace cards in a row,"* hackers will ruin your game in 10 minutes.
  - Instead, we use a **Client-Server Topology** ([Gabriel Gambetta Part I](https://www.gabrielgambetta.com/client-server-game-architecture.html)) powered by **Unity Netcode for GameObjects (NGO)** ([Unity Netcode Docs](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@latest/)). Clients send **Inputs/Requests (`ServerRpc`)** to the Server; the Server validates the rules, updates **`NetworkVariable<T>`** state, and broadcasts results (**`ClientRpc`**) to all connected players!

## 2. Track A: Application Development - React Native, Mobile Navigation & Device State

### What You Need to Understand

#### A. How React Native Differs from Web React

Your JavaScript logic (`useState`, `useEffect`, `fetch`/Axios) stays **100% identical**. Only the visual building blocks change because a phone doesn't have an HTML DOM:

| Web React (HTML DOM) | React Native (Native Mobile) | Crucial Mobile Gotcha |
| :--- | :--- | :--- |
| `<div>`, `<section>` | `<View>` | `<View>` uses **Flexbox (`flexDirection: 'column'`)** by default! |
| `<p>`, `<h1>`, `<span>` | `<Text>` | **Every single string of text MUST be wrapped inside `<Text>`**, or the app crashes immediately! |
| `<button onClick={...}>` | `<Pressable onPress={...}>` | Phones have fingers, not mice: use `onPress` instead of `onClick`. |
| `<ul>` + `.map()` | `<FlatList data={...} />` | Never use `.map()` inside a `<ScrollView>` for 500 items on mobile—`<FlatList>` lazily recycles off-screen views so your phone stays at 60 FPS! |
| `localStorage` | `AsyncStorage` / `SecureStore` | Mobile storage is asynchronous (`await AsyncStorage.getItem('jwt_token')`), just like in [PlateMate_Frontend PR #1](https://github.com/Nikunj1310/PlateMate_Frontend/pull/1). |

```tsx
// App.tsx — A React Native Mobile Screen with FlatList & API Fetching
import React, { useState, useEffect } from "react";
import { SafeAreaView, View, Text, FlatList, Pressable, ActivityIndicator, StyleSheet } from "react-native";

interface LeaderboardEntry {
  id: string;
  handle: string;
  game: string;
  points: number;
}

export default function MobileLeaderboardScreen() {
  const [players, setPlayers] = useState<LeaderboardEntry[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Tip: On a physical phone using Expo Go, replace 'localhost' with your laptop's LAN IP (e.g., 192.168.1.42)!
    fetch("http://192.168.1.42:3000/api/leaderboard")
      .then((res) => res.json())
      .then((json) => setPlayers(json.data))
      .catch(() => {
        // Deadlock Rule Mobile Fallback
        setPlayers([{ id: "1", handle: "TitanSlayer", game: "Valorant", points: 2450 },
          { id: "2", handle: "QueenGambit", game: "Chess", points: 2310 }
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <ActivityIndicator size="large" style={styles.center} />;

  return (
    <SafeAreaView style={styles.container}>
      <Text style={styles.header}>BGSC Live Mobile Standings</Text>
      <FlatList
        data={players}
        keyExtractor={(item) => item.id}
        renderItem={({ item, index }) => (
          <Pressable style={styles.card} onPress={() => console.log("Selected:", item.handle)}>
            <Text style={styles.rank}>#{index + 1} {item.handle}</Text>
            <Text style={styles.subtext}>{item.game} • {item.points} PTS</Text>
          </Pressable>
        )}
      />
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: "#0F172A", padding: 16 },
  center: { flex: 1, justifyContent: "center", alignItems: "center" },
  header: { fontSize: 22, fontWeight: "700", color: "#F8FAFC", marginBottom: 12 },
  card: { backgroundColor: "#1E293B", padding: 16, borderRadius: 10, marginBottom: 10 },
  rank: { fontSize: 18, fontWeight: "600", color: "#38BDF8" },
  subtext: { fontSize: 14, color: "#94A3B8", marginTop: 4 }
});
```

### Curated Top-Rated Learning Resources (AppDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [React Native Official Docs: Getting Started (`reactnative.dev`)](https://reactnative.dev/docs/getting-started) | The official documentation from Meta & Expo—teaches Core Components, Flexbox layout, and `<FlatList>` cleanly. | Complete *The Basics* (Introduction, Core Components and Native Components, React Fundamentals, Using a ListView). | 90 mins |
| [PlateMate_Frontend PR #1: Expo & AsyncStorage Setup](https://github.com/Nikunj1310/PlateMate_Frontend/pull/1) | Our club's actual React Native / Expo architecture using `EXPO_PUBLIC_*` env variables and `AsyncStorage` token management. | Study how `authApi.ts` and `AsyncStorage` persist user sessions across app restarts. | 45 mins |

## 3. Track B: Game Development - Multiplayer Architecture & Unity Netcode for GameObjects

### What You Need to Understand

#### A. Host vs. Dedicated Server vs. Client (`NetworkManager`)

In Unity **Netcode for GameObjects (NGO)** ([Unity Netcode Docs](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@latest/)):

- **Server:** Runs the authoritative simulation (spawns objects, calculates damage, shuffles the card deck). Has no local player.
- **Host:** One player's PC acts as *both* the Server AND a local Client at the same time (awesome for testing 2-player lobbies on your laptop or LAN!).
- **Client:** Connects to the Server/Host, sends inputs, and renders what the Server tells it.

#### B. `NetworkBehaviour`, `IsOwner`, `NetworkVariable<T>` & `[Rpc]`

When a script inherits from `NetworkBehaviour` (instead of plain `MonoBehaviour`):

1. **`if (!IsOwner) return;`**: Remember that in a 4-player game, there are **4 Player Prefabs** inside your scene! Without `if (!IsOwner) return;`, pressing `W` on your keyboard will move *all 4 players' characters at the same time*!
2. **`NetworkVariable<int>`:** Automatically synchronizes a value (like Health, Mana, or Score) from the Server to every connected Client whenever it changes.
3. **`[Rpc(SendTo.Server)]` (ServerRPC):** A function a Client calls on its own machine that actually executes **on the Server** (e.g., `"Server, I want to play Card #3"` or `"Server, I just fired my rifle"`).

```csharp
using Unity.Netcode;
using UnityEngine;

public class NetworkedPlayerController : NetworkBehaviour
{
    public float moveSpeed = 6f;

    // Server-authoritative variable: Everyone can read it, ONLY the Server can modify it!
    public NetworkVariable<int> PlayerScore = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        // Subscribe to changes so our HUD updates whenever the Server changes PlayerScore!
        PlayerScore.OnValueChanged += (oldVal, newVal) =>
        {
            Debug.Log($"[NETCODE] Player {OwnerClientId} score changed: {oldVal} -> {newVal}");
        };
    }

    private void Update()
    {
        // CRITICAL: Only process keyboard input for the player instance WE own!
        if (!IsOwner) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(h, 0f, v).normalized * (moveSpeed * Time.deltaTime);

        if (move.sqrMagnitude > 0f)
        {
            RequestMoveServerRpc(move);
        }

        // Press Space to ask the Server to play a card / score a hit
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SubmitActionServerRpc(10);
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestMoveServerRpc(Vector3 deltaMove)
    {
        // Server validates the move so players can't speed-hack!
        if (deltaMove.magnitude <= moveSpeed * Time.deltaTime * 1.5f)
        {
            transform.position += deltaMove;
        }
    }

    [Rpc(SendTo.Server)]
    private void SubmitActionServerRpc(int pointsToAdd)
    {
        // Server updates the NetworkVariable, automatically syncing to all clients!
        PlayerScore.Value += pointsToAdd;
    }
}
```

### Curated Top-Rated Learning Resources (GameDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Unity Manual: Netcode for GameObjects (`com.unity.netcode.gameobjects`)](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@latest/) & [Unity 6 Manual](https://docs.unity3d.com/6000.6/Documentation/Manual/com.unity.netcode.gameobjects.html) | Official documentation for installing Netcode for GameObjects, configuring `NetworkManager`, and writing `NetworkBehaviour` scripts. | Complete *Get Started with NGO* ("Hello World", `NetworkObject`, `NetworkVariable`, and `RPCs`). | 90 mins |
| [Gabriel Gambetta: Fast-Paced Multiplayer (Part I — Client-Server Game Architecture)](https://www.gabrielgambetta.com/client-server-game-architecture.html) | The single most famous, visually intuitive article series in the game industry on how multiplayer servers work. | Read *Part I: Client-Server Game Architecture* (20 mins). | 30 mins |

## 4. Step-by-Step Sprint Action Plan ("What To Do")

### Phase 1 & 2 - **DEADLINE 1 (Soft Checkpoint: Jan 11, 2027)**

- **AppDev D1 Deliverable:** Initialize an Expo / React Native app (`npx create-expo-app@latest`), build a multi-component mobile layout using `<SafeAreaView>`, `<View>`, `<Text>`, and `<FlatList>`, and push your `[WIP][Module 7 - D1]` PR.
- **GameDev D1 Deliverable:** Install `Netcode for GameObjects` via Unity Package Manager, add a `NetworkManager` with buttons for `StartHost()` and `StartClient()`, and spawn a `NetworkObject` player prefab. Open `[WIP][Module 7 - D1]` PR.

### Phase 3 & 4 - **DEADLINE 2 (Final Verification: Jan 14, 2027)**

- **AppDev D2 Final Deliverable:**
  - **Heavy Load:** A cross-platform **React Native mobile prototype** featuring multi-screen stack/tab navigation, `<FlatList>` data rendering, client-side API fetching (with Deadlock Rule fallback), and local session/preference persistence via `AsyncStorage` ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Single-screen React Native interface rendering fetched/mocked API items inside a `<FlatList>` with interactive `<Pressable>` cards.
- **GameDev D2 Final Deliverable:**
  - **Heavy Load:** A networked **Multiplayer Unity Scene** where a Host and at least 1 Client connect in a lobby, control only their own player instance (`IsOwner`), synchronize real-time movement across both windows, and trigger a server-authoritative `[Rpc(SendTo.Server)]` action that updates a shared `NetworkVariable` (e.g., FPS hit counter or Card Game turn/mana state) ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Host + Client connection with `IsOwner` movement synchronization across two local instances.

## 5. Cross-Domain Peer Audit & Submission Portal

- **Submit Your Pull Request Here:** Place your deliverable inside [`submissions/Module_07/<Track>/<your-github-handle>/`](./submissions/Module_07/README.md), open your PR following the **[BGSC PR & Submission Guide](./PR_And_Submission_Guide.md)**, and log your PR URL in the [Official Submission GForm](https://forms.gle/CYJWGiw8ivvS5SWT8).
