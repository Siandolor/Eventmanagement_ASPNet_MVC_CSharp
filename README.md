# WIP Event Management System (ASP.NET MVC / C#)

A web-based event management system built with **ASP.NET MVC** and **C#**, designed to efficiently organize, manage, and track events and participants. The project is currently a **Work in Progress (WIP)** and under continuous development.

---

## Overview

The goal of this project is to develop a **modular and scalable web application** for event management — suitable for organizations, clubs, universities, and businesses. It provides core functionalities for creating, managing, and tracking events and participants while maintaining a clean, maintainable architecture.

---

## Key Features

### Implemented / In Progress
- **User Management:** Registration, authentication, and role-based access control (Admin, Organizer, Participant)
- **Event CRUD Operations:** Create, edit, delete, and view detailed information about events
- **Participant Management:** Register and manage attendees for each event
- **Event Search and Filter:** Search events by category, date, or location
- **Responsive Frontend:** Adaptive UI for mobile and desktop
- **Error Handling & Logging:** Centralized exception management
- **Repository & Unit of Work Pattern:** Clean data access layer

### Planned
- Calendar and timeline views (daily, weekly, monthly)
- Notifications and email invitations
- RESTful API for external access (mobile / IoT integration)
- Export of participant data (CSV, PDF)
- PWA support for mobile devices
- Docker and Azure deployment setup

---

## Architecture & Tech Stack

| Layer | Description |
|-------|--------------|
| **Frontend** | Razor Views + Bootstrap + JavaScript (jQuery / vanilla JS) |
| **Backend** | ASP.NET MVC (C#) using .NET 6+ |
| **Database** | SQL Server / LocalDB via Entity Framework Core (Code-First) |
| **Dependency Injection** | Microsoft.Extensions.DependencyInjection |
| **Design Pattern** | Repository + Unit of Work + Service Layer |
| **Logging** | Built-in .NET ILogger + Serilog (optional) |
| **Testing** | xUnit / NUnit planned |

---

## Installation & Setup

### 1. Clone the repository
```bash
git clone https://github.com/Siandolor/WIP_Eventmanagement_ASPNet_MVC_CSharp.git
```

### 2. Open the project
Open the solution in **Visual Studio 2022+**.

### 3. Configure the database connection
Edit `appsettings.json` (or `Web.config` if applicable):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EventManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 4. Apply EF Core migrations
```bash
Update-Database
```

### 5. Run the project
Press **F5** or click **Start Debugging** in Visual Studio.

---

## Project Structure
```
WIP_Eventmanagement_ASPNet_MVC_CSharp/
│
├── Controllers/        # MVC controllers
├── Models/             # Data models and view models
├── Views/              # Razor pages (UI)
├── Data/               # DbContext and EF migrations
├── Services/           # Business logic layer
├── wwwroot/            # Static files (CSS, JS, images)
├── Tests/              # Unit & integration tests
├── appsettings.json    # App configuration
└── Program.cs          # Application entry point
```

---

## Roadmap
- [ ] Finalize user roles and permissions
- [ ] Add email/SMS notifications for new events
- [ ] Develop calendar-based event visualization
- [ ] Introduce RESTful API endpoints
- [ ] Integrate CI/CD via GitHub Actions or Azure DevOps
- [ ] Add Dockerfile and deployment templates
- [ ] Create demo seed data for testing

---

## Contributing

Contributions are highly welcome! To contribute:
1. Fork this repository
2. Create a new branch: `feature/my-new-feature`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin feature/my-new-feature`
5. Open a pull request

Please create an issue first if you plan a major change, so it can be discussed beforehand.

---

## Author
**Daniel Fitz, MBA, MSc, BSc**  
Vienna, Austria  
Developer & Security Technologist — Post-Quantum Cryptography, Blockchain/Digital Ledger & Simulation  
C/C++ · C# · Java · Python · Visual Basic · ABAP · JavaScript/TypeScript  

International Accounting · Macroeconomics & International Relations · Physiotherapy · Computer Sciences  
Former Officer of the German Federal Armed Forces

---

## License
**MIT License — free for educational and research use.**  
Attribution required for redistribution or commercial adaptation.

---

> “Every event is a system of decisions — balance them well, and time will follow.”
> — Daniel Fitz, 2024
