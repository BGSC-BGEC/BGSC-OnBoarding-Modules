import express from "express";
import "dotenv/config";
import { createUser, createMatch, getMatches } from "./db.js";

const app = express();
app.use(express.json());

// Create a user
app.post("/api/users", async (req, res) => {
    try {
        const { username, email } = req.body;

        if (!username || !email) {
            return res.status(400).json({ error: "Username and email are required" });
        }

        const user = await createUser(username, email);
        return res.status(201).json(user);
    } catch (error) {
        return res.status(500).json({ error: error.message });
    }
});

// Create a tournament match
app.post("/api/matches", async (req, res) => {
    try {
        const { title, gameCategory, creatorId, scheduledAt } = req.body;

        if (!title || !gameCategory || !creatorId || !scheduledAt) {
            return res.status(400).json({ error: "All fields are required" });
        }

        const match = await createMatch(title, gameCategory, creatorId, scheduledAt);
        return res.status(201).json(match);
    } catch (error) {
        return res.status(500).json({ error: error.message });
    }
});

// Retrieve matches with their organizers
app.get("/api/matches", async (req, res) => {
    try {
        const matches = await getMatches();
        return res.status(200).json(matches);
    } catch (error) {
        return res.status(500).json({ error: error.message });
    }
});

const PORT = process.env.PORT || 3000;

app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
