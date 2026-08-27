# RaceDay – Event Management System

## Project Overview

RaceDay is a full-stack web-based event management system designed for the South African road running, walking and cycling community. The system aims to make it easier for event organisers to manage events, categories, participant enrolments and race results, while allowing participants to browse upcoming events, enter races and track their personal performance.

The system is being developed progressively across three parts. Part 1 focuses on system planning and database design before application code is written. This includes the Entity Relationship Diagram (ERD), API endpoint plan and SQL Server database script.

## System Roles

### Organiser

The Organiser is responsible for managing events on the RaceDay platform. An Organiser can:

* Create, edit and delete events.
* Create and manage event categories.
* View participant enrolments.
* Capture and manage participant results.
* Manage information required for race-day events.

### Participant

The Participant is a user who takes part in RaceDay events. A Participant can:

* Create an account and log in.
* Browse upcoming events.
* Select an event category and enrol in an event.
* View their own event enrolments.
* View and track their personal race results.

## Part 1 – System Planning and Database

Part 1 establishes the foundation of the RaceDay system before application development begins. The planning documents include:

* Entity Relationship Diagram (ERD)
* API Endpoint Plan
* SQL Server Database Script

The SQL database contains the entities required to support organisers, participants, events, categories, routes, enrolments and results. The database script also contains fictional sample data used to test and demonstrate the database.

## Repository Structure

```text
RaceDay/
├── docs/
│   ├── RaceDay-ERD.png
│   ├── RaceDay-Endpoint-Plan.md
│   ├── RaceDay.sql
│   └── Seed-Data-Note.md
│
├── .github/
│   └── workflows/
│       └── validate-docs.yml
│
└── README.md
```

**## Part 1 Video Presentation**

The Part 1 video presentation demonstrates the RaceDay system planning, ERD, API Endpoint Plan, SQL Server database and GitHub Actions validation.

[Watch the Part 1 Video on YouTube](https://youtu.be/KbGUxsuErm0)

## CI/CD

The GitHub Actions validation workflow was successfully executed to verify the required RaceDay documentation structure.

The successful workflow run is shown in the CI/CD evidence screenshot below.

![CI/CD Success](evidence/CI-CD-Success.png)

## Database

The RaceDay system uses a relational SQL Server database to store and manage information required by the platform.

The database design includes users, events, categories, routes, enrolments and results. Primary keys are used to uniquely identify records, while foreign keys establish relationships between related entities.

The database script is located in the `/docs` folder and contains the table definitions, constraints and fictional sample data required for testing.

The SQL script was tested using SQL Server Management Studio (SSMS).

## Part 1 Requirements

The planning stage of the RaceDay project establishes the foundation for the later development of the system.

The main deliverables for Part 1 are:

- Entity Relationship Diagram (ERD) showing the database entities, attributes, keys and relationships.
- API Endpoint Plan covering authentication, user profiles, events, categories, enrolments and results.
- SQL Server database script containing the complete database schema, constraints and sample data.
- GitHub repository containing the required planning documents.
- GitHub Actions workflow to validate the required repository structure.
- Video presentation explaining the planning decisions and demonstrating the SQL database script in SSMS.

## Documentation

All Part 1 planning and database documents are stored in the `/docs` folder.

### ERD

The ERD provides a visual representation of the RaceDay database. It identifies the entities, attributes, primary keys, foreign keys and relationships required by the system.

### API Endpoint Plan

The API Endpoint Plan defines the RESTful endpoints that will be implemented in later parts of the project. It specifies the HTTP method, route, purpose, required role, request body and expected response for each endpoint.

### SQL Database Script

The SQL script creates and populates the RaceDay SQL Server database. It includes the database tables, primary keys, foreign keys, constraints and sample data required for testing.

These documents form the foundation for the API and MVC application that will be developed in Parts 2 and 3.

## Database Testing

The RaceDay SQL database script was tested using SQL Server Management Studio (SSMS).

The script was executed to create the database tables, relationships, constraints and sample records. Verification queries were also used to confirm that the sample organisers, participants, events, categories, enrolments and results were stored correctly.

The SQL script and supporting testing evidence are included in the `/docs` folder.

## Part 1 Progress

The main planning and database activities for Part 1 have been completed.

- [x] ERD created
- [x] API endpoint plan created
- [x] SQL Server database script created
- [x] Database sample data added
- [x] SQL script tested in SSMS
- [x] Project documentation added
- [x] GitHub Actions validation
- [x] CI/CD success screenshot
- [x] Part 1 video presentation
- [x] YouTube video link added to README


## GitHub Repository

The repository is organised so that the planning documents and supporting configuration files are kept in clearly defined locations.

The `/docs` folder contains the Part 1 planning and database documentation, while the `.github/workflows` folder contains the GitHub Actions workflow used to validate the required documentation structure.

This structure will be maintained as the project progresses through Parts 2 and 3.

## Future Development

Part 1 provides the foundation for the development of the RaceDay platform.

In Part 2, the planned RESTful API will be implemented and connected to the SQL Server database. Authentication and role-based access control will also be implemented and tested.

In Part 3, an MVC web application will consume the API and provide the user interface for Organisers and Participants. Additional functionality such as Azure Blob Storage and Docker containerisation will also be integrated.

The database and API planning completed in Part 1 will therefore be used as the foundation for the remaining stages of the project.