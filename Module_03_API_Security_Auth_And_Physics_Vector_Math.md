# Module 3: Locks, Bouncers & Gravity - API Controllers, Manual Security, Auth & 3D Physics

> **[Curriculum Master Hub](./00_Curriculum_Master_Hub.md)** | **[PR & Submission Guide](./PR_And_Submission_Guide.md)** | **[Module 3 Submissions (`submissions/Module_03/`)](./submissions/Module_03/README.md)**

- **Sprint Dates:** Nov 05 – Nov 10, 2026 | **Estimated Workload:** 4 to 5 Hours Max
- **Deadline 1 (Soft Checkpoint):** Nov 10, 2026 | **Deadline 2 (Final Verification):** Nov 13, 2026
- **Prerequisite:** [Module 2 (PostgreSQL Schemas / Unity MonoBehaviour)](./Module_02_Postgres_Schemas_And_Unity_Engine_Architecture.md) | **Track Options:** Heavy Load or Maintenance Load

## Previously on BGSC Onboarding...

In **Module 2**, your projects gained persistence and spatial existence:

- **AppDev Crew:** You designed relational PostgreSQL schemas (`schema.sql`) and wired them to Node.js. **The problem right now?** All your routes are sitting inside one giant `server.js` file, and *anyone on the internet can hit your endpoints, pretend to be any user, or store plain-text passwords like `123456` in your database!*
- **GameDev Crew:** You built a Unity scene where primitive shapes floated and spun using `transform.position` and `Time.deltaTime`. **The problem right now?** If you move an object purely by teleporting its `Transform` coordinates, it phases straight through walls like a ghost and ignores gravity! Today, we introduce **Vector Math**, **Rigidbody Physics**, and player movement controls.

## 1. Why Are We Learning This? ("The Why")

