# RaceDay Event Management System

## 1. Project Overview

RaceDay is an Event Management System developed using ASP.NET Core Web API and ASP.NET Core MVC. The application allows event organisers to manage running events, categories and participant results, while participants can browse events, enrol in events and view their results.

This project was developed as part of the PROG6212 Programming Portfolio of Evidence (POE), Part 3.

## 2. Technologies Used

- C#
- ASP.NET Core Web API (.NET 8)
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- Microsoft SQL Server
- HTML, CSS and Bootstrap
- JavaScript
- Docker
- Azure Blob Storage (API integration)

## 3. System Features

### Participants
- Register and log in.
- Browse upcoming events.
- View event details.
- View available event categories.
- Enrol in events.
- View personal enrolments.
- View results.

### Organisers
- Log in using an organiser account.
- View and manage events.
- Create, edit and delete events.
- Manage event categories.
- View participant enrolments.
- Manage participant results.

## 4. System Architecture

The system consists of two main applications:

**RaceDayAPI**
- Handles application data and business operations.
- Provides RESTful API endpoints.
- Handles authentication and authorisation.
- Communicates with the SQL Server database.
- Provides the integration point for Azure Blob Storage.

**RaceDayMVC**
- Provides the user interface.
- Uses MVC controllers and views.
- Communicates with the API using HttpClient.
- Displays information returned by the API.
- Provides role-based navigation for participants and organisers.

The MVC application communicates with the API instead of accessing the database directly.

## 5. Project Structure

```text
RaceDay-Event-Management-System/
│
├── RaceDayAPI/
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   └── Program.cs
│
├── RaceDayMVC/
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   ├── wwwroot/
│   ├── Program.cs
│   └── Dockerfile
│
├── README.md
└── .github/
    └── workflows/
```

*Note: Folder locations may differ slightly depending on the final repository structure.*

## 6. Running the Application Locally

### Requirements
- Visual Studio 2022
- .NET 8 SDK
- SQL Server
- Docker Desktop (for container execution)

### Step 1: Run the API

1. Open the RaceDay solution in Visual Studio.
2. Set RaceDayAPI as the startup project.
3. Start the API.
4. The API should be accessible at:

```text
https://localhost:7209
http://localhost:5225
```

Swagger documentation:

```text
https://localhost:7209/swagger
```

### Step 2: Run the MVC Application

1. Open the RaceDayMVC project.
2. Ensure the API is running.
3. Set RaceDayMVC as the startup project.
4. Run the MVC application.

The MVC application communicates with the API using HttpClient.

## 7. Running the MVC Application Using Docker

Docker is used to containerise the RaceDayMVC application.

### Build the Docker image

Open PowerShell in the RaceDayMVC project folder and run:

```powershell
docker build -t raceday-mvc .
```

### Run the Docker container

```powershell
docker run -d -p 8080:8080 --name raceday-mvc raceday-mvc
```

### Access the application

Open your browser and visit:

```text
http://localhost:8080
```

**Important:** The RaceDayAPI must also be running on the host computer using HTTP port 5225. The Docker container connects to it using:

```text
http://host.docker.internal:5225/
```

### Stop the container

```powershell
docker stop raceday-mvc
```

### Remove the container

```powershell
docker rm raceday-mvc
```

## 8. Azure Blob Storage

Azure Blob Storage is intended to provide cloud-based storage for event banner images. Image uploads are handled through the API, and the MVC application communicates with the API to upload and display event images.

*Complete this section with the actual Azure configuration, upload endpoint and image-display implementation once verified.*

## 9. Authentication and Security

The application uses JWT authentication and role-based authorisation.

The system supports two roles:
- Participant
- Organiser

Protected operations are restricted according to the user's role.

## 10. GitHub Repository

Source code and project development history:

https://github.com/reggykoketso/RaceDay-Event-Management-System.git

## 11. Demonstration Video

YouTube demonstration:

[Insert your unlisted YouTube video link here]

The demonstration covers the MVC application, role-based functionality, API integration, Azure Blob Storage and Docker configuration.

## 12. Conclusion

The RaceDay Event Management System brings together a RESTful API and an MVC frontend to provide an event management platform for organisers and participants. Docker provides a containerised environment for running the MVC application.