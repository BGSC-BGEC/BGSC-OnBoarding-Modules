import pg from "pg";
import "dotenv/config";

const pool = new pg.Pool({
    connectionString: process.env.DATABASE_URL
});

export const createUser = async (username, email) =>
    (await pool.query(
        "INSERT INTO users (username, email) VALUES ($1, $2) RETURNING *",
        [username, email]
    )).rows[0];

export const createMatch = async (title, gameCategory, creatorId, scheduledAt) =>
    (await pool.query(
        `INSERT INTO tournament_matches
        (title, game_category, creator_id, scheduled_at)
        VALUES ($1, $2, $3, $4) RETURNING *`,
        [title, gameCategory, creatorId, scheduledAt]
    )).rows[0];

export const getMatches = async () =>
    (await pool.query(`
        SELECT m.title, m.game_category, u.username AS organizer
        FROM tournament_matches m
        INNER JOIN users u ON m.creator_id = u.id
    `)).rows;