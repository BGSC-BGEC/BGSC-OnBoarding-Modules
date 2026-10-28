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
console.log(totalMmr);


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

handleProfileRequest(1);
handleProfileRequest(-1);