# Technischer Kontext

## Stack

- **Runtime:** .NET 10
- **Framework:** ASP.NET Core Web API, **Controller-basiert** (keine Minimal APIs)
- **Sprache:** C# (modern, file-scoped namespaces, `nullable enable`)
- **Test-Framework:** xUnit (Standard für .NET), Assertions über **Shouldly**
- **Persistenz:** keine (stateless)
- **Frontend:** keins
- **Auth:** keine

## Build / Run

- Build: `dotnet build` (Solution-Datei: `TipSplitter.slnx`)
- Start API lokal: `dotnet run --project src/TipSplitter.Api`
- Restore: `dotnet restore`

## Test

- Ausführen: `dotnet test`
- Assertions: **Shouldly** (`x.ShouldBe(...)`), Test-Framework xUnit.
- Api-Integrationstests: `Microsoft.AspNetCore.Mvc.Testing` (`WebApplicationFactory<Program>`); `Program.cs` hat dazu ein `public partial class Program;` am Ende.
- Vorgehen: **TDD in Baby-Steps** — pro Verhaltensschritt genau ein neuer, initial roter Test.
- Erst wenn der Test steht und rot ist, wartet der Agent auf ein explizites **„go“** vom Nutzer, bevor die Implementierung erfolgt.

## Quality Gates (nach jedem Task verpflichtend)

1. `dotnet format TipSplitter.slnx` (basiert auf `.editorconfig` im Repo-Root, bereits angelegt). Warnings/Errors werden vom KI-Agenten in derselben Runde behoben.
2. `dotnet test` ausführen; rote Tests werden vom KI-Agenten behoben.
3. CI: **GitHub Actions** (noch nicht eingerichtet) — soll `dotnet format --verify-no-changes`, `dotnet build` und `dotnet test` ausführen.

## Known Issues

- `TipSplitter.Api` referenziert transitiv `Microsoft.OpenApi` 2.0.0 (über `Microsoft.AspNetCore.OpenApi` 10.0.9), das eine bekannte High-Severity-Sicherheitslücke hat (`NU1903`, GHSA-v5pm-xwqc-g5wc). Fix (Pin auf `Microsoft.OpenApi` 2.7.5) wurde vorgeschlagen, aber vom Nutzer vorerst zurückgestellt.

## Constraints

- Keine Datenbank, keine Persistenz, keine externen Services.
- Kein Frontend / kein UI in diesem Repo.
- Kein Auth-/Login-System.
- Reine, deterministische Berechnungslogik im Domain-Layer (leicht testbar, ohne Framework-Abhängigkeiten).
