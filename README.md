# Linh Long ERP

Linh Long ERP is a minimal full-stack sample project demonstrating clean layering, authentication (JWT + Refresh Token), and a React client.  
This repository is a coding-challenge style reference implementation showing practical patterns for a small enterprise app.

---

## Key Features
- Clean Architecture with layered projects (API, Application, Domain, Infrastructure)
- Authentication using ASP.NET Identity with **cookie-based JWT** (access + refresh)
- CQRS-style organization with MediatR and FluentValidation
- React frontend built with Vite + TypeScript
- Works with SQL Server (production) or SQLite (easy local dev)

---

## Tech Stack
- **Backend:** ASP.NET Core 8, Entity Framework Core, ASP.NET Identity, MediatR, FluentValidation  
- **Frontend:** React (Vite + TypeScript), Axios, Zustand (UI state)  
- **Database:** SQL Server (recommended) or SQLite (for quick demos)

---

## Repository Layout
```
LinhLongERP/
 ├─ LinhLongApi/
 │   ├─ LinhLong.Api/           # Web API (controllers, DI, middleware)
 │   ├─ LinhLong.Application/   # Commands, Queries, Handlers, DTOs, Validators
 │   ├─ LinhLong.Domain/        # Entities, Value Objects, Domain Rules
 │   └─ LinhLong.Infrastructure/# EF Core, Identity, Repositories, JWT, Migrations
 └─ LinhLongApp/                # React + Vite + TypeScript frontend
```

> All commands below assume you run them from the repository root.

---

## Prerequisites
- **Git**
- **Node.js ≥ 18** and **npm ≥ 9**
- **.NET SDK 8.x**
- **Database:** SQL Server (local or container) or SQLite (recommended for local testing)

---

## Configuration / Environment Variables

### Backend
`LinhLongApi/appsettings.Development.json` (or user secrets / environment variables)

```jsonc
{
  "ConnectionStrings": {
    // SQL Server
    // "Default": "Server=localhost,1433;Database=LinhLong;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;",
    // SQLite (default)
    "Default": "Data Source=linhlong.db"
  },
  "Jwt": {
    "Issuer": "LinhLong.Issuer",
    "Audience": "LinhLong.Audience",
    "Key": "REPLACE_WITH_A_LONG_RANDOM_SECRET_KEY"
  },
  "AllowedHosts": "*"
}
```

### Frontend  
Create `LinhLongApp/.env` (or copy from the provided `.env.example`):

```
VITE_API_BASE_URL=https://localhost:5161
```

> `.env` is ignored by Git, but `.env.example` is committed as a template.

---

## Quick Start (Local)

### 1️⃣ Backend
```bash
# Restore dependencies
dotnet restore
dotnet tool restore

# Apply EF Core migrations
dotnet ef database update -p ./LinhLong.Infrastructure -s ./LinhLong.Api

# Run API
dotnet run --project ./LinhLong.Api
```

- API URL: https://localhost:5161  
- Swagger: https://localhost:5161/swagger

---

### 2️⃣ Frontend
```bash
cd LinhLongApp
# Copy the example env (works on Windows CMD / PowerShell / Bash)
copy .env.example .env  2>nul || cp .env.example .env 2>/dev/null || true

npm install
npm run dev
```

- Frontend URL: https://localhost:5174
- Reads `VITE_API_BASE_URL` to connect to the API.

---

## 🧩 Database Migration Notes

By default, the project uses **SQLite** for demo purposes.  
If you want to switch to **SQL Server**, follow these steps:

1. Delete the existing migration folder:  
   ```
   LinhLong.Infrastructure/Data/Migrations
   ```

2. Open `LinhLong.Infrastructure/DependencyInjection.cs` and change the database provider line:  
   ```csharp
   options.UseSqlServer(configuration.GetConnectionString("Default"));
   ```

3. Create a new migration for SQL Server:
   ```bash
   dotnet ef migrations add InitialSql -p LinhLong.Infrastructure -s LinhLong.Api -o Data/Migrations
   ```

4. Update the database:
   ```bash
   dotnet ef database update -p LinhLong.Infrastructure -s LinhLong.Api
   ```

After switching, the system will use **SQL Server** instead of SQLite.

---

## 🧑‍💻 Demo Credentials
| Role | Username | Password |
|------|-----------|-----------|
| **Admin** | `admin` | `Admin@123456!` |
| **Viewer** | `viewer` | `Viewer@123456!` |

> These users are seeded automatically during database initialization.  
> Use them to test authentication, JWT refresh, and role-based access.

---

## Authentication (Cookie-Based, Brief)

- Frontend sends `POST /auth/login` with credentials.  
- API validates via ASP.NET Identity and **sets `accessToken` and `refreshToken` as HTTP-only cookies** (with `Secure` in production and appropriate `SameSite`).  
- Frontend **does not attach an Authorization header**. Instead, it calls APIs with `axios` configured as `withCredentials: true`, so cookies are sent automatically.  
- When the access token expires, the frontend calls `POST /auth/refresh-token` (also with credentials). The API reads the **refresh token from the cookie**, issues new cookies, and the client retries if needed.  
- Logout endpoint clears cookies by setting them with an expired date (server-side invalidation as applicable).

**Frontend Axios setup (example):**
```ts
// src/app/axios.ts
import axios from "axios";

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  withCredentials: true, // send cookies with every request
});
```

**CORS essentials (Backend):**
```csharp
services.AddCors(options =>
{
    options.AddPolicy("FE", policy =>
    {
        policy
           .WithOrigins("https://localhost:5174") // frontend dev origin
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials(); // <-- required for cookies
    });
});
app.UseCors("FE");
```

**Cookie flags (Backend when issuing tokens):**
```csharp
var cookieOptions = new CookieOptions
{
    HttpOnly = true,
    Secure = true, // true in production
    SameSite = SameSiteMode.Lax, // or Strict/None depending on scenario
    Path = "/",
    Expires = DateTimeOffset.UtcNow.AddMinutes(15) // for access token; refresh token longer
};
Response.Cookies.Append("accessToken", jwt, cookieOptions);
```

---

## Common Commands

### Backend
```bash
# New migration
dotnet ef migrations add InitialIdentity -p ./LinhLong.Infrastructure -s ./LinhLong.Api -o Data/Migrations
# Update database
dotnet ef database update -p ./LinhLong.Infrastructure -s ./LinhLong.Api
# Run API
dotnet run --project ./LinhLong.Api
```

### Frontend
```bash
npm run dev        # Start dev server
npm run lint       # Check code style
npm run format     # Format code
npm run build      # Production build
npm run preview    # Preview production build
```

---

## Troubleshooting
- **401 without Authorization header** → Ensure `axios` has `withCredentials: true` and CORS policy has `.AllowCredentials()` with the correct origin.  
- **Cookies not set** → Check `Secure`, `SameSite`, and that you are using HTTPS in dev (or relax cookie options during dev).  
- **404** → Ensure API is running at `https://localhost:5161` and CORS allows `https://localhost:5174`.  
- **EF migration not found** → Check `LinhLong.Infrastructure/Migrations` exists and use `-p` and `-s` flags.  
- **HTTPS cert errors** → Run `dotnet dev-certs https --trust` or switch to HTTP for local-only testing.

---

## Notes
- Keep secrets (JWT keys, DB passwords) **out of source control**.  
- SQLite is the easiest option for quick testing.  
- Swagger provides interactive API documentation at `/swagger`.

---

**Prepared by:** Dao Hoang Sao – October 2025
