# SkillSwap ASP.NET Core MVC Application

## Overview
This is the web version of the SkillSwap application, migrated from WinForms to ASP.NET Core MVC. The application allows students to share and trade their talents with each other.

## Project Structure
```
SkillSwap.Web/
├── Controllers/           # MVC Controllers
├── Views/                # Razor Views
├── ViewModels/           # View-specific models
├── Models/               # Domain models (reused from WinForms)
├── Services/             # Business logic services (reused from WinForms)
├── Data/                 # Entity Framework data layer (reused from WinForms)
└── wwwroot/             # Static files (CSS, JS, images)
```

## Features
- **Authentication**: Student login and registration
- **Dashboard**: Overview of talents and trade requests
- **Talent Management**: Add, edit, and delete personal talents
- **Trade System**: Create, accept, reject, and complete trade requests
- **Student Management**: View and manage student profiles

## Getting Started

### Prerequisites
- .NET 10.0 SDK
- Visual Studio 2022 or VS Code

### Setup
1. Clone the repository
2. Navigate to the SkillSwap.Web directory
3. Restore packages:
   ```bash
   dotnet restore
   ```
4. Update database (if needed):
   ```bash
   dotnet ef database update
   ```
5. Run the application:
   ```bash
   dotnet run
   ```

### Database
The application uses SQLite for data storage. The database file will be created automatically in the project directory.

## Architecture
- **MVC Pattern**: Separation of concerns with Controllers, Views, and Models
- **Dependency Injection**: Services are registered and injected automatically
- **Entity Framework Core**: Database operations and migrations
- **Session Management**: User authentication and session state

## Migration Notes
This web version reuses approximately 70% of the original WinForms code:
- ✅ **Models**: Domain models are identical
- ✅ **Services**: Business logic services are unchanged
- ✅ **Data Layer**: Entity Framework setup is preserved
- 🔄 **UI**: Completely replaced with web interface

The migration maintains the same business logic while providing a modern web interface.
