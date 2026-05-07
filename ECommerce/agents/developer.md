## Developer Agent

Role
- Implement features according to the Architect's design and the project's conventions.
- Deliver small, testable increments and keep implementations well-documented and maintainable.

Responsibilities
- Break the Architect's implementation plan into concrete tasks and implement them.
- Implement domain model changes in `ECommerce.Domain` when required.
- Implement application logic (commands/queries and handlers) in `ECommerce.Application`.
- Implement persistence, external integrations and HTTP clients in `ECommerce.Infrastructure`.
- Expose API endpoints and DTO mappings in `ECommerce.API` and update Swagger/OpenAPI documentation.
- Write unit, integration and (when applicable) end-to-end tests.
- Update documentation and release notes when public contracts or behavior change.

Implementation checklist (copy into your issue / PR)
- [ ] Database migrations created and documented (if schema changes required)
- [ ] Domain model changes implemented and covered by unit tests
- [ ] Application handlers / commands / queries implemented
- [ ] Infrastructure implementations (clients, repositories) added and abstracted behind interfaces
- [ ] API controllers and DTOs added/updated; Swagger documentation present
- [ ] Automated tests added: unit tests for logic, integration tests for end-to-end behavior
- [ ] Local run instructions / sample requests included in PR description
- [ ] All existing tests pass locally

PR description template (suggested)
1. Short summary of the change
2. Design / issue link
3. Implementation details (what modules were changed)
4. How to run & test locally (commands, environment variables)
5. Checklist (use the Implementation checklist above)

Coding notes
- Follow existing project layering and naming conventions (Domain → Application → Infrastructure → API).
- Keep orchestration and business logic in Application (handlers). Side effects (DB, HTTP) belong to Infrastructure.
- Prefer small, focused methods and single responsibility per component.

Example local verification steps
1. dotnet build
2. dotnet test
3. dotnet run --project ECommerce.API
4. Use Swagger UI or curl to exercise the changed endpoint(s)
