# Module 2: Giving Your Code a Memory & a Body - PostgreSQL Schemas & Unity Engine Architecture

> **[Curriculum Master Hub](./00_Curriculum_Master_Hub.md)** | **[PR & Submission Guide](./PR_And_Submission_Guide.md)** | **[Module 2 Submissions (`submissions/Module_02/`)](./submissions/Module_02/README.md)**

- **Sprint Dates:** Oct 30 – Nov 04, 2026 | **Estimated Workload:** 4 to 5 Hours Max
- **Deadline 1 (Soft Checkpoint):** Nov 04, 2026 | **Deadline 2 (Final Verification):** Nov 07, 2026

- **Prerequisite:** [Module 1 (Express Routing / Pure C# OOP)](./Module_01_Zero_To_Code_JS_Express_And_Pure_CSharp.md) | **Track Options:** Heavy Load or Maintenance Load

## Previously on BGSC Onboarding...

In **Module 1**, you went from zero coding experience to writing real logic!

- **AppDev Crew:** You built an Express.js server that served JSON over REST routes using `async/await` and `.map()`/`.filter()`. **But there was a hilarious fatal flaw:** every time you hit `Ctrl + C` to restart your server, *every single item you POSTed vanished into thin air* because JavaScript arrays live in temporary RAM!
- **GameDev Crew:** You built a C# OOP battle between classes in the black-and-white terminal. It worked great—if your target audience was 1978 mainframe operators. Now it is time to take those exact C# classes and plug them into a real-time 3D graphics engine: **Unity**.

## 1. Why Are We Learning This? ("The Why")

- **AppDev — Why Relational Databases (PostgreSQL) Instead of Just Saving a `.json` Text File?** If two users buy the last concert ticket at the exact same millisecond, a plain text file will corrupt itself or sell the ticket twice. **PostgreSQL** enforces strict rules (**Schemas**, **Primary Keys**, **Foreign Keys**, and **ACID Transactions**) so your data never turns into soup—even if 10,000 users hit your server at once ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
- **GameDev — Why the Component Pattern (`GameObject` + `MonoBehaviour`) & Grayboxing?** In Module 1, you learned Inheritance (`FpsSoldier : Entity`). Big game engines discovered a problem with deep inheritance trees: what if you want a *Flying Indestructible Exploding Barrel Card*? Inheritance gets messy fast. Unity solves this with the **Component Pattern** ([Game Programming Patterns](https://gameprogrammingpatterns.com/component.html)): every object in your scene is an empty container (`GameObject`) that you snap Lego bricks (**Components** like `Transform`, `MeshRenderer`, and your custom C# `MonoBehaviour` scripts) onto! Plus, you'll learn **Grayboxing**—building fun levels with ugly primitive cubes first so you never waste 3 weeks waiting on 3D artists ([Deadlock Rule](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).

## 2. Track A: Application Development - Relational Database Design & PostgreSQL

### What You Need to Understand

#### A. Tables, Primary Keys, Foreign Keys & Normalization

Think of a relational database as a collection of super-strict Excel spreadsheets (**Tables**) that can link to each other:

- **Primary Key (`PRIMARY KEY`):** A unique ID card for every row (e.g., `id SERIAL PRIMARY KEY` or `UUID`). Two people can be named "Rahul", but they will never share the same Primary Key.
- **Foreign Key (`REFERENCES`):** A pointer in one table that points to the Primary Key of another table. Instead of typing a user's name and email inside every single order they place (which wastes space and breaks if they change their email—a sin called **Data Redundancy**), we store `user_id INTEGER REFERENCES users(id)`. Splitting data cleanly into linked tables is called **Normalization**.

```sql
-- schema.sql: A normalized 2-table schema for our Esports / Food App
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(120) UNIQUE NOT NULL,
    reputation_score INTEGER DEFAULT 100,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE tournament_matches (
    id SERIAL PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    game_category VARCHAR(50) NOT NULL,
    creator_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    scheduled_at TIMESTAMP NOT NULL
);

-- Querying linked data across both tables using an INNER JOIN
SELECT m.title, m.game_category, u.username AS organizer
FROM tournament_matches m
INNER JOIN users u ON m.creator_id = u.id
WHERE m.game_category = 'Valorant';
```

#### B. Connecting Node.js to PostgreSQL

Your Express server from Module 1 talks to PostgreSQL using either the `pg` (node-postgres) driver or a modern type-safe ORM like **Prisma** ([Prisma Docs](https://www.prisma.io/docs/getting-started)). Notice how we use **Parameterized Queries (`$1, $2`)**—never string concatenation—to stop hackers from injecting rogue SQL commands!

```javascript
import pg from "pg";
const pool = new pg.Pool({
  connectionString: process.env.DATABASE_URL // e.g., postgres://user:pass@localhost:5432/devsoc_db
});

// Safe parameterized query inside an async function
export const createUser = async (username, email) => {
  const query = `
    INSERT INTO users (username, email)
    VALUES ($1, $2)
    RETURNING id, username, email, created_at;
  `;
  const result = await pool.query(query, [username, email]);
  return result.rows[0];
};
```

### Curated Top-Rated Learning Resources (AppDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Neon PostgreSQL Tutorial](https://neon.com/postgresql/tutorial) & [Getting Started](https://neon.com/postgresql/getting-started) | The cleanest, most practical interactive PostgreSQL guide for web developers. | Complete *Querying Data*, *Filtering Data*, *Joining Multiple Tables*, and *Creating Tables / Constraints*. | 90 mins |
| [Official PostgreSQL 18 Tutorial](https://www.postgresql.org/docs/current/tutorial.html) | The official PostgreSQL manual—authoritative reference for SQL syntax and relational concepts. | Read *Chapter 2: The SQL Language* and *Chapter 3: Advanced Features (Foreign Keys)*. | 45 mins |
| [freeCodeCamp: Introduction to SQL and PostgreSQL](https://www.freecodecamp.org/learn/introduction-to-sql-and-postgresql) | Hands-on curriculum built specifically for first-time database learners. | Complete the relational database schema and foreign key lessons. | 45 mins |
| [Prisma ORM Getting Started Docs](https://www.prisma.io/docs/getting-started) | Shows how modern Node/TypeScript teams model schemas and run migrations in production. | Optional bonus read (20 mins) to see how `schema.prisma` maps to SQL tables. | 20 mins |

## 3. Track B: Game Development - Unity Engine Architecture, Lifecycles & Grayboxing

### What You Need to Understand

#### A. The 4 Pillars of the Unity Editor Interface

When you open Unity, don't let the 47 buttons intimidate you. Only **four windows** matter right now:

1. **Hierarchy Window:** The roster of every `GameObject` currently alive in your active Scene (cameras, lights, floor planes, cards, players).
2. **Scene View vs. Game View:** *Scene View* is the director's backstage set where you move objects around; *Game View* is what the player actually sees through the `Main Camera`.
3. **Inspector Window:** Click any `GameObject` in the Hierarchy, and the Inspector shows all the **Components** attached to it (starting with `Transform`: Position `(X, Y, Z)`, Rotation, and Scale).
4. **Project Window:** Your actual hard-drive folder (`Assets/`) holding your C# scripts, materials, and prefabs.

#### B. `MonoBehaviour` & The Frame Lifecycle (`Awake`, `Start`, `Update`, `FixedUpdate`)

In Module 1, your C# program started at `static void Main()`, ran top-to-bottom once, and exited. **Games don't exit after 1 millisecond!** A game is an infinite loop that redraws the screen 60 to 144 times every single second (**Frames Per Second / FPS**). When your C# class inherits from `MonoBehaviour` instead of a plain class, Unity automatically calls special lifecycle methods on your script:

```csharp
using UnityEngine;

// Inheriting from MonoBehaviour lets us drag this script onto ANY GameObject in Unity!
public class FloatingArtifact : MonoBehaviour
{
    [Header("Movement Settings (Editable in Inspector!)")]
    public float rotationSpeed = 90f; // Degrees per second
    public float bobSpeed = 2f;
    public float bobHeight = 0.5f;

    private Vector3 _startPosition;

    // 1. Awake() runs FIRST as soon as the object is loaded (great for internal references)
    private void Awake()
    {
        Debug.Log($"[Awake] {gameObject.name} woke up!");
    }

    // 2. Start() runs ONCE before the very first frame (great for reading initial positions)
    private void Start()
    {
        _startPosition = transform.position;
    }

    // 3. Update() runs EVERY SINGLE FRAME (60+ times/sec).
    // CRITICAL RULE: Always multiply movement by Time.deltaTime so a 240 FPS PC
    // doesn't move your object 4x faster than a 60 FPS laptop!
    private void Update()
    {
        // Rotate smoothly around the Y (vertical) axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        // Bob up and down using a smooth Sine wave
        float newY = _startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);
    }

    // 4. FixedUpdate() runs on a strict fixed timer (default 50 times/sec) -> used in Module 3 for Physics!
    private void FixedUpdate()
    {
        // Physics calculations go here next week!
    }
}
```

#### C. Genre Blueprint: How `Transform` Powers Both FPS & Card Games

- **In an FPS:** Rotating a radar dish, recoiling a gun barrel, or bobbing a health pack pickup uses `transform.position` and `transform.Rotate` with `Time.deltaTime`.
- **In a Card Game:** Smoothly hovering a card up on the table when selected or flipping a card 180 degrees face-up when drawn is just interpolating its `Transform` position and rotation!

### Curated Top-Rated Learning Resources (GameDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Unity Learn: Junior Programmer Pathway](https://learn.unity.com/pathway/junior-programmer) | Unity's official flagship beginner curriculum—interactive, project-based, and constantly updated for Unity 6. | Complete *Unit 1: Player Control* (Editor setup, Hierarchy, Inspector, `transform.Translate`, `Time.deltaTime`). | 90 mins |
| [Catlike Coding: Building a Graph & Movement Basics](https://catlikecoding.com/unity/tutorials/) | Revered across the Unity community for teaching *how* Unity math and transforms work under the hood without hand-waving. | Read the introductory *Basics: Building a Graph* tutorial to see how C# scripts manipulate `Transform` components. | 45 mins |
| [Game Programming Patterns: Component Pattern](https://gameprogrammingpatterns.com/component.html) | Robert Nystrom's legendary free book on game architecture—explains *why* Unity uses `GameObject` + `Component` instead of deep inheritance. | Read the *Component* chapter (15 mins). | 20 mins |
| [Brackeys: How to Make a Video Game in Unity (E01–E02)](https://www.youtube.com/watch?v=9ZEu_I-ido4) | The most iconic beginner Unity video series on YouTube for quick visual orientation. | Watch E01 (Editor setup & primitives) and E02 (First `MonoBehaviour` script). | 25 mins |

## 4. Step-by-Step Sprint Action Plan ("What To Do")

### Phase 1 & 2 - **DEADLINE 1 (Soft Checkpoint: Nov 4, 2026)**

- **AppDev D1 Deliverable:** Push your raw `schema.sql` containing at least **2 tables** with Primary Keys and a Foreign Key link. Open your `[WIP][Module 2 - D1]` PR.
- **GameDev D1 Deliverable:** Set up your Unity project with the proper `.gitignore` and `.gitattributes` (from Week 1!), create a graybox scene with primitive 3D shapes (Cubes, Spheres, Cylinders), and push your scene with a basic `MonoBehaviour` logging `Awake()` and `Start()`. Open your `[WIP][Module 2 - D1]` PR.

### Phase 3 & 4 - **DEADLINE 2 (Final Verification: Nov 7, 2026)**

- **AppDev D2 Final Deliverable:**
  - **Heavy Load:** A normalized `schema.sql` file defining **at least 2 linked tables** with `PRIMARY KEY`, `FOREIGN KEY ... REFERENCES`, and `NOT NULL` constraints, PLUS a seed/query script (`db.js` or `seed.sql`) connected to your Express server from Module 1 that inserts and queries joined records ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Complete `schema.sql` with 2 linked tables and 3 verified SQL queries (`INSERT`, `SELECT`, `INNER JOIN`).
- **GameDev D2 Final Deliverable:**
  - **Heavy Load:** A grayboxed Unity scene (e.g., an FPS arena blockout or a 3D Card Table layout made of primitive shapes) where at least **3 primitive GameObjects** dynamically modify their position, rotation, and scale at runtime using custom C# `MonoBehaviour` scripts with frame-rate independent `Time.deltaTime` math ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** 1 Unity scene with 2 primitives moving/rotating smoothly via a parameterized `MonoBehaviour` script.

## 5. Cross-Domain Peer Audit & Submission Portal

- **Reviewer Checklist:**
  - [ ] **AppDev:** Foreign keys have explicit `ON DELETE` behavior; zero hardcoded database passwords in Git (must use `.env` + `.env.example`!).
  - [ ] **GameDev:** `Library/` and `Temp/` folders are ignored via `.gitignore`; all `Update()` movement multiplies by `Time.deltaTime`.
- **Submit Your Pull Request Here:** Place your deliverable inside [`submissions/Module_02/<Track>/<your-github-handle>/`](./submissions/Module_02/README.md), open your PR following the **[BGSC PR & Submission Guide](./PR_And_Submission_Guide.md)**, and log your PR URL in the Official Submission GForm.