- **AppDev — Why Manual Controller Architecture, Password Hashing (`bcrypt`), and JWTs?**
  1. **Separation of Concerns (Routes vs. Controllers):** Dumping 500 lines of SQL and validation logic inside `server.js` creates unmaintainable spaghetti. Splitting **Routes** (the traffic signs) from **Controllers** (the business logic) and **Middleware** (the security guards) makes your backend clean and testable.
  2. **Manual Security First:** Never store plain-text passwords! If your database leaks, plain-text passwords compromise your users' bank and email accounts. We use **one-way cryptographic salting and hashing (`bcrypt`)** so even *you* cannot read your users' passwords, and **JSON Web Tokens (JWT)** ([JWT.io](https://jwt.io/introduction)) so users can prove who they are on every request without re-typing their password.
- **GameDev — Why Vector3 Math & `Rigidbody` Physics?**
  - Whether you are calculating **knockback from a rocket launcher in an FPS**, **curving a card from the deck into the player's hand**, or **checking if an enemy is behind the player**, **Vectors (`Vector3`)** are the language of 3D space. And by delegating movement to Unity's **`Rigidbody`** physics component inside `FixedUpdate()`, Unity handles gravity, momentum, friction, and wall collisions for you automatically!

## 2. Track A: Application Development - Controllers, Manual Security, Bcrypt & JWT

### What You Need to Understand

#### A. Clean Backend Structure (Routes -> Middleware -> Controllers)

Instead of one mega-file, organize your backend manually so any teammate (or Shadow!) can find code in 5 seconds:

- `src/routes/authRoutes.js` — Maps URLs (`POST /api/auth/register`, `POST /api/auth/login`) to controllers.
- `src/controllers/authController.js` — Validates input, hashes passwords, queries Postgres, and returns responses.
- `src/middleware/authMiddleware.js` — Intercepts incoming requests, checks the `Authorization: Bearer <token>` header, and blocks unauthenticated users with `401 Unauthorized`.

#### B. Password Hashing (`bcrypt`) & JWT Authentication Flow

1. **Registration (`POST /api/auth/register`):** Validate email/password -> Hash password with `await bcrypt.hash(password, 12)` -> Save user with `password_hash` in PostgreSQL.
2. **Login (`POST /api/auth/login`):** Find user by email -> Compare password with `await bcrypt.compare(password, user.password_hash)` -> If true, sign a **JWT** using `jwt.sign({ userId: user.id, role: user.role }, process.env.JWT_SECRET, { expiresIn: "1h" })`.
3. **Protected Route (`GET /api/profile`):** Middleware verifies the JWT with `jwt.verify(token, process.env.JWT_SECRET)` before the controller ever runs!

```javascript
// src/controllers/authController.js
import bcrypt from "bcrypt";
import jwt from "jsonwebtoken";
import { pool } from "../db.js";

export const registerUser = async (req, res, next) => {
  try {
    const { username, email, password } = req.body;
    // Manual Input Validation & Sanitization
    if (!username || !email || !password || password.length < 8) {
      return res.status(400).json({ error: "Username, email, and 8+ char password required." });
    }

    // Salt and Hash Password (12 salt rounds)
    const passwordHash = await bcrypt.hash(password, 12);

    const result = await pool.query(
      "INSERT INTO users (username, email, password_hash) VALUES ($1, $2, $3) RETURNING id, username, email",
      [username.trim(), email.toLowerCase().trim(), passwordHash]
    );

    res.status(201).json({ message: "User registered!", user: result.rows[0] });
  } catch (err) {
    next(err); // Pass to centralized error middleware!
  }
};

export const loginUser = async (req, res, next) => {
  try {
    const { email, password } = req.body;
    const result = await pool.query("SELECT * FROM users WHERE email = $1", [email.toLowerCase().trim()]);
    const user = result.rows[0];

    // Generic error message so attackers can't guess which emails exist (User Enumeration defense!)
    if (!user || !(await bcrypt.compare(password, user.password_hash))) {
      return res.status(401).json({ error: "Invalid email or password." });
    }

    const token = jwt.sign(
      { userId: user.id, username: user.username },
      process.env.JWT_SECRET,
      { expiresIn: "2h" }
    );

    res.status(200).json({ token, user: { id: user.id, username: user.username } });
  } catch (err) {
    next(err);
  }
};
```

### Curated Top-Rated Learning Resources (AppDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [MDN Web Docs: Express Tutorial Part 4 — Routes and Controllers](https://developer.mozilla.org/en-US/docs/Learn_web_development/Extensions/Server-side/Express_Nodejs/routes) | The clearest architectural explanation of separating Express routes from controller logic and middleware. | Read the full page and structure your project with `routes/` and `controllers/` folders. | 45 mins |
| [JWT.io: Introduction to JSON Web Tokens](https://jwt.io/introduction) | The official visual guide to JWT structure (`Header.Payload.Signature`) and stateless bearer authentication. | Read the entire introduction and inspect a token in the interactive debugger at [jwt.io](https://jwt.io/). | 30 mins |
| [OWASP API Security Project](https://github.com/OWASP/API-Security) | The global industry standard for securing REST APIs against Broken Authentication (API2) and Injection. | Review the summary of **API2: Broken Authentication** and **API8: Security Misconfiguration**. | 30 mins |
| [Backend Complete Course: NodeJS, Express, JWT, PostgreSQL](https://www.youtube.com/watch?v=g09PoiCob4Y) | Practical hands-on walkthrough of wiring `bcrypt` and `jsonwebtoken` with Express and Postgres. | Watch the **Authentication, Password Hashing & JWT Middleware** segment. | 45 mins |

## 3. Track B: Game Development - 3D Vector Math, Rigidbody Dynamics & Movement Controls

### What You Need to Understand

#### A. 3D Vector Math Without Tears (`Vector3`)

A `Vector3(x, y, z)` can represent two things:

1. **A Point in Space:** Where an object is located relative to the world origin `(0, 0, 0)`.
2. **An Arrow (Direction + Magnitude):** Which way an object is moving and how fast!

| Vector Operation | Code in Unity | Why You Use It in FPS & Card Games |
| :--- | :--- | :--- |
| **Direction from A to B** | `Vector3 dir = target.position - player.position;` | Pointing an enemy turret or flying a spell projectile from a Card to an enemy! |
| **Normalization (`.normalized`)** | `Vector3 moveDir = rawInput.normalized;` | **The Diagonal Speed Bug Fix!** If pressing Forward (`1`) + Right (`1`) gives length `sqrt(1^2 + 1^2) = 1.414`, players run **41% faster diagonally** unless you `.normalized` the vector back to length `1.0`! |
| **Dot Product (`Vector3.Dot`)** | `float lookAlignment = Vector3.Dot(forward, dirToEnemy);` | Returns `+1` if looking straight at the enemy, `0` if perpendicular, `-1` if behind you (used for stealth backstabs, FPS flashbangs, and FOV checks!). |

#### B. `Rigidbody` Physics & Why Input Goes in `Update()` While Physics Goes in `FixedUpdate()`

- Read keyboard/controller input in **`Update()`** so you never miss a fast key press between frames.
- Apply physical forces (`rb.AddForce()` or `rb.linearVelocity`) in **`FixedUpdate()`** so the physics engine remains deterministic and smooth!

```csharp
using UnityEngine;

// RequireComponent automatically adds a Rigidbody if you forget to add one in the Inspector!
[RequireComponent(typeof(Rigidbody))]
public class PhysicsSphereController : MonoBehaviour
{
    [Header("Movement Tuning")]
    public float accelerationForce = 25f;
    public float maxSpeed = 8f;
    public float jumpImpulse = 6f;

    private Rigidbody _rb;
    private Vector3 _inputDirection;
    private bool _jumpRequested;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Step 1: Read player input every visual frame
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrows (-1 to +1)
        float vertical = Input.GetAxisRaw("Vertical");     // W/S or Up/Down arrows (-1 to +1)

        // Normalize so diagonal movement isn't 41% faster!
        _inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rb.linearVelocity.y) < 0.05f)
        {
            _jumpRequested = true;
        }
    }

    // Step 2: Apply forces on the fixed physics clock
    private void FixedUpdate()
    {
        // Apply movement force if below max horizontal speed
        Vector3 flatVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        if (flatVelocity.magnitude < maxSpeed)
        {
            _rb.AddForce(_inputDirection * accelerationForce, ForceMode.Acceleration);
        }

        if (_jumpRequested)
        {
            _rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
            _jumpRequested = false;
        }
    }
}
```

### Curated Top-Rated Learning Resources (GameDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Catlike Coding: Movement — Sliding a Sphere & Physics](https://catlikecoding.com/unity/tutorials/movement/) | The single best deep-dive into Unity movement math, velocity, acceleration, and `Rigidbody` physics ever written. | Complete *01: Sliding a Sphere* and *02: Physics*. | 90 mins |
| [Unity Learn: Junior Programmer Pathway (Unit 2 & 4)](https://learn.unity.com/pathway/junior-programmer) | Official hands-on tutorials for `Rigidbody` forces, `Vector3` math, and sphere controllers. | Complete *Unit 4: Gameplay Mechanics (Physics & Focal Point)*. | 60 mins |
| [Brackeys: How to Make a Video Game in Unity — Movement (E03)](https://www.youtube.com/watch?v=Au8oX5pu5u4) | Fast, crystal-clear visual demonstration of `Rigidbody.AddForce` and `FixedUpdate`. | Watch the 12-minute episode and test the physics cube/sphere setup. | 20 mins |

## 4. Step-by-Step Sprint Action Plan ("What To Do")

### Phase 1 & 2 - **DEADLINE 1 (Soft Checkpoint: Nov 10, 2026)**

- **AppDev D1 Deliverable:** Push refactored folder structure (`routes/`, `controllers/`, `middleware/`) with basic registration/login endpoints and request body validation. Open `[WIP][Module 3 - D1]` PR.
- **GameDev D1 Deliverable:** Push a Unity scene with a `Rigidbody` sphere on a 3D obstacle floor responding to `AddForce` inside `FixedUpdate()`. Open `[WIP][Module 3 - D1]` PR.

### Phase 3 & 4 - **DEADLINE 2 (Final Verification: Nov 13, 2026)**

- **AppDev D2 Final Deliverable:**
  - **Heavy Load:** Complete Authentication Controller (`POST /api/auth/register`, `POST /api/auth/login`) with `bcrypt` password hashing, signed JWT issuance, an `authMiddleware` protecting at least 1 private route (`GET /api/me`), and centralized error handling middleware ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** `register` and `login` controller routes with `bcrypt` hashing and JWT generation.
- **GameDev D2 Final Deliverable:**
  - **Heavy Load:** A smooth, physics-driven 3D player controller script (`PhysicsSphereController.cs`) featuring normalized `Vector3` directional movement, max-speed clamping, impulse jumping, and a ramp/obstacle test course ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Physics-bound sphere controller moving smoothly across a 3D plane with normalized `Vector3` input.

## 5. Cross-Domain Peer Audit & Submission Portal

- **Reviewer Checklist:**
  - [ ] **AppDev:** Passwords are hashed with `bcrypt` before DB insert; `JWT_SECRET` is loaded from `.env`; protected route returns `401` when token is missing or tampered with.
  - [ ] **GameDev:** Input is captured in `Update()`, physics forces are applied in `FixedUpdate()`, and movement vector is `.normalized`.
- **Submit Your Pull Request Here:** Place your deliverable inside [`submissions/Module_03/<Track>/<your-github-handle>/`](./submissions/Module_03/README.md), open your PR following the **[BGSC PR & Submission Guide](./PR_And_Submission_Guide.md)**, and log your PR URL in the [Official Submission GForm](https://forms.gle/F8P3rPNjDWDKPDr86). 
