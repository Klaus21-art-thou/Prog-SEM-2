# API Endpoints Documentation & Section C: SQL Script

This document provides the API endpoint tables (following the project routing specification) for each entity and includes the complete SQL Server Management Studio (SSMS) database schema script and seed data.

---

## Part 1: Entity API Endpoints Specification

### 1. USERS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/users` | Retrieves a list of all registered users in the system. | Admin | None | 200 OK - JSON array of user objects |
| **POST** | `/api/users` | Registers a new user account within the platform. | None (public) | `{ username, userEmail, userRole, userBio }` | 201 Created - new user object; 400 Bad Request |
| **GET** | `/api/users/{id}` | Retrieves detailed profile information for a specific user. | Any (logged in) | None | 200 OK - user profile object; 404 Not Found |
| **PUT** | `/api/users/{id}` | Updates profile details for a specific user ID. | Member / Admin | `{ username, userEmail, userBio }` | 200 OK - updated user object; 404 Not Found |
| **DELETE** | `/api/users/{id}` | Deletes a user account from the system. | Admin | None | 204 No Content; 404 Not Found |

---

### 2. EVENTS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/events` | Retrieves a list of all scheduled race events. | None (public) | None | 200 OK - JSON array of event objects |
| **POST** | `/api/events` | Creates a new race event under the organizer's profile. | Organizer | `{ eventName, eventDescription, eventDate, eventLocation, eventType }` | 201 Created - created event object; 400 Bad Request |
| **GET** | `/api/events/{id}` | Retrieves specific details and tracks for a given event ID. | None (public) | None | 200 OK - event object; 404 Not Found |
| **PUT** | `/api/events/{id}` | Updates configuration details for an existing event. | Organizer | `{ eventName, eventDescription, eventDate, eventLocation, eventType }` | 200 OK - updated event object; 403 Forbidden |
| **DELETE** | `/api/events/{id}` | Removes an event from the platform schedule. | Organizer | None | 204 No Content; 404 Not Found |

---

### 3. TRACKS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/tracks` | Lists all available race tracks across all events. | None (public) | None | 200 OK - JSON array of track items |
| **POST** | `/api/tracks` | Adds a new track or route layout to a specific event. | Organizer | `{ eventID, trackName, distanceKM }` | 201 Created - track object; 404 Not Found |
| **GET** | `/api/tracks/{id}` | Retrieves details for a specific track route. | None (public) | None | 200 OK - track object; 404 Not Found |
| **PUT** | `/api/tracks/{id}` | Modifies track specifications or distance. | Organizer | `{ trackName, distanceKM }` | 200 OK - updated track object; 404 Not Found |
| **DELETE** | `/api/tracks/{id}` | Deletes a track configuration from an event. | Organizer | None | 204 No Content; 404 Not Found |

---

### 4. FEES Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/fees` | Retrieves all registration fee tiers. | None (public) | None | 200 OK - JSON array of fee records |
| **POST** | `/api/fees` | Establishes a new fee structure for a track. | Organizer | `{ trackID, feeName, feeAmount, feeCurrency }` | 201 Created - fee record object; 400 Bad Request |
| **GET** | `/api/fees/{id}` | Fetches details for a specific pricing tier. | None (public) | None | 200 OK - fee object; 404 Not Found |
| **PUT** | `/api/fees/{id}` | Updates fee amounts or descriptions. | Organizer | `{ feeName, feeAmount, feeCurrency }` | 200 OK - updated fee object; 404 Not Found |
| **DELETE** | `/api/fees/{id}` | Removes a fee tier from the system. | Organizer | None | 204 No Content; 404 Not Found |

---

### 5. TEAMS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/teams` | Retrieves a list of all registered racing teams. | None (public) | None | 200 OK - JSON array of teams |
| **POST** | `/api/teams` | Registers a new team within the platform. | Member | `{ teamName, teamDescription }` | 201 Created - team object; 409 Conflict |
| **GET** | `/api/teams/{id}` | Fetches information for a specific team. | None (public) | None | 200 OK - team object; 404 Not Found |
| **PUT** | `/api/teams/{id}` | Updates team profile details. | Member | `{ teamName, teamDescription }` | 200 OK - updated team object; 403 Forbidden |
| **DELETE** | `/api/teams/{id}` | Disbands or deletes a team profile. | Organizer | None | 204 No Content; 404 Not Found |

---

### 6. PARTICIPANTS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/participants` | Retrieves all race entry and participation records. | Organizer | None | 200 OK - JSON array of participants |
| **POST** | `/api/participants` | Registers a logged-in user into an event track and fee. | Member | `{ userID, eventID, trackID, feeID, teamID }` | 201 Created - participant record; 409 Conflict (already entered) |
| **GET** | `/api/participants/{id}` | Retrieves specific participation details. | Member | None | 200 OK - participant object; 404 Not Found |
| **PUT** | `/api/participants/{id}` | Updates participant registration or team alignment. | Member | `{ trackID, feeID, teamID }` | 200 OK - updated participant record; 404 Not Found |
| **DELETE** | `/api/participants/{id}` | Cancels a participant's race registration. | Member | None | 204 No Content; 404 Not Found |

---

### 7. RESULTS Endpoints
| HTTP method | Route | Description | Role required | Request body | Expected response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/results` | Retrieves race results and leaderboards. | None (public) | None | 200 OK - JSON array of race results |
| **POST** | `/api/results` | Records timing and finishing position for a participant. | Organizer | `{ participantID, finishTimeSeconds, finishingPosition }` | 201 Created - result record; 400 Bad Request |
| **GET** | `/api/results/{id}` | Fetches details for a specific result entry. | None (public) | None | 200 OK - result object; 404 Not Found |
| **PUT** | `/api/results/{id}` | Updates participant finish time or placement rank. | Organizer | `{ finishTimeSeconds, finishingPosition }` | 200 OK - updated result record; 404 Not Found |
| **DELETE** | `/api/results/{id}` | Deletes an incorrect result record. | Organizer | None | 204 No Content; 404 Not Found |

---