## Feature Request Template

Use this template when creating a new feature issue. Paste it into the issue body and fill the sections.

Title: [short feature title]

1) Business goal
- What business problem does this feature solve? Why is it needed?

2) Context / Impacted modules
- Which parts of the system are affected? (e.g. `ECommerce.API`, `ECommerce.Application`, `ECommerce.Domain`, `ECommerce.Infrastructure`)
- Any external services or DB schema changes?

3) Proposed design (summary)
- Short description of the proposed approach. Architect should expand this into `agents/ARCHITECT.md` design sections.

4) Constraints & dependencies
- Performance, security, operational constraints, and external dependencies.

5) Acceptance criteria
- List measurable acceptance criteria (examples: endpoints, expected status codes, performance targets, error handling behavior).

6) Implementation notes / initial task checklist for developer
- DB migration:
- Domain changes:
- Application handlers:
- Infrastructure clients/repositories:
- API endpoints / DTOs:
- Tests to add:

7) Test plan summary (for Tester)
- Brief list of important test cases and any special test data required. Link to `agents/TESTER.md` for details.

8) Suggested reviewers
- Architect: @...
- Developer: @...
- QA/Tester: @...

Usage note: After drafting the issue, tag the Architect for review. Once design is approved, the Developer and Tester will follow `agents/DEVELOPER.md` and `agents/TESTER.md`.

