# MBStream

A modern ASP.NET Core web API for music streaming, featuring full CRUD operations, JWT authentication, and a clean separation of concerns.  
Easily manage users, artists, albums, songs, and playlists with robust role-based access control.

---

## Features

- **User Authentication** (JWT)
- **Admin Role** with full CRUD access
- **Manage Artists, Albums, Songs, Playlists, Users**
- **Many-to-many relationships** (e.g., Artists & Albums, Playlists & Songs)
- **Unit tests** using xUnit, Moq, and EF Core In-Memory
- **RESTful API** with clear endpoints
- **EF Core** with code-first migrations
- **Static file serving** (for frontend integration)

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or use SQLite/PostgreSQL (adjust `appsettings.json`)
- (Optional) [Node.js](https://nodejs.org/) if you want to build a separate frontend

### Setup

1. **Clone the repository**
   
git clone https://github.com/marijadejanoska/MBStream.git
cd MBStream/MBStream/MBStream


3. **Restore dependencies**
   
dotnet restore


4. **Apply database migrations**
   
dotnet ef database update


6. **Run the application**
   
dotnet run


8. **Access the API**
   
- Default: [http://localhost:7216](http://localhost:7216)
- Swagger UI: [http://localhost:7216/swagger](http://localhost:7216/swagger)

---

## API Overview

| Entity     | Endpoint Example         | Methods                | Auth Required     |
|------------|-------------------------|------------------------|-------------------|
| Users      | `/api/users`            | GET, POST, PUT, DELETE | Admin             |
| Artists    | `/api/artists`          | GET, POST, PUT, DELETE | GET: All, others: Admin |
| Albums     | `/api/albums`           | GET, POST, PUT, DELETE | GET: All, others: Admin |
| Songs      | `/api/songs`            | GET, POST, PUT, DELETE | GET: All, others: Admin |
| Playlists  | `/api/playlists`        | GET, POST, PUT, DELETE | GET: All, others: Admin |
| Auth       | `/api/auth/login`       | POST                   | -                 |
| Auth       | `/api/auth/register`    | POST                   | -                 |

---

## Testing

Unit tests are located in `MBStream.TestProject1`.  
To run all tests:

dotnet test


---

## Project Structure

MBStream/
Controllers/
Data/
DTOs/
Mapping/
Models/
Repositories/
Services/
wwwroot/
MBStream.csproj
Program.cs
appsettings.json



---

## Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

---

## License
 
&copy; 2025 marijadejanoska, bogoljub grujeski

---

## Author

- [marijadejanoska](https://github.com/marijadejanoska)

