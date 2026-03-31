# Contacts Manager (WPF)

A **Windows desktop Contacts Manager** built with **WPF** on **.NET 8**, using **MVVM** (CommunityToolkit) and **Entity Framework Core + SQLite** for persistence. The UI includes CRUD actions (Add/Edit/Delete) and a LINQ-focused panel for filtering, sorting, projections, quantifiers, and aggregations.

---

## Features

- **CRUD**: add, edit, delete contacts
- **DataGrid** view of contacts
- **LINQ operations UI**:
  - filtering
  - sorting
  - projection (changing what data is shown)
  - quantifiers/checks (e.g., Any/All-style checks)
  - aggregation (e.g., Count/Max/Average-style results)
- **SQLite persistence** via Entity Framework Core
- **Seed data** is inserted automatically when the database is empty

---

## Tech Stack

- **Language:** C#
- **Framework:** .NET 8 (`net8.0-windows`)
- **UI:** WPF
- **Architecture:** MVVM (`CommunityToolkit.Mvvm`)
- **Database:** SQLite
- **ORM:** Entity Framework Core (`Microsoft.EntityFrameworkCore.Sqlite`)
- **Migrations:** EF Core Tools (`Microsoft.EntityFrameworkCore.Tools`)

---

## Project Layout

The solution/project live inside the `Contacts/` folder:

```text
.
├─ .gitignore
└─ Contacts/
   ├─ Contacts.sln
   ├─ Contacts.csproj
   ├─ App.xaml
   ├─ MainWindow.xaml
   ├─ Converters/
   ├─ Models/
   ├─ ViewModels/
   ├─ Views/
   └─ Migrations/
```

---

## Requirements

- **Windows** (WPF apps run on Windows)
- **.NET SDK 8.x**
- Optional (recommended): **Visual Studio 2022** with **Desktop development with .NET**

---

## Setup & Run

### Clone

```bash
git clone https://github.com/bohdan5202/Contacts.git
cd Contacts
```

### Run (Visual Studio)

1. Open `Contacts/Contacts.sln`
2. Run with **F5**

### Run (CLI)

```bash
dotnet restore Contacts/Contacts.sln
dotnet build Contacts/Contacts.sln
dotnet run --project Contacts/Contacts.csproj
```

---

## Database (SQLite + EF Core)

This app uses **SQLite** via **Entity Framework Core**.

### Connection string / DB file

The DB is configured in code in `Contacts/Models/AppDbContext.cs`:

- `Data Source=contacts.db`

So the database file is:

- `contacts.db`

### Where is `contacts.db` created?

SQLite creates the file in the app’s **current working directory** (commonly the build output folder), for example:

- `Contacts/bin/Debug/net8.0-windows/contacts.db`

### Migrations

Migrations are applied automatically on startup (the app runs EF Core migration code when creating the repository). If you want to manage them manually:

```bash
cd Contacts
dotnet ef migrations list
dotnet ef database update
```

---

## Notes

- If you delete `contacts.db`, the app will recreate the database and re-seed initial data on next run.
- If you move the executable, the database file location may change (because it follows the working directory).

---

## Author

- GitHub: https://github.com/bohdan5202
