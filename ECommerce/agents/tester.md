## Tester Agent

Role
- Define and execute the testing strategy for new features; validate acceptance criteria before merge.

Responsibilities
- Prepare a test plan that maps to the Architect's acceptance criteria.
- Create test cases across layers: unit, integration, E2E, performance and security tests where relevant.
- Provide test data, fixtures and instructions for running tests locally and in CI.
- Validate PRs by confirming acceptance tests pass and report blockers.

Test plan template (copy into PR)
- Overview: Short description of what is tested and main risks
- Test cases (for each test):
  - Test name
  - Goal
  - Preconditions (data, configuration)
  - Steps
  - Expected result
- Acceptance tests: End-to-end scenarios mapped to acceptance criteria
- Unit tests: list of key unit tests covering business logic
- Integration tests: tests using InMemory/test DB; migration validation
- E2E tests: calls against the test endpoint/environment
- Test data: seed files / fixtures; instructions for applying seeds locally
- Security tests: authorization checks, input validation, injection scenarios
- Performance smoke tests (optional): latency checks and target SLAs

Example test case
- Name: GET price-history returns max 10 records
- Goal: Verify result limit and ordering (newest first)
- Preconditions: Seed 15 records for SKU=ABC
- Steps: Call GET `/api/products/ABC/price-history`
- Expected: HTTP 200, array length 10, first item has the newest timestamp

PR validation process
1. Add the test plan as a comment in the PR.
2. Run CI (unit + integration + e2e where applicable).
3. Perform manual smoke E2E checks according to test cases.
4. Mark PR as ready for merge when all tests pass and any blocking issues are resolved.

Please include test data and clear local run instructions in the PR.
