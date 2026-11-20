import express from "express";

const app = express();
app.use(express.json());

let matches = [
  { id: 1, game: "Valorant", teamA: "Bitsians", teamB: "Titans", score: "13-9" }
];

let nextId = 2;

// GET: Retrieve and filter matches
app.get("/api/matches", async (req, res) => {
  try {
    const data = req.query.game
      ? matches.filter(m => m.game.toLowerCase() === req.query.game.toLowerCase())
      : matches;

    return res.status(200).json(data);
  } catch (error) {
    return res.status(400).json({ error: error.message });
  }
});

// POST: Create a match
app.post("/api/matches", async (req, res) => {
  try {
    const { game, teamA, teamB, score } = req.body;

    if (!game || !teamA || !teamB) {
      return res.status(400).json({ error: "Missing required fields" });
    }

    const match = { id: nextId++, game, teamA, teamB, score: score || "0-0" };
    matches.push(match);

    return res.status(201).json(match);
  } catch (error) {
    return res.status(400).json({ error: error.message });
  }
});

// DELETE: Delete a match
app.delete("/api/matches/:id", async (req, res) => {
  try {
    const id = Number(req.params.id);
    const index = matches.findIndex(m => m.id === id);

    if (index === -1) {
      return res.status(404).json({ error: "Match not found" });
    }

    const deleted = matches.splice(index, 1)[0];
    return res.status(200).json(deleted);
  } catch (error) {
    return res.status(400).json({ error: error.message });
  }
});

// Demonstrate map() and reduce()
const names = matches.map(m => m.teamA);
const total = matches.reduce((sum, m) => sum + 1, 0);

console.log("Teams:", names);
console.log("Total matches:", total);

app.listen(3000, () => console.log("Server running on port 3000"));