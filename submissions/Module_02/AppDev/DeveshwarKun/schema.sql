CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(120) UNIQUE NOT NULL,
    reputation_score INTEGER DEFAULT 100,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE tournament_matches (
    id SERIAL PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    game_category VARCHAR(50) NOT NULL,
    creator_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    scheduled_at TIMESTAMP NOT NULL
);


SELECT m.title, m.game_category, u.username AS organizer
FROM tournament_matches m
INNER JOIN users u ON m.creator_id = u.id;