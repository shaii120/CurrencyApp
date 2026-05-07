# Currency Rates Simulator

A simple multi-layered currency rates simulator built with:

- C#
- WinForms
- ADO.NET
- SQL Server

The application simulates live currency rate changes and updates the UI in real time while storing minimum and maximum values in the database.

---

# Project Structure

```text
Solution
│
├── Database
│   └── Setup SQL
│
├── DataLayer
│   └── Database access using ADO.NET
│
├── BusinessLayer
│   └── Simulation logic and business rules
│
└── UILayer
    └── WinForms user interface
```

---

# Requirements

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 (recommended)

---

# Database Setup

1. Open SQL Server Management Studio (SSMS)

2. Run the SQL setup scripts:

```text
001_CreateTables.sql
002_SeedData.sql
```

The script will:
- Create the database
- Create the required tables
- Insert initial seed data

---

# Configuration
The connection string can be configured in:

```text
UILayer/App.config
```

Update the SQL Server instance name if needed.

---

# Running the Application

1. Open the solution in Visual Studio

2. Set:

```text
UILayer
```

as the Startup Project

3. Build the solution

4. Run the application

---

# Features

- Real-time currency rate simulation
- Automatic UI updates
- Min/Max tracking
- Database persistence
- Multi-layer architecture
- Event-driven UI refresh

---

# Notes

- The application updates currency values every few seconds.
- Database updates occur only when Min/Max values change.
- UI updates are performed safely using the UI thread.
