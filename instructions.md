## General Principles

**Code Quality & Structure:**

- Always implement guard clauses first and return early
- Avoid deeply nested `if` statements (max 2-3 levels)
- Never put return statements on the same line as `if` statements
- Keep functions focused on a single responsibility
- Format code to 100 characters per line maximum
- Follow SOLID principles consistently
- Design with testability in mind from the start

**Code Style:**

- Keep it simple and concise - avoid "clever" or overly complex solutions
- Limit comments to essential explanations of business logic or complex algorithms
- Prefer explicit, readable code over terse implementations
- Use descriptive variable and function names

**Backend:**

- Follow the .NET community's best practices and coding styles for all backend code.

**Testing Guidelines:**

- All new features and bug fixes must include appropriate tests using xUnit.
- Favor the biggest win per test: prefer integration-style tests over unit tests when possible.
- Test method names should clearly describe the scenario being tested.
- Aim for high test coverage, especially for core game logic.
- Tests should be fast, isolated, and repeatable.
- To run tests: use `dotnet test` from the solution root.
- Mock external dependencies where possible to ensure test reliability.
- The AI assistant will not directly manage or enforce testing guidelines unless explicitly asked to do so by the user.

**Dependency Management:**

- Use NuGet packages for external dependencies whenever possible.
- Keep dependencies up to date, but avoid unnecessary or experimental packages.
- Remove unused dependencies promptly.
- Prefer well-maintained and widely adopted libraries.
- The AI assistant will not directly add, update, or remove dependencies unless explicitly asked to do so by the user.

**AI Assistant Behavior:**

- The AI assistant will always confirm with the user before writing or implementing any code.
- The AI assistant will always ask before writing or implementing any tests.
- The AI assistant will look for a .editorconfig file and follow the rules specified there.
- The AI assistant will NOT add xml or jsdoc comments unless asked to do so.
- The AI assistant will NOT run any tasks in the `task.json` file. It should run them directly in its terminal.