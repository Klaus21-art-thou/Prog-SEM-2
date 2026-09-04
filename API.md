# API Endpoints Documentation & Section C: SQL Script

This document provides the API endpoint tables (following the project routing specification) for each entity and includes the complete SQL Server Management Studio (SSMS) database schema script and seed data[cite: 10, 11].

---

## Part 1: Entity API Endpoints Specification

### 1. USERS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/users` | Retrieves a list of all registered users in the system[cite: 10]. | Admin[cite: 10] | None[cite: 10] | 200 OK - JSON array of user objects[cite: 10] |
| **POST** | `/api/users` | Registers a new user account within the platform[cite: 10]. | None (public)[cite: 10] | `{ username, userEmail, userRole, userBio }`[cite: 10, 11] | 201 Created - new user object; 400 Bad Request[cite: 10] |
| **GET** | `/api/users/{id}` | Retrieves detailed profile information for a specific user[cite: 10]. | Any (logged in)[cite: 10] | None[cite: 10] | 200 OK - user profile object; 404 Not Found[cite: 10] |
| **PUT** | `/api/users/{id}` | Updates profile details for a specific user ID[cite: 10]. | Member / Admin[cite: 10] | `{ username, userEmail, userRole, userBio }`[cite: 10, 11] | 200 OK - updated user object; 404 Not Found[cite: 10] |
| **DELETE** | `/api/users/{id}` | Deletes a user account from the system[cite: 10]. | Admin[cite: 10] | None[cite: 10] | 204 No Content; 404 Not Found[cite: 10] |

---

### 2. EVENTS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/events` | Retrieves a list of all scheduled race events[cite: 10]. | None (public)[cite: 10] | None[cite: 10] | 200 OK - JSON array of event objects[cite: 10] |
| **POST** | `/api/events` | Creates a new race event under the organizer's profile[cite: 10]. | Organiser[cite: 10, 11] | `{ organiserID, eventName, eventDescription, eventDate, eventLocation, eventType }`[cite: 10, 11] | 201 Created - created event object; 400 Bad Request[cite: 10] |
| **GET** | `/api/events/{id}` | Retrieves specific details and tracks for a given event ID[cite: 10]. | None (public)[cite: 10] | None[cite: 10] | 200 OK - event object; 404 Not Found[cite: 10] |
| **PUT** | `/api/events/{id}` | Updates configuration details for an existing event[cite: 10]. | Organiser[cite: 10, 11] | `{ organiserID, eventName, eventDescription, eventDate, eventLocation, eventType }`[cite: 10, 11] | 200 OK - updated event object; 403 Forbidden[cite: 10] |
| **DELETE** | `/api/events/{id}` | Removes an event from the platform schedule[cite: 10]. | Organiser[cite: 10, 11] | None[cite: 10] | 204 No Content; 404 Not Found[cite: 10] |

---

### 3. TRACKS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/tracks` | Lists all available race tracks across all events[cite: 10]. | None (public)[cite: 10] | None[cite: 10] | 200 OK - JSON array of track items[cite: 10] |
| **POST** | `/api/tracks` | Adds a new track or route layout to a specific event[cite: 10]. | Organiser[cite: 10, 11] | `{ eventID, trackName, distanceKM }`[cite: 10, 11] | 201 Created - track object; 404 Not Found[cite: 10] |
| **GET** | `/api/tracks/{id}` | Retrieves details for a specific track route[cite: 10]. | None (public)[cite: 10] | None[cite: 10] | 200 OK - track object; 404 Not Found[cite: 10] |
| **PUT** | `/api/tracks/{id}` | Modifies track specifications or distance[cite: 10]. | Organiser[cite: 10, 11] | `{ eventID, trackName, distanceKM }`[cite: 10, 11] | 200 OK - updated track object; 404 Not Found[cite: 10] |
| **DELETE** | `/api/tracks/{id}` | Deletes a track configuration from an event[cite: 10]. | Organiser[cite: 10, 11] | None[cite: 10] | 204 No Content; 404 Not Found[cite: 10] |

---

