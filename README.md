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

## Part 1 Video Presentation

The Part 1 video presentation will be added here once it has been recorded and uploaded to YouTube.

## CI/CD

The GitHub Actions validation workflow will be demonstrated here once the workflow has been successfully executed.

