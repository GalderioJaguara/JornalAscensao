# Interações do Admin

## Visão Geral

O painel administrativo (`/Admin`) permite gerenciar todo o conteúdo e usuários da plataforma. Existem duas categorias de administradores:

- **Admin** - Controle total (gerencia colaboradores)
- **Moderador** - Revisão de conteúdo e gestão de pautas

## Dashboard Admin

**Rota:** `GET /Admin`
**Acesso:** Admin, Moderador

### Métricas Exibidas

```
┌─────────────────────────────────────────────────────────┐
│                   Dashboard Admin                       │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐    │
│  │ Artigos     │  │ Artigos     │  │ Pautas      │    │
│  │ em Fila     │  │ Publicados  │  │ Abertas     │    │
│  │     X       │  │     X       │  │     X       │    │
│  └─────────────┘  └─────────────┘  └─────────────┘    │
│                                                         │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐    │
│  │ Pautas      │  │ Novos       │  │ Bloqueados  │    │
│  │ Fechadas    │  │ Colab.      │  │             │    │
│  │     X       │  │     X       │  │     X       │    │
│  └─────────────┘  └─────────────┘  └─────────────┘    │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

### Cálculo das Métricas

| Métrica | Fórmula | Descrição |
|---------|---------|-----------|
| **Artigos em Fila** | `Artigos WHERE Aprovado = false` | Artigos aguardando revisão |
| **Artigos Publicados** | `Artigos WHERE Aprovado = true` | Artigos já aprovados |
| **Pautas Abertas** | `Pautas WHERE Fechada = false` | Pautas disponíveis para artigos |
| **Pautas Fechadas** | `Pautas WHERE Fechada = true` | Pautas encerradas |
| **Novos Colaboradores** | `Usuarios WHERE Criado BETWEEN domingo-hoje E proximo-domingo` | Cadastros na semana atual |
| **Colaboradores Bloqueados** | `Usuarios WHERE LockoutEnd > agora` | Usuários com bloqueio ativo |

**Código:**
```csharp
public async Task<int> GetNovosColaboradoresAsync()
{
    var diaAtual = DateTime.UtcNow;
    var domingo = diaAtual.AddDays(-(int)diaAtual.DayOfWeek);
    var proximoDomungo = domingo.AddDays(7);

    return await context.Users
        .Where(u => u.Criado >= domingo && u.Criado < proximoDomungo)
        .CountAsync();
}
```

## Gestão de Pautas

**Rota:** `GET /Admin/Pautas`
**Acesso:** Admin, Moderador

### Listar Pautas

1. Exibe lista paginada de todas as pautas (abertas e fechadas)
2. Cada pauta mostra:
   - ID
   - Título
   - Categoria
   - Tipo
   - Status (Fechada/Aberta)
   - Criador (Apelido)
   - Data de criação

### Fechar Pauta

**Rota:** `POST /Admin/FecharPauta`

1. Admin/Moderador seleciona pauta aberta
2. Clica em "Fechar Pauta"
3. Requisição POST com `[ValidateAntiForgeryToken]`
4. `PautaService.FecharPautaAsync(id)`:
   - Busca pauta por ID
   - Define `Fechada = true`
   - Salva no banco
5. Retorna `NoContent` (sucesso) ou `NotFound` (pauta não existe)

**Código:**
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> FecharPauta(Guid id)
{
    var pauta = await pautaService.FecharPautaAsync(id);
    if (pauta == false)
        return NotFound();
    return NoContent();
}
```

## Gestão de Artigos

### Listar Artigos

**Rota:** `GET /Admin/Artigos`
**Acesso:** Admin, Moderador

1. Exibe lista paginada de todos os artigos
2. Cada artigo mostra:
   - Título
   - Categoria
   - Status
   - Aprovado (Sim/Não)

### Fila de Revisão

**Rota:** `GET /Artigos/Fila-de-Revisao`
**Acesso:** Admin, Moderador, Revisor

1. Exibe artigos com `Aprovado = false`
2. Cada artigo mostra:
   - Título
   - Imagem
   - Slug
   - Categoria
   - Status

### Revisar Artigo

**Rota:** `GET /Artigos/Revisar/{id}`
**Acesso:** Admin, Moderador, Revisor

1. Exibe conteúdo completo do artigo
2. Revisor pode:
   - **Aprovar** → `POST /Artigos/AprovarArtigo`
   - **Solicitar correção** → Retorna ao status "Corrigindo"

