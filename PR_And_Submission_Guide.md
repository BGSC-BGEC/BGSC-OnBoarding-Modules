# BGSC Pull Request and Module Submission Guide

> **[Back to Curriculum Master Hub](./00_Curriculum_Master_Hub.md)** | **[Module 0: Git and GitHub Revision Guide](./Module_00_Git_And_GitHub_Revision_Guide.md)** | **[Browse Submissions Directory](./submissions/README.md)**

Welcome to the official **BGSC Submission and Pull Request Protocol**. Every module in the [BGSC Technical Onboarding Hub](./00_Curriculum_Master_Hub.md) is submitted, peer-audited, and verified through **GitHub Pull Requests (PRs)** directly inside this repository, followed by logging your PR link in the **[Official Submission GForm](https://forms.gle/WDUghqipgjs8TDDc7)**.

If you ever forget a Git command while following this guide, keep [Module 0: The Ultimate Git, CLI, SSH and Merge Conflict Revision Guide](./Module_00_Git_And_GitHub_Revision_Guide.md) open in another tab.

---

## 1. How This GitHub Pages Repository Works

This repository serves two purposes at the same time:

1. **The Live Curriculum Website (GitHub Pages):** All the Markdown files in the root folder (`00_Curriculum_Master_Hub.md`, `Module_00_...md` through `Module_09_...md`, and this guide) automatically render into our club's **GitHub Pages Curriculum Website**.
2. **The Central Submission Portal (`submissions/`):** The [`submissions/`](./submissions/README.md) directory holds every member's code deliverables across all 9 modules.

### The Two Golden Rules of the Repository

1. **NEVER Edit or Delete Root Curriculum Files:** Do not modify `00_Curriculum_Master_Hub.md`, `README.md`, or any `Module_0X_*.md` file in the root directory. Editing root files in your PR will alter the live GitHub Pages website for the entire club and your PR will be rejected.
2. **Stay Inside Your Personal Handle Folder:** Every file you create or push **must** live inside your own personal folder under the specific module and track:

```text
submissions/Module_0X/<Track>/<your-github-handle>/
```

**Why?** When 50+ builders open Pull Requests into `main` at the same time, isolating your code inside `<your-github-handle>/` guarantees **zero Merge Conflicts** with your teammates.

---

## 2. Repository and Submission Folder Structure

Here is how the repository is organized for all modules (`Module_01` to `Module_09`):

```text
BGSC-OnBoarding-Modules/                               <-- Repo Root (Published via GitHub Pages)
|-- README.md / 00_Curriculum_Master_Hub.md        <-- Master Timetable & Rules Hub
|-- PR_And_Submission_Guide.md                     <-- This Submission Guide
|-- Module_00_Git_And_GitHub_Revision_Guide.md     <-- Week 1 Git & CLI Revision
|-- Module_01_Zero_To_Code_JS_Express_And_Pure_CSharp.md
|-- ...
|-- Module_09_Docker_DevOps_Security_And_Dedicated_Game_Servers.md
|
`-- submissions/                                   <-- ALL STUDENT SUBMISSIONS GO HERE
    |-- README.md
    |-- Module_01/
    |   |-- AppDev/
    |   |   `-- rahul-sharma/                      <-- Example AppDev Member Folder
    |   |       |-- README.md                      <-- Required: Load type, setup & proof
    |   |       |-- package.json
    |   |       `-- server.js
    |   `-- GameDev/
    |       `-- priya-patel/                       <-- Example GameDev Member Folder
    |           |-- README.md                      <-- Required: Load type, setup & proof
    |           |-- Module01OOP.csproj
    |           `-- Program.cs
    |-- Module_02/
    |   |-- AppDev/
    |   `-- GameDev/
    |-- ...
    `-- Module_09/
        |-- AppDev/
        `-- GameDev/
```

> **Note on Ignored Files:** Never commit generated or secret files inside your submission folder: `node_modules/`, `.env` (commit `.env.example` instead), Unity `Library/`, `Temp/`, `Obj/`, `Builds/`, or C# `bin/` and `obj/` folders.

---

## 3. Step-by-Step Walkthrough: How to Make Your PR and Submit a Module

Below is a complete real-world example for **Module 1** assuming your GitHub username is `rahul-sharma` targeting the **AppDev** track (or `priya-patel` on the **GameDev** track).

### Step 1: Sync `main` and Create Your Feature Branch

Always start from an up-to-date `main` branch and create a dedicated feature branch named `feat/module-XX-<your-github-handle>`:

```bash
# 1. Move into your cloned repo folder
cd BGSC-OnBoarding-Modules

# 2. Switch to main and pull the latest curriculum and folder updates
git switch main
git pull origin main

# 3. Create and switch to your Module 1 feature branch
git switch -c feat/module-01-rahul-sharma
```

### Step 2: Create Your Personal Folder Inside `submissions/Module_0X/<Track>/`

Create your folder under `AppDev` or `GameDev` (or both if you are attempting both tracks):

```bash
# For AppDev Track (Module 01):
mkdir -p submissions/Module_01/AppDev/rahul-sharma
cd submissions/Module_01/AppDev/rahul-sharma

# Or for GameDev Track (Module 01):
# mkdir -p submissions/Module_01/GameDev/priya-patel
# cd submissions/Module_01/GameDev/priya-patel
```

### Step 3: Build Your Deliverable and Add a Submission `README.md`

Inside `submissions/Module_01/AppDev/rahul-sharma/`, write your code (`server.js`, `package.json`, etc.) and create a brief `README.md` so your **Cross-Domain Peer Auditor** can test your code in under 2 minutes:

```markdown
# Module 01 Submission - Rahul Sharma (@rahul-sharma)

- **Track:** AppDev
- **Capacity Load:** Heavy Load (Full Spec)
- **How to Run:**
  1. `cd submissions/Module_01/AppDev/rahul-sharma && npm install`
  2. `node server.js`
- **Endpoints Tested:**
  - `GET /api/matches` (Returns filtered matches)
  - `POST /api/matches` (Validates body and appends match)
  - `DELETE /api/matches/:id` (Removes match by ID)
```

> **Special Note for GameDev Unity Modules (Modules 2-9):**
> Because full Unity projects contain large assets, place the following inside `submissions/Module_0X/GameDev/<your-handle>/`:
>
> 1. Your custom C# `Scripts/` (and `Prefabs/` or `Scenes/` if lightweight).
> 2. A `README.md` containing a link to your standalone Unity repository (if hosted separately) **PLUS** a 30-60 second screen recording GIF or video link demonstrating your scene and mechanics in Play Mode.

### Step 4: Commit and Push for Deadline 1 (Day 4 Soft Checkpoint)

By **23:59 IST on Day 4**, stage **only** your submission folder, commit with a clean message, and push your branch to GitHub:

```bash
# 1. Return to the repository root
cd ../../..

# 2. Check git status to make sure ONLY your submission folder was modified
git status

# 3. Stage your submission folder
git add submissions/Module_01/AppDev/rahul-sharma/

# 4. Commit using Conventional Commit format
git commit -m "feat(module-01): add D1 baseline Express routes for rahul-sharma"

# 5. Push your branch to GitHub
git push -u origin feat/module-01-rahul-sharma
```

### Step 5: Open Your `[WIP]` Draft Pull Request on GitHub (Deadline 1)

1. Open the repository on GitHub in your browser. You will see a banner: **`feat/module-01-rahul-sharma had recent pushes`** -> Click **"Compare & pull request"**.
2. Verify the branches at the top:
   - **base:** `main` <- **compare:** `feat/module-01-rahul-sharma`
3. Set your **Pull Request Title** using the exact **Deadline 1 Tag** format:

```text
[WIP][Module 1 - D1] AppDev: Rahul Sharma
```

*(Or for GameDev: `[WIP][Module 1 - D1] GameDev: Priya Patel`)*

4. Fill out the PR Description using the **BGSC PR Template** in Section 4 below.
5. Click the dropdown arrow next to the green **"Create pull request"** button, select **"Create draft pull request"**, and submit it.

### Step 6: Push Final Polish, Pass Peer Audit and Submit for Deadline 2 (Day 9 Final Verification)

Between Days 5 and 9, you will finish your error handling, security/physics polish, and testing.

1. **Do NOT open a second PR.** Simply commit and push your new changes to the **exact same branch**:

```bash
git add submissions/Module_01/AppDev/rahul-sharma/
git commit -m "feat(module-01): complete D2 error handling, DELETE route, and README"
git push
```

*(GitHub automatically updates your existing Pull Request with the new commits.)*

2. On your GitHub Pull Request page:
   - Click **"Ready for review"** at the bottom of the PR to take it out of Draft mode.
   - Click **"Edit"** on the PR title and change `[WIP][Module X - D1]` to **`[READY][Module X - D2]`**:

```text
[READY][Module 1 - D2] AppDev: Rahul Sharma
```

   - Tag your **Cross-Domain Peer Auditor** in the **Reviewers** sidebar (or mention `@peer-handle` in a comment) so they can run the module's **Reviewer Checklist** on your PR.
3. **Log Your Submission in the GForm:** Copy the URL of your Pull Request (for example, `https://github.com/BGSC-BGEC/BGSC-OnBoarding-Modules/pull/42`) and submit it in the **[Official BGSC Module Submission GForm](https://forms.gle/WDUghqipgjs8TDDc7)** before **23:59 IST on the Deadline 2 date**.

---

## 4. Copy-Paste Pull Request Description Template

Every time you open a Pull Request, copy and paste this Markdown template into the PR description box (it will also auto-load on GitHub via `.github/pull_request_template.md`):

```markdown
## BGSC Module Submission

- **Builder Name:** <Your Full Name> (@<your-github-handle>)
- **Module Number & Name:** Module <X> - <Module Title>
- **Submission Folder Path:** `submissions/Module_0X/<Track>/<your-github-handle>/`

### Track Targeted
- [ ] AppDev
- [ ] GameDev

### Current Checkpoint
- [ ] Deadline 1 (`[WIP]`)
- [ ] Deadline 2 (`[READY]`)

### Weekly Capacity Load
- [ ] Heavy Load (Full Spec)
- [ ] Maintenance Load (Exam/Eval Subset)

---

## What Was Built (Summary of Changes)

- **Phase 1 & 2 (Deadline 1 Baseline):**
  - Describe baseline files/routes/scripts pushed for D1
- **Phase 3 & 4 (Deadline 2 Final Polish):**
  - Describe validation, security, UI, or physics polish pushed for D2

---

## How to Test and Verify (2-Command Setup)

1. `cd submissions/Module_0X/<Track>/<your-github-handle>`
2. `<install and run command>`

---

## Cross-Domain Peer Audit Checklist

- **Assigned Peer Auditor:** @<peer-reviewer-github-handle>
- [ ] Code is strictly inside `submissions/Module_0X/<Track>/<your-github-handle>/` (zero root curriculum files modified).
- [ ] Zero `node_modules/`, `.env` secrets, `Library/`, `Temp/`, `bin/`, or `obj/` folders committed.
- [ ] `README.md` is included in the submission folder with clear setup instructions.
- [ ] All module-specific Reviewer Checklist items pass cleanly.
```

---

## 5. Gold-Standard Filled-Out PR Example

Not sure how much detail to write? Here is an example of a complete **BGSC Pull Request** at **Deadline 2**:

- **PR Title:** `[READY][Module 1 - D2] AppDev: Rahul Sharma`
- **Branches:** `feat/module-01-rahul-sharma` -> `main`

### Example PR Description Body

#### BGSC Module Submission

- **Builder Name:** Rahul Sharma (`@rahul-sharma`)
- **Module Number & Name:** Module 1 - Teaching Rocks to Think (JS, Express & Pure C#)
- **Submission Folder Path:** `submissions/Module_01/AppDev/rahul-sharma/`

##### Track Targeted

- [x] AppDev
- [ ] GameDev

##### Current Checkpoint

- [ ] Deadline 1 (`[WIP]`)
- [x] Deadline 2 (`[READY]`)

##### Weekly Capacity Load

- [x] Heavy Load (Full Spec)
- [ ] Maintenance Load (Exam/Eval Subset)

#### What Was Built (Summary of Changes)

- **Phase 1 & 2 (Deadline 1 Baseline):**
  - Initialized Express server in `submissions/Module_01/AppDev/rahul-sharma/server.js`.
  - Implemented `GET /api/matches` using `.filter()` and `.map()` and `POST /api/matches`.
- **Phase 3 & 4 (Deadline 2 Final Polish):**
  - Added `DELETE /api/matches/:id` endpoint and MMR leaderboard summary route using `.reduce()`.
  - Wrapped all async handlers in `try/catch` blocks returning proper `200`, `201`, `400`, and `404` status codes.

#### How to Test and Verify (2-Command Setup)

```bash
cd submissions/Module_01/AppDev/rahul-sharma && npm install
node server.js
```

#### Proof of Work

```json
{
  "count": 2,
  "totalActiveMmr": 4150,
  "matches": [{ "id": 1, "title": "BGSC Valorant Finals", "status": "live", "mmr": 2100 },
    { "id": 2, "title": "Midnight Card Duel", "status": "live", "mmr": 2050 }
  ]
}
```

#### Cross-Domain Peer Audit Checklist

- **Assigned Peer Auditor:** `@priya-patel` (GameDev Track Peer)
- [x] Code is strictly inside `submissions/Module_01/AppDev/rahul-sharma/` (zero root curriculum files modified).
- [x] Zero `node_modules/`, `.env` secrets, `Library/`, `Temp/`, `bin/`, or `obj/` folders committed.
- [x] `README.md` is included in the submission folder with clear setup instructions.
- [x] All module-specific Reviewer Checklist items pass cleanly (zero `var` keywords, clean `const`/`let` usage).

---

## 6. Quick Reference Table: PR Titles and Folders by Module

| Module | Feature Branch Name | Submission Folder Path | Deadline 1 PR Title (`[WIP]`) | Deadline 2 PR Title (`[READY]`) |
| :--- | :--- | :--- | :--- | :--- |
| **[Module 1](./Module_01_Zero_To_Code_JS_Express_And_Pure_CSharp.md)** | `feat/module-01-<handle>` | `submissions/Module_01/<Track>/<handle>/` | `[WIP][Module 1 - D1] <Track>: <Name>` | `[READY][Module 1 - D2] <Track>: <Name>` |
| **[Module 2](./Module_02_Postgres_Schemas_And_Unity_Engine_Architecture.md)** | `feat/module-02-<handle>` | `submissions/Module_02/<Track>/<handle>/` | `[WIP][Module 2 - D1] <Track>: <Name>` | `[READY][Module 2 - D2] <Track>: <Name>` |
| **[Module 3](./Module_03_API_Security_Auth_And_Physics_Vector_Math.md)** | `feat/module-03-<handle>` | `submissions/Module_03/<Track>/<handle>/` | `[WIP][Module 3 - D1] <Track>: <Name>` | `[READY][Module 3 - D2] <Track>: <Name>` |
| **[Module 4](./Module_04_React_AI_Frontend_And_Input_Raycasting.md)** | `feat/module-04-<handle>` | `submissions/Module_04/<Track>/<handle>/` | `[WIP][Module 4 - D1] <Track>: <Name>` | `[READY][Module 4 - D2] <Track>: <Name>` |
| **[Module 5](./Module_05_Async_State_Mocking_And_Prefabs_ScriptableObjects.md)** | `feat/module-05-<handle>` | `submissions/Module_05/<Track>/<handle>/` | `[WIP][Module 5 - D1] <Track>: <Name>` | `[READY][Module 5 - D2] <Track>: <Name>` |
| **[Module 6](./Module_06_FullStack_CRUD_And_Game_Loop_Canvas_UI.md)** | `feat/module-06-<handle>` | `submissions/Module_06/<Track>/<handle>/` | `[WIP][Module 6 - D1] <Track>: <Name>` | `[READY][Module 6 - D2] <Track>: <Name>` |
| **[Module 7](./Module_07_React_Native_Mobile_And_Multiplayer_Netcode.md)** | `feat/module-07-<handle>` | `submissions/Module_07/<Track>/<handle>/` | `[WIP][Module 7 - D1] <Track>: <Name>` | `[READY][Module 7 - D2] <Track>: <Name>` |
| **[Module 8](./Module_08_Mongo_Kafka_Backend_Optimization_And_Networked_Physics.md)** | `feat/module-08-<handle>` | `submissions/Module_08/<Track>/<handle>/` | `[WIP][Module 8 - D1] <Track>: <Name>` | `[READY][Module 8 - D2] <Track>: <Name>` |
| **[Module 9](./Module_09_Docker_DevOps_Security_And_Dedicated_Game_Servers.md)** | `feat/module-09-<handle>` | `submissions/Module_09/<Track>/<handle>/` | `[WIP][Module 9 - D1] <Track>: <Name>` | `[READY][Module 9 - D2] <Track>: <Name>` |
