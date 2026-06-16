## Context

The Jornal Ascensão project is an ASP.NET Core 9 application for a collaborative news platform. It currently has minimal documentation (only a 4-line README.md). The project uses:
- **Backend**: ASP.NET Core 9 with Entity Framework Core
- **Database**: PostgreSQL 15.3 (Docker)
- **Authentication**: ASP.NET Core Identity with role-based authorization
- **Reverse Proxy**: Nginx
- **Containerization**: Docker Compose with 3 services
- **Architecture**: Clean architecture with Controllers, Services (abstractions + implementations), Models, DTOs, Utils

Current state: No structured documentation exists beyond the basic README.

## Goals / Non-Goals

**Goals:**
- Create comprehensive, well-organized documentation in Markdown format
- Cover all 7 capability areas identified in the proposal
- Make documentation accessible at the repository root or in a `/docs` folder
- Use consistent formatting and structure across all documents
- Include code examples and configuration snippets where helpful

**Non-Goals:**
- No code changes to the application
- No infrastructure changes (Docker, CI/CD, etc.)
- No translation to other languages (Portuguese only for now)
- No automated documentation generation from code

## Decisions

### Documentation Structure
**Decision**: Create a `/docs` folder at repository root with separate files for each capability area.

**Rationale**: 
- Keeps repository root clean
- Allows modular documentation that can be linked cross-referenced
- Follows common open-source project conventions
- Easy to navigate and maintain

**Alternatives considered**: 
- Single large README.md → Too unwieldy for comprehensive docs
- Wiki → Not version-controlled with code
- Separate repo → Adds friction for contributors

### Documentation Format
**Decision**: Use Markdown (.md) with consistent heading structure, code blocks for configs, and mermaid diagrams for flows.

**Rationale**: 
- Native GitHub/GitLab rendering
- Version controllable
- Supports diagrams via mermaid
- Familiar to all developers

### Docker Documentation Approach
**Decision**: Document each service (jornalascensao, ascensao-postgres, nginx) with its configuration, environment variables, ports, volumes, and health checks.

**Rationale**: 
- Matches the actual compose.yaml structure
- Helps with troubleshooting and customization
- Clear separation of concerns

### Authentication/Authorization Documentation
**Decision**: Document the Identity configuration, password policies, roles (User, Admin, Moderador), and how `[Authorize]` attributes are used on controllers.

**Rationale**: 
- Critical for security understanding
- Shows the role hierarchy and permissions
- Helps with future role additions

## Risks / Trade-offs

| Risk | Mitigation |
|------|------------|
| Documentation becomes outdated | Add note to update docs with code changes; include in PR checklist |
| Inconsistent formatting across files | Use a style guide; single author for initial version |
| Missing edge cases in user flows | Review with actual users/stakeholders; iterate based on feedback |
| Over-documentation (too verbose) | Focus on "what a new developer needs to know"; link to code for details |
| Diagrams not rendering in all viewers | Provide both mermaid source and rendered PNG fallbacks |