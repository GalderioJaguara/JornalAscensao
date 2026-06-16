## Why

O projeto Jornal Ascensão possui apenas um README.md de 4 linhas, sem documentação estruturada sobre arquitetura, configuração, fluxos de usuário ou operações. Isso dificulta a onboarding de novos desenvolvedores, manutenção do sistema e contribuições da comunidade. A documentação é essencial para garantir que o projeto possa ser compreendido, rodado localmente e mantido a longo prazo.

## What Changes

- Criar pasta `/docs` no repositório com arquivos Markdown para cada área de conhecimento
- Documentar visão geral do projeto, stack tecnológica e arquitetura
- Instruções de desenvolvimento local com Docker Compose e manual
- Documentação dos serviços Docker (jornalascensao, ascensao-postgres, nginx) e suas configurações
- Documentação de autenticação, autorização e papéis (User, Moderador, Admin)
- Fluxos de usuário: leitor anônimo, colaborador, administrador
- Comportamentos da aplicação: ciclo de vida de pautas e artigos, listing, filtros
- Interações do admin: dashboard, gestão de pautas, artigos e colaboradores

## Capabilities

### New Capabilities
- `project-overview`: Visão geral do projeto, stack tecnológica, arquitetura e features principais
- `local-development`: Instruções para rodar localmente com Docker e manualmente
- `docker-services`: Documentação dos serviços Docker e configurações
- `auth-authorization`: Configuração de Identity, fluxos de login/registro, papéis e permissões
- `user-flows`: Fluxos de usuário por papel (leitor, colaborador, admin)
- `application-behaviors`: Comportamentos da aplicação (pautas, artigos, listing, dashboard)
- `admin-interactions`: Interações do admin no dashboard e gestão de conteúdo

### Modified Capabilities
(nenhuma - todas são novas)

## Impact

- **Arquivos criados**: `/docs/` com 7 arquivos Markdown
- **Código**: Nenhuma alteração no código da aplicação
- **Dependências**: Nenhuma
- **Infraestrutura**: Nenhuma alteração em Docker, CI/CD ou configurações
