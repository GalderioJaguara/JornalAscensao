## ADDED Requirements

### Requirement: Pauta (pitch/topic) management behavior is documented
The documentation SHALL describe how pautas work: creation, listing, metadata extraction from URLs, closing, and deletion.

#### Scenario: User creates a pauta
- **WHEN** an authenticated user submits a pauta with title, description, category, type, image URL, and content link
- **THEN** the system creates a pauta with Fechada=false, UsuarioId=current user, Criado=now
- **THEN** the pauta appears in the open pautas list

#### Scenario: System extracts metadata from URL
- **WHEN** a user provides a LinkConteudo when creating a pauta
- **THEN** the system fetches the URL and extracts og:title, og:description, og:image meta tags
- **THEN** extracted data pre-fills the pauta form (Titulo, Descricao, Imagem)

#### Scenario: Admin/moderator closes a pauta
- **WHEN** an admin or moderator closes a pauta
- **THEN** the pauta Fechada becomes true
- **THEN** the pauta no longer appears in open pautas list
- **THEN** users can still create articles for closed pautas (if they already started)

### Requirement: Article lifecycle behavior is documented
The documentation SHALL describe article states and transitions: writing → reviewing → correcting → published.

#### Scenario: User creates an article
- **WHEN** a user creates an article for an open pauta
- **THEN** the article is created with Status=Corrigindo, Aprovado=false, AutorId=current user
- **THEN** the associated pauta is marked Fechada=true
- **THEN** article appears in user's pending articles

#### Scenario: Article review workflow
- **WHEN** an article is submitted for review
- **THEN** Status=Revisando, appears in admin/moderator review queue
- **WHEN** reviewer approves
- **THEN** Status=Publicado, Aprovado=true, Publicado=now
- **WHEN** reviewer requests corrections
- **THEN** Status=Corrigindo, Aprovado=false, returns to author

#### Scenario: User edits their article
- **WHEN** an author edits their article (any status)
- **THEN** the article is updated with new content
- **THEN** Atualizado timestamp is set

#### Scenario: User deletes their article
- **WHEN** an author deletes their article
- **THEN** the article is removed from database
- **THEN** the associated pauta may be reopened (if no other articles exist)

### Requirement: Article listing and filtering behavior is documented
The documentation SHALL describe how articles are listed, paginated, and filtered.

#### Scenario: Public article listing
- **WHEN** any user visits home page
- **THEN** they see paginated list of approved articles (Aprovado=true)
- **THEN** ordered by Publicado descending
- **THEN** each item shows: slug, title, gancho, image, published date, category, author apelido

#### Scenario: Category filtering
- **WHEN** a user filters by category
- **THEN** only articles matching that category are shown
- **THEN** pagination works within filtered results

### Requirement: Collaborator dashboard behavior is documented
The documentation SHALL describe what collaborators see in their dashboard.

#### Scenario: Collaborator views dashboard
- **WHEN** a logged-in user visits collaborator dashboard
- **THEN** they see their published articles, pending articles, and can create new articles
- **THEN** they can update their profile (apelido, avatar)