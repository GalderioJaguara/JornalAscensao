# Comportamentos da Aplicação

## Ciclo de Vida das Pautas

### Criação

```
Usuário preenche formulário
         │
         ▼
┌─────────────────────────────┐
│ Pauta é criada com:         │
│ • Fechada = false           │
│ • UsuarioId = usuário atual │
│ • Criado = data/hora atual  │
└─────────────────────────────┘
         │
         ▼
Pauta aparece na lista de abertas
```

**Dados obrigatórios:**
- Tipo (string, até 256 caracteres)
- Categoria (string, até 256 caracteres)
- Título (10-256 caracteres)
- Descrição (30+ caracteres)
- Link de Conteúdo (URL válida)

**Dados opcionais:**
- Imagem (URL)

### Extração de Metadata de URLs

Quando o usuário informa um `LinkConteudo`, o sistema:

1. Faz requisição HTTP para a URL
2. Faz parse do HTML com HtmlAgilityPack
3. Extrai metatags Open Graph:
   - `og:title` → Título (fallback: `<title>`)
   - `og:description` → Descrição (fallback: `<meta name="description">`)
   - `og:image` → Imagem (fallback: imagem padrão)
4. Decodifica HTML entities
5. Retorna `PautaMetadados` para preenchimento automático do formulário

```csharp
public async Task<PautaMetadados> GetPautaMetadadosAsync(string request)
{
    // Busca URL e extrai metadata
    string titulo = doc.DocumentNode.SelectSingleNode("//meta[@property='og:title']")
        .GetAttributeValue("content", "");
    // ... fallback para <title>
}
```

### Fechamento

```
Admin/Moderador clica "Fechar Pauta"
         │
         ▼
┌─────────────────────────────┐
│ Pauta.Fechada = true        │
│ (persistido no banco)       │
└─────────────────────────────┘
         │
         ▼
Pauta sai da lista de abertas
```

**Efeitos:**
- Pauta não aparece mais na listagem principal
- Usuários não podem criar novos artigos para ela
- Artigos já em andamento continuam existindo

### Exclusão

```
Admin/Moderador confirma exclusão
         │
         ▼
┌─────────────────────────────┐
│ Pauta removida do banco     │
└─────────────────────────────┘
```

---

## Ciclo de Vida dos Artigos

### Estados

```
┌──────────────┐
│  Corrigindo  │ ◄── Estado inicial (criação)
└──────┬───────┘
       │
       ▼
┌──────────────┐
│  Revisando   │ ◄── Submetido para revisão
└──────┬───────┘
       │
       ├──► Aprovar ──► ┌──────────────┐
       │                │  Publicado   │ ◄── Estado final
       │                └──────────────┘
       │
       └──► Correção ──► ┌──────────────┐
                         │  Corrigindo  │ ◄── Volta ao autor
                         └──────────────┘
```

### Diagrama de Estados

```
         ┌─────────────────────────────────────┐
         │                                     │
         ▼                                     │
    ┌──────────┐      Revisão        ┌──────────┐
    │Corrigindo│ ──────────────────► │Revisando │
    └──────────┘                     └──────────┘
         ▲                               │
         │                               │
         │        Solicita Correção      │
         └───────────────────────────────┘
                                        │
                                        │ Aprovação
                                        ▼
                                  ┌──────────┐
                                  │Publicado │
                                  └──────────┘
```

### Transições de Estado

| Evento | De | Para | Ação |
|--------|-----|------|------|
| Criar artigo | - | Corrigindo | `Status=Corrigindo`, `Aprovado=false` |
| Submeter revisão | Corrigindo | Revisando | `Status=Revisando` |
| Aprovar | Revisando | Publicado | `Status=Publicado`, `Aprovado=true`, `Publicado=now` |
| Solicitar correção | Revisando | Corrigindo | `Status=Corrigindo`, `Aprovado=false` |

### Criação de Artigo

