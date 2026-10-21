# Module 4: Faces, Lasers & AI Co-Pilots - React Fundamentals, AI-Assisted Frontend, Monitoring & Raycasting

> **[Curriculum Master Hub](./00_Curriculum_Master_Hub.md)** | **[PR & Submission Guide](./PR_And_Submission_Guide.md)** | **[Module 4 Submissions (`submissions/Module_04/`)](./submissions/Module_04/README.md)**

- **Sprint Dates:** Nov 11 – Nov 16, 2026 | **Estimated Workload:** 3 to 4 Hours Max
- **Deadline 1 (Soft Checkpoint):** Nov 16, 2026 | **Deadline 2 (Final Verification):** Nov 19, 2026
- **Prerequisite:** [Module 3 (Auth API / Physics Controller)](./Module_03_API_Security_Auth_And_Physics_Vector_Math.md) | **Track Options:** Heavy Load or Maintenance Load

## Previously on BGSC Onboarding...

In **Module 3**, you locked down the engine room:

- **AppDev Crew:** You built a secure, `bcrypt`-hashed, JWT-protected Express + PostgreSQL backend. **The problem?** No normal human wakes up and says, *"Gee, I can't wait to order food or check esports tournament brackets by typing `curl -X POST` in a terminal!"* Your backend needs a visual face: a **Frontend**.
- **GameDev Crew:** You got a physics sphere rolling and jumping around a 3D obstacle course using `Vector3` and `Rigidbody`. **The problem?** Right now, your sphere can bump into walls, but it doesn't *do* anything when it hits a power-up, steps on a trap, or fires a laser! Today, you unlock **Unity's New Input System**, **Triggers/Collisions**, and **Raycasting**—the invisible laser beam that powers both **FPS guns** and **3D Card selection**.

## 1. Why Are We Learning This? ("The Why")

