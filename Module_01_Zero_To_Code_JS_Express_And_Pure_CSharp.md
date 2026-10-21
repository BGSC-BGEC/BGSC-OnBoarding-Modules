# Module 1: Teaching Rocks to Think : Core Logic, Express Servers & Pure C# OOP

> **[Curriculum Master Hub](./00_Curriculum_Master_Hub.md)** | **[PR & Submission Guide](./PR_And_Submission_Guide.md)** | **[Module 1 Submissions (`submissions/Module_01/`)](./submissions/Module_01/README.md)**

- **Sprint Dates:** Oct 21 – Oct 29, 2026 | **Estimated Workload:** 3 to 4 Hours Max
- **Deadline 1 (Soft Checkpoint):** Oct 29, 2026 | **Deadline 2 (Final Verification):** Nov 01, 2026

- **Prerequisite:** [Week 1 Git Workshop / Module 0](./Module_00_Git_And_GitHub_Revision_Guide.md) | **Track Options:** Heavy Load (Full Spec) or Maintenance Load (Core Subset)

## Previously on BGSC Onboarding...

Welcome back from the Mid-Semester Exam Freeze! Before the freeze, in **Week 1**, you learned **Git and GitHub** ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)). You installed VS Code, set up SSH keys, configured Git LFS for chunky files, and resolved your first merge conflict without setting your laptop on fire.

Wait—let's address the elephant in the room: **until right now, you have only used Git to move text files around.** You haven't actually written code yet. If the words "variable," "array," "asynchronous," or "inheritance" sound like legal terms from a dramatic soap opera, take a deep breath. This module starts from **Absolute Zero** and takes you all the way to running your first backend web server (AppDev) or your first object-oriented battle simulation in the terminal (GameDev).

## 1. Zero-to-Code Crash Course: How Programming Actually Works (For Both Tracks)

