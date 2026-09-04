# Section B: API Endpoint Tables (Per Table Entity)

### 1. Users Endpoints
| Method | Route | Description | Role | Response |
| :--- | :--- | :--- | :--- | :--- |
| **POST** | `/api/users/register` | Registers a user account | Public | `201 Created` |
| **POST** | `/api/users/login` | Authenticates user login | Public | `200 OK` |
| **GET** | `/api/users/profile` | Gets authenticated user profile | User | `200 OK` |
| **PUT** | `/api/users/profile` | Updates user profile details | User | `200 OK` |

---

### 2. Events Endpoints
| Method | Route | Description | Role | Response |
| :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/events` | Retrieves all sporting events | Public | `200 OK` |
| **GET** | `/api/events/{id}` | Gets event details by ID | Public | `200 OK` |
| **POST** | `/api/events` | Creates a new race event | Organiser | `201 Created` |
| **PUT** | `/api/events/{id}` | Updates event parameters | Organiser | `200 OK` |
| **DELETE** | `/api/events/{id}` | Removes event from system | Organiser | `200 OK` |

---

### 3. Tracks Endpoints
| Method | Route | Description | Role | Response |
| :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/tracks` | Lists all track routes | Public | `200 OK` |
| **GET** | `/api/events/{eventId}/tracks` | Gets tracks for a specific event | Public | `200 OK` |
| **POST** | `/api/tracks` | Creates event route track | Organiser | `201 Created` |

---

### 4. Fees Endpoints
| Method | Route | Description | Role | Response |
| :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/fees` | Lists all fee tiers | Public | `200 OK` |
| **GET** | `/api/tracks/{trackId}/fees` | Gets fee options for a track | Public | `200 OK` |
| **POST** | `/api/fees` | Adds fee option to track | Organiser | `201 Created` |

---

### 5. Teams Endpoints
| Method | Route | Description | Role | Response |
| :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/teams` | Retrieves all active teams | Public | `200 OK` |
| **POST** | `/api/teams` | Creates a new sports club/team | User | `201 Created` |

---

### 6. Participants Endpoints
| Method | Route | Description | Role | Response |
| :--- | :--- | :--- | :--- | :--- |
| **POST** | `/api/participants` | Enrols user into event track | Participant | `201 Created` |
| **GET** | `/api/participants/my-enrolments` | Lists user's enrolments | Participant | `200 OK` |
| **GET** | `/api/events/{eventId}/participants` | Lists event participant roster | Organiser | `200 OK` |

---

### 7. Results Endpoints
| Method | Route | Description | Role | Response |
| :--- | :--- | :--- | :--- | :--- |
| **POST** | `/api/results` | Logs participant finish time/rank | Organiser | `201 Created` |
| **GET** | `/api/results/my-results` | Gets user's performance history | Participant | `200 OK` |
| **GET** | `/api/events/{eventId}/results` | Gets complete event leaderboard | Public | `200 OK` |
