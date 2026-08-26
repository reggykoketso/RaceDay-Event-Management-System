# RaceDay – API Endpoint Plan

## Purpose
This document defines the planned RESTful API for RaceDay before Part 2 implementation. The implementation should closely follow this plan.

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | `/api/auth/register` | Creates a new RaceDay user account. | None (public) | `{ firstName, lastName, email, password, role, phone }` | `201 Created` – user; `400 Bad Request`; `409 Conflict` |
| POST | `/api/auth/login` | Authenticates a user and returns an access token. | None (public) | `{ email, password }` | `200 OK` – token and user; `401 Unauthorized` |
| GET | `/api/users/me` | Returns the profile of the logged-in user. | Any logged in | None | `200 OK`; `401 Unauthorized` |
| PUT | `/api/users/me` | Updates the logged-in user's profile. | Any logged in | `{ firstName, lastName, phone }` | `200 OK`; `400 Bad Request`; `401 Unauthorized` |
| GET | `/api/events` | Lists upcoming RaceDay events. | Any logged in | None | `200 OK` – event list |
| GET | `/api/events/{id}` | Returns details for one event. | Any logged in | None | `200 OK`; `404 Not Found` |
| POST | `/api/events` | Creates an event. | Organiser | `{ name, description, eventDate, location, distance, eventType }` | `201 Created`; `400 Bad Request`; `403 Forbidden` |
| PUT | `/api/events/{id}` | Updates an organiser's event. | Organiser | Event fields | `200 OK`; `403 Forbidden`; `404 Not Found` |
| DELETE | `/api/events/{id}` | Deletes an organiser's event. | Organiser | None | `204 No Content`; `403 Forbidden`; `404 Not Found` |
| GET | `/api/events/{eventId}/categories` | Lists categories for an event. | Any logged in | None | `200 OK`; `404 Not Found` |
| POST | `/api/events/{eventId}/categories` | Creates a category for an event. | Organiser | `{ categoryName, ageMinimum, ageMaximum, distance }` | `201 Created`; `400 Bad Request`; `403 Forbidden` |
| PUT | `/api/categories/{id}` | Updates an event category. | Organiser | Category fields | `200 OK`; `403 Forbidden`; `404 Not Found` |
| DELETE | `/api/categories/{id}` | Deletes an event category. | Organiser | None | `204 No Content`; `403 Forbidden`; `404 Not Found` |
| GET | `/api/events/{eventId}/route` | Returns route information for an event. | Any logged in | None | `200 OK`; `404 Not Found` |
| POST | `/api/events/{eventId}/route` | Adds route information to an event. | Organiser | `{ routeName, startLocation, finishLocation, distance, routeDescription }` | `201 Created`; `400 Bad Request`; `403 Forbidden` |
| PUT | `/api/events/{eventId}/route` | Updates route information for an event. | Organiser | Route fields | `200 OK`; `403 Forbidden`; `404 Not Found` |
| POST | `/api/events/{eventId}/enrolments` | Enrols the logged-in participant in an event and selected category. | Participant | `{ categoryId }` | `201 Created`; `400 Bad Request`; `404 Not Found`; `409 Conflict` |
| GET | `/api/enrolments/me` | Returns the logged-in participant's enrolments. | Participant | None | `200 OK`; `401 Unauthorized` |
| GET | `/api/events/{eventId}/enrolments` | Returns all enrolments for an organiser's event. | Organiser | None | `200 OK`; `403 Forbidden`; `404 Not Found` |
| POST | `/api/enrolments/{enrolmentId}/result` | Captures the result for an enrolment. | Organiser | `{ finishTime, finishPosition }` | `201 Created`; `400 Bad Request`; `403 Forbidden`; `404 Not Found` |
| GET | `/api/results/me` | Returns the logged-in participant's personal results/performance history. | Participant | None | `200 OK`; `401 Unauthorized` |
| GET | `/api/events/{eventId}/weather` | Retrieves live weather information for an event location/date from an external weather service. | Any logged in | None | `200 OK`; `404 Not Found`; `502 Bad Gateway` |

## Role Summary

### Organiser
- Create, edit and delete events.
- Manage event categories.
- Manage route information.
- View event enrolments.
- Capture participant results.

### Participant
- Create an account and log in.
- Browse upcoming events.
- View categories and routes.
- Enter an event by selecting a category.
- View their own enrolments.
- Track their personal results.
- View live weather information for race preparation.