Before splitting into AppDev and GameDev, every developer needs the same 20-minute mental model of what code is. A computer CPU is basically a very fast, very literal toddler. It does **exactly** what you tell it to do, in order, millions of times per second. Every programming language on Earth (JavaScript, C#, Python, Rust) is built out of just **five building blocks**:

| Building Block | Real-World Analogy | AppDev (JavaScript) Example | GameDev (C#) Example |
| :--- | :--- | :--- | :--- |
| **1. Variables** | Labelled Tupperware boxes in your fridge that hold data (numbers, text, true/false). | `const maxPlayers = 10;` | `int maxHealth = 100;` |
| **2. Conditionals (`if/else`)** | A bouncer at a club door checking a condition before letting code run. | `if (age >= 18) { allowEntry(); }` | `if (health <= 0) { Die(); }` |
| **3. Loops (`for`/`while`)** | Doing pushups until a counter hits 10 so you don't copy-paste code 10 times. | `for (const item of cart) { ... }` | `for (int i = 0; i < 5; i++) { ... }` |
| **4. Functions / Methods** | A blender: you drop ingredients in (**parameters**), it blends them, and hands you a smoothie (**return value**). | `const add = (a, b) => a + b;` | `int Add(int a, int b) { return a + b; }` |
| **5. Data Structures (Arrays/Objects)** | An egg carton (ordered list of items) or an ID card (key-value properties). | `const scores = [10, 25, 40];` | `List<string> deck = new List<string>();` |

## 2. Why Are We Learning This Without Fancy Visuals First? ("The Why")

- **Why no React UI or Unity 3D Graphics in Week 1?** If you try to learn how a car engine works while driving 140 km/h on a highway in heavy rain, you will crash into a tree. Both **React** (in AppDev) and the **Unity Editor** (in GameDev) wrap your code in massive visual frameworks. When beginners jump straight into clicking buttons in Unity or copying HTML templates, they panic the moment an error pops up because they cannot tell where the *language* ends and the *engine* begins.
- **Why Asynchronous JS & Express for AppDev?** Modern web and mobile apps spend 95% of their life waiting, waiting for databases, waiting for slow campus Wi-Fi, waiting for payment gateways. If your server froze every time one user requested their profile, 100 users would crash your app. Learning `async/await` and **Express.js** routing teaches you how servers juggle thousands of requests at once.
- **Why Pure C# OOP in the Terminal for GameDev?** Whether you are building an **FPS** (where Pistols, Shotguns, and Sniper Rifles all share `Weapon` behavior), a **Card Game** (where Minion Cards, Spell Cards, and Trap Cards all inherit from a base `Card` class), or a **Multiplayer Lobby**, **Object-Oriented Programming (OOP)** is the skeleton of every game ever made.

## 3. Track A: Application Development - JS Core (ES6+) & Express.js Architecture

### What You Need to Understand

#### A. Modern JavaScript (ES6+) & Array Superpowers

In JavaScript, you declare variables with `const` (cannot be reassigned, use this 95% of the time!) or `let` (can change later, like a counter). Never use `var` (it's a haunted relic from 1995).

When your backend handles a list of tournament players or food orders, you don't write clunky `for` loops, you use **Higher-Order Array Methods** (`.map()`, `.filter()`, `.reduce()`):

```javascript
// Imagine this is our raw tournament player list in memory
const players = [{ id: 1, handle: "Viper", mmr: 2400, active: true },
  { id: 2, handle: "Ghost", mmr: 1100, active: false },
  { id: 3, handle: "Phoenix", mmr: 2850, active: true }
];

// 1. .filter() keeps only items where the function returns true
const activePlayers = players.filter(player => player.active === true);

// 2. .map() transforms every item into a new shape (e.g., extracting handles)
const leaderboardNames = activePlayers.map(player => `${player.handle} (${player.mmr} MMR)`);

// 3. .reduce() boils an entire array down to a single value (e.g., total MMR)
const totalMmr = activePlayers.reduce((sum, player) => sum + player.mmr, 0);
```

#### B. Asynchronous JavaScript (`Promises` & `async/await`)

JavaScript runs on a **single thread** (one waiter serving an entire restaurant). If Table 1 orders a pizza that takes 15 minutes to bake, a **synchronous** waiter stands in front of the oven staring at the cheese melting for 15 minutes while Tables 2 through 20 starve. An **asynchronous** waiter takes the order, gets a **Promise** buzzer from the kitchen, serves other tables, and comes back when the buzzer rings (`await`).

```javascript
// Simulating a 1-second database lookup using a Promise
const fetchUserFromFakeDB = (userId) => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (userId <= 0) reject(new Error("Invalid User ID!"));
      else resolve({ id: userId, role: "Crew_Developer" });
    }, 1000);
  });
};

// Using async/await with try/catch so errors never crash our server
const handleProfileRequest = async (id) => {
  try {
    console.log("Waiter sent order to kitchen...");
    const user = await fetchUserFromFakeDB(id);
    console.log("Order arrived:", user);
  } catch (error) {
    console.error("Kitchen caught fire:", error.message);
  }
};
```

#### C. The Request-Response Cycle & Express.js REST APIs

Every web or mobile app talks to a backend server using **HTTP Verbs** over a **REST API**:

- `GET`: "Read data" (e.g., `GET /api/matches` — show me upcoming matches).
- `POST`: "Create new data" (e.g., `POST /api/matches` — register a new match result).
- `PUT` / `PATCH`: "Update existing data" (e.g., `PUT /api/matches/42` — update score).
- `DELETE`: "Remove data" (e.g., `DELETE /api/matches/42` — cancel match).

```javascript
// server.js (using ES6 Modules: set "type": "module" in package.json)
import express from "express";

const app = express();
app.use(express.json()); // Middleware that lets Express read JSON request bodies!

const matches = [{ id: 1, game: "Valorant", teamA: "Bitsians", teamB: "Titans", score: "13-9" }
];

// 1. GET endpoint with async handler and array filtering
app.get("/api/matches", async (req, res) => {
  const { game } = req.query;
  const result = game ? matches.filter(m => m.game.toLowerCase() === game.toLowerCase()) : matches;
  res.status(200).json({ count: result.length, data: result });
});

// 2. POST endpoint to add a match
app.post("/api/matches", async (req, res) => {
  const { game, teamA, teamB, score } = req.body;
  if (!game || !teamA || !teamB) {
    return res.status(400).json({ error: "Missing required match fields!" });
  }
  const newMatch = { id: matches.length + 1, game, teamA, teamB, score: score || "0-0" };
  matches.push(newMatch);
  res.status(201).json({ message: "Match created!", data: newMatch });
});

app.listen(3000, () => console.log("Server live on http://localhost:3000"));
```

### Curated Top-Rated Learning Resources (AppDev)

We have filtered out the 40-hour bloated tutorials. Study only these specific, industry-standard resources to stay well within your **3–4 hour weekly cap**:

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [The Modern JavaScript Tutorial (javascript.info)](https://javascript.info/) | Universally acknowledged by senior JS engineers as the clearest, best-structured interactive guide to JS on the internet. | Part 1: *JavaScript Fundamentals* (Variables, Comparisons, Arrow Functions), *Data Types* (`Array methods`), and *Promises, async/await*. | 90 mins |
| [MDN Web Docs: Express/Node Introduction](https://developer.mozilla.org/en-US/docs/Learn_web_development/Extensions/Server-side/Express_Nodejs/Introduction) | Mozilla's official web documentation—the canonical encyclopedia of web development. | Read *Express/Node introduction* to understand how Node's event loop and HTTP request-response cycles work. | 30 mins |
| [Express.js Official Basic Routing Guide](https://expressjs.com/en/5x/starter/basic-routing/) & [Full Routing Guide](https://expressjs.com/en/5x/guide/routing/) | Straight from the creators of Express. Zero fluff, exact syntax for route parameters (`req.params`), query strings (`req.query`), and handlers. | Complete both short pages and test a `GET` and `POST` route locally. | 30 mins |
| [freeCodeCamp: Node.js and Express.js Full Course](https://www.youtube.com/watch?v=Oe421EPjeBE) | Over 5M+ views; taught by John Smilga with crystal-clear visual breakdowns for absolute beginners. | Optional video alternative if you prefer watching code being typed: watch the **Express Basics & API vs SSR** section only. | 45 mins |

## 4. Track B: Game Development - Pure C# Fundamentals, OOP & Game Rules

### What You Need to Understand

#### A. Game Design Principle #1: Rules Are Just Code Before It's Compiled

Before you write C#, think like a Game Designer ([MDA Framework](https://deliberategamedesign.com/mda-framework/)). Every game—whether it's **Valorant (FPS)**, **Slay the Spire / Hearthstone (Card Game)**, or **Chess**—starts with **Mechanics (Rules)**:

- What entities exist in the world? (*Players, Weapons, Cards, Enemies*)
- What stats do they hold? (*HP, ManaCost, AmmoCount, Armor*)
- What actions can they take, and how do those actions change the stats? (*Attack, PlayCard, Reload, TakeDamage*)

In **Object-Oriented Programming (OOP)**, we model those exact entities using **Classes** (blueprints) and **Objects** (actual instances spawned from those blueprints).

#### B. The Four Pillars of OOP (Built for FPS & Card Games)

1. **Encapsulation (`public`, `private`, `protected`):** Never let random scripts reach inside a Player and set `health = -99999`. Keep internal state `protected` or `private` and expose safe methods like `TakeDamage(int amount)`.
2. **Inheritance (`:`):** Instead of copy-pasting `name`, `manaCost`, and `Play()` into 50 different card scripts or 20 different FPS weapon scripts, create one base `Entity` or `Card` class and let child classes inherit from it!
3. **Polymorphism (`virtual` and `override`):** A base `Card` class has a `virtual void ExecuteEffect()` method. A `FireballCard` overrides it to deal damage; a `HealCard` overrides it to restore HP. Your game loop just calls `card.ExecuteEffect()` without caring which card it is!

```csharp
using System;
using System.Collections.Generic;

namespace GameDevModule1
{
    // Base Class: Could be a character in an FPS or a Unit/Card on a board!
    public class Entity
    {
        public string Name { get; private set; }
        public int Health { get; protected set; }

        // Constructor: runs automatically when we spawn a new Entity
        public Entity(string name, int startingHealth)
        {
            Name = name;
            Health = startingHealth;
        }

        // 'virtual' means child classes are allowed to customize this behavior!
        public virtual void PerformAction(Entity target)
        {
            Console.WriteLine($"{Name} stares menacingly at {target.Name}.");
        }

        public void TakeDamage(int dmg)
        {
            Health = Math.Max(0, Health - dmg);
            Console.WriteLine($"  -> {Name} took {dmg} damage! Remaining HP: {Health}");
        }
    }

    // Child Class 1: Inherits from Entity using ':'
    public class FpsSoldier : Entity
    {
        public int Ammo { get; private set; }

        public FpsSoldier(string name, int health, int ammo) : base(name, health)
        {
            Ammo = ammo;
        }

        // 'override' replaces the base PerformAction with FPS shooting logic!
        public override void PerformAction(Entity target)
        {
            if (Ammo > 0)
            {
                Ammo--;
                Console.WriteLine($"[FPS] {Name} fires a burst at {target.Name}! (Ammo left: {Ammo})");
                target.TakeDamage(25);
            }
            else
            {
                Console.WriteLine($"[FPS] *Click!* {Name} is out of ammo and reloads!");
                Ammo = 5;
            }
        }
    }

    // Child Class 2: A Card Mage who casts spells using Mana
    public class CardSummoner : Entity
    {
        public int Mana { get; private set; }

        public CardSummoner(string name, int health, int mana) : base(name, health)
        {
            Mana = mana;
        }

        public override void PerformAction(Entity target)
        {
            if (Mana >= 3)
            {
                Mana -= 3;
                Console.WriteLine($"[CARD] {Name} plays 'Arcane Bolt' (Cost: 3 Mana) on {target.Name}!");
                target.TakeDamage(40);
            }
            else
            {
                Console.WriteLine($"[CARD] {Name} draws a card and channels +2 Mana.");
                Mana += 2;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Polymorphism in action: a single List<Entity> holds different child classes!
            List<Entity> arena = new List<Entity>
            {
                new FpsSoldier("Vanguard-7", 100, 2),
                new CardSummoner("Archmage Lyra", 80, 5)
            };

            Console.WriteLine("=== ROUND 1 FIGHT ===");
            arena[0].PerformAction(arena[1]);
            arena[1].PerformAction(arena[0]);
        }
    }
}
```

### Curated Top-Rated Learning Resources (GameDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Microsoft Learn: Write Your First Code Using C#](https://learn.microsoft.com/en-us/training/paths/get-started-c-sharp-part-1/) | Microsoft invented C#. Their interactive browser sandbox lets you write C# without even waiting for an install. | Complete *Write your first C# code*, *Store and retrieve data*, and *Perform basic operations*. | 60 mins |
| [Microsoft Learn: Classes and Objects Tutorial](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/tutorials/classes) & [A Tour of C#](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/overview) | The official step-by-step console tutorial for building a C# class hierarchy with inheritance and overrides. | Follow the console bank account / OOP inheritance walkthrough end-to-end. | 60 mins |
| [C# Fundamentals for Absolute Beginners (Microsoft Learn Video Series)](https://learn.microsoft.com/en-us/shows/csharp-fundamentals-for-absolute-beginners/) | Bob Tabor's legendary beginner series—famous for turning total non-coders into confident C# programmers. | Watch episodes on *If Decision Statements*, *Arrays*, *Helper Methods*, and *Understanding Classes*. | 45 mins |
| [MDA Framework: Deliberate Game Design](https://deliberategamedesign.com/mda-framework/) | Explains the most famous academic & industry framework for Game Design: **Mechanics -> Dynamics -> Aesthetics**. | Read the 10-minute breakdown of how code rules (Mechanics) create player experiences (Aesthetics). | 15 mins |

## 5. Step-by-Step Sprint Action Plan ("What To Do")

### Phase 1: Setup & Zero-to-Code Practice

- **AppDev:** Install Node.js LTS, run `npm init -y` in a new Git branch `feat/module-01-<your-handle>`, install Express (`npm install express`), and practice `.map()`, `.filter()`, `.reduce()`, and `async/await` in a scratch file.
- **GameDev:** Install the .NET SDK, run `dotnet new console -n Module01OOP` in your branch `feat/module-01-<your-handle>`, and test creating a basic class with a constructor.

### Phase 2 - **DEADLINE 1 (Soft Checkpoint: Oct 29, 2026)**

- **AppDev D1 Deliverable:** Push your initial `server.js` with at least **2 working route definitions** (`GET` and `POST`) and an array manipulation helper function. Open your `[WIP][Module 1 - D1]` Pull Request.
- **GameDev D1 Deliverable:** Push your initial `Program.cs` containing your base `Entity` (or `Card`/`Weapon`) class and at least **1 child class** with a constructor. Open your `[WIP][Module 1 - D1]` Pull Request.

### Phase 3 & 4 - **DEADLINE 2 (Final Verification: Nov 1, 2026)**

- **AppDev D2 Final Deliverable:**
  - **Heavy Load:** An Express.js server exposing **at least 3 distinct REST API endpoints** (`GET`, `POST`, `DELETE` or filter route) using `async/await` handlers, higher-order array methods (`.map()`, `.filter()`, `.reduce()`), and proper HTTP status codes (`200`, `201`, `400`, `404`) with `try/catch` error handling .
  - **Maintenance Load (Exam/Eval Option):** 2 REST endpoints (`GET` + `POST`) with `.filter()` and clean JSON responses.
- **GameDev D2 Final Deliverable:**
  - **Heavy Load:** A standalone, warning-free C# console application demonstrating a **3-class inheritance hierarchy** (1 Base Class + at least 2 Child Classes with `virtual`/`override` methods and `private`/`protected` encapsulation) simulating either a 3-turn **FPS skirmish** or a 3-turn **Card Game duel** in the terminal .
  - **Maintenance Load (Exam/Eval Option):** Base class + 1 child class demonstrating method overriding and safe health/stat modification.

## 6. Cross-Domain Peer Audit & Submission Portal

Before merging by **November 1, 2026**, tag your assigned peer reviewer:

- **Reviewer Checklist:**
  - [ ] Zero `var` keywords in JS / zero exposed mutable fields where `private`/`protected` belongs in C#.
  - [ ] Code is placed strictly inside [`submissions/Module_01/<Track>/<your-github-handle>/`](./submissions/Module_01/README.md) and runs cleanly from a fresh `git clone` with a `README.md` explaining how to run it in 2 commands.
- **Submit Your Pull Request Here:** Open your PR into [`submissions/Module_01/`](./submissions/Module_01/README.md) following the step-by-step instructions and PR template in the **[BGSC PR & Submission Guide](./PR_And_Submission_Guide.md)**, and log your PR URL in the [GForm Submission Link](https://forms.gle/huEdmBvN6xeAx2aLA).
