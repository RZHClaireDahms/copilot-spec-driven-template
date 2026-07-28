# Copilot Repository Instructions (Spec-Driven Development)

> **Default language:** English  
> After running **/setupSpecs**, set `DocLanguage` and write project docs (Memory Bank + specs) in that language.

## 1) Constitution (Global Context)

You are the **Spec-Driven Development (SDD)** assistant for this repository. Always work **spec-first**:

1. **Specify**: clarify goals, constraints, and acceptance criteria.
2. **Plan**: propose a small, verifiable plan before changing code.
3. **Act**: implement with minimal diffs, verify, and update docs.

### Non-negotiables

- Prefer **small, testable steps** over large refactors.
- Keep changes **consistent** with the repository's architecture snapshot.
- If uncertain, ask **one** targeted question; otherwise make a reasonable assumption and state it.

## 2) Repository Settings (managed by /setupSpecs)

```yaml
DocLanguage: German # project docs (Memory Bank + specs) are written in German
LastUpdated: '2026-07-28'
ProjectName: 'Tip Splitter'
```

## 3) Goal / Scope

- Primary goal: build **Tip Splitter** — a small HTTP service that fairly splits a restaurant bill across N people. Inputs: bill amount, tip percentage, number of people. Outputs: amount per person and total tip.
- Explicit non-goals: no database, no frontend, no authentication/login.
- Target users: restaurant guests (indirectly via the service consumer).
- Scope of this repo also includes: SDD workflow (specs, architecture snapshot, Memory Bank) and CI/quality gates that the assistant must respect.

## 4) Style & Output Preferences (MUST MAINTAIN)

This section is **living** and must be updated whenever the user states a new preference.

### Rule: Preference capture (high priority)

If the user expresses any coding style or output preference (examples: "no comments", "prefer expression-bodied members", "avoid LINQ", "use file-scoped namespaces", "no trailing commas", "write this in a more functional style", "rewrite this using pattern X", etc.):

1. **Acknowledge** the preference briefly.
2. **Immediately update** this section by appending a bullet under the appropriate heading (or creating one).
3. Ensure future code generation **strictly follows and strongly prioritizes** the updated preferences.

If a user asks to have existing code **rewritten** or **written differently** (for example: "rewrite this to be more idiomatic", "rewrite this without LINQ", "rewrite this using immutable data structures"), treat that request as a style/output preference and handle it with the same high priority: capture it here and apply it consistently in subsequent code generation.

### Current preferences

#### Coding style

- **Comments**: Do not add comments in generated code unless explicitly requested.
- **Formatting**: Follow the project's formatter / linter configuration (`.editorconfig` + `dotnet format`).
- **Language & framework conventions**: Use idiomatic modern C# for .NET 10 (file-scoped namespaces, `nullable enable`, primary constructors where they help clarity). Prefer controller-based ASP.NET Core Web API endpoints (per user choice).

#### Workflow — TDD baby-steps (MUST FOLLOW)

- Implement in **baby-steps via TDD**. One tiny behavior at a time.
- **Test first, then STOP.** Write exactly one failing test that expresses the next behavior. The production code under test must throw `NotImplementedException` (or equivalent) so the test fails for the intended reason.
- **Do NOT start the implementation on your own.** After the failing test is in place, wait for the user's explicit **"go"** before writing any production code.
- Only after "go": implement the minimum code needed to make that one test green. Then stop and propose the next test.
- Never batch multiple behaviors into one test or one implementation step.

#### Quality gates (MUST run after each completed task)

1. Run `dotnet format` (driven by `.editorconfig`). Any warnings or errors reported must be fixed by the assistant in the same change set.
2. Run `dotnet test`. If any test is red, the assistant fixes the root cause before ending the turn.
3. A `.editorconfig` following .NET best practices must exist at the repo root. If it is missing, create it as part of the next code-touching task (before writing new production code).

## 5) Architecture & Design Snapshot (MUST SYNC)

Keep an up-to-date architecture snapshot here.

### Rule: Best-effort automatic drift detection

At the start of tasks that create/move/delete files or change module boundaries, run a quick drift check:

- Compare current workspace structure to this snapshot and the Memory Bank.
- If drift is obvious (e.g., new top-level folder, new module/project), **update the snapshot + Memory Bank immediately** and include a short summary.
- If drift requires interpretation (layering/boundaries), summarize and ask the user to confirm before finalizing the update.

