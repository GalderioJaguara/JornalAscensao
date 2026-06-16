## ADDED Requirements

### Requirement: Admin dashboard data is documented
The documentation SHALL describe all data shown on the admin dashboard and how it's computed.

#### Scenario: Admin views dashboard metrics
- **WHEN** an admin/moderator visits /Admin
- **THEN** they see: ArtigosFila (count of articles with Aprovado=false)
- **THEN** they see: ArtigosPublicados (count of articles with Aprovado=true)
- **THEN** they see: PautasAbertas (count of pautas with Fechada=false)
- **THEN** they see: PautasFechadas (count of pautas with Fechada=true)
- **THEN** they see: NovosColaboradores (users created in current week, Sun-Sat)
- **THEN** they see: ColaboradoresBloqueados (users with LockoutEnd > now)

### Requirement: Pauta management by admin is documented
The documentation SHALL describe admin capabilities for managing pautas.

#### Scenario: Admin lists all pautas
- **WHEN** an admin/moderator visits /Admin/Pautas
- **THEN** they see paginated list of all pautas (open and closed)
- **THEN** each pauta shows: id, titulo, categoria, tipo, fechado, criador, data criação

#### Scenario: Admin closes a pauta
- **WHEN** an admin/moderator clicks close on a pauta
- **THEN** POST to /Admin/FecharPauta with pauta id
- **THEN** PautaService.FecharPautaAsync sets Fechada=true
- **THEN** returns NoContent on success, NotFound if pauta doesn't exist

### Requirement: Article management by admin is documented
The documentation SHALL describe admin capabilities for reviewing and managing articles.

#### Scenario: Admin lists all articles
- **WHEN** an admin/moderator visits /Admin/Artigos
- **THEN** they see paginated list of all articles
- **THEN** each article shows: titulo, categoria, status, aprovado

#### Scenario: Admin reviews pending articles
- **WHEN** an admin/moderator accesses article review
- **THEN** they see articles with Aprovado=false and Status=Revisando
- **THEN** they can approve (sets Aprovado=true, Status=Publicado, Publicado=now)
- **THEN** they can request corrections (sets Status=Corrigindo)

### Requirement: Collaborator management by admin is documented
The documentation SHALL describe admin-only user management features.

#### Scenario: Admin views all collaborators
- **WHEN** an admin visits /Admin/Colaboradores
- **THEN** they see all users with: email, apelido, roles, created date, lockout status

#### Scenario: Admin deletes a collaborator
- **WHEN** an admin deletes a collaborator
- **WHEN** POST to /Admin/ExcluirColaborador with email
- **THEN** UsuarioService.DeleteUsuarioAsync removes the user
- **THEN** returns NoContent on success, NotFound if user doesn't exist
- **THEN** only Admin role can access (not Moderador)

### Requirement: Anti-forgery token protection is documented
The documentation SHALL describe CSRF protection on admin actions.

#### Scenario: Admin actions are protected
- **WHEN** an admin performs POST actions (FecharPauta, ExcluirColaborador)
- **THEN** [ValidateAntiForgeryToken] attribute validates the request
- **THEN** prevents CSRF attacks on state-changing operations