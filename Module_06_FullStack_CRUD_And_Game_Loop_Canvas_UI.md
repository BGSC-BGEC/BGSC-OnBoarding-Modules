# Module 6: Closing the Loop - Full-Stack CRUD Wiring, Canvas HUDs, Game Loops & Capstone Handover

> **[Curriculum Master Hub](./00_Curriculum_Master_Hub.md)** / **[PR & Submission Guide](./PR_And_Submission_Guide.md)** / **[Module 6 Submissions (`submissions/Module_06/`)](./submissions/Module_06/README.md)**

- **Sprint Dates:** Nov 23 – Nov 28, 2026 / **Estimated Workload:** 4 to 5 Hours Max
- **Deadline 1 (Soft Checkpoint):** Nov 28, 2026 / **Deadline 2 (Final Verification):** Dec 01, 2026
- **Prerequisite:** [Modules 1–5](./00_Curriculum_Master_Hub.md) / **Track Options:** Heavy Load or Maintenance Load

## Previously on BGSC Onboarding...

Look how far you have come since mid-October when you only knew `git status`:

- **AppDev Crew:** You built an Express + PostgreSQL + JWT backend (Modules 1–3) and a reactive React frontend with `useState` and `useEffect` (Modules 4–5). **Today is the grand marriage:** you are wiring your React forms directly to your authenticated Express + PostgreSQL endpoints so a user can Create, Read, Update, and Delete (**CRUD**) real database rows end-to-end!
- **GameDev Crew:** You built C# OOP hierarchies, `MonoBehaviour` transforms, `Rigidbody` physics controllers, New Input System raycasters, and `ScriptableObject` prefabs (Modules 1–5). **Today you turn a tech demo into an actual Video Game:** you are building a responsive **Unity Canvas HUD** (Health/Mana sliders, `TextMeshPro` score counters), a **Game Loop State Machine** (Start -> Play -> Win/Loss), and automatic scene transitions with `SceneManager`!

## 1. Why Are We Learning This? ("The Why")

- **AppDev — Why Full-Stack Integration Is Where 90% of Beginner Bugs Hide (CORS, Controlled Forms & Optimistic UI):**
  - When your React app (`http://localhost:5173`) tries to send a `POST` request to your Express server (`http://localhost:3000`), your browser will immediately scream in red letters: **`Blocked by CORS policy: No 'Access-Control-Allow-Origin' header`**. Why? Because browsers block random websites from secretly calling other servers on your behalf! Learning how to configure **CORS**, manage **Controlled Forms** in React, attach your **JWT Bearer Token** to headers, and update UI state after a `POST`/`DELETE` completes the full-stack circle ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