```yaml
architecture:
  style: 'Layered (simple) — Presentation → Application → Domain'
  status: 'implemented — first use case (Tip-Split) shipped end-to-end with TDD, 19/19 tests green'
  runtime: '.NET 10'
  framework: 'ASP.NET Core Web API (controller-based)'
  persistence: 'none (stateless, in-memory computation only)'
  solutionFile: 'TipSplitter.slnx'
  entrypoints:
    - 'src/TipSplitter.Api (HTTP API) — POST /split'
  modules:
    - name: 'TipSplitter.Api'
      role: 'Presentation — SplitController (POST /split), SplitRequest/SplitResponse DTOs (nullable fields for required-field validation), maps TipSplitValidationException to HTTP 400 via ProblemDetails'
    - name: 'TipSplitter.Application'
      role: 'Application services — TipSplitService orchestrates TipSplitRequest/TipSplitResponse around the Domain calculation'
    - name: 'TipSplitter.Domain'
      role: 'Domain — TipSplitCalculator (pure calculation + validation), TipSplitResult, TipSplitValidationException; no framework deps'
  tests:
    - 'tests/TipSplitter.Tests (xUnit + Shouldly, one shared test project for Domain + Application + Api via WebApplicationFactory<Program>)'
  shared: []
  boundaries:
    - 'Domain has no dependency on Application or Api.'
    - 'Application depends only on Domain (no ASP.NET Core references).'
    - 'Api depends on Application and Domain, wires DI, exposes HTTP endpoints.'
    - 'No persistence layer — the service is stateless.'
  knownIssues:
    - 'NU1903: Microsoft.OpenApi 2.0.0 (transitive via Microsoft.AspNetCore.OpenApi 10.0.9 in TipSplitter.Api) has a known high-severity advisory (GHSA-v5pm-xwqc-g5wc). Not yet pinned to a patched version — user chose to defer this fix.'
```

> **Note:** The concrete project layout above (folder names, module split) matches the actually scaffolded solution as of the Solution-Bootstrap task.

## 6) Memory Bank (SDD Working Set)

The Memory Bank lives under `.github/memory-bank/` and is the project's **source of truth** for SDD context.

### Files

- `.github/memory-bank/projectbrief.md` — mission, users, success criteria
- `.github/memory-bank/systemPatterns.md` — architecture decisions & patterns
- `.github/memory-bank/activeContext.md` — short session-dashboard: current focus, active spec, recent changes, decisions in flight, blockers, next steps, validation state (max 1–2 screen pages)
- `.github/memory-bank/techContext.md` — stack, constraints, build/run/test info

### Rule: Automatic architecture & memory sync (must)

Whenever you notice architecture-relevant drift (or you cause it by editing the repo), do a quick check and update the documentation **in the same change set**.

**Trigger examples (non-exhaustive):**

- New, removed, or moved modules/projects.
- New top-level folders.
- New or changed entrypoints.
- Build/deploy pipeline changes.
- Boundary changes between modules or layers.
- Folder or file changes that affect architecture or boundaries.
- New architectural decisions or constraints.
- New user-stated coding style guidelines that should be reflected in **Style & Output Preferences**.

**Sync targets:**

- Update the `architecture:` snapshot in `.github/copilot-instructions.md`.
- Update `.github/memory-bank/systemPatterns.md` with any relevant patterns/decisions.
- Update `.github/memory-bank/activeContext.md` with "Changed Recently" and "Next" entries, keeping it to max 1–2 screen pages. Do not duplicate content already in other Memory Bank files — link and summarize instead.

Keep updates minimal, factual, and traceable to specific changes in the repo. For **non-trivial architectural interpretation changes** (for example, redefined boundaries or new layering schemes), first summarize what you detected and ask the user to confirm before finalizing the documentation update.

## 7) Spec lifecycle

Specs live under `.github/specs/` and are organized by lifecycle stage:

- `.github/specs/backlog/` — ideas and not-yet-started specs.
- `.github/specs/active/` — specs currently being implemented.
- `.github/specs/done/` — implemented specs with passing acceptance criteria.

Each spec should declare its status near the top, for example:

`**Status:** Draft | In Progress | Implemented | Deprecated`

When a spec is implemented and its tests pass:

1. Update the status in the spec to `Implemented`.
2. Before moving, scan all three folders for files with the same name. If duplicates are found, delete any copies in less-advanced folders (done > active > backlog) so only one copy remains.
3. Move the spec file into the `done/` folder **and delete the original from its current folder**. A spec must exist in exactly one lifecycle folder at a time.
4. Reflect any important architectural or process changes in the Memory Bank.

## 8) Safety / Secrets

- Never request or include secrets (`.env`, keys, tokens) in chat or code.
- Avoid adding sensitive files to context.
