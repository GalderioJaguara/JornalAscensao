## ADDED Requirements

### Requirement: Reader (anonymous) user flow is documented
The documentation SHALL describe what anonymous users can do.

#### Scenario: Anonymous user browses articles
- **WHEN** an anonymous user visits the home page
- **THEN** they see paginated list of published articles
- **THEN** they can click an article to read full content
- **THEN** they can filter by category
- **THEN** they cannot create pautas, articles, or access admin areas

#### Scenario: Anonymous user registers
- **WHEN** an anonymous user clicks register
- **THEN** they provide email, password (8+ chars, upper, lower, digit), apelido (3-50 chars)
- **THEN** system creates Usuario with User role
- **THEN** user is logged in and redirected to home

### Requirement: Collaborator (authenticated User role) flow is documented
The documentation SHALL describe the complete collaborator journey.

#### Scenario: Collaborator suggests a pauta
- **WHEN** a logged-in user creates a pauta
- **THEN** they fill: tipo, categoria, titulo, descricao, imagem (optional), linkConteudo
- **THEN** system auto-extracts metadata from linkConteudo
- **THEN** pauta is saved with Fechada=false

#### Scenario: Collaborator writes an article
- **WHEN** a collaborator selects an open pauta to write for
- **THEN** they provide: titulo, gancho, texto (1000+ chars), referencias, imagem, pautaId
- **THEN** article is created with Status=Corrigindo, linked to pauta
- **THEN** pauta is closed (Fechada=true)

#### Scenario: Collaborator tracks article status
- **WHEN** a collaborator views their dashboard
- **THEN** they see: published articles, pending review articles, articles needing correction
- **THEN** they can edit articles in Corrigindo status
- **THEN** they can delete their own articles

#### Scenario: Collaborator updates profile
- **WHEN** a collaborator updates their profile
- **THEN** they can change apelido and avatar
- **THEN** changes are saved to Usuario record

### Requirement: Admin/Moderator flow is documented
The documentation SHALL describe the admin/moderator capabilities.

#### Scenario: Admin accesses dashboard
- **WHEN** an admin or moderator visits /Admin
- **THEN** they see: artigos em fila, artigos publicados, pautas abertas, pautas fechadas, novos colaboradores, colaboradores bloqueados

#### Scenario: Admin manages pautas
- **WHEN** an admin/moderator views /Admin/Pautas
- **THEN** they see paginated list of all pautas
- **THEN** they can close pautas (Fechada=true)

#### Scenario: Admin manages articles
- **WHEN** an admin/moderator views /Admin/Artigos
- **THEN** they see paginated list of all articles with status
- **THEN** they can review, approve, or request corrections

#### Scenario: Admin manages collaborators (Admin only)
- **WHEN** an admin views /Admin/Colaboradores
- **THEN** they see all users with roles
- **THEN** they can delete/block users