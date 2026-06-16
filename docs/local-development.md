# Desenvolvimento Local

## Pré-requisitos

### Para rodar com Docker (recomendado)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (ou Docker Engine + Docker Compose)
- [Git](https://git-scm.com/)

### Para rodar manualmente (sem Docker)
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)

## Opção 1: Rodar com Docker (Recomendado)

### Iniciar o projeto

```bash
# Clone o repositório
git clone <url-do-repositorio>
cd JornalAscensao

# Inicie todos os serviços
docker compose up -d
```

Os seguintes serviços serão iniciados:
- **jornalascensao** - Aplicação ASP.NET Core (porta 8080)
- **ascensao-postgres** - Banco de dados PostgreSQL (porta 5432)
- **nginx** - Reverse proxy (porta 80)

### Acessar a aplicação

- Aplicação: http://localhost:80 (via Nginx)
- Direto na aplicação: http://localhost:8080

### Parar o projeto

```bash
docker compose down
```

> **Nota:** Os dados do PostgreSQL persistem em volume nomeado mesmo após `docker compose down`. Para remover os dados, use `docker compose down -v`.

## Opção 2: Rodar Manualmente

### 1. Configurar o banco de dados

Crie um banco de dados PostgreSQL:

```sql
CREATE DATABASE jornal_ascensao;
```

### 2. Configurar a connection string

Edite o arquivo `JornalAscensao/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=127.0.0.1;Port=5432;Database=jornal_ascensao;Username=postgres;Password=SUA_SENHA"
  }
}
```

Ou use variáveis de ambiente:

```bash
export ConnectionStrings__DefaultConnection="Host=127.0.0.1;Port=5432;Database=jornal_ascensao;Username=postgres;Password=SUA_SENHA"
```

### 3. Instalar dependências e executar

```bash
cd JornalAscensao

# Restaurar pacotes NuGet
dotnet restore

# Aplicar migrações do banco de dados
dotnet ef database update

# Executar a aplicação
dotnet run
```

A aplicação estará disponível em http://localhost:5000 (ou http://localhost:5001 com HTTPS).

## Configuração do Banco de Dados

### Inicialização automática

O Entity Framework Core cria o schema do banco automaticamente na primeira execução.

### Seed de dados iniciais

Para inserir dados de teste, descomente o trecho no `Program.cs`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InitializeAsync(services);
}
```

## Variáveis de Ambiente

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente da aplicação | `Development` |
| `ConnectionStrings__DefaultConnection` | String de conexão PostgreSQL | (ver appsettings.json) |

## Solução de Problemas

### Erro de conexão com o banco
- Verifique se o PostgreSQL está rodando
- Confirme a connection string em `appsettings.json`
- Para Docker: aguarde o healthcheck do PostgreSQL estar OK

### Porta já em uso
- Altere a porta no `compose.yaml` (seção `ports`)
- Ou encerre o processo que está usando a porta

### Migrações pendentes
```bash
dotnet ef migrations add <NomeDaMigracao>
dotnet ef database update
```
