## ADDED Requirements

### Requirement: jornalascensao service is documented
The documentation SHALL describe the jornalascensao service configuration including image, build context, environment variables, ports, dependencies, and network.

#### Scenario: Developer reviews app service config
- **WHEN** a developer reads the jornalascensao service documentation
- **THEN** they see: builds from JornalAscensao/Dockerfile, uses multi-stage build (sdk → aspnet), exposes ports 8080/8081
- **THEN** they see environment: ASPNETCORE_ENVIRONMENT=Development, ConnectionStrings__DefaultConnection pointing to ascensao-postgres
- **THEN** they see depends_on ascensao-postgres with condition service_started
- **THEN** they see network: ascensao_network

### Requirement: ascensao-postgres service is documented
The documentation SHALL describe the PostgreSQL service configuration including image, restart policy, ports, environment, volumes, health check, and network.

#### Scenario: Developer reviews postgres service config
- **WHEN** a developer reads the ascensao-postgres service documentation
- **THEN** they see: image postgres:15.3-alpine, restart: always, port 5432:5432
- **THEN** they see environment: POSTGRES_PASSWORD, volume: postgres:/var/lib/postgresql/data
- **THEN** they see health check: pg_isready every 30s, timeout 10s, retries 5
- **THEN** they see network: ascensao_network

### Requirement: nginx service is documented
The documentation SHALL describe the nginx service configuration including image, ports, volume mounts, dependencies, and network.

#### Scenario: Developer reviews nginx service config
- **WHEN** a developer reads the nginx service documentation
- **THEN** they see: image nginx:latest, port 80:80
- **THEN** they see volume mount: ./nginx/nginx.conf:/etc/nginx/nginx.conf
- **THEN** they see depends_on jornalascensao with condition service_started
- **THEN** they see network: ascensao_network

### Requirement: Docker volumes are documented
The documentation SHALL describe the named volumes used in the compose file.

#### Scenario: Developer understands data persistence
- **WHEN** a developer reads the volumes section
- **THEN** they see: postgres volume for PostgreSQL data persistence
- **THEN** they understand data survives container recreation

### Requirement: Docker networks are documented
The documentation SHALL describe the custom network configuration.

#### Scenario: Developer understands service communication
- **WHEN** a developer reads the networks section
- **THEN** they see: ascensao_network with bridge driver
- **THEN** they understand all services communicate via service names on this network

### Requirement: nginx configuration is documented
The documentation SHALL describe the nginx.conf used for reverse proxy.

#### Scenario: Developer reviews nginx config
- **WHEN** a developer reads the nginx configuration documentation
- **THEN** they see: proxy_pass to jornalascensao:8080
- **THEN** they see proxy headers: Host, X-Real-IP, X-Forwarded-For, X-Forwarded-Proto
- **THEN** they understand nginx terminates SSL (if configured) and forwards to app