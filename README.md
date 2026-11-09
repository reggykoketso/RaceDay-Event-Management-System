# RaceDay Event Management System

## PROG6212 – Part 3

**Student:** Malesela Regina Mothiba  
**Project:** RaceDay Event Management System  
**Repository:** [GitHub – RaceDay Event Management System](https://github.com/reggykoketso/RaceDay-Event-Management-System)  
**Demonstration Video:** [Watch the RaceDay Project Presentation](https://youtu.be/U_qnsVxZCAY)

---

## 1. Project Overview

RaceDay is an event management system developed to manage running events and participants. The system allows organisers to manage events and allows participants to browse available events, enrol and view their event information.

Part 3 extends the RESTful API developed in Part 2 by introducing an ASP.NET Core MVC frontend. The MVC application communicates with the API to retrieve and manage information.

## 2. Technologies Used

- C#
- ASP.NET Core Web API
- ASP.NET Core MVC
- .NET 8
- Entity Framework Core
- Microsoft SQL Server
- HTML and CSS
- Bootstrap
- JavaScript
- Docker
- Azure Blob Storage
- Git and GitHub
- Swagger / OpenAPI

## 3. System Features

### Participant Features

- Register and log in.
- Browse available events.
- View event information.
- Enrol in events.
- View personal enrolments.
- Access available results and profile functionality.

### Organiser Features

- Register and log in as an organiser.
- Access organiser functionality.
- Create and manage events.
- Manage event information.
- Manage categories and enrolments.
- Access results management functionality.

### API Integration

The MVC frontend communicates with the RaceDay RESTful API using HTTP requests and `HttpClient`.

The API handles communication with the database, while the MVC application provides the user interface.

## 4. Project Architecture

The system consists of two main applications:

**RaceDay API**
- Handles application data and business operations.
- Communicates with the SQL Server database.
- Provides RESTful endpoints.
- Handles authentication and authorisation.

**RaceDay MVC**
- Provides the user interface.
- Communicates with the API.
- Displays event information.
- Provides participant and organiser functionality.

## 5. Database

The application uses Microsoft SQL Server with Entity Framework Core.

The database stores information relating to:
- Users
- Events
- Categories
- Routes
- Enrolments
- Results

Entity Framework Core is used to manage database operations and migrations.

## 6. Authentication and Security

The system includes registration and login functionality, with role-based access for organisers and participants.

Authorisation is used to restrict access to functionality according to user roles.

## 7. Azure Blob Storage

Azure Blob Storage integration was implemented in the API to support event banner image uploads.

The implementation includes:
- A `BlobStorageService` for uploading images.
- A `BlobStorageController` endpoint for handling image uploads.
- Image file validation, including file type and size checks.
- An MVC upload flow that sends the selected image to the API.

**Important:** Azure Storage account setup was attempted, but container validation prevented successful cloud upload testing. Therefore, the Azure Blob Storage upload functionality has not been fully verified in the Azure environment.

A valid Azure Blob Storage connection string and container configuration are still required to test the complete upload process.

## 8. Docker

The MVC application was containerised using Docker.

A Dockerfile was created using the .NET 8 SDK image to build and publish the application and the .NET 8 ASP.NET runtime image to run it.

### Build the Docker image

Open a terminal inside the `RaceDayMVC` project folder and run:

```bash
docker build -t raceday-mvc .
```

### Run the Docker container

```bash
docker run -d -p 8080:8080 --name raceday-mvc raceday-mvc
```

### Access the application

Open the following address in a browser:

```text
http://localhost:8080
```

**Note:** The RaceDay API must also be running on the host computer at:

```text
http://localhost:5225
```

The Docker configuration uses `host.docker.internal` to allow the MVC container to communicate with the API running on the host machine.

## 9. Running the Application Locally

1. Clone the GitHub repository.
2. Open the solution in Visual Studio.
3. Configure the SQL Server connection string.
4. Ensure the database is created and migrations are applied.
5. Start the RaceDay API.
6. Start the RaceDay MVC application.
7. Open the MVC application in your browser.

The API Swagger documentation can be accessed at:

```text
https://localhost:7209/swagger
```

## 10. Demonstration Video

The project demonstration video explains the RaceDay system, its MVC frontend, API integration, application functionality, Azure Blob Storage implementation and Docker containerisation.

**Video link:** https://youtu.be/U_qnsVxZCAY

## 11. GitHub Repository

The complete project source code is available here:

https://github.com/reggykoketso/RaceDay-Event-Management-System

## 12. Conclusion

The RaceDay Event Management System demonstrates the development of a web-based event management application using ASP.NET Core MVC and a RESTful API.

Through this project, I gained practical experience in API integration, database management, authentication, role-based functionality and Docker containerisation. The project also introduced Azure Blob Storage integration for handling event banner images.

