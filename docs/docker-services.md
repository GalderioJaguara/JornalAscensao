# Serviços Docker

O projeto utiliza Docker Compose com 3 serviços para rodar o stack completo.

## Visão Geral dos Serviços

```
┌─────────────────────────────────────────────────────────┐
│                    ascensao_network                     │
│                                                         │
│  ┌─────────────┐    ┌──────────────┐    ┌──────────┐   │
│  │    nginx     │───→│jornalascensao│───→│ascensao- │   │
│  │   (porta 80) │    │ (porta 8080) │    │ postgres │   │
│  └─────────────┘    └──────────────┘    │(porta5432)   │
│                                         └──────────┘   │
└─────────────────────────────────────────────────────────┘
```

## Serviço: jornalascensao

Aplicação ASP.NET Core 9 com build multi-stage.

### Configuração

```yaml
jornalascensao:
  image: jornalascensao
  build:
    context: .
    dockerfile: JornalAscensao/Dockerfile
  environment:
    - ASPNETCORE_ENVIRONMENT=Development
    - ConnectionStrings__DefaultConnection=Host=ascensao-postgres;Port=5432;Database=jornal_ascensao;Username=postgres;Password=DDREzNlJI98FhpZ6SG4BePGCarQPmm
  ports:
    - "8080:8080"
  depends_on:
    ascensao-postgres:
      condition: service_started
  networks:
    - ascensao_network
```

### Parâmetros

| Parâmetro | Valor | Descrição |
|-----------|-------|-----------|
| Imagem | `jornalascensao` | Build local a partir do Dockerfile |
| Porta | 8080 | Porta HTTP da aplicação |
| Depende de | ascensao-postgres | Aguarda PostgreSQL iniciar |

### Dockerfile (Multi-stage Build)

```dockerfile
# Stage 1: Base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build (compilação)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Stage 4: Final (runtime otimizado)
FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JornalAscensao.dll"]
```

## Serviço: ascensao-postgres

Banco de dados PostgreSQL 15.3 com persistência de dados.

### Configuração

```yaml
ascensao-postgres:
  image: postgres:15.3-alpine
  restart: always
  ports:
    - 5432:5432
  environment:
    POSTGRES_PASSWORD: DDREzNlJI98FhpZ6SG4BePGCarQPmm
  volumes:
    - postgres:/var/lib/postgresql/data
  networks:
    - ascensao_network
  healthcheck:
    test: ["CMD-SHELL", "pg_isready -U postgres"]
    interval: 30s
    timeout: 10s
    retries: 5
```

### Parâmetros

| Parâmetro | Valor | Descrição |
|-----------|-------|-----------|
| Imagem | `postgres:15.3-alpine` | PostgreSQL 15.3 (variante leve Alpine) |
| Restart | `always` | Reinicia automaticamente em caso de falha |
| Porta | 5432 | Porta padrão do PostgreSQL |
| Volume | `postgres` | Persiste dados entre reinicializações |
| Healthcheck | `pg_isready` | Verifica disponibilidade a cada 30s |

### Variáveis de Ambiente

| Variável | Valor | Descrição |
|----------|-------|-----------|
| `POSTGRES_PASSWORD` | `DDREzNlJI98FhpZ6SG4BePGCarQPmm` | Senha do superuser postgres |

> **Segurança:** A senha está hardcoded no compose.yaml para desenvolvimento. Em produção, use secrets do Docker ou variáveis de ambiente externas.

## Serviço: nginx

Reverse proxy para roteamento de tráfego.

### Configuração

```yaml
nginx:
  image: nginx:latest
  ports:
    - "80:80"
  volumes:
    - ./nginx/nginx.conf:/etc/nginx/nginx.conf
  networks:
    - ascensao_network
  depends_on:
    jornalascensao:
      condition: service_started
```

### Parâmetros

| Parâmetro | Valor | Descrição |
|-----------|-------|-----------|
| Imagem | `nginx:latest` | Nginx latest |
| Porta | 80 | Porta HTTP externa |
| Volume | `./nginx/nginx.conf` | Configuração customizada |

### Configuração nginx.conf

```nginx
events {}

http {
    include       /etc/nginx/mime.types;
    default_type  application/octet-stream;

    server {
        listen 80;

        location / {
            proxy_pass http://jornalascensao:8080;
            proxy_set_header Host $host;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
        }
    }
}
```

### Headers de Proxy

| Header | Descrição |
|--------|-----------|
| `Host` | Host original da requisição |
| `X-Real-IP` | IP real do cliente |
| `X-Forwarded-For` | Lista de IPs intermediários |
| `X-Forwarded-Proto` | Protocolo original (HTTP/HTTPS) |

## Volumes

```yaml
volumes:
  postgres:
```

| Volume | Descrição |
|--------|-----------|
| `postgres` | Armazena dados do PostgreSQL em `/var/lib/postgresql/data` |

> Os dados persistem mesmo após `docker compose down`. Use `docker compose down -v` para remover.

## Networks

```yaml
networks:
  ascensao_network:
    driver: bridge
```

| Network | Driver | Descrição |
|---------|--------|-----------|
| `ascensao_network` | bridge | Rede isolada para comunicação entre serviços |

Todos os serviços se comunicam via nomes de serviço (ex: `ascensao-postgres`, `jornalascensao`) nesta rede.

## Comandos Úteis

```bash
# Iniciar todos os serviços
docker compose up -d

# Ver logs
docker compose logs -f

# Ver logs de um serviço específico
docker compose logs -f jornalascensao

# Parar serviços
docker compose down

# Parar e remover dados
docker compose down -v

# Reconstruir após alterações no código
docker compose up -d --build

# Ver status dos serviços
docker compose ps
```