```
1. Usuário seleciona pauta aberta
2. Preenche formulário (título, gancho, texto, referências, imagem)
3. Sistema valida:
   ├── Slug único (gerado a partir do título)
   ├── Pauta existe e está aberta
   └── Texto tem 1000+ caracteres
4. Artigo é criado:
   ├── Status = Corrigindo
   ├── Aprovado = false
   ├── AutorId = usuário atual
   └── Slug = UrlUtils.UrlFriendlyUtil(titulo)
5. Pauta associada é fechada automaticamente
```

**Código:**
```csharp
pauta.Fechado = true;

var artigo = new Artigo
{
    Titulo = request.Titulo,
    Gancho = request.Gancho,
    Texto = request.Texto,
    Categoria = pauta.Categoria,
    Referencias = request.Referencias,
    Imagem = request.Imagem,
    PautaId = request.PautaId,
    Slug = UrlUtils.UrlFriendlyUtil(request.Titulo),
    Status = StatusArtigo.Corrigindo,
    AutorId = usuarioId
};
```

### Edição de Artigo

**Permissões:**
- Autor do artigo (qualquer status)
- Admin ou Moderador (qualquer artigo)
- Artigo não aprovado (qualquer usuário autenticado)

**Código:**
```csharp
if (String.IsNullOrEmpty(usuario.Id) || 
    usuario.Roles.Any(roles => roles == "Admin" || roles == "Moderador")
    || !artigo.Aprovado || usuario.Id == artigo.AutorId)
{
    // Permite edição
}
```

### Aprovação de Artigo

```
Revisor acessa artigo
         │
         ▼
┌─────────────────────────────┐
│ Decide:                     │
│ • Aprovar                   │
│ • Solicitar correção        │
└─────────────────────────────┘
         │
         ├──► Aprovar
         │    • Aprovado = true
         │    • Status = Publicado
         │    • Publicado = DateTime.UtcNow
         │
         └──► Correção
              • Status = Corrigindo
              • Retorna ao autor
```

---

## Listing e Filtros de Artigos

### Listagem Principal

**Rota:** `GET /` ou `GET /Artigos`

- Apenas artigos com `Aprovado=true`
- Ordenados por `Publicado` descendente (mais recentes primeiro)
- Paginação configurável

**Campos exibidos:**
- Slug
- Título
- Gancho (resumo)
- Imagem
- Data de publicação
- Categoria
- Apelido do autor

### Filtro por Categoria

**Rota:** `GET /Artigos/Categoria/{categoria}`

1. Filtra artigos por `Categoria` exata
2. Mantém ordenação por data
3. Paginação funciona dentro dos resultados filtrados

### Listagem do Colaborador

**Rota:** `GET /Colaborador`

- Apenas artigos do usuário atual (`AutorId = usuarioId`)
- Inclui artigos aprovados e pendentes
- Mostra informações do revisor (se houver)

### Fila de Revisão

**Rota:** `GET /Artigos/Fila-de-Revisao`

- Apenas artigos com `Aprovado=false`
- Para Admin, Moderador e Revisor
- Paginado

---

## Dashboard do Colaborador

**Rota:** `GET /Colaborador`

### Dados Exibidos

```
┌─────────────────────────────────────────┐
│           Dashboard Colaborador         │
├─────────────────────────────────────────┤
│                                         │
│  ┌─────────────┐  ┌─────────────────┐  │
│  │ Artigos     │  │ Artigos         │  │
│  │ Publicados  │  │ Pendentes       │  │
│  └─────────────┘  └─────────────────┘  │
│                                         │
│  ┌─────────────────────────────────┐   │
│  │ Criar Novo Artigo               │   │
│  └─────────────────────────────────┘   │
│                                         │
│  ┌─────────────────────────────────┐   │
│  │ Meus Artigos                    │   │
│  │ • [editar] Artigo 1 - Publicado │   │
│  │ • [editar] Artigo 2 - Pendente  │   │
│  └─────────────────────────────────┘   │
│                                         │
└─────────────────────────────────────────┘
```

### Funcionalidades

1. **Visualizar artigos publicados** - Lista dos artigos já aprovados
2. **Visualizar artigos pendentes** - Artigos aguardando revisão
3. **Criar novo artigo** - Acessa formulário de criação
4. **Editar artigo** - Modifica artigo existente
5. **Atualizar perfil** - Altera apelido e avatar
