# Module 8: Polyglot Databases, Event Buses & Time Travel - MongoDB, Kafka/RabbitMQ, Backend Algorithms & Lag Compensation

> **[Curriculum Master Hub](./00_Curriculum_Master_Hub.md)** / **[PR & Submission Guide](./PR_And_Submission_Guide.md)** / **[Module 8 Submissions (`submissions/Module_08/`)](./submissions/Module_08/README.md)**

- **Sprint Dates:** Jan 12 – Jan 17, 2027 / **Estimated Workload:** 4 to 5 Hours Max
- **Deadline 1 (Soft Checkpoint):** Jan 17, 2027 / **Deadline 2 (Final Verification):** Jan 20, 2027
- **Prerequisite:** [Module 7 (React Native / Multiplayer Netcode)](./Module_07_React_Native_Mobile_And_Multiplayer_Netcode.md) / **Track Options:** Heavy Load or Maintenance Load

## Previously on BGSC Onboarding...

In **Module 7**, you expanded to mobile devices and live multiplayer connections:

- **AppDev Crew:** You built a React Native mobile client talking to your Node.js + PostgreSQL backend. **Now, imagine your app blows up during [Waves or Spree](https://drive.google.com/open?id=1qMlqk_bqyzjaoteGGh6axxkbwbe9r0kDmYAlLfNJrco):** 5,000 students are sending live chat messages, claiming food posts, and refreshing leaderboards simultaneously. If every single microservice calls every other microservice synchronously over REST (`Service A -> waits for Service B -> waits for Service C`), one slow database query creates a traffic jam that takes down the entire system! Today, you master **Polyglot Persistence (PostgreSQL + MongoDB)**, **Event Bus Protocols (Apache Kafka & RabbitMQ)**, and **Backend Optimization Algorithms**.
- **GameDev Crew:** You built a multiplayer lobby where Clients sent `RequestMoveServerRpc` to the Server. **Did you notice what happens when ping is 120ms?** When you press `W`, your character waits 120ms for the message to reach the server and come back before moving—making your game feel like you're wading through cold molasses! Today, you learn how **Valorant, Counter-Strike, and Rocket League** make 100ms ping feel instantaneous using **Client-Side Prediction, Server Reconciliation, Entity Interpolation**, and **Game Backend Persistence**.

## 1. Why Are We Learning This? ("The Why")

- **AppDev — Why Combine PostgreSQL + MongoDB, Add Kafka/RabbitMQ, and Learn Backend Algorithms?**
  1. **PostgreSQL vs. MongoDB (Polyglot Persistence):** Use **PostgreSQL** for structured relational data that requires strict ACID transactions (Users, Wallets, Claims, Tournament Brackets). Use **MongoDB** ([MongoDB University](https://learn.mongodb.com/)) for high-velocity, flexible JSON documents whose shape changes often (Live Chat Messages, Match Telemetry Logs, Dynamic Feed Payloads).
  2. **REST vs. Event Bus Protocols (Apache Kafka & RabbitMQ):** Synchronous **REST** is like a phone call—you are blocked on the line until the other service answers. An **Event Bus / Message Broker** (**[Apache Kafka](https://kafka.apache.org/quickstart/)** or **[RabbitMQ](https://www.rabbitmq.com/tutorials)**, as used in our [PlateMate Backend](https://github.com/Nikunj1310/PlateMate_Frontend/pull/1)) is like dropping a letter in a mailbox: when a user claims food in the `Claim Service`, it publishes a `CLAIM_APPROVED` event to the bus in 1ms and returns immediately, while the `Reputation Service` and `Messaging Service` process that event asynchronously in the background!
  3. **Backend Algorithms & Optimizations:** A missing database index turns a `O(log N)` **B-Tree lookup** (0.2ms) into an `O(N)` **Full Table Scan** (4,000ms). Combining **B-Tree Indexing**, **Token Bucket Rate Limiting**, and **In-Memory Caching** is what separates a toy script from backend engineering ([System Design Primer](https://github.com/donnemartin/system-design-primer)).
- **GameDev — Why Lag Compensation & Game Backends Are Essential for FPS & Card Games:**
  - **In an FPS:** You need **Client-Side Prediction** (move immediately on key press, then reconcile with the Server's authoritative position) and **Server-Side Lag Compensation (Time Rewind)** so when a player with 100ms ping shoots where an enemy *appeared* on their screen 100ms ago, the Server rewinds its hitbox history by 100ms to verify the headshot fairly!
  - **In a Multiplayer Card Game:** You need a **Game Backend Service** (Node/Postgres/Mongo) to store player card collections, ELO/MMR ratings, and match history outside the Unity match instance so players don't lose their unlocked cards when the match ends!

## 2. Track A: Application Development - Dual DBs (Postgres + Mongo), Kafka/RabbitMQ & Backend Optimization

### What You Need to Understand

#### A. Polyglot Persistence: PostgreSQL (Relational) + MongoDB (NoSQL Document)

Here is the golden architectural rule for when to use which database in the same Node.js ecosystem:

| Feature / Requirement | PostgreSQL (Relational SQL) | MongoDB (Document NoSQL) |
| :--- | :--- | :--- |
| **Data Structure** | Strict rows & columns across normalized tables (`JOIN`s). | Flexible, self-contained BSON/JSON documents (`{ _id, messages: [...] }`). |
| **Best Use Cases in Club Apps** | User accounts, Auth, Claims, Financial/Point ledgers ([PlateMate DB 1–4](https://github.com/Nikunj1310/PlateMate_Frontend/pull/1)). | Chat histories, real-time game event logs, flexible telemetry payloads. |
| **Optimization Mechanism** | Composite B-Tree Indexes (`CREATE INDEX idx_claims_user ON claims(user_id);`). | Compound Document Indexes & Aggregation Pipelines (`$match`, `$group`). |

#### B. API & Event Bus Protocols Compared: REST vs. Kafka vs. RabbitMQ

```javascript
// 1. Mongoose Schema for MongoDB (High-speed Activity/Chat Logs)
import mongoose from "mongoose";

const activityLogSchema = new mongoose.Schema({
  userId: { type: Number, required: true, index: true }, // B-Tree Index for O(log N) lookup!
  eventType: { type: String, enum: ["MATCH_JOINED", "CLAIM_CREATED", "REPUTATION_UPDATED"], required: true },
  metadata: { type: mongoose.Schema.Types.Mixed }, // Flexible JSON payload!
  createdAt: { type: Date, default: Date.now, index: true }
});

export const ActivityLog = mongoose.model("ActivityLog", activityLogSchema);
```

And here is how an **Apache Kafka** (or **RabbitMQ**) producer/consumer decouples your microservices so `Claim Service` never blocks waiting for `Reputation Service`:

```javascript
// 2. Event Bus Pattern (KafkaJS / AMQP Conceptual Producer & Consumer)
// When a user completes a match or food claim in PostgreSQL:
export const publishDomainEvent = async (producer, topic, payload) => {
  await producer.send({
    topic, // e.g., "reputation-events"
    messages: [{ key: String(payload.userId), value: JSON.stringify(payload) }]
  });
};

// 3. Backend Algorithm: Token Bucket Rate Limiter (O(1) Time Complexity per Request!)
const buckets = new Map();
export const tokenBucketLimiter = (capacity = 10, refillPerSec = 1) => (req, res, next) => {
  const ip = req.ip;
  const now = Date.now() / 1000;
  const bucket = buckets.get(ip) || { tokens: capacity, lastRefill: now };

  // Refill tokens based on elapsed time
  const elapsed = now - bucket.lastRefill;
  bucket.tokens = Math.min(capacity, bucket.tokens + elapsed * refillPerSec);
  bucket.lastRefill = now;

  if (bucket.tokens < 1) {
    return res.status(429).json({ error: "Too Many Requests — Token Bucket empty!" });
  }
  bucket.tokens -= 1;
  buckets.set(ip, bucket);
  next();
};
```

### Curated Top-Rated Learning Resources (AppDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [MongoDB University (`learn.mongodb.com`)](https://learn.mongodb.com/) & [Prisma MongoDB Quickstart](https://www.prisma.io/docs/prisma-orm/quickstart/mongodb) | Official free interactive courses from the creators of MongoDB + Prisma's hybrid ORM guide. | Complete *Getting Started with MongoDB & Node.js* (Document model, CRUD, Indexes). | 60 mins |
| [Apache Kafka Official Quickstart](https://kafka.apache.org/quickstart/) & [RabbitMQ Tutorials](https://www.rabbitmq.com/tutorials) | The official quickstarts for the world's two most widely used event streaming and message broker protocols. | Read the *Apache Kafka Quickstart* (Topics, Partitions, Producers, Consumers) and *RabbitMQ Tutorial 1 & 3 (Pub/Sub)*. | 60 mins |
| [The System Design Primer (`donnemartin/system-design-primer`)](https://github.com/donnemartin/system-design-primer) | Over 270,000+ GitHub stars—the undisputed #1 open-source guide to backend architecture, SQL vs NoSQL, caching, and message queues. | Read *SQL vs. NoSQL*, *Message Queues*, and *Database Indexing / Caching*. | 45 mins |

## 3. Track B: Game Development - Client Prediction, Server Reconciliation & Game Backends

### What You Need to Understand

#### A. The Holy Trinity of Multiplayer Netcode ([Gabriel Gambetta](https://www.gabrielgambetta.com/client-side-prediction-server-reconciliation.html))

How do we defeat network latency so an FPS or physics game feels responsive even at 100ms ping?

1. **Client-Side Prediction:** The moment the player presses `W` on Tick #105, the Client **does not wait for the Server**—it predicts the physics move locally right away and stores `{ tick: 105, input: W, predictedPos }` in a ring buffer!
2. **Server Reconciliation:** 100ms later, the Server replies: *"At Tick #105, your authoritative position was `(X, Y, Z)`."* If the Client's historical prediction for Tick #105 matches the Server (within `0.05f` meters), awesome! If a wall or enemy knockback made them diverge, the Client snaps to the Server's Tick #105 position and **re-simulates Ticks #106 to current** in a single frame so there is zero rubber-banding jitter!
3. **Entity Interpolation ([Gabriel Gambetta Part III](https://www.gabrielgambetta.com/entity-interpolation.html)):** For *other* players on your screen, you smoothly interpolate (`Vector3.Lerp`) between their two most recent server snapshots so they glide smoothly at 60 FPS even if the server only sends network packets 20 times per second (20 Hz Tick Rate).

```csharp
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct InputPayload : INetworkSerializable
{
    public int tick;
    public Vector3 inputVector;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref tick);
        serializer.SerializeValue(ref inputVector);
    }
}

public class PredictedMovement : NetworkBehaviour
{
    public float speed = 6f;
    private int _currentTick = 0;
    private readonly Dictionary<int, Vector3> _predictedPositions = new Dictionary<int, Vector3>();

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        _currentTick++;
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // 1. CLIENT PREDICTION: Apply movement immediately on the local client!
        transform.position += input * (speed * Time.fixedDeltaTime);
        _predictedPositions[_currentTick] = transform.position;

        // 2. Send the stamped input to the authoritative Server
        SendInputServerRpc(new InputPayload { tick = _currentTick, inputVector = input });
    }

    [Rpc(SendTo.Server)]
    private void SendInputServerRpc(InputPayload payload)
    {
        // Server processes authoritative movement and sends back authoritative state
        transform.position += payload.inputVector * (speed * Time.fixedDeltaTime);
        ReconcileClientRpc(payload.tick, transform.position);
    }

    [Rpc(SendTo.Owner)]
    private void ReconcileClientRpc(int serverTick, Vector3 authoritativePos)
    {
        if (_predictedPositions.TryGetValue(serverTick, out Vector3 predictedPos))
        {
            // 3. SERVER RECONCILIATION: If prediction drifted > 10cm, smoothly correct!
            if (Vector3.Distance(predictedPos, authoritativePos) > 0.1f)
            {
                Debug.LogWarning($"[RECONCILE] Tick {serverTick} drift detected! Correcting position.");
                transform.position = Vector3.Lerp(transform.position, authoritativePos, 0.5f);
            }
            _predictedPositions.Remove(serverTick);
        }
    }
}
```

### Curated Top-Rated Learning Resources (GameDev)

| Resource & Link | Why It Is Gold Standard | Exact Sections to Complete | Est. Time |
| :--- | :--- | :--- | :--- |
| [Gabriel Gambetta: Client-Side Prediction and Server Reconciliation (Part II)](https://www.gabrielgambetta.com/client-side-prediction-server-reconciliation.html) & [Interactive Live Demo](https://www.gabrielgambetta.com/client-side-prediction-live-demo.html) | Every multiplayer engineer in the industry has studied this exact article and interactive simulator. | Read *Part II* and play with the live browser latency/prediction sliders in the *Live Demo*. | 60 mins |
| [Gabriel Gambetta: Entity Interpolation (Part III)](https://www.gabrielgambetta.com/entity-interpolation.html) | Explains how to render enemy players smoothly AND how FPS servers rewind time for lag-compensated headshots (Part IV). | Read *Part III* and the lag-compensated raycast section. | 45 mins |
| [Unity Netcode for GameObjects Docs: NetworkTransform & Physics](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@latest/) | Shows how Unity's `NetworkTransform` and `NetworkRigidbody` handle interpolation and authority. | Review the *NetworkTransform* and *Physics* pages in the NGO manual. | 45 mins |

## 4. Step-by-Step Sprint Action Plan ("What To Do")

### Phase 1 & 2 - **DEADLINE 1 (Soft Checkpoint: Jan 17, 2027)**

- **AppDev D1 Deliverable:** Connect your Node.js backend to **both** PostgreSQL and MongoDB (via Mongoose or Prisma), defining 1 relational table and 1 document collection. Open `[WIP][Module 8 - D1]` PR.
- **GameDev D1 Deliverable:** Implement a basic tick counter and Client-Side Prediction movement script that moves locally before the server RPC round-trip finishes. Open `[WIP][Module 8 - D1]` PR.

### Phase 3 & 4 - **DEADLINE 2 (Final Verification: Jan 20, 2027)**

- **AppDev D2 Final Deliverable:**
  - **Heavy Load:** A hybrid **Node.js Backend Service** integrating **PostgreSQL** (for core relational records) and **MongoDB** (for activity/chat logs with indexed queries), an **Asynchronous Event Bus / Message Queue** producer-consumer flow (using Kafka, RabbitMQ, or an event bus broker), and **1 Backend Optimization Algorithm** (e.g., Token Bucket rate limiter or SQL + NoSQL index benchmark) ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Node.js service writing core state to PostgreSQL and audit/event logs to MongoDB with proper database indexes.
- **GameDev D2 Final Deliverable:**
  - **Heavy Load:** A **Lag-Compensated Multiplayer Prototype** featuring Client-Side Prediction and Server Reconciliation (tested with simulated latency in Unity's Network Simulator so movement remains smooth at 100ms+ ping) PLUS a mock/local REST call persisting end-of-match stats (score/cards won) to a backend endpoint ([Onboarding Framework](https://drive.google.com/open?id=1lLzWQ_4Z_VB8-pOG_xtUY-4UD-if9mvNM_35hKFpIuA)).
  - **Maintenance Load:** Client-predicted movement with server position reconciliation under simulated network latency.

## 5. Cross-Domain Peer Audit & Submission Portal

- **Submit Your Pull Request Here:** Place your deliverable inside [`submissions/Module_08/<Track>/<your-github-handle>/`](./submissions/Module_08/README.md), open your PR following the **[BGSC PR & Submission Guide](./PR_And_Submission_Guide.md)**, and log your PR URL in the [Official Submission GForm](https://forms.gle/dkZYBnm9XnXoVqzJ9).
