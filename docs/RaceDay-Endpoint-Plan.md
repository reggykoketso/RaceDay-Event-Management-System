# RaceDay API Endpoint Plan

## Authentication

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | /api/auth/register | Creates a new user account for the RaceDay system. | None (public) | `{ firstName, lastName, email, password, role, phone }` | 201 Created - account created successfully; 400 Bad Request - invalid or incomplete data; 409 Conflict - email already registered |
| POST | /api/auth/login | Authenticates a registered user and provides an authentication token. | None (public) | `{ email, password }` | 200 OK - login successful with token and user details; 401 Unauthorized - invalid credentials |

## User Profile

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/users/me | Retrieves the profile of the currently logged-in user. | Any (logged in) | None | 200 OK - user profile; 401 Unauthorized - authentication required |
| PUT | /api/users/me | Updates the profile of the currently logged-in user. | Any (logged in) | `{ firstName, lastName, phone }` | 200 OK - updated profile; 400 Bad Request - invalid data; 401 Unauthorized - authentication required |

## Events

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/events | Retrieves a list of upcoming RaceDay events. | None (public) | None | 200 OK - list of events |
| GET | /api/events/{id} | Retrieves details for a specific event. | None (public) | None | 200 OK - event details; 404 Not Found - event does not exist |
| POST | /api/events | Creates a new RaceDay event. | Organiser | `{ name, description, eventDate, location, distance, eventType }` | 201 Created - event created; 400 Bad Request - invalid data |
| PUT | /api/events/{id} | Updates an existing event. | Organiser | `{ name, description, eventDate, location, distance, eventType }` | 200 OK - updated event; 404 Not Found - event does not exist |
| DELETE | /api/events/{id} | Deletes an existing event. | Organiser | None | 204 No Content - event deleted; 404 Not Found - event does not exist |

## Categories

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/events/{eventId}/categories | Retrieves all categories available for a specific event. | None (public) | None | 200 OK - list of categories; 404 Not Found - event does not exist |
| POST | /api/events/{eventId}/categories | Creates a new category for a specific event. | Organiser | `{ categoryName, ageMinimum, ageMaximum, distance }` | 201 Created - category created successfully; 400 Bad Request - invalid data; 404 Not Found - event does not exist |
| GET | /api/categories/{id} | Retrieves the details of a specific event category. | None (public) | None | 200 OK - category details; 404 Not Found - category does not exist |
| PUT | /api/categories/{id} | Updates an existing event category. | Organiser | `{ categoryName, ageMinimum, ageMaximum, distance }` | 200 OK - category updated successfully; 400 Bad Request - invalid data; 404 Not Found - category does not exist |
| DELETE | /api/categories/{id} | Deletes an existing event category. | Organiser | None | 204 No Content - category deleted successfully; 404 Not Found - category does not exist; 409 Conflict - category is already being used by an enrolment |

## Event Enrolments

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | /api/events/{eventId}/enrolments | Enrols the logged-in participant in an event by selecting an available category. | Participant | `{ categoryId }` | 201 Created - enrolment created successfully; 400 Bad Request - invalid category; 404 Not Found - event or category does not exist; 409 Conflict - participant is already enrolled |
| GET | /api/enrolments/me | Retrieves all event enrolments belonging to the logged-in participant. | Participant | None | 200 OK - list of the participant's enrolments; 401 Unauthorized - authentication required |
| GET | /api/events/{eventId}/enrolments | Retrieves all participants enrolled in a specific event. | Organiser | None | 200 OK - list of event enrolments; 404 Not Found - event does not exist |
| GET | /api/enrolments/{id} | Retrieves details of a specific enrolment. | Participant or Organiser | None | 200 OK - enrolment details; 404 Not Found - enrolment does not exist; 403 Forbidden - insufficient permissions |
| DELETE | /api/enrolments/{id} | Removes a participant's enrolment from an event. | Participant | None | 204 No Content - enrolment removed; 404 Not Found - enrolment does not exist; 403 Forbidden - enrolment does not belong to the logged-in participant |

## Results

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | /api/enrolments/{enrolmentId}/result | Records a participant's result for a specific event enrolment. | Organiser | `{ finishTime, finishPosition }` | 201 Created - result recorded successfully; 400 Bad Request - invalid result data; 404 Not Found - enrolment does not exist |
| GET | /api/results/me | Retrieves the logged-in participant's personal race results. | Participant | None | 200 OK - list of personal results; 401 Unauthorized - authentication required |
| GET | /api/events/{eventId}/results | Retrieves the results for all participants in a specific event. | Organiser | None | 200 OK - event results; 404 Not Found - event does not exist |
| GET | /api/results/{id} | Retrieves details of a specific participant result. | Participant or Organiser | None | 200 OK - result details; 404 Not Found - result does not exist; 403 Forbidden - insufficient permissions |
| PUT | /api/results/{id} | Updates an existing participant result. | Organiser | `{ finishTime, finishPosition }` | 200 OK - result updated successfully; 400 Bad Request - invalid result data; 404 Not Found - result does not exist |

## Routes and Weather

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/events/{eventId}/route | Retrieves route information for a specific event. | None (public) | None | 200 OK - route information; 404 Not Found - route does not exist |
| GET | /api/events/{eventId}/weather | Retrieves current or forecast weather information for a specific event. | None (public) | None | 200 OK - weather information; 404 Not Found - event does not exist |