### 4. FEES Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/fees` | Retrieves all registration fee tiers[cite: 10]. | None (public)[cite: 10] | None[cite: 10] | 200 OK - JSON array of fee records[cite: 10] |
| **POST** | `/api/fees` | Establishes a new fee structure for a track[cite: 10]. | Organiser[cite: 10, 11] | `{ trackID, feeName, feeAmount, feeCurrency }`[cite: 10, 11] | 201 Created - fee record object; 400 Bad Request[cite: 10] |
| **GET** | `/api/fees/{id}` | Fetches details for a specific pricing tier[cite: 10]. | None (public)[cite: 10] | None[cite: 10] | 200 OK - fee object; 404 Not Found[cite: 10] |
| **PUT** | `/api/fees/{id}` | Updates fee amounts or descriptions[cite: 10]. | Organiser[cite: 10, 11] | `{ trackID, feeName, feeAmount, feeCurrency }`[cite: 10, 11] | 200 OK - updated fee object; 404 Not Found[cite: 10] |
| **DELETE** | `/api/fees/{id}` | Removes a fee tier from the system[cite: 10]. | Organiser[cite: 10, 11] | None[cite: 10] | 204 No Content; 404 Not Found[cite: 10] |

---

### 5. TEAMS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/teams` | Retrieves a list of all registered racing teams[cite: 10]. | None (public)[cite: 10] | None[cite: 10] | 200 OK - JSON array of teams[cite: 10] |
| **POST** | `/api/teams` | Registers a new team within the platform[cite: 10]. | Member[cite: 10] | `{ teamName, teamDescription }`[cite: 10, 11] | 201 Created - team object; 409 Conflict[cite: 10] |
| **GET** | `/api/teams/{id}` | Fetches information for a specific team[cite: 10]. | None (public)[cite: 10] | None[cite: 10] | 200 OK - team object; 404 Not Found[cite: 10] |
| **PUT** | `/api/teams/{id}` | Updates team profile details[cite: 10]. | Member[cite: 10] | `{ teamName, teamDescription }`[cite: 10, 11] | 200 OK - updated team object; 403 Forbidden[cite: 10] |
| **DELETE** | `/api/teams/{id}` | Disbands or deletes a team profile[cite: 10]. | Organiser[cite: 10, 11] | None[cite: 10] | 204 No Content; 404 Not Found[cite: 10] |

---

### 6. PARTICIPANTS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/participants` | Retrieves all race entry and participation records[cite: 10]. | Organiser[cite: 10, 11] | None[cite: 10] | 200 OK - JSON array of participants[cite: 10] |
| **POST** | `/api/participants` | Registers a user into an event track and fee[cite: 10]. | Participant[cite: 10, 11] | `{ userID, eventID, trackID, feeID, teamID }`[cite: 10, 11] | 201 Created - participant record; 409 Conflict[cite: 10] |
| **GET** | `/api/participants/{id}` | Retrieves specific participation details[cite: 10]. | Participant[cite: 10, 11] | None[cite: 10] | 200 OK - participant object; 404 Not Found[cite: 10] |
| **PUT** | `/api/participants/{id}` | Updates participant registration or team alignment[cite: 10]. | Participant[cite: 10, 11] | `{ userID, eventID, trackID, feeID, teamID }`[cite: 10, 11] | 200 OK - updated participant record; 404 Not Found[cite: 10] |
| **DELETE** | `/api/participants/{id}` | Cancels a participant's race registration[cite: 10]. | Participant[cite: 10, 11] | None[cite: 10] | 204 No Content; 404 Not Found[cite: 10] |

---

### 7. RESULTS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/results` | Retrieves race results and leaderboards[cite: 10]. | None (public)[cite: 10] | None[cite: 10] | 200 OK - JSON array of race results[cite: 10] |
| **POST** | `/api/results` | Records timing and finishing position for a participant[cite: 10]. | Organiser[cite: 10, 11] | `{ participantID, finishTimeSeconds, finishingPosition }`[cite: 10, 11] | 201 Created - result record; 400 Bad Request[cite: 10] |
| **GET** | `/api/results/{id}` | Fetches details for a specific result entry[cite: 10]. | None (public)[cite: 10] | None[cite: 10] | 200 OK - result object; 404 Not Found[cite: 10] |
| **PUT** | `/api/results/{id}` | Updates participant finish time or placement rank[cite: 10]. | Organiser[cite: 10, 11] | `{ participantID, finishTimeSeconds, finishingPosition }`[cite: 10, 11] | 200 OK - updated result record; 404 Not Found[cite: 10] |
| **DELETE** | `/api/results/{id}` | An incorrect result record is deleted[cite: 10]. | Organiser[cite: 10, 11] | None[cite: 10] | 204 No Content; 404 Not Found[cite: 10] |
