# Module 9: Shipping to the Real World - Docker, DevOps CI/CD, API Hardening & Dedicated Game Servers

> **[Curriculum Master Hub](./00_Curriculum_Master_Hub.md)** | **[PR & Submission Guide](./PR_And_Submission_Guide.md)** | **[Module 9 Submissions (`submissions/Module_09/`)](./submissions/Module_09/README.md)**

- **Sprint Dates:** Jan 18 – Jan 23, 2027 | **Estimated Workload:** 4 to 5 Hours Max
- **Deadline 1 (Soft Checkpoint):** Jan 23, 2027 | **Deadline 2 (Final Verification):** Jan 26, 2027
- **Prerequisite:** [Modules 1–8](./00_Curriculum_Master_Hub.md) | **Track Options:** Heavy Load or Maintenance Load

## Previously on BGSC Onboarding...

In **Module 8**, you engineered serious systems:

- **AppDev Crew:** You built a polyglot Node.js backend running PostgreSQL, MongoDB, event buses (Kafka/RabbitMQ), and custom rate-limiting algorithms. **Now imagine handing your repo to a new teammate:** *"Hey, to run my code, just install Node 22.14, PostgreSQL 18, MongoDB 8, Kafka with KRaft, and set 19 environment variables... wait, why is it throwing `ECONNREFUSED` on Windows?!"* Today, we kill *"It works on my machine"* forever using **Docker**, **Docker Compose**, **OWASP API Security Hardening**, and **GitHub Actions CI/CD**.
- **GameDev Crew:** You built a lag-compensated multiplayer game in Unity. **Now imagine launching your game during a campus-wide [BGSC Esports Tournament](https://drive.google.com/open?id=1qMlqk_bqyzjaoteGGh6axxkbwbe9r0kDmYAlLfNJrco):** if Player 1 hosts the match on their laptop and their hostel Wi-Fi disconnects—or they rage-quit after losing a round—*everyone gets kicked from the match!* Today, you learn how real studios compile **Linux Headless Dedicated Game Servers**, package them inside **Docker containers**, wire **Matchmaker Routing**, and deploy them to the cloud!

## 1. Why Are We Learning This? ("The Why")

- **AppDev — Why Docker, OWASP Hardening & Automated CI/CD Pipelines Are the Final Boss of Full-Stack Engineering:**
  1. **Containers (`Dockerfile` + `docker-compose.yml`):** A Docker container packages your exact OS libraries, Node runtime, and code into a lightweight, isolated box. With one single command—`docker compose up --build`—your entire ecosystem (React frontend, Node microservices, PostgreSQL, MongoDB, and Kafka/RabbitMQ) boots up identically on Mac, Windows, Linux, and production cloud servers ([Docker Docs](https://docs.docker.com/get-started/)).
  2. **Defense-in-Depth Security ([OWASP API Security Top 10](https://github.com/OWASP/API-Security)):** Before exposing any server to the public internet, you must harden HTTP headers (`helmet`), enforce strict rate limits, validate payload schemas (`zod`/`joi`) against Mass Assignment attacks, and run containers as a **non-root user** so a compromised process cannot take over the host server.
  3. **CI/CD (`GitHub Actions`):** Every time anyone opens a Pull Request, an automated robot boots up on GitHub's servers, runs your linter, executes your tests, and builds your Docker image ([GitHub Actions Docs](https://docs.github.com/en/actions/get-started/quickstart)). Broken code is blocked *before* a human even reviews the PR!
- **GameDev — Why Headless Dedicated Servers (`Unity Dedicated Server Build`) Power Every Competitive Multiplayer Game:**
  - When a server runs in a cloud data center (AWS, GCP, or our campus CC server room), **there is no monitor plugged into it!** If your Unity server build tries to render 3D shaders, play 3D audio, or draw particle explosions at 144 FPS, it wastes 80% of its CPU/GPU on invisible graphics.
  - Unity's **Dedicated Server Build Target** ([Unity Dedicated Server Manual](https://docs.unity3d.com/6000.6/Documentation/Manual/dedicated-server.html)) strips out rendering and audio pipelines (`-batchmode -nographics`), letting a single cheap cloud VM run dozens of concurrent FPS or Card Game matches inside lightweight **Linux Docker containers**!

## 2. Track A: Application Development - Docker Compose, API Hardening & GitHub Actions CI/CD

### What You Need to Understand

#### A. Multi-Stage `Dockerfile` & `docker-compose.yml`

Never run your Node app as `root` inside a bloated 1.2GB image. Use a slim Alpine image and orchestrate your databases cleanly with `docker-compose.yml`:

```dockerfile
# Dockerfile — Production-Grade Node.js Container
FROM node:22-alpine AS base
WORKDIR /app

# Copy only package files first so Docker caches 'npm ci' unless dependencies change!
COPY package*.json ./
RUN npm ci --omit=dev

# Copy application source
COPY src ./src

# Security Hardening: Run as built-in non-root 'node' user!
USER node
EXPOSE 3000
CMD ["node", "src/server.js"]
```

```yaml
# docker-compose.yml — Spins up API + PostgreSQL + MongoDB in one command!
services:
  api:
    build: .
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=production
      - DATABASE_URL=postgres://bgsc_user:secure_pass@postgres:5432/bgsc_db
      - MONGO_URI=mongodb://mongo:27017/bgsc_logs
      - JWT_SECRET=${JWT_SECRET}
    depends_on:
      - postgres
      - mongo

  postgres:
    image: postgres:18-alpine
    environment:
      POSTGRES_USER: bgsc_user
      POSTGRES_PASSWORD: secure_pass
      POSTGRES_DB: bgsc_db
    volumes:
      - pgdata:/var/lib/postgresql/data

  mongo:
    image: mongo:8
    volumes:
      - mongodata:/data/db

volumes:
  pgdata:
  mongodata:
```

#### B. OWASP API Security Hardening Checklist in Express

Before shipping to production, verify these 4 manual security layers ([OWASP API Security Testing Framework](https://github.com/OWASP/www-project-api-security-testing-framework)):

1. **Security Headers (`helmet`):** `app.use(helmet())` strips the `X-Powered-By: Express` header and adds strict Content-Security-Policy (CSP) and HSTS headers.
2. **Body Size Limits:** `app.use(express.json({ limit: "10kb" }))` prevents attackers from crashing your server RAM by sending a 2GB JSON payload!
3. **BOLA (Broken Object Level Authorization — OWASP API1):** When handling `DELETE /api/posts/:id`, *never* just run `DELETE FROM posts WHERE id = $1`. Always check `WHERE id = $1 AND creator_id = $2` (using `req.user.userId` from the verified JWT) so User A cannot delete User B's posts!
4. **Automated CI Pipeline (`.github/workflows/ci.yml`):** Automatically runs `npm test` and `docker build` on every Pull Request.

### Curated Top-Rated Learning Resources (AppDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Docker Official Docs: Get Started (`docs.docker.com/get-started`)](https://docs.docker.com/get-started/) & [Writing a Dockerfile](https://docs.docker.com/get-started/docker-concepts/building-images/writing-a-dockerfile/) | The official hands-on guide to container images, volumes, networks, and `docker compose`. | Complete *Run an App*, *Writing a Dockerfile*, and *Multi-container Apps with Docker Compose*. | 75 mins |
| [Prisma Guide: How to Use Prisma & PostgreSQL in Docker](https://www.prisma.io/docs/guides/deployment/docker) | Step-by-step walkthrough of containerizing an Express + PostgreSQL app with Docker Compose. | Follow the `Dockerfile` and `docker-compose.yml` networking guide. | 30 mins |
| [OWASP API Security Project](https://github.com/OWASP/API-Security) | The industry bible for API security vulnerabilities (BOLA, Broken Auth, Mass Assignment, Unrestricted Resource Consumption). | Audit your API routes against the top 5 OWASP API risks. | 30 mins |
| [GitHub Actions Official Quickstart](https://docs.github.com/en/actions/get-started/quickstart) | Shows how to create `.github/workflows/ci.yml` in 5 minutes to automate testing on every PR. | Add a working GitHub Actions workflow to your repository. | 25 mins |

## 3. Track B: Game Development - Headless Dedicated Unity Servers, Docker & Matchmaking

### What You Need to Understand

#### A. Compiling a Headless Dedicated Server in Unity

When you switch your Unity Build Profile to **Dedicated Server (Linux x86_64)** ([Unity Dedicated Server Build Docs](https://docs.unity3d.com/6000.6/Documentation/Manual/dedicated-server-build.html)):

- Unity automatically defines the `UNITY_SERVER` scripting symbol so you can strip client-only code (like camera shake or UI animations) at compile time!
- In your server bootstrap script, you check `Application.isBatchMode` (or `#if UNITY_SERVER`), lock the server tick rate with `Application.targetFrameRate = 60;` (otherwise an uncapped headless loop will spin at 5,000 FPS and burn 100% of your cloud CPU!), and automatically call `NetworkManager.Singleton.StartServer()`.

```csharp
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using System;

public class DedicatedServerBootstrap : MonoBehaviour
{
    private void Start()
    {
        // If running as a headless Dedicated Server (-batchmode / Server Build Target)
        if (Application.isBatchMode)
        {
            // CRITICAL: Cap headless server tick rate to 60 Hz so CPU doesn't hit 100%!
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            ushort port = 7777;
            // Read port from command line args if passed by Docker/Matchmaker (e.g., -port 7778)
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == "-port" && ushort.TryParse(args[i + 1], out ushort parsedPort))
                {
                    port = parsedPort;
                }
            }

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData("0.0.0.0", port);

            Debug.Log($"[DEDICATED SERVER] Booting headless Unity Server on UDP port {port} at 60Hz...");
            NetworkManager.Singleton.StartServer();
        }
    }
}
```

#### B. Containerizing the Unity Dedicated Server & Matchmaker Flow

Once Unity compiles your Linux server binary (`GameServer.x86_64`), you wrap it in a lightweight Ubuntu Docker image exposing UDP port `7777/udp`:

```dockerfile
# Dockerfile.gameserver — Containerized Headless Unity Dedicated Server
FROM ubuntu:24.04
RUN apt-get update && apt-get install -y ca-certificates libstdc++6 && rm -rf /var/lib/apt/lists/*

WORKDIR /gameserver
COPY ./Builds/LinuxServer/ /gameserver/
RUN chmod +x /gameserver/GameServer.x86_64

# Unity Transport (UTP) uses UDP for fast real-time game packets!
EXPOSE 7777/udp

ENTRYPOINT ["./GameServer.x86_64", "-batchmode", "-nographics", "-logfile", "-", "-port", "7777"]
```

**How Matchmaking Works in Production:**

1. Two players click **"Find Match (1v1 Card Duel or 5v5 FPS)"** in their Unity Client.
2. The Client sends an HTTP `POST /api/matchmaking/queue` to a lightweight Matchmaker API.
3. Once enough players are matched, the Matchmaker assigns them to a running Dockerized Dedicated Server instance (`IP:Port` + a one-time `JoinToken`) and both Clients connect directly to `IP:7777`!

### Curated Top-Rated Learning Resources (GameDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Unity Manual: Introduction to Dedicated Server](https://docs.unity3d.com/6000.2/Documentation/Manual/dedicated-server-introduction.html) & [Dedicated Server Requirements](https://docs.unity3d.com/6000.1/Documentation/Manual/dedicated-server-requirements.html) | Official Unity guide explaining headless execution, asset stripping, and the Dedicated Server build target. | Read both short manual pages and install the *Linux Dedicated Server Build Support* module in Unity Hub. | 45 mins |
| [Unity Manual: Build Your Application for Dedicated Server](https://docs.unity3d.com/6000.6/Documentation/Manual/dedicated-server-build.html) | Step-by-step walkthrough of compiling and running your headless server executable from the terminal. | Follow the build steps to compile your server binary. | 45 mins |
| [Docker Official Docs: Get Started](https://docs.docker.com/get-started/) | Teaches you how to package your compiled Linux server binary into a reproducible container. | Read *Writing a Dockerfile* and test building your server container. | 45 mins |

## 4. Step-by-Step Sprint Action Plan ("What To Do")

### Phase 1 & 2 - **DEADLINE 1 (Soft Checkpoint: Jan 23, 2027)**

- **AppDev D1 Deliverable:** Push a working `Dockerfile` and `docker-compose.yml` that boots your Node.js API alongside PostgreSQL/MongoDB locally with `docker compose up`. Open `[WIP][Module 9 - D1]` PR.
- **GameDev D1 Deliverable:** Write your `DedicatedServerBootstrap.cs` script (`Application.isBatchMode` + `Application.targetFrameRate = 60`), compile a headless Dedicated Server build in Unity, and verify that a local Client can connect to it on `127.0.0.1:7777`. Open `[WIP][Module 9 - D1]` PR.

### Phase 3 & 4 - **DEADLINE 2 (Final Verification: Jan 26, 2027)**

- **AppDev D2 Final Deliverable:**
  - **Heavy Load:** A fully containerized, production-hardened **Full-Stack / Microservice Infrastructure** featuring a multi-stage `Dockerfile`, `docker-compose.yml` orchestrating your backend and databases, OWASP API security hardening (`helmet`, rate limiting, payload size caps, BOLA ownership checks, `.env.example`), and an automated `.github/workflows/ci.yml` pipeline ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Working `Dockerfile` + `docker-compose.yml` running the backend and database with basic security middleware (`helmet` + rate limiting).
- **GameDev D2 Final Deliverable:**
  - **Heavy Load:** A containerized **Headless Dedicated Unity Game Server** (`Dockerfile.gameserver`) running with capped tick rate (`60 Hz`), UDP port mapping (`7777/udp`), a simple matchmaker/lobby connection flow, and verified multi-client gameplay synchronization ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Compiled headless Unity Dedicated Server build with `DedicatedServerBootstrap.cs` and verified client connection.

## 5. Graduation to Semester 2 Core Production!

Congratulations! By merging your **Module 9 Final Verification PR on January 26, 2027**, you have officially gone from *"I only know `git init` and zero programming"* to shipping containerized microservices, AI-augmented & monitored React/React Native frontends, event-driven polyglot databases, and lag-compensated headless multiplayer Unity game servers!

As detailed in the [Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA), you are now ready for **Semester 2 Core Feature Production**—pitching on the **Idea Board**, forming **Canary Squads**, and building live campus products!

- **Submit Your Final Pull Request Here:** Place your deliverable inside [`submissions/Module_09/<Track>/<your-github-handle>/`](./submissions/Module_09/README.md), open your PR following the **[BGSC PR & Submission Guide](./PR_And_Submission_Guide.md)**, and log your completion in the [Official Submission GForm](https://forms.gle/6srMX1JG1DSwgQCD6).