### Aprovar Artigo

**Rota:** `POST /Artigos/AprovarArtigo/{id}`
**Acesso:** Admin, Moderador, Revisor

1. Requisição POST com `[ValidateAntiForgeryToken]`
2. `ArtigoService.AprovarArtigoAsync(slug, request)`:
   - Atualiza dados do artigo
   - Define `Aprovado = true`
   - Define `Status = Publicado`
   - Define `Publicado = DateTime.UtcNow`
3. Retorna `RedirectToAction("Index")`

**Código:**
```csharp
public async Task<bool> AprovarArtigoAsync(string slug, ArtigoFormViewModel request)
{
    var artigo = await context.Artigos.FirstOrDefaultAsync(a => a.Slug == slug);
    if (artigo == null) return false;
    
    artigo.UpdateArtigo(request);
    artigo.Aprovado = request.Aprovado;
    artigo.Status = StatusArtigo.Publicado;
    await context.SaveChangesAsync();
    return true;
}
```

### Excluir Artigo

**Rota:** `POST /Artigos/Excluir/{id}`
**Acesso:** Admin, Moderador

1. Exibe confirmação de exclusão
2. Requisição POST com `[ValidateAntiForgeryToken]`
3. `ArtigoService.DeletarArtigoAsync(slug)`:
   - Busca artigo por slug
   - Remove do banco
4. Retorna `RedirectToAction("Index")`

## Gestão de Colaboradores

**Rota:** `GET /Admin/Colaboradores`
**Acesso:** **Apenas Admin**

### Listar Colaboradores

1. Exibe lista de todos os usuários
2. Cada usuário mostra:
   - Email
   - Apelido
   - Papéis (roles)
   - Data de criação
   - Status de bloqueio

### Excluir Colaborador

**Rota:** `POST /Admin/ExcluirColaborador`
**Acesso:** **Apenas Admin**

1. Admin seleciona colaborador
2. Clica em "Excluir"
3. Requisição POST com email do usuário
4. `UsuarioService.DeleteUsuarioAsync(email)`:
   - Busca usuário por email
   - Remove do banco
5. Retorna `NoContent` (sucesso) ou `NotFound`

**Código:**
```csharp
[Authorize(Roles = "Admin")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ExcluirColaborador(string email)
{
    var usuario = await usuarioService.DeleteUsuarioAsync(email);
    if (usuario == false)
        return NotFound();
    return NoContent();
}
```

> **Nota:** Apenas o papel "Admin" pode excluir colaboradores. Moderadores não têm essa permissão.

## Proteção CSRF

Todas as ações de estado (POST) utilizam `[ValidateAntiForgeryToken]`:

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> FecharPauta(Guid id) { ... }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ExcluirColaborador(string email) { ... }
```

**Como funciona:**
1. Formulários HTML incluem um token anti-forgery
2. No POST, o token é validado contra o armazenamento do servidor
3. Previne ataques CSRF onde sites maliciosos executam ações em nome do usuário

## Resumo de Permissões por Rota

| Rota | Ação | Admin | Moderador | Revisor | User |
|------|------|:-----:|:---------:|:-------:|:----:|
| `GET /Admin` | Ver dashboard | ✅ | ✅ | ❌ | ❌ |
| `GET /Admin/Pautas` | Listar pautas | ✅ | ✅ | ❌ | ❌ |
| `POST /Admin/FecharPauta` | Fechar pauta | ✅ | ✅ | ❌ | ❌ |
| `GET /Admin/Artigos` | Listar artigos | ✅ | ✅ | ❌ | ❌ |
| `GET /Artigos/Fila-de-Revisao` | Fila de revisão | ✅ | ✅ | ✅ | ❌ |
| `GET /Artigos/Revisar/{id}` | Revisar artigo | ✅ | ✅ | ✅ | ❌ |
| `POST /Artigos/AprovarArtigo` | Aprovar artigo | ✅ | ✅ | ✅ | ❌ |
| `POST /Artigos/Excluir/{id}` | Excluir artigo | ✅ | ✅ | ❌ | ❌ |
| `GET /Admin/Colaboradores` | Listar users | ✅ | ❌ | ❌ | ❌ |
| `POST /Admin/ExcluirColaborador` | Excluir user | ✅ | ❌ | ❌ | ❌ |
