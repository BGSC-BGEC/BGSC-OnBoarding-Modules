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
