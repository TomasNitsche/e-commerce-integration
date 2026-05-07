# Agents workflow — quick start

This folder contains role descriptions and templates to coordinate design, implementation and testing of new features.

## How to start
1. Create a new issue and assign the `feature` label.
2. Paste the design template from `agents/ARCHITECT.md` (or copy from `agents/architect.md`) into the issue and fill it out.
3. Tag the responsible Architect(s) for review.
4. After the design is approved, the Developer will create a branch and split the work into tasks following `agents/DEVELOPER.md`.
5. Developer creates a PR, attaches the implementation plan and a completed PR checklist.
6. Tester prepares a test plan following `agents/TESTER.md` and posts it as a comment in the PR.

## Communication
- Architect publishes the design in the issue and updates it based on feedback.
- Developer reports progress in the PR (commits, checklist updates).
- Tester adds the test plan and test results to the PR and reports blockers.

## Updating templates
- To change a template, edit the files in `agents/` and open a PR describing the changes.
- For significant changes add a short changelog entry at the top of the edited template.

This workflow aims to ensure consistency: Architect defines the design, Developer implements according to the checklist, Tester validates before merge.
