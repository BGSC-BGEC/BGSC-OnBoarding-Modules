# Module 01 Submission - Chaitanya Ahuja (@zenodev01)


- **Track:** AppDev
- **Capacity Load:** Heavy Load (Full Spec)
- **How to Run:**
  1. `cd submissions/Module_01/AppDev/<your-github-handle> && npm install`
  2. `node server.js`
- **Endpoints Implemented:**
  - `GET /api/players` (Returns formatted players, supports `?minMmr=` filter, returns total MMR via `.reduce()`)
  - `POST /api/players` (Validates body, generates unique ID, returns 201 Created)
  - `DELETE /api/players/:id` (Checks existence via `.filter()`, returns 404 or deletes and returns 200 OK)
