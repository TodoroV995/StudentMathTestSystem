# Student Math Test System

## Overview

Student Math Test System is a .NET-based application for automatic processing and grading of arithmetic exams provided through XML documents.

The system allows teachers to manage students, upload XML exam files, automatically process and evaluate arithmetic expressions, and review generated results. Students can review their exam analytics and see which tasks were answered correctly or incorrectly.

---

## Features

- Student management
- Import students from XML
- Upload and process exam XML files
- Automatic arithmetic expression evaluation
- Mass processing of multiple students and exams
- Student analytics and exam review
- REST API integration point for third-party systems
- Independent mathematical expression processor

---

## Architecture

The solution follows a layered architecture and is separated into multiple projects:

### Api
ASP.NET Core Web API project that exposes endpoints for student management, XML uploads, and result retrieval.

### Application
Contains DTOs, interfaces, and application contracts shared across layers.

### Domain
Contains core entities and enums representing the business domain.

### Infrastructure
Contains Entity Framework Core database access, services, XML parsing, and application logic implementations.

### MathProcessor
Independent project responsible for arithmetic expression evaluation and mathematical processing.

### Wpf
Desktop application built with WPF and MVVM pattern for teacher and student user interfaces.

---

## Technologies Used

- .NET
- ASP.NET Core Web API
- WPF
- MVVM Pattern
- Entity Framework Core
- SQL Server
- XML Processing
- REST API
- C#

---

## How to Run

### 1. Configure Database

Update the SQL Server connection string inside:

```json
Api/appsettings.json
```

### 2. Apply Database Migration

Run the following command from Package Manager Console:

```powershell
Update-Database
```

### 3. Start the Solution

The solution is configured with multiple startup projects (`Api` and `Wpf`).

Run the solution from Visual Studio.

### 4. API Access

After starting the application, Swagger documentation will be available on the API host URL, for example:

```text
https://localhost:{port}/swagger
```

The port may vary depending on the local environment and Visual Studio configuration.

---

## Example XML Files

Example XML files are included in the project:

- `StudentsExample.xml`
- `ExamExample.xml`

These files can be used for testing student import and exam processing functionality.

---

## Main API Endpoints

### Students

```http
GET    /api/Students
POST   /api/Students
PUT    /api/Students/{id}
DELETE /api/Students/{id}
POST   /api/Students/import-xml
```

### Exam Upload

```http
POST /api/ExamUploads/xml
```

### Results

```http
GET /api/Results
GET /api/Results/student/{studentId}
GET /api/Results/{id}
```

---

## Assumptions

- Students and exams should already exist in the database before processing exam XML files.
- XML schema is predefined and known by the system.
- Arithmetic expressions support standard arithmetic operations.
- The solution focuses on Microsoft technologies and layered architecture.
- XML files are uploaded and processed through API endpoints.
- The math processor is separated into an independent project for easier maintenance and reuse.

---

## Future Improvements

- Add automated unit and integration tests
- Add global exception handling and centralized error management
- Add authentication and login functionality
- Separate teacher and student user roles and permissions
- Expand the system to support multiple teachers and classrooms
- Improve XML validation and reporting
- Improve UI styling and user experience