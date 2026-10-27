import express from "express";

const app = express();
app.use(express.json()); 

let players = [
    { id: 1, handle: "Viper", mmr: 2400, active: true },
    { id: 2, handle: "Ghost", mmr: 1100, active: false },
    { id: 3, handle: "Phoenix", mmr: 2850, active: true }
];

const simulateNetworkDelay = () => new Promise(resolve => setTimeout(resolve, 500));

// GET

app.get("/api/players", async (req, res) => {
    try {
        await simulateNetworkDelay(); 

        const { minMmr } = req.query;
        let result = players;

        // .filter()
        if (minMmr) {
            result = players.filter(player => player.mmr >= parseInt(minMmr));
        }

        // .map()
        const formattedPlayers = result.map(player => ({
            profile: `${player.handle} [MMR:${player.mmr}]`,
            status: player.active ? "Online" : "Offline"
        }));

        // .reduce()
        const totalMmr = result.reduce((sum, player) => sum + player.mmr, 0);

        // reponse
        res.status(200).json({
            count: result.length,
            total_mmr: totalMmr,
            data: formattedPlayers
        });
    } catch (error) {
        console.error("GET Error:", error);
        res.status(500).json({ error: "Internal Server Error" });
    }
});

// POST 
app.post("/api/players", async (req, res) => {
    try {
        await simulateNetworkDelay(); 
        
        const { handle, mmr, active } = req.body;

        if (!handle || mmr === undefined) {
            return res.status(400).json({ error: "Missing required fields: handle and mmr" });
        }

        // Generate new ID 
        const newId = players.length > 0 ? Math.max(...players.map(p => p.id)) + 1 : 1;

        const newPlayer = {
            id: newId,
            handle,
            mmr: Number(mmr),
            active: active !== undefined ? active : true
        };

        players.push(newPlayer);

        // respomse
        res.status(201).json({ 
            message: "Player created successfully!", 
            data: newPlayer 
        });
    } catch (error) {
        console.error("POST Error:", error);
        res.status(500).json({ error: "Failed to create player" });
    }
});

// Delete
app.delete("/api/players/:id", async (req, res) => {
    try {
        await simulateNetworkDelay();

        const playerId = parseInt(req.params.id);
        
        const playerExists = players.filter(p => p.id === playerId).length > 0;

        if (!playerExists) {
            return res.status(404).json({ error: `Player with ID ${playerId} not found :(` });
        }

        players = players.filter(p => p.id !== playerId);

        res.status(200).json({ 
            message: `Player ${playerId} deleted succesfully.` 
        });
    } catch (error) {
        console.error("DELETE Error:", error);
        res.status(500).json({ error: "Failed to delete player" });
    }
});


const PORT = 3000;
app.listen(PORT, () => {
    console.log(`Server live on http://localhost:${PORT}`);
});