- **AppDev — Why Learn Manual React First, Then Add AI & Frontend Monitoring?**
  1. **Why React Components?** Writing raw 1999 HTML means copy-pasting the same navbar and card markup 50 times. **React** ([react.dev](https://react.dev/learn)) lets you build reusable Lego blocks (**Components**) driven by data (**Props**).
  2. **The Golden Rule of AI in Frontend:** Yes, AI tools like [GitHub Copilot](https://docs.github.com/en/copilot/get-started/best-practices) can generate a shiny Tailwind/React card in 3 seconds. **So why must you write basic React components manually first?** Because if you don't understand component trees, keys, and props yourself, you can't tell when the AI just hallucinated an infinite re-render loop that melts your user's phone battery! In this module, you will write your core framework components by hand first, then learn how to prompt AI for repetitive styling/layout variations—and verify it using **Frontend Performance Monitoring** ([Sentry Core Web Vitals](https://sentry.io/for/web-vitals/)).
- **GameDev — Why the New Input System, Triggers & Raycasting?**
  - Hardcoding `if (KeyCode.Space)` breaks the moment a player plugs in a PlayStation controller or rebinds their keys. Unity's **Input System Package** ([Unity Input System Docs](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/)) separates *Actions* ("Fire", "PlayCard") from physical keys.
  - And **Raycasting (`Physics.Raycast`)** shoots an invisible laser beam from point A in direction B and tells you *what* it hit. In an **FPS**, a raycast from the center of your camera *is* a hitscan bullet (like Valorant's Vandal). In a **3D Card Game**, a raycast from the mouse cursor into the 3D scene tells you *which card on the table the player is hovering or dragging*!

## 2. Track A: Application Development - React Components, Manual Mastery, AI Workflow & Monitoring

### What You Need to Understand

#### A. Manual Mastery First: JSX, Functional Components, Props & List Rendering

A React component is literally just a JavaScript function that takes an object of inputs (**`props`**) and returns UI markup (**JSX**):

```jsx
// src/components/TournamentCard.jsx — Written and understood manually FIRST!
export function TournamentCard({ title, game, prizePool, isLive, onJoin }) {
  return (
    <article className="tournament-card">
      <header>
        <span className={isLive ? "badge-live" : "badge-upcoming"}>
          {isLive ? "LIVE NOW" : "UPCOMING"}
        </span>
        <h3>{title}</h3>
      </header>
      <p>Game: <strong>{game}</strong> | Prize: ₹{prizePool}</p>
      <button onClick={() => onJoin(title)}>
        Register Squad
      </button>
    </article>
  );
}

// src/App.jsx — Composing a Component Tree & Rendering Lists with Unique Keys
import { TournamentCard } from "./components/TournamentCard";

const MOCK_TOURNAMENTS = [{ id: "t1", title: "BGEC Valorant Cup", game: "Valorant", prizePool: 15000, isLive: true },
  { id: "t2", title: "Grandmaster Blitz", game: "Chess", prizePool: 5000, isLive: false }
];

export default function App() {
  const handleJoin = (tournamentTitle) => {
    console.log(`Clicked join for: ${tournamentTitle}`);
  };

  return (
    <main>
      <h1>BGSC Active Leagues</h1>
      <section className="card-grid">
        {MOCK_TOURNAMENTS.map((item) => (
          // CRITICAL: Always pass a unique, stable 'key' prop (never array index!)
          <TournamentCard
            key={item.id}
            title={item.title}
            game={item.game}
            prizePool={item.prizePool}
            isLive={item.isLive}
            onJoin={handleJoin}
          />
        ))}
      </section>
    </main>
  );
}
```

#### B. How to Incorporate AI in Frontend Without Writing "AI Slop"

Once you can write `App.jsx` and `TournamentCard.jsx` by hand, here is the professional **3-Step AI Frontend Workflow** ([GitHub Copilot Best Practices](https://docs.github.com/en/copilot/get-started/best-practices)):

1. **You Write the Skeleton & Props Contract Manually:** Define the component name, the exact props (`{ title, game, prizePool, isLive }`), and the semantic HTML structure yourself.
2. **Delegate Repetitive UI Polish to AI:** Ask your AI assistant to generate responsive CSS Grid/Flexbox rules, dark-mode color tokens, or accessible `aria-*` attributes for your component.
3. **Audit Every Line Before Committing:** Check that the AI did not:
   - Use array indices (`key={index}`) as React keys (causes subtle UI bugs when lists reorder).
   - Put inline object creations or heavy synchronous loops inside the render body.
   - Hallucinate non-existent npm packages.

#### C. Frontend Monitoring: Core Web Vitals & Error Boundaries

How do you know if your React frontend (or AI-generated CSS/JS) is actually fast and bug-free on real student laptops? You monitor three **Core Web Vitals** ([Web Vitals in React](https://dev.to/munna_thakur_2019444f0351/web-vitals-in-react-the-complete-guide-to-measuring-and-optimizing-performance-2026-5aj3) & [Sentry Web Vitals](https://sentry.io/for/web-vitals/)):

- **LCP (Largest Contentful Paint < 2.5s):** How fast the main content appears on screen.
- **INP (Interaction to Next Paint < 200ms):** How fast the UI responds when a user clicks a button.
- **CLS (Cumulative Layout Shift < 0.1):** Does the button jump down 200px right as the user tries to click it because an image loaded without dimensions?

### Curated Top-Rated Learning Resources (AppDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [React Official Docs: Quick Start (`react.dev/learn`)](https://react.dev/learn) | The redesigned official React documentation is universally praised as one of the best interactive programming guides ever created. | Complete *Quick Start*, *Describing the UI* (Your First Component, Passing Props to a Component, Rendering Lists). | 90 mins |
| [MDN Web Docs: Getting Started with React](https://developer.mozilla.org/en-US/docs/Learn_web_development/Core/Frameworks_libraries/React_getting_started) | Great beginner bridge explaining how Vite scaffolds a React app and how JSX compiles to DOM elements. | Read the full overview and scaffold your Vite + React repo. | 30 mins |
| [GitHub Docs: Best Practices for Using GitHub Copilot](https://docs.github.com/en/copilot/get-started/best-practices) | Official engineering guide on context prompting, code review, and avoiding AI pitfalls. | Read the full best-practices guide (15 mins). | 15 mins |
| [Sentry: Core Web Vitals Monitoring](https://sentry.io/for/web-vitals/) & [Web Vitals in React Guide](https://dev.to/munna_thakur_2019444f0351/web-vitals-in-react-the-complete-guide-to-measuring-and-optimizing-performance-2026-5aj3) | Explains LCP, INP, CLS, and how to measure real-world React frontend health in Chrome DevTools Lighthouse & Sentry. | Read the Core Web Vitals definitions and run a Lighthouse check in Chrome DevTools (`F12 -> Lighthouse`). | 30 mins |

## 3. Track B: Game Development - Unity New Input System, Collisions, Triggers & Raycasting

### What You Need to Understand

#### A. Collisions (`OnCollisionEnter`) vs. Triggers (`OnTriggerEnter`)

Every tangible object in Unity has a **Collider** component (`BoxCollider`, `SphereCollider`, `MeshCollider`):

- **Physical Collision (`Is Trigger = UNCHECKED`):** Solid walls, floors, and shields. Objects bounce off each other, and Unity calls `OnCollisionEnter(Collision collision)`.
- **Volumetric Trigger (`Is Trigger = CHECKED`):** Ghost zones! Players walk right through them without slowing down, and Unity calls `OnTriggerEnter(Collider other)`. Use this for **FPS health pickups**, **capture-the-flag zones**, or **lava damage pits**!

#### B. Raycasting (`Physics.Raycast`): The Swiss Army Knife of FPS & Card Games

Imagine holding a laser pointer at your camera's eye position (`transform.position`) and shining it straight ahead (`transform.forward`):

- **In an FPS:** When the player clicks `Fire`, you cast a ray from the center of the camera. If `Physics.Raycast` hits a collider on the `Enemy` layer within `100f` meters, you call `hit.collider.GetComponent<Health>()?.TakeDamage(35)`!
- **In a Card Game:** When the player moves their mouse over the screen, `Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue())` converts the 2D mouse pixel into a 3D laser pointing into the game board to highlight or pick up a Card!

```csharp
using UnityEngine;
using UnityEngine.InputSystem; // Unity's New Input System!

public class RaycastAndTriggerInteraction : MonoBehaviour
{
    [Header("Raycast Configuration")]
    public float maxRayDistance = 50f;
    public LayerMask interactableLayers; // Filter so we don't shoot invisible trigger zones!

    // Called by Unity's New Input System when the 'Fire' / 'Select' action is pressed
    public void OnFire(InputValue value)
    {
        if (!value.isPressed) return;

        // Create a ray starting at this object's position pointing forward
        Ray ray = new Ray(transform.position, transform.forward);

        // Draw a visible red laser in the Scene View for 1 second so you can debug it!
        Debug.DrawRay(ray.origin, ray.direction * maxRayDistance, Color.red, 1.0f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxRayDistance, interactableLayers))
        {
            Debug.Log($"[RAYCAST HIT] Hit '{hitInfo.collider.name}' at distance {hitInfo.distance:F2}m!");

            // Example: Destroy target dummy or select a 3D Card on the table
            if (hitInfo.collider.CompareTag("Target"))
            {
                Destroy(hitInfo.collider.gameObject);
            }
        }
    }

    // Fires automatically when entering a Collider with "Is Trigger" checked!
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HazardZone"))
        {
            Debug.Log($"[TRIGGER] Entered Hazard Zone: {other.name}!");
        }
    }
}
```

### Curated Top-Rated Learning Resources (GameDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Unity Manual: Input System Package Quick Start Guide](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/QuickStartGuide.html) & [Latest Input System Docs](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/) | Official documentation for setting up Input Action Assets, `PlayerInput` components, and callbacks. | Complete the *Quick Start Guide* to bind keyboard/mouse actions cleanly. | 60 mins |
| [Unity Learn: Junior Programmer Pathway (Unit 2 & 3)](https://learn.unity.com/pathway/junior-programmer) | Covers `OnTriggerEnter`, `OnCollisionEnter`, Tags, and object destruction with visual exercises. | Complete the *Collisions and Triggers* lessons in Unit 2/3. | 60 mins |
| [CodeHS / MDA Game Design Overview](https://codehs.com/course/28561/explore/module/41944) | Connects input responsiveness and spatial feedback to player game feel. | Review how instant visual/console feedback on input shapes player dynamics. | 20 mins |

## 4. Step-by-Step Sprint Action Plan ("What To Do")

### Phase 1 & 2 - **DEADLINE 1 (Soft Checkpoint: Nov 16, 2026)**

- **AppDev D1 Deliverable:** Scaffold a Vite + React project (`npm create vite@latest`), manually write at least **2 reusable functional components** accepting `props`, and push your static layout mockup. Open `[WIP][Module 4 - D1]` PR.
- **GameDev D1 Deliverable:** Install Unity's **Input System** package via Package Manager, set up a trigger zone (`Is Trigger = true`) that logs `OnTriggerEnter` when your player enters it, and open `[WIP][Module 4 - D1]` PR.

### Phase 3 & 4 - **DEADLINE 2 (Final Verification: Nov 19, 2026)**

- **AppDev D2 Final Deliverable:**
  - **Heavy Load:** A responsive React UI application composed of at least **3 modular functional components** rendering a dynamic list with proper `key` props, interactive click handlers, a brief comment/note in your PR showing **1 component you wrote 100% manually vs. 1 CSS/UI enhancement you pair-programmed with AI**, AND a screenshot/report of your Chrome DevTools **Lighthouse Core Web Vitals (LCP, CLS)** score ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Responsive React layout with 2 reusable components rendering a list via `.map()` and `props`.
- **GameDev D2 Final Deliverable:**
  - **Heavy Load:** A playable Unity scene using the **New Input System** where the player triggers events on entering volumetric trigger zones (`OnTriggerEnter`) AND fires a `Physics.Raycast` (using `LayerMask` filtering) to either hit FPS targets or select/highlight 3D Cards on a table ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Playable scene with 1 `OnTriggerEnter` zone and 1 working `Physics.Raycast` interaction.

## 5. Cross-Domain Peer Audit & Submission Portal

- **Reviewer Checklist:**
  - [ ] **AppDev:** Zero missing `key` warnings in the browser console; developer can explain every prop and JSX line in their PR audit; Lighthouse CLS is `< 0.1`.
  - [ ] **GameDev:** Uses LayerMask or Tags cleanly; `Debug.DrawRay` is visible in Scene View; no hardcoded legacy input polling where New Input System actions are bound.
- **Submit Your Pull Request Here:** Place your deliverable inside [`submissions/Module_04/<Track>/<your-github-handle>/`](./submissions/Module_04/README.md), open your PR following the **[BGSC PR & Submission Guide](./PR_And_Submission_Guide.md)**, and log your PR URL in the Official Submission GForm.
