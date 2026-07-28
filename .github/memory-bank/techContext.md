# Technischer Kontext

## Stack

- **Runtime:** .NET 10
- **Framework:** ASP.NET Core Web API, **Controller-basiert** (keine Minimal APIs)
- **Sprache:** C# (modern, file-scoped namespaces, `nullable enable`)
- **Test-Framework:** xUnit (Standard für .NET), Assertions bevorzugt über `Assert` oder eine schmale Library nach Bedarf
- **Persistenz:** keine (stateless)
- **Frontend:** keins
- **Auth:** keine

## Build / Run (geplant, sobald das Projekt angelegt ist)

- Build: `dotnet build`
- Start API lokal: `dotnet run --project src/TipSplitter.Api`
- Restore: `dotnet restore`

## Test

- Ausführen: `dotnet test`
- Vorgehen: **TDD in Baby-Steps** — pro Verhaltensschritt genau ein neuer, initial roter Test.
- Erst wenn der Test steht und rot ist, wartet der Agent auf ein explizites **„go“** vom Nutzer, bevor die Implementierung erfolgt.

## Quality Gates (nach jedem Task verpflichtend)

1. `dotnet format` (basiert auf `.editorconfig` im Repo-Root; muss nach .NET-Best-Practices einmalig angelegt werden). Warnings/Errors werden vom KI-Agenten in derselben Runde behoben.
2. `dotnet test` ausführen; rote Tests werden vom KI-Agenten behoben.
3. CI: **GitHub Actions** — führt `dotnet format --verify-no-changes`, `dotnet build` und `dotnet test` aus.

## Constraints

- Keine Datenbank, keine Persistenz, keine externen Services.
- Kein Frontend / kein UI in diesem Repo.
- Kein Auth-/Login-System.
- Reine, deterministische Berechnungslogik im Domain-Layer (leicht testbar, ohne Framework-Abhängigkeiten).
