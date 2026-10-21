# BGSC Technical Onboarding Hub (2026-2027)

> **[PR and Submission Guide](./PR_And_Submission_Guide.md)** | **[Module 0: Git and GitHub Revision Guide](./Module_00_Git_And_GitHub_Revision_Guide.md)** | **[Module Submissions Directory (`submissions/`)](./submissions/README.md)**

Welcome to the official GitHub Pages Curriculum Hub for **Application Development (Web, Mobile, Backend, DevOps & AI)** and **Game Development (Unity, C#, Game Design, Multiplayer & Dedicated Servers)**, built upon the [Project Team & Game Dev Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA).

> "We build in modules. If one module fails, the system stays online. Communicate early, code often, and respect the 'Tap Out'." : **The Crew Mantra**

---

## 1. Who Is This Curriculum Built For?

You. Yes, you.
The person who just survived Week 1's Git workshop, learned how to type `git commit -m "please work"`, and currently thinks **C#** is a musical note, **Express** is a train, **React** is what you do when midsem grades drop, and **Kafka** is just that existential author with the giant bug.

### Our Starting Assumption

- You know basic **Git and GitHub** (cloning a repo, creating branches, making commits, and opening a Pull Request).
- You know **absolutely zero programming or coding**. We explain what variables, memory, loops, functions, servers, databases, and game engines actually are from scratch before asking you to architect systems.

### Where You Will Be by January 30, 2027

- **AppDev Track End Goal:** You will be able to build, understand, and monitor modern **React** web frontends and **React Native** mobile apps (writing core framework code manually first, then supercharging your workflow with AI tools like [GitHub Copilot](https://docs.github.com/en/copilot/get-started/best-practices) and monitoring performance with [Sentry Core Web Vitals](https://sentry.io/for/web-vitals/)). On the backend, you will manually design and code secure **Node.js & Express** microservices, model relational data in **PostgreSQL** and document data in **MongoDB**, implement manual security hardening (**bcrypt**, **JWT**, [OWASP API Security Top 10](https://github.com/OWASP/API-Security)), connect event-driven systems using **REST APIs**, **Apache Kafka**, and **RabbitMQ**, apply backend optimization algorithms (indexing, caching, rate limiting), and deploy everything with **Docker** and **GitHub Actions CI/CD**.
- **GameDev Track End Goal:** You will master **C# Object-Oriented Programming** and **Unity Engine** architecture, apply core **Game Design Principles** (the [MDA Framework](https://deliberategamedesign.com/mda-framework/), Core Loops, Grayboxing, and Game Feel), and build architecture capable of powering **First-Person Shooters (FPS)**, **Card Games**, and **Real-Time Multiplayer Games**. You will understand Unity's physics engine, vector math, raycasting, `ScriptableObject` data pipelines, [Netcode for GameObjects](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@latest/), lag compensation ([Client-Side Prediction & Server Reconciliation](https://www.gabrielgambetta.com/client-side-prediction-server-reconciliation.html)), game backend services, and **Dockerized Linux Headless Dedicated Game Servers**.

---

## 2. How the System Works: Rules of Survival

Before you touch a single line of code, memorize the operational rules from our [Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA). These exist so that academics never destroy your mental health and one delayed teammate never blocks the entire squad:

| Rule / Mechanism | How It Works in Practice | Why It Exists |
| :--- | :--- | :--- |
| **5-Hour Weekly Cap** | Every module is strictly scoped to **3 to 5 hours per week** maximum. | You are a full-time student first. Burnout writes terrible code. |
| **Capacity Points (Heavy vs. Maintenance)** | Each week, declare **Heavy Load** (full feature build) or **Maintenance Load** (isolated sub-component or bugfix during heavy academic weeks). | Keeps you shipping consistently even when evaluations hit. |
| **The Dual-Deadline System** | **Deadline 1 (Day 4 Soft Checkpoint):** Push unpolished baseline logic, raw schemas, or graybox mechanics. **Deadline 2 (Day 9 Final Verification):** Push linted, error-handled, peer-audited code via PR. | Kills "Sunday Night Panic" and catches architectural mistakes on Day 4 instead of Day 9. |
| **Reliefs & The Purge Method** | You get **2 to 3 Deadline Reliefs** in module phase. Exhausting all reliefs triggers a **3-4 day Assignment** on core basics + choosing **1-2 Specificities** with 1 final relief buffer. Missing assignment = offboarding. | Protects active builders while giving honest second chances for academic emergencies. |
| **The Deadlock Rule** | **AppDev:** If Backend is delayed, Frontend *must* build against hardcoded JSON Mock Data. **GameDev:** If 3D art or player code is delayed, designers *must* build with primitive cubes (**Grayboxing**). | Nobody ever sits idle saying "I'm waiting on the other team." |
| **Spaghetti Prevention (Cross-Domain Peer Audit)** | Every Pull Request must be audited by a peer from a complementary domain before merging (e.g., Frontend audits Backend routes; Game Designer audits Programmer's Prefab hierarchy). | Ensures that if someone taps out for exams, another human understands how their code works. |
| **The Shadow Rule & Tap Out** | Every critical feature has a **Lead** and a **Shadow** (who reviews every PR). Need a week off for exams? Call a **Tap Out** or post a **Canary Handover** doc, zero guilt, zero penalty as long as you document where you stopped. | Silences the 24-hour ghosting problem and keeps the pipeline moving. |

---

## 3. The Realistic Weekly Sprint Cadence

Every module follows a 4-phase rhythm so you never cram 5 hours of coding into 11:45 PM before the deadline (taking an example of a 7-day duration):

| Sprint Phase | Days | Focus Area | AppDev Track Action | GameDev Track Action |
| :--- | :--- | :--- | :--- | :--- |
| **Phase 1** | Days 1-2 | **Concept & Core Logic** | Read curated docs, write core JS/Backend logic or DB schema. | Read curated docs, write pure C# classes or core math/physics logic. |
| **Phase 2 (Deadline 1)** | Days 3-4 | **Prototype & Soft Checkpoint** | Push working baseline route/schema/mockup PR for **Deadline 1**. | Push working console script or graybox Unity scene PR for **Deadline 1**. |
| **Phase 3** | Days 5-7 | **Integration, Security & Audit** | Add validation, security, error handling, or AI/monitoring polish + Peer Audit. | Wire prefabs, input callbacks, UI bindings, or netcode sync + Peer Audit. |
| **Phase 4 (Deadline 2)** | Days 8-9 | **Final Verification & Merge** | Pass Cross-Domain PR Audit and merge final deliverable by **Deadline 2**. | Export playable build/scene, pass Prefab Audit, and merge by **Deadline 2**. |

---

## 4. Master Parallel Timetable and Module Directory (Oct 13, 2026 - Jan 30, 2027)

Use this table as your master tracker for your GitHub Pages deployment. Each module has its own dedicated Markdown guide and submission folder in this repository:

| Module & Guide Link | Sprint Window | AppDev Focus | GameDev Focus | Deadline 1 (Soft) | Deadline 2 (Final) | Submission Folder |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **[Module 0](./Module_00_Git_And_GitHub_Revision_Guide.md)** | Pre-Requisite / Ref | CLI Survival, Git Config, SSH, Branching, Rebasing & Merge Conflicts | CLI Survival, Git Config, SSH, Branching, Rebasing & Merge Conflicts | Self-Paced | Self-Paced | Practice Lab |
| **[Module 1](./Module_01_Zero_To_Code_JS_Express_And_Pure_CSharp.md)** | Oct 21 - Oct 29, 2026 | Zero-to-Code, ES6+ JS, Async/Await, REST APIs & Express Setup | Zero-to-Code, Pure C# CLI, OOP Classes, Inheritance & Game Rules | Oct 29, 2026 | Nov 01, 2026 | [`submissions/Module_01/`](./submissions/Module_01/README.md) |
| **[Module 2](./Module_02_Postgres_Schemas_And_Unity_Engine_Architecture.md)** | Oct 30 - Nov 04, 2026 | Relational DB Design, PostgreSQL Schemas, Foreign Keys & SQL | Unity Editor, GameObjects, Transforms, MonoBehaviour & MDA Design | Nov 04, 2026 | Nov 07, 2026 | [`submissions/Module_02/`](./submissions/Module_02/README.md) |
| **[Module 3](./Module_03_API_Security_Auth_And_Physics_Vector_Math.md)** | Nov 05 - Nov 10, 2026 | Controllers, Middleware, Manual Security, Bcrypt & JWT Auth | 3D Vector Math, Rigidbody Physics, Forces & FPS/3D Movement | Nov 10, 2026 | Nov 13, 2026 | [`submissions/Module_03/`](./submissions/Module_03/README.md) |
| **[Module 4](./Module_04_React_AI_Frontend_And_Input_Raycasting.md)** | Nov 11 - Nov 16, 2026 | React Fundamentals, Manual UI Code, AI-Assisted Dev & Monitoring | Unity New Input System, Collisions, Triggers & FPS/Card Raycasting | Nov 16, 2026 | Nov 19, 2026 | [`submissions/Module_04/`](./submissions/Module_04/README.md) |
| **[Module 5](./Module_05_Async_State_Mocking_And_Prefabs_ScriptableObjects.md)** | Nov 17 - Nov 22, 2026 | React Hooks (`useState`/`useEffect`), Axios, Error States & Mocking | Prefabs, Object Pooling, `ScriptableObjects` (Card Decks & FPS Guns) | Nov 22, 2026 | Nov 25, 2026 | [`submissions/Module_05/`](./submissions/Module_05/README.md) |
| **[Module 6](./Module_06_FullStack_CRUD_And_Game_Loop_Canvas_UI.md)** | Nov 23 - Nov 28, 2026 | Full-Stack CRUD Wiring, Controlled Forms & Capstone Handover | Canvas HUD (`TextMeshPro`), Turn/Real-Time Game Loops & Scene Reset | Nov 28, 2026 | Dec 01, 2026 | [`submissions/Module_06/`](./submissions/Module_06/README.md) |
| **[Module 7](./Module_07_React_Native_Mobile_And_Multiplayer_Netcode.md)** | Jan 06 - Jan 11, 2027 | React Native, Expo Navigation, Mobile State & Token Storage | Multiplayer Topologies, Unity Netcode, RPCs & Lobby State Sync | Jan 11, 2027 | Jan 14, 2027 | [`submissions/Module_07/`](./submissions/Module_07/README.md) |
| **[Module 8](./Module_08_Mongo_Kafka_Backend_Optimization_And_Networked_Physics.md)** | Jan 12 - Jan 17, 2027 | Dual DBs (Postgres + MongoDB), Kafka/RabbitMQ Bus & Backend Algos | Client Prediction, Server Reconciliation, Lag Comp & Game Backend | Jan 17, 2027 | Jan 20, 2027 | [`submissions/Module_08/`](./submissions/Module_08/README.md) |
| **[Module 9](./Module_09_Docker_DevOps_Security_And_Dedicated_Game_Servers.md)** | Jan 18 - Jan 23, 2027 | Docker Compose, OWASP Hardening, CI/CD & Microservices Design | Headless Dedicated Unity Servers, Docker, Matchmaking & Cloud Deploy | Jan 23, 2027 | Jan 26, 2027 | [`submissions/Module_09/`](./submissions/Module_09/README.md) |

---

## 5. Standard Submission Links and Pull Request Protocol

For every single module, submissions are tracked through GitHub Pull Requests following the **[BGSC PR and Submission Guide](./PR_And_Submission_Guide.md)** and [Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA) workflow:

- **Step-by-Step PR Tutorial & Template:** Read the **[BGSC Pull Request and Module Submission Guide](./PR_And_Submission_Guide.md)** for terminal commands, folder rules, and a copy-pasteable PR description template.
- **Submission Folder Structure:** Place your deliverable strictly inside `submissions/Module_0X/<Track>/<your-github-handle>/` (see [`submissions/`](./submissions/README.md)) so your PR never conflicts with other members or modifies the root GitHub Pages curriculum files.
- **Submission Portal (GitHub PRs + GForm):** Open a Pull Request from your feature branch (`feat/module-XX-<your-handle>`) into `main` of this repository and link your PR URL inside the Official Submission GForm.
- **Deadline 1 Tag:** Open your PR as a Draft/WIP PR titled `[WIP][Module X - D1] <Track>: <Your Name>` by **23:59 IST on Day 4**.
- **Deadline 2 Tag:** Mark your PR Ready for Review, attach your **Cross-Domain Peer Auditor**, and update the title to `[READY][Module X - D2] <Track>: <Your Name>` by **23:59 IST on Day 9**.

**(Here `<Track>` is the domain you targeted: `AppDev` or `GameDev`)**
