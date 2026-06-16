## ADDED Requirements

### Requirement: ASP.NET Core Identity configuration is documented
The documentation SHALL describe the Identity setup including password policies, user requirements, and token providers.

#### Scenario: Developer reviews Identity configuration
- **WHEN** a developer reads the authentication documentation
- **THEN** they see: PasswordOptions with RequireDigit=true, RequireLowercase=true, RequireUppercase=true, RequireNonAlphanumeric=false, RequiredLength=8
- **THEN** they see: User.RequireUniqueEmail=true
- **THEN** they see: EntityFrameworkStores with AppDbContext
- **THEN** they see: DefaultTokenProviders enabled
- **THEN** they see: Roles enabled with AddRoles<IdentityRole>()

### Requirement: User registration flow is documented
The documentation SHALL describe how new users register and the default role assignment.

#### Scenario: New user registers
- **WHEN** a user submits registration form with email, password, apelido
- **THEN** AutenticacaoService.CadastrarUsuarioAsync creates Usuario with Email=UserName=email, Apelido=apelido
- **THEN** password is hashed via UserManager.CreateAsync
- **THEN** user is assigned "User" role via AddToRoleAsync
- **THEN** IdentityResult is returned

### Requirement: User login flow is documented
The documentation SHALL describe the login process including remember me functionality.

#### Scenario: User logs in
- **WHEN** a user submits login form with email, password, lembrarDeMim
- **THEN** AutenticacaoService.LogarUsuarioAsync finds user by email
- **THEN** SignInManager.PasswordSignInAsync validates credentials with isPersistent=lembrarDeMim
- **THEN** SignInResult returned (Succeeded/Failed)
- **THEN** on success, user is redirected to home

### Requirement: User logout flow is documented
The documentation SHALL describe the logout process.

#### Scenario: User logs out
- **WHEN** a user clicks logout
- **THEN** AutenticacaoService.DeslogarUsuarioAsync calls SignInManager.SignOutAsync
- **THEN** user is redirected to home page

### Requirement: Role-based authorization is documented
The documentation SHALL describe the roles and their permissions.

#### Scenario: Role hierarchy and permissions
- **WHEN** a developer reads the authorization documentation
- **THEN** they see three roles: User (default), Moderador, Admin
- **THEN** they see: [Authorize(Roles = "Admin, Moderador")] on AdminController.Index, Pautas, Artigos, FecharPauta
- **THEN** they see: [Authorize(Roles = "Admin")] on AdminController.Colaboradores, ExcluirColaborador
- **THEN** they understand: User role has no explicit authorize attributes (access via collaborator controller)

### Requirement: Authentication middleware pipeline is documented
The documentation SHALL describe the auth middleware order in Program.cs.

#### Scenario: Developer reviews auth pipeline
- **WHEN** a developer reads the middleware documentation
- **THEN** they see: app.UseAuthentication() before app.UseAuthorization()
- **THEN** they see: app.UseRouting() before auth middleware
- **THEN** they understand the correct order for ASP.NET Core auth