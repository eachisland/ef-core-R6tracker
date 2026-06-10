# R6 Tracker — ASP.NET Core 8 Web API

A Rainbow Six Siege player statistics tracker built with ASP.NET Core 8, Entity Framework Core and ASP.NET Identity.

## Project Structure

| Project | Purpose |
|---|---|
| `R6tracker.API` | Web API controllers, Program.cs, HTML/CSS/JS frontend |
| `R6tracker.Core` | Services, interfaces, DTOs, exceptions |
| `R6tracker.Infrastructure` | EF Core models, DbContext, migrations, data seeder |
| `R6tracker.Tests` | NUnit unit tests with Moq |

## Entity Models

- **ApplicationUser** — custom Identity user with DisplayName, Country, RegisteredOn
- **R6Player** — player profile with stats (K/D, level, platform, country)
- **GameSession** — individual game session linked to a player and map
- **Rank** — rank tiers from Copper to Champion
- **R6Map** — siege maps with location, type and active status

## API Endpoints

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | /api/auth/register | — | Register new user |
| POST | /api/auth/login | — | Login, receive JWT token |
| GET | /api/auth/users | Admin | Get all registered users |
| GET | /api/players | — | Get all players (search/filter) |
| GET | /api/players/{id} | — | Get player by ID |
| POST | /api/players | Admin | Add player |
| PUT | /api/players/{id} | Admin | Update player |
| DELETE | /api/players/{id} | Admin | Delete player |
| GET | /api/gamesessions | — | Get all sessions |
| GET | /api/gamesessions/{id} | — | Get session by ID |
| GET | /api/gamesessions/player/{id} | — | Sessions for a player |
| POST | /api/gamesessions | Admin | Log session |
| DELETE | /api/gamesessions/{id} | Admin | Delete session |
| GET | /api/ranks | — | Get all ranks |
| GET | /api/ranks/{id} | — | Get rank by ID |
| GET | /api/maps | — | Get all maps |
| GET | /api/maps/active | — | Get active ranked maps |
| GET | /api/maps/{id} | — | Get map by ID |
| POST | /api/maps | Admin | Add map |
| PUT | /api/maps/{id} | Admin | Update map |
| DELETE | /api/maps/{id} | Admin | Delete map |

## Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core 8
- ASP.NET Identity + JWT Bearer authentication
- Microsoft SQL Server
- NUnit + Moq (unit tests)
- HTML5, CSS3, JavaScript (frontend)

## Requirements

- .NET 8 SDK
- Docker (for SQL Server)
- DBeaver or any SQL client (optional)

## How to Run

### 1. Start Docker and SQL Server

Make sure Docker is running with SQL Server. Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=R6TrackerDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
}
```

### 2. Run the API

```bash
cd R6tracker.API
dotnet run
```

The API will automatically:
- Apply all pending migrations
- Seed the database with ranks, maps, sample players and an admin user

### 3. Open the frontend

Open your browser and go to:
```
http://localhost:5000
```

### 4. Login as Admin

```
Email:    admin@r6tracker.com
Password: Admin123!
```

## Running Tests

```bash
dotnet test R6tracker.Tests
```

19 unit tests covering PlayerService, GameSessionService and RankService.

## Default Seeded Data

- 7 rank tiers (Copper → Champion)
- 10 maps (Border, Clubhouse, Coastline, etc.)
- 3 sample players (ShadowR, NovaSix, IceWall)
- 1 admin user (admin@r6tracker.com)
- 2 roles (Administrator, User)