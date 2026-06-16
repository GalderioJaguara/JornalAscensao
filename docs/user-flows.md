# Fluxos de Usuário

## Visão Geral

A plataforma define três papéis principais com jornadas distintas:

```
┌─────────────────────────────────────────────────────────────┐
│                      Fluxos de Usuário                      │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌──────────┐    ┌──────────────┐    ┌──────────────────┐  │
│  │ Leitor   │───→│ Colaborador  │───→│ Admin/Moderador  │  │
│  │ Anônimo  │    │ (User role)  │    │                  │  │
│  └──────────┘    └──────────────┘    └──────────────────┘  │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

## 1. Leitor Anônimo (Não autenticado)

### Navegação na Página Inicial

**Rota:** `GET /`

1. Acessa a página inicial
2. Visualiza lista paginada de artigos publicados
3. Pode filtrar por categoria
4. Pode clicar em um artigo para ler o conteúdo completo

### Cadastro

**Rota:** `POST /Cadastrar`

1. Clica em "Cadastrar" no menu
2. Preenche formulário:
   - Apelido (3-50 caracteres)
   - Email
   - Senha (8+ chars, maiúscula, minúscula, dígito)
   - Confirmar Senha
3. Sistema cria conta com papel "User"
4. Usuário é redirecionado para Home já autenticado

### Restrições

O leitor anônimo **NÃO** pode:
- Criar pautas
- Escrever artigos
- Acessar áreas administrativas
- Editar ou excluir conteúdo

## 2. Colaborador (Papel "User")

### Criar Pauta

**Rota:** `POST /Pautas/Cadastrar`

1. Acessa "Criar Pauta"
2. Preenche formulário:
   - **Tipo** - Categoria da pauta (ex: Notícia, Opinião)
   - **Categoria** - Área temática
   - **Título** - Título da pauta (10-256 caracteres)
   - **Descrição** - Descrição detalhada (30+ caracteres)
   - **Imagem** - URL da imagem (opcional)
   - **Link de Conteúdo** - URL para extração de metadata
3. Sistema cria pauta com `Fechada=false`
4. Pauta aparece na lista de pautas abertas

### Escrever Artigo

**Rota:** `POST /Artigos/Escrever`

1. Seleciona uma pauta aberta para escrever
2. Preenche formulário:
   - **Título** - Título do artigo (10-256 caracteres)
   - **Gancho** - Resumo chamativo (até 256 caracteres)
   - **Texto** - Conteúdo completo (1000+ caracteres)
   - **Referências** - Links e fontes utilizadas
   - **Imagem** - URL da imagem de capa
3. Sistema cria artigo com:
   - `Status = Corrigindo`
   - `Aprovado = false`
   - `AutorId = usuário atual`
4. Pauta associada é marcada como `Fechada=true`

### Editar Artigo

**Rota:** `POST /Artigos/Editar/{id}`

1. Acessa "Meus Artigos" no dashboard
2. Clica em "Editar" no artigo desejado
3. Modifica campos desejados
4. Salva alterações
5. `Atualizado` é registrado com data/hora atual

### Excluir Artigo

**Rota:** `GET /Artigos/Excluir/{id}`

1. Acessa "Meus Artigos"
2. Clica em "Excluir" no artigo
3. Confirma exclusão
4. Artigo é removido do sistema

### Acompanhar Status

**Rota:** `GET /Colaborador`

1. Acessa dashboard do colaborador
2. Visualiza:
   - Artigos publicados
   - Artigos pendentes de revisão
   - Artigos com correções solicitadas
3. Pode editar artigos com status "Corrigindo"

### Atualizar Perfil

**Rota:** `POST /Colaborador/UpdateColaborador`

1. Acessa perfil do colaborador
2. Altera apelido e/ou avatar
3. Salva alterações

## 3. Admin/Moderador

### Dashboard

**Rota:** `GET /Admin`

1. Acessa painel administrativo
2. Visualiza métricas:
   - Artigos em fila de revisão
   - Artigos publicados
   - Pautas abertas
   - Pautas fechadas
   - Novos colaboradores (semana atual)
   - Colaboradores bloqueados

### Gerenciar Pautas

**Rota:** `GET /Admin/Pautas`

1. Visualiza lista paginada de todas as pautas
2. Pode fechar pautas:
   - Clica em "Fechar Pauta"
   - `Fechada` torna-se `true`
   - Pauta sai da lista de abertas

### Revisar Artigos

**Rota:** `GET /Artigos/Fila-de-Revisao`

1. Acessa fila de revisão
2. Visualiza artigos com `Aprovado=false` e `Status=Revisando`
3. Clica em "Revisar" para ver conteúdo completo
4. Decide:
   - **Aprovar** → `Status=Publicado`, `Aprovado=true`, `Publicado=now`
   - **Solicitar Correção** → `Status=Corrigindo`, retorna ao autor

### Gerenciar Colaboradores (Admin apenas)

**Rota:** `GET /Admin/Colaboradores`

1. Visualiza lista de todos os usuários
2. Informações exibidas:
   - Email
   - Apelido
   - Papéis (roles)
   - Data de criação
   - Status de bloqueio
3. Pode excluir colaboradores:
   - Clica em "Excluir"
   - Confirma ação
   - Usuário é removido do sistema

## Fluxos Combinados

### Fluxo Completo de Publicação

```
1. Usuário cria pauta → Pauta aberta
2. Colaborador seleciona pauta → Artigo em criação
3. Colaborador submete artigo → Status: Corrigindo
4. Artigo vai para fila → Status: Revisando
5. Moderador/Admin revisa
   ├── Aprova → Status: Publicado (visível para todos)
   └── Solicita correção → Status: Corrigindo (volta ao autor)
6. Autor corrige e resubmete → Volta para revisão
```

### Timeline de um Artigo

```
Criação → Corrigindo → Revisando → Publicado
                      ↕           ↕
                   Correção    Rejeição
```
