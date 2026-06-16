# Jornal Ascensão

Uma plataforma de notícias e artigos colaborativa criada visando o jornalismo independente. Leitores sugerem pautas e submetem artigos.

## Documentação

Consulte a pasta [`/docs`](docs/) para documentação completa:

- [Visão Geral do Projeto](docs/project-overview.md) - Propósito, stack tecnológica e arquitetura
- [Desenvolvimento Local](docs/local-development.md) - Como rodar o projeto localmente
- [Serviços Docker](docs/docker-services.md) - Configuração dos serviços Docker
- [Autenticação e Autorização](docs/auth-authorization.md) - Identity, login, papéis e permissões
- [Fluxos de Usuário](docs/user-flows.md) - Jornadas do leitor, colaborador e admin
- [Comportamentos da Aplicação](docs/application-behaviors.md) - Ciclo de vida de pautas e artigos
- [Interações do Admin](docs/admin-interactions.md) - Dashboard e gestão de conteúdo

## Início Rápido

### Com Docker (recomendado)

```bash
git clone <url-do-repositorio>
cd JornalAscensao
docker compose up -d
```

Acesse: http://localhost:80

### Sem Docker

```bash
cd JornalAscensao
dotnet restore
dotnet ef database update
dotnet run
```

Acesse: http://localhost:5000

## Stack Tecnológica

- **Backend:** ASP.NET Core 9
- **Banco de Dados:** PostgreSQL 15.3
- **ORM:** Entity Framework Core
- **Autenticação:** ASP.NET Core Identity
- **Reverse Proxy:** Nginx
- **Containerização:** Docker Compose

## Como Contribuir

1. Leia a [documentação do projeto](docs/) para entender a arquitetura
2. Siga as instruções de [desenvolvimento local](docs/local-development.md)
3. Crie uma branch para sua feature (`git checkout -b feature/nome-da-feature`)
4. Faça commit das suas alterações
5. Abra um Pull Request

## Licença

(Licença a ser definida)