- **GameDev — Why Game Design Loops, Canvas UI & Event Decoupling Matter:**
  - A sandbox where you shoot targets forever with no goal, no health bar, and no win/loss screen isn't a game—it's a screensaver ([MDA Framework](https://deliberategamedesign.com/mda-framework/)). Every great **FPS** (round timer, health/armor HUD, kill feed, match victory screen) and **Card Game** (mana crystal counter, turn button, deck count, victory/defeat banner) relies on **Unity Canvas UI (`TextMeshPro` + `Slider`)** and **`SceneManager`** to close the core gameplay loop.

## 2. Track A: Application Development - Controlled Forms, CORS & End-to-End CRUD

### What You Need to Understand

#### A. Fixing CORS on Express & Sending JWT Headers from React

On your Express backend, enable the `cors` middleware locked to your frontend origin:

```javascript
// Backend: server.js
import cors from "cors";
app.use(cors({ origin: "http://localhost:5173", credentials: true }));
```

#### B. Controlled React Forms & Full-Stack `POST` + State Sync

In a **Controlled Form**, React state is the single source of truth for every `<input>` field. When the user submits the form:

1. Call `e.preventDefault()` so the browser doesn't do a 1998 full-page refresh!
2. Validate the fields client-side.
3. Send the `POST` request with `Authorization: Bearer ${token}` and `Content-Type: application/json`.
4. Append the newly created database record returned by Express straight into your React state (`setItems(prev => [createdRow, ...prev])`) so the screen updates instantaneously!

```jsx
// Frontend: src/components/CreateMatchForm.jsx
import { useState } from "react";

export function CreateMatchForm({ token, onMatchCreated }) {
  const [title, setTitle] = useState("");
  const [gameCategory, setGameCategory] = useState("Valorant");
  const [statusMsg, setStatusMsg] = useState({ type: "", text: "" });
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (title.trim().length < 3) {
      return setStatusMsg({ type: "error", text: "Match title must be at least 3 characters." });
    }

    setIsSubmitting(true);
    setStatusMsg({ type: "", text: "" });

    try {
      const res = await fetch("http://localhost:3000/api/matches", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({ title: title.trim(), game_category: gameCategory })
      });

      const payload = await res.json();
      if (!res.ok) throw new Error(payload.error || "Failed to create match");

      // Clear form and notify parent list view!
      setTitle("");
      setStatusMsg({ type: "success", text: "Match saved to PostgreSQL!" });
      onMatchCreated(payload.data);
    } catch (err) {
      setStatusMsg({ type: "error", text: err.message });
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Match Title (e.g., Quarterfinal #1)"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
      />
      <select value={gameCategory} onChange={(e) => setGameCategory(e.target.value)}>
        <option value="Valorant">Valorant</option>
        <option value="Chess">Chess</option>
        <option value="FIFA">FIFA</option>
      </select>
      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? "Saving..." : "Create Match"}
      </button>
      {statusMsg.text && <p className={statusMsg.type}>{statusMsg.text}</p>}
    </form>
  );
}
```

### Curated Top-Rated Learning Resources (AppDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [React Official Docs: Reacting to Input with State](https://react.dev/learn) | Explains controlled inputs, form states (empty, typing, submitting, success, error), and avoiding redundant state. | Complete *Managing State: Reacting to Input with State*. | 45 mins |
| [Class Central / freeCodeCamp: PERN Stack Course (Postgres, Express, React, Node)](https://www.classcentral.com/course/freecodecamp-pern-stack-course-postgres-express-react-and-node-105011) | Shows the exact end-to-end wiring of PostgreSQL + Express + React CRUD forms. | Watch/review the *Connecting React Frontend Form to Express/Postgres CRUD* section. | 60 mins |
| [PlateMate_Frontend PR #1](https://github.com/Nikunj1310/PlateMate_Frontend/pull/1) | Shows how our club's production app wires Create/Update/Delete API calls with token headers and error states. | Review the *Post/Food Inventory API* and *Claims API* sections. | 30 mins |

## 3. Track B: Game Development - Unity Canvas UI, HUDs, Game Design Loops & `SceneManager`

### What You Need to Understand

#### A. Setting Up a Responsive Unity Canvas (`Scale With Screen Size`)

Beginner trap #1 in Unity UI: leaving the `Canvas Scaler` set to **`Constant Pixel Size`**. When you build your game on a 1080p laptop and open it on a 4K monitor, your health bar shrinks to the size of a postage stamp!

- **Always set `Canvas Scaler -> UI Scale Mode` to `Scale With Screen Size`** (Reference Resolution: `1920 x 1080`, Match: `0.5`).
- Use **`TextMeshPro (TMP_Text)`** for crisp text at any resolution, and **`Slider`** for FPS health/shield bars or Card Game mana meters!

#### B. Game Loop State Machine & `SceneManager`

Never check health or win conditions inside 15 scattered scripts. Build a single **`GameManager`** that tracks the game state (`Playing`, `Victory`, `GameOver`), updates the Canvas HUD, and reloads or transitions scenes via `UnityEngine.SceneManagement.SceneManager`:

```csharp
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshPro!

public class GameLoopManager : MonoBehaviour
{
    [Header("HUD UI References")]
    public Slider healthSlider;
    public TMP_Text scoreOrManaText;
    public GameObject endScreenPanel;
    public TMP_Text endScreenTitle;

    [Header("Game Rules")]
    public int maxHealth = 100;
    public int targetScoreToWin = 50;

    private int _currentHealth;
    private int _currentScore;
    private bool _isGameOver;

    private void Start()
    {
        _currentHealth = maxHealth;
        _currentScore = 0;
        _isGameOver = false;

        if (endScreenPanel != null) endScreenPanel.SetActive(false);
        UpdateHud();
    }

    public void AddScore(int points)
    {
        if (_isGameOver) return;
        _currentScore += points;
        UpdateHud();

        if (_currentScore >= targetScoreToWin)
        {
            TriggerEndState(won: true);
        }
    }

    public void ApplyDamage(int dmg)
    {
        if (_isGameOver) return;
        _currentHealth = Mathf.Max(0, _currentHealth - dmg);
        UpdateHud();

        if (_currentHealth <= 0)
        {
            TriggerEndState(won: false);
        }
    }

    private void UpdateHud()
    {
        if (healthSlider != null)
            healthSlider.value = (float)_currentHealth / maxHealth;
        if (scoreOrManaText != null)
            scoreOrManaText.text = $"Score: {_currentScore} / {targetScoreToWin}";
    }

    private void TriggerEndState(bool won)
    {
        _isGameOver = true;
        if (endScreenPanel != null) endScreenPanel.SetActive(true);
        if (endScreenTitle != null)
            endScreenTitle.text = won ? "VICTORY! MISSION COMPLETE" : "DEFEAT! TRY AGAIN";
    }

    // Bind this method directly to your Canvas UI 'Restart' Button's OnClick() event!
    public void RestartCurrentScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }
}
```

### Curated Top-Rated Learning Resources (GameDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Unity Learn: Junior Programmer Pathway (Unit 5: User Interface)](https://learn.unity.com/pathway/junior-programmer) | Complete walkthrough of Canvas UI, `TextMeshPro`, Buttons, Game Over screens, and `SceneManager.LoadScene`. | Complete *Unit 5: User Interface* (Lessons 5.1 to 5.4). | 90 mins |
| [Game Programming Patterns: State & Observer Patterns](https://gameprogrammingpatterns.com/design-patterns-revisited.html) | Explains why decoupling gameplay events (like taking damage) from UI updates keeps your game modular. | Read the *Observer* and *State* pattern overviews. | 30 mins |
| [MDA Framework: Mechanics, Dynamics & Aesthetics](https://deliberategamedesign.com/mda-framework/) | Helps you audit your playable mini-game loop: are the goals, feedback (HUD), and win/loss tension clear to the player? | Review the feedback loop diagram and apply it to your mini-game. | 20 mins |

## 4. Step-by-Step Sprint Action Plan ("What To Do")

### Phase 1 & 2 - **DEADLINE 1 (Soft Checkpoint: Nov 28, 2026)**

- **AppDev D1 Deliverable:** Push working CORS configuration and a Controlled React Form that successfully sends a `POST` request to your Express + PostgreSQL backend and logs the created row. Open `[WIP][Module 6 - D1]` PR.
- **GameDev D1 Deliverable:** Push a Unity scene with a scaled Canvas HUD (`Slider` + `TMP_Text`) and a `RestartCurrentScene()` button wired via `SceneManager`. Open `[WIP][Module 6 - D1]` PR.

### Phase 3 & 4 - **DEADLINE 2 (Final Verification: Dec 1, 2026)**

- **AppDev D2 Final Deliverable:**
  - **Heavy Load:** A complete end-to-end **Full-Stack CRUD Web Application** where a user can log in (JWT), submit new records via a validated React form into PostgreSQL, view the live list dynamically, and update or delete an item with real-time UI feedback ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** End-to-end Create (`POST`) + Read (`GET`) web flow persisting data into PostgreSQL and updating the React UI without a page reload.
- **GameDev D2 Final Deliverable:**
  - **Heavy Load:** A complete, playable **Mini-Game Vertical Slice** (either an FPS target/survival arena or a turn-based Card/Tactical table) combining player movement/raycasting, `ScriptableObject` pickups/cards, real-time Canvas HUD (`Slider` health/mana bar + `TMP_Text` score), win/loss state triggers, and one-click scene reset via `SceneManager` ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Playable scene with real-time HUD update on pickup/damage and automatic `SceneManager` reset on win/loss.

## 5. Bonus Phase (Nov 22 – Nov 28, 2026): Fall Handover & Winter Capstone Pitch Prep

As outlined in the [Onboarding Framework (Section 6 & 10)](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA), completing Module 6 unlocks the **Fall Handover & Idea Board Pitch Window (Nov 22–28)** before comprehensives:

1. **Clean Repo Handover:** Export your Postman API documentation (AppDev) or pass your Graybox Prefab Audit (GameDev).
2. **Declare Your Specialization:** GameDev crew declare their **2D vs. 3D** focus; AppDev crew declare **Frontend, Backend, Infra, or Full-Stack Bridge** + choose their **Lead / Shadow** pairings.
3. **Draft an Idea Board Pitch (Optional):** Have a cool feature idea for the BGSC App or Studio Game? Post a pitch using the **MVT (Minimum Viable Team)** template (`The Why`, `The Stack`, `The MVT`, `The Resource Link`).

## 6. Cross-Domain Peer Audit & Submission Portal

- **Submit Your Pull Request Here:** Place your deliverable inside [`submissions/Module_06/<Track>/<your-github-handle>/`](./submissions/Module_06/README.md), open your PR following the **[BGSC PR & Submission Guide](./PR_And_Submission_Guide.md)**, and log your PR URL in the [Official Submission GForm](https://forms.gle/jmkKbVeNDdqhPLBSA).
