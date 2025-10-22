# Linh-Long ERP

Linh-Long ERP is a minimal full-stack sample project demonstrating clean layering, authentication (JWT + Refresh Token), and a React client.  
This repository is a coding-challenge style reference implementation showing practical patterns for a small enterprise app.

---

## Key Features
- Clean Architecture with layered projects (API, Application, Domain, Infrastructure)
- Authentication using ASP.NET Identity, JWT Access Tokens + Refresh Tokens
- CQRS-style organization with MediatR and FluentValidation
- React frontend built with Vite + TypeScript
- Works with SQL Server (production) or SQLite (easy local dev)

---

## Tech Stack
- **Backend:** ASP.NET Core 8, Entity Framework Core, ASP.NET Identity, MediatR, FluentValidation  
- **Frontend:** React (Vite + TypeScript), Axios, Zustand (auth state)  
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

## 🧑‍💻 Demo Credentials
| Role | Username | Password |
|------|-----------|-----------|
| **Admin** | `admin` | `Admin@123456!` |
| **Viewer** | `viewer` | `Viewer@123456!` |

> These users are seeded automatically during database initialization.  
> Use them to test authentication, JWT refresh, and role-based access.

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

## Authentication (Brief)
- Frontend sends `POST /auth/login` with credentials.  
- API validates user via ASP.NET Identity → returns **Access Token** + **Refresh Token**.  
- Frontend stores tokens in memory (Zustand) and Axios attaches `Authorization: Bearer <token>`.  
- On 401 (expired), Axios calls `POST /auth/refresh-token`, receives new tokens, and retries.  
- Logout clears tokens and invalidates the refresh token server-side.

---

## Troubleshooting
- **404** → Ensure API is running at `https://localhost:5161` and CORS allows `https://localhost:5174`.  
- **EF migration not found** → Check `LinhLong.Infrastructure/Migrations` exists and use `-p` and `-s` flags.  
- **SQL Server container fails** → Use a strong SA password (≥ 8 chars with uppercase, lowercase, number, symbol).  
- **CORS issues** → Configure policy in `LinhLong.Api` Startup to allow frontend origin.  
- **HTTPS cert errors** → Run `dotnet dev-certs https --trust` or switch to HTTP for local testing.

---

## Notes
- Keep secrets (JWT keys, DB passwords) **out of source control**.  
- SQLite is the easiest option for quick testing.  
- Swagger provides interactive API documentation at `/swagger`.

---

**Prepared by:** Dao Hoang Sao – October 2025
