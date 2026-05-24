# Contributing Guide

## Branch Strategy

We follow a simplified Git Flow:

```
main        ← production-ready, protected. Never commit directly.
develop     ← main integration branch. All features merge here via PR.
feature/*   ← new features branched from develop
fix/*       ← bug fixes branched from develop
hotfix/*    ← urgent production fixes branched from main
release/*   ← release preparation branched from develop
chore/*     ← maintenance, dependency updates
```

### Branch naming examples
```
feature/add-user-profile
feature/identity-oauth
fix/login-token-expiry
fix/role-permission-sync
hotfix/jwt-secret-validation
release/1.2.0
chore/update-ef-core
chore/upgrade-vite
```

## Commit Convention

We use [Conventional Commits](https://www.conventionalcommits.org/).

### Format
```
<type>(<scope>): <subject>

[optional body]

[optional footer]
```

### Types
| Type | When to use |
|---|---|
| `feat` | New feature |
| `fix` | Bug fix |
| `docs` | Documentation only |
| `style` | Formatting, no logic change |
| `refactor` | Code restructure, no feature/fix |
| `test` | Adding or fixing tests |
| `chore` | Maintenance, build, tooling |
| `perf` | Performance improvement |
| `ci` | CI/CD pipeline changes |
| `build` | Build system or dependencies |
| `revert` | Revert a previous commit |

### Scopes
| Scope | Area |
|---|---|
| `api` | Backend API / Bootstrapper |
| `frontend` | Vue 3 frontend |
| `identity` | Identity module |
| `auth` | Authentication logic |
| `users` | User management |
| `roles` | Role management |
| `permissions` | Permission management |
| `shared` | SharedKernel |
| `docker` | Docker / docker-compose |
| `ci` | GitHub Actions |
| `deps` | Dependency updates |
| `docs` | Documentation |

### Examples
```
feat(identity): add email verification flow
fix(auth): handle expired jwt on concurrent requests
refactor(users): extract pagination logic to shared helper
docs(api): update endpoint reference in readme
chore(deps): upgrade ef core to 10.0.9
ci: add integration test step to workflow
perf(frontend): lazy-load rolesview component
test(identity): add unit tests for createrolecommandhandler
```

### Rules
- Subject line: lowercase, no period at end, max 72 chars
- Body: explain WHY not WHAT, wrap at 100 chars
- Breaking changes: add `BREAKING CHANGE:` in footer

## Pull Request Flow

```
1. Branch from develop:     git checkout -b feature/your-feature develop
2. Commit with convention:  feat(scope): description
3. Push branch:             git push -u origin feature/your-feature
4. Open PR → develop
5. Squash merge after review
6. Delete branch after merge
```

Production releases:
```
develop → release/x.x.x → main (tag vx.x.x) + back-merge to develop
```
