## 1. Estrutura da Documentação

- [x] 1.1 Criar pasta `/docs` no repositório raiz
- [x] 1.2 Criar arquivo `docs/README.md` como índice da documentação

## 2. Visão Geral do Projeto

- [x] 2.1 Criar `docs/project-overview.md` com descrição do projeto e propósito
- [x] 2.2 Documentar stack tecnológica (ASP.NET Core 9, EF Core, PostgreSQL, Nginx, Docker)
- [x] 2.3 Documentar arquitetura clean architecture (Controllers, Services, Models, DTOs, Utils, Data)
- [x] 2.4 Listar features principais da plataforma

## 3. Desenvolvimento Local

- [x] 3.1 Criar `docs/local-development.md`
- [x] 3.2 Documentar pré-requisitos (Docker Desktop, .NET 9 SDK, PostgreSQL, Git)
- [x] 3.3 Instruções para rodar com Docker Compose (`docker compose up -d`)
- [x] 3.4 Instruções para rodar manualmente (`dotnet run`)
- [x] 3.5 Documentar configuração de variáveis de ambiente e connection strings
- [x] 3.6 Explicar inicialização do banco de dados com Entity Framework

## 4. Serviços Docker

- [x] 4.1 Criar `docs/docker-services.md`
- [x] 4.2 Documentar serviço `jornalascensao` (build, environment, ports, depends_on)
- [x] 4.3 Documentar serviço `ascensao-postgres` (image, volumes, healthcheck)
- [x] 4.4 Documentar serviço `nginx` (reverse proxy, configuração nginx.conf)
- [x] 4.5 Documentar volumes nomeados e networks

## 5. Autenticação e Autorização

- [x] 5.1 Criar `docs/auth-authorization.md`
- [x] 5.2 Documentar configuração do ASP.NET Core Identity
- [x] 5.3 Documentar políticas de senha (8+ chars, maiúscula, minúscula, dígito)
- [x] 5.4 Documentar fluxo de registro de usuário
- [x] 5.5 Documentar fluxo de login e logout
- [x] 5.6 Documentar papéis (User, Moderador, Admin) e permissões
- [x] 5.7 Documentar pipeline de middleware de autenticação

## 6. Fluxos de Usuário

- [x] 6.1 Criar `docs/user-flows.md`
- [x] 6.2 Documentar fluxo do leitor anônimo (navegação, registro)
- [x] 6.3 Documentar fluxo do colaborador (criar pauta, escrever artigo, perfil)
- [x] 6.4 Documentar fluxo do admin/moderador (dashboard, gestão)

## 7. Comportamentos da Aplicação

- [x] 7.1 Criar `docs/application-behaviors.md`
- [x] 7.2 Documentar ciclo de vida das pautas (criação, extração de metadata, fechamento)
- [x] 7.3 Documentar ciclo dos artigos (escrita → revisão → correção → publicação)
- [x] 7.4 Documentar listing e filtros de artigos
- [x] 7.5 Documentar dashboard do colaborador

## 8. Interações do Admin

- [x] 8.1 Criar `docs/admin-interactions.md`
- [x] 8.2 Documentar dados do dashboard admin (métricas computadas)
- [x] 8.3 Documentar gestão de pautas (listar, fechar)
- [x] 8.4 Documentar gestão de artigos (listar, revisar, aprovar)
- [x] 8.5 Documentar gestão de colaboradores (listar, excluir)
- [x] 8.6 Documentar proteção CSRF com AntiForgeryToken

## 9. Atualização do README

- [x] 9.1 Atualizar `README.md` raiz com link para pasta `/docs`
- [x] 9.2 Adicionar seção "Como Contribuir" com referência à documentação
