# Autenticação e Autorização

## Visão Geral

O projeto utiliza **ASP.NET Core Identity** para autenticação e autorização baseada em papéis (roles).

## Configuração do Identity

### Configuração de Senha

```csharp
// Data/IoC/IdentityInjection.cs
options.Password = new PasswordOptions
{
    RequireDigit = true,           // Pelo menos 1 dígito
    RequireLowercase = true,       // Pelo menos 1 letra minúscula
    RequireUppercase = true,       // Pelo menos 1 letra maiúscula
    RequireNonAlphanumeric = false, // Caracteres especiais NÃO são obrigatórios
    RequiredLength = 8             // Mínimo 8 caracteres
};
```

### Requisitos de Usuário

```csharp
options.User.RequireUniqueEmail = true; // Email deve ser único
```

### Configuração de Roles

```csharp
services.AddIdentity<Usuario, IdentityRole>(options => { ... })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>(); // Habilita sistema de papéis
```

## Fluxos de Autenticação

### 1. Registro de Usuário

**Rota:** `POST /Cadastrar`

**Dados de entrada:**
- `Apelido` - Nome de exibição (3-50 caracteres)
- `Email` - Email do usuário (será único)
- `Senha` - Senha (8+ caracteres, maiúscula, minúscula, dígito)
- `ConfirmarSenha` - Confirmação da senha

**Fluxo interno:**
1. Cria objeto `Usuario` com Email=UserName=email
2. `UserManager.CreateAsync(usuario, senha)` - cria usuário com senha hasheada
3. `UserManager.AddToRoleAsync(usuario, "User")` - atribui papel padrão
4. Retorna `IdentityResult` (Sucesso/Falha)

**Código:**
```csharp
var newUser = new Usuario
{
    Email = body.Email,
    UserName = body.Email,
    Apelido = body.Apelido
};

var usuarioCadastro = await userManager.CreateAsync(newUser, body.Senha);
await userManager.AddToRoleAsync(newUser, "User");
return usuarioCadastro;
```

### 2. Login

**Rota:** `POST /Login`

**Dados de entrada:**
- `Email` - Email do usuário
- `Senha` - Senha
- `LembrarDeMim` - Persistir sessão (boolean)

**Fluxo interno:**
1. `UserManager.FindByEmailAsync(email)` - busca usuário
2. `SignInManager.PasswordSignInAsync(email, senha, isPersistent, lockoutOnFailure)`
3. Retorna `SignInResult` (Succeeded/Failed)
4. Sucesso → redireciona para Home

**Código:**
```csharp
var usuario = await userManager.FindByEmailAsync(body.Email);
if (usuario == null)
    return SignInResult.Failed;

var resultado = await signInManager.PasswordSignInAsync(
    usuario.Email, 
    body.Senha, 
    body.LembrarDeMim, 
    false
);
return resultado;
```

### 3. Logout

**Rota:** `POST /Login/Logout`

**Fluxo interno:**
1. `SignInManager.SignOutAsync()` - encerra a sessão
2. Redireciona para a página inicial

## Papéis e Permissões

### Papéis Disponíveis

| Papel | Descrição | Atribuição |
|-------|-----------|------------|
| **User** | Usuário padrão (colaborador) | Automático no registro |
| **Moderador** | Pode revisar artigos e gerenciar pautas | Manual (Admin) |
| **Admin** | Controle total do sistema | Manual (Admin) |

### Matriz de Permissões

| Ação | User | Moderador | Admin |
|------|:----:|:---------:|:-----:|
| Ver artigos publicados | ✅ | ✅ | ✅ |
| Criar pauta | ✅ | ✅ | ✅ |
| Escrever artigo | ✅ | ✅ | ✅ |
| Editar próprio artigo | ✅ | ✅ | ✅ |
| Excluir próprio artigo | ✅ | ✅ | ✅ |
| Revisar artigos | ❌ | ✅ | ✅ |
| Aprovar artigos | ❌ | ✅ | ✅ |
| Fechar pautas | ❌ | ✅ | ✅ |
| Excluir artigos | ❌ | ✅ | ✅ |
| Gerenciar colaboradores | ❌ | ❌ | ✅ |
| Excluir colaboradores | ❌ | ❌ | ✅ |
| Acessar dashboard admin | ❌ | ✅ | ✅ |

### Atribuição de Papéis nos Controllers

```csharp
// Admin/Index, Admin/Pautas, Admin/Artigos - Moderador e Admin
[Authorize(Roles = "Admin, Moderador")]

// Admin/Colaboradores, Admin/ExcluirColaborador - Apenas Admin
[Authorize(Roles = "Admin")]

// Artigos/Fila, Artigos/Revisar, Artigos/AprovarArtigo - Admin, Moderador e Revisor
[Authorize(Roles = "Admin,Moderador,Revisor")]

// Artigos/Excluir - Admin e Moderador
[Authorize(Roles = "Admin,Moderador")]
```

## Pipeline de Middleware

A ordem correta no `Program.cs` é essencial:

```csharp
app.UseRouting();           // 1. Configura rotas

app.UseAuthentication();    // 2. Autenticação (identifica o usuário)
app.UseAuthorization();     // 3. Autorização (verifica permissões)

app.MapControllerRoute(...); // 4. Mapeia rotas dos controllers
```

> **Importante:** `UseAuthentication()` deve vir ANTES de `UseAuthorization()`. Caso contrário, o contexto do usuário não estará disponível para verificar permissões.

## Modelo de Dados

### Usuario

```csharp
public class Usuario : IdentityUser
{
    public DateTime Criado { get; set; } = DateTime.UtcNow;
    public DateTime Atualizado { get; set; } = DateTime.UtcNow;
    public DateTime? Excluido { get; set; }
    
    [Required, StringLength(50, MinimumLength = 3)]
    public string Apelido { get; set; }
    
    public string? Avatar { get; set; }
}
```

### Tabelas do Identity

O Identity cria automaticamente tabelas como:
- `AspNetUsers` - Usuários
- `AspNetRoles` - Papéis
- `AspNetUserRoles` - Relação Usuário-Papel
- `AspNetUserClaims` - Claims de usuários
- `AspNetUserLogins` - Logins externos
- `AspNetUserTokens` - Tokens de segurança
- `AspNetRoleClaims` - Claims de papéis

## Segurança

### Senha Hasheada
As senhas são armazenadas com hash usando PBKDF2 com salt aleatório. A senha nunca é armazenada em texto plano.

### Anti-Forgery Tokens
Todas as requisições POST utilizam `[ValidateAntiForgeryToken]` para proteção contra CSRF:

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Acao(...) { ... }
```

### HTTPS Redirection
```csharp
app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS
```
