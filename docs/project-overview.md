# Visão Geral do Projeto

## O que é o Jornal Ascensão?

O Jornal Ascensão é uma plataforma de notícias e artigos colaborativa criada visando o jornalismo independente. Leitores sugerem pautas e submetem artigos, que passam por um processo de revisão antes de serem publicados.

## Propósito

A plataforma democratiza o jornalismo ao permitir que qualquer pessoa possa:
- Sugerir pautas (temas/assuntos para cobertura jornalística)
- Escrever artigos sobre pautas abertas
- Participar de um processo colaborativo de revisão e publicação

## Stack Tecnológica

| Camada | Tecnologia | Versão |
|--------|------------|--------|
| Backend | ASP.NET Core | 9.0 |
| ORM | Entity Framework Core | 9.0.8 |
| Banco de Dados | PostgreSQL | 15.3 |
| Autenticação | ASP.NET Core Identity | 9.0.8 |
| Reverse Proxy | Nginx | latest |
| Containerização | Docker Compose | - |
| Runtime | .NET | 9.0 |

### Pacotes NuGet Principais

- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` - Autenticação e autorização
- `Npgsql.EntityFrameworkCore.PostgreSQL` - Provider PostgreSQL para EF Core
- `EFCore.NamingConventions` - Convenções de nomenclatura (snake_case)
- `HtmlAgilityPack` - Parsing de HTML para extração de metadata

## Arquitetura

O projeto segue o padrão **Clean Architecture** com separação de responsabilidades:

```
JornalAscensao/
├── Controllers/     # Endpoints HTTP e lógica de requisição
├── Services/        # Lógica de negócio
│   ├── Abstraction/ # Interfaces dos serviços
│   └── IoC/         # Injeção de dependência
├── Models/          # Entidades de domínio e ViewModels
├── Dtos/            # Data Transfer Objects
├── Data/            # Contexto do Entity Framework e configuração
│   └── IoC/         # Injeção de dependência do banco
├── Views/           # Razor Views (ASP.NET Core MVC)
├── Utils/           # Utilitários (slugify, selects, etc.)
├── Migrations/      # Migrações do Entity Framework
└── wwwroot/         # Arquivos estáticos (CSS, JS, imagens)
```

### Camadas

| Camada | Responsabilidade |
|--------|------------------|
| **Controllers** | Receber requisições HTTP, validar input, retornar respostas |
| **Services** | Lógica de negócio, orquestração, regras de domínio |
| **Models** | Entidades (Artigo, Pauta, Usuario) e ViewModels |
| **Data** | Contexto do EF Core, configuração do banco, Identity |
| **Views** | Interfaces HTML com Razor |
| **Utils** | Funções auxiliares (slugify, categorias, etc.) |

## Features Principais

### Para Leitores
- Navegação por artigos publicados com paginação
- Filtros por categoria
- Cadastro e login de usuários

### Para Colaboradores (usuários autenticados)
- Criação de pautas (sugestões de pauta)
- Escrita de artigos vinculados a pautas
- Edição de artigos próprios
- Dashboard com status dos artigos

### Para Administradores/Moderadores
- Dashboard com métricas (artigos pendentes, publicados, pautas abertas/fechadas)
- Revisão e aprovação de artigos
- Fechamento de pautas
- Gestão de colaboradores (Admin exclusivo)
- Exclusão de artigos

## Fluxo de Trabalho

```
Leitor → Cadastra-se → Cria Pauta → Colaborador escreve Artigo
                                          ↓
                              Admin/Moderador revisa
                                          ↓
                              Aprovado → Publicado
                              Reprovado → Correção → Revisão novamente
```
