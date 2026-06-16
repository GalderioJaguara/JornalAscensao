## ADDED Requirements

### Requirement: Docker-based local development instructions exist
The documentation SHALL provide step-by-step instructions for running the full stack locally using Docker Compose.

#### Scenario: Developer starts project with Docker
- **WHEN** a developer runs `docker compose up -d` from repository root
- **THEN** all three services (jornalascensao, ascensao-postgres, nginx) start successfully
- **THEN** the application is accessible at http://localhost:80

#### Scenario: Developer stops project with Docker
- **WHEN** a developer runs `docker compose down`
- **THEN** all services stop and containers are removed
- **THEN** postgres data persists in the named volume

### Requirement: Manual local development instructions exist
The documentation SHALL provide instructions for running the application without Docker (directly on host machine).

#### Scenario: Developer runs app locally without Docker
- **WHEN** a developer has .NET 9 SDK and PostgreSQL installed locally
- **THEN** they can run `dotnet run` from JornalAscensao folder after configuring connection string
- **THEN** the application starts on http://localhost:5xxx (default Kestrel port)

### Requirement: Prerequisites are clearly listed
The documentation SHALL list all prerequisites for both Docker and manual development approaches.

#### Scenario: New developer checks prerequisites
- **WHEN** a new developer reads prerequisites
- **THEN** they see: Docker Desktop, .NET 9 SDK (for manual), PostgreSQL 15+ (for manual), Git

### Requirement: Environment configuration is documented
The documentation SHALL explain how to configure environment variables and connection strings for local development.

#### Scenario: Developer configures local environment
- **WHEN** a developer needs to customize configuration
- **THEN** they know to modify appsettings.Development.json or use environment variables
- **THEN** they understand the DefaultConnection string format for PostgreSQL

### Requirement: Database initialization is explained
The documentation SHALL describe how the database is created and seeded on first run.

#### Scenario: First-time database setup
- **WHEN** a developer runs the application for the first time
- **THEN** Entity Framework Core creates the database schema automatically
- **THEN** SeedData.InitializeAsync can be uncommented in Program.cs to seed initial data