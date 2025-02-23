# Poetry Lovers API

The Poetry Lovers API is a RESTful API built using ASP.NET Core that allows users to interact with poems from [The PoetryDB](https://poetrydb.org/index.html). The API provides endpoints for creating, reading, and saving poems, as well as rate limiting to prevent abuse. 

## Installation

To run the Poetry Lovers API locally, you'll need to have the following installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (for production) or [SQLite](https://www.sqlite.org/index.html) (for development)

1. Clone the repository:

```
git clone https://github.com/lilleyz7/PoetryLovers.git
```

2. Navigate to the project directory and restore the NuGet packages:

```
cd PoetryLovers
dotnet restore
```

3. Set up the database:

   - For development, the API uses a SQLite database. The connection string is configured in the `appsettings.json` file.
   - For production, the API uses a SQL Server database. Update the `appsettings.json` file with the appropriate connection string.

4. Build and run the application:

```
dotnet build
dotnet run
```

The API will be available at `https://localhost:7132` or `http://localhost:5113`.

## Usage

The Poetry Lovers API provides the following endpoints:

### Poems

- `GET /api/byTitle/{title}`: Poem by title
- `GET /api/mySaves`: Retrieve active user's saved poems
- `GET /api/random`: Get a random poem
- `GET /api/byAuthor/{author}/{count}`: Search by author
- `POST /api/save`: Save or unsave Poem in body request

Remove Authorize attribute in controller to avoid authentication step


### JWT Authentication

- `POST /login`: Email and Password
- `POST /register`: Email and Password

The identity auth system is also setup for potential email verification and 2FA

### Poem Data transfer Structure

```
string title
string author
string lines
string linecount
```

### Rate Limiting

The API has the following rate limiting policies:

- `postLimiter`: Limits the number of POST requests to 5 per 30 seconds, with a queue limit of 4.
- `getLimiter`: Limits the number of GET requests to 5 per 90 seconds, with a queue limit of 2.

## API

The Poetry Lovers API uses the following technologies:

- ASP.NET Core v9
- Entity Framework Core
- SQLite (development) and SQL Server (production)
- OpenAPI (Scalar) for API documentation
- Rate limiting using the `Microsoft.AspNetCore.RateLimiting` package

## License

The Poetry Lovers API is licensed under the [MIT License](LICENSE).

