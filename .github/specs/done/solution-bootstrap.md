**Status:** Implemented

# Solution-Bootstrap: Projektstruktur, Test-Tooling & Code-Style

## Summary

Anlegen der .NET-Solution und Projektstruktur für Tip Splitter gemäß dem Architektur-Snapshot (`Api → Application → Domain`), inklusive Test-Projekt (xUnit + Shouldly) und `.editorconfig`-Dateien nach .NET-Best-Practices. Diese Spec liefert noch **keine fachliche Logik** — sie schafft nur das Grundgerüst, auf dem die erste TDD-Feature-Spec aufsetzt.

## Problem / Motivation

Aktuell existiert im Repository noch kein Code — nur `.github/`-Tooling. Bevor die erste fachliche Anforderung (Tip-Split-Berechnung) per TDD umgesetzt werden kann, braucht es eine kompilierbare, testbare Solution-Struktur mit funktionierenden Quality Gates (`dotnet format`, `dotnet test`).

## Goals / Non-goals

**Goals**

- Solution-Datei + drei Quellprojekte gemäß Architektur-Snapshot: `TipSplitter.Domain`, `TipSplitter.Application`, `TipSplitter.Api`.
- Ein Test-Projekt `TipSplitter.Tests` mit **xUnit** + **Shouldly**, das Domain und Application referenziert.
- `.editorconfig` nach .NET-Best-Practices (Root-Config + ggf. Override für Tests).
- Solution baut fehlerfrei (`dotnet build`), Tests laufen durch (`dotnet test`, initial 0 Tests oder ein Platzhalter), `dotnet format` liefert keine Warnungen/Fehler.

**Non-goals**

- Keine fachliche Tip-Split-Logik (Berechnung, Validierung, Endpunkt) — das folgt in einer eigenen, TDD-getriebenen Spec.
- Kein CI-Workflow (GitHub Actions) — separate Spec/Task.
- Keine Authentifizierung, keine Persistenz, kein Frontend.

## Requirements

1. **Solution-Layout**
   - `TipSplitter.sln` im Repo-Root.
   - `src/TipSplitter.Domain/` — Class Library, keine Abhängigkeit auf `Application`/`Api`.
   - `src/TipSplitter.Application/` — Class Library, referenziert nur `Domain`.
   - `src/TipSplitter.Api/` — ASP.NET Core Web API (Controller-basiert), referenziert `Application` + `Domain`.
   - `tests/TipSplitter.Tests/` — xUnit-Testprojekt, referenziert `Domain` + `Application`.
2. **Test-Tooling**
   - Pakete im Testprojekt: `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`, `Shouldly`.
   - Assertions im Repo künftig über Shouldly (`x.ShouldBe(...)`) statt `Assert.Equal`.
3. **Code-Style / `.editorconfig`**
   - Ein `.editorconfig` im Repo-Root mit .NET-Best-Practice-Regeln (Namenskonventionen, `dotnet_sort_system_directives_first`, `csharp_new_line_*`, Nullable-relevante Analyzer-Severities etc.), gültig für `src/**` und `tests/**`.
   - Sinnvolle, dokumentierte Overrides für `tests/**` (z. B. gelockerte Naming-Regeln für Testmethodennamen mit Unterstrichen), falls nötig.
4. **Zielframework:** .NET 10 für alle Projekte.
5. **Nullable & Implicit Usings:** in allen Projekten aktiviert.

## Acceptance Criteria (testable)

- [x] `dotnet build` auf `TipSplitter.slnx` läuft ohne Fehler durch.
- [x] `dotnet test` läuft ohne Fehler durch (Testprojekt kompiliert gegen xUnit + Shouldly; ein Platzhalter-Test (`ToolchainSmokeTests`) grün, um die Toolchain zu verifizieren).
- [x] `dotnet format --verify-no-changes` meldet keine Änderungen/Warnungen.
- [x] Projekt-Referenzen entsprechen exakt den Boundary-Regeln aus dem Architektur-Snapshot (`Domain` hat keine Referenz auf `Application`/`Api`; `Application` referenziert nur `Domain`).
- [x] Architektur-Snapshot in `.github/copilot-instructions.md` und `systemPatterns.md` wird von „planned“ auf „vorhanden/umgesetzt“ aktualisiert.

## Open Questions (resolved)

- Ein gemeinsames `TipSplitter.Tests`-Projekt für Domain+Application, oder getrennte Testprojekte pro Quellprojekt? → **Entschieden: ein gemeinsames Testprojekt** (`tests/TipSplitter.Tests`), referenziert Domain + Application.
- Reicht ein einziges Root-`.editorconfig`, oder wird zusätzlich ein `tests/.editorconfig` mit Overrides benötigt? → **Entschieden: ein Root-`.editorconfig`** mit einem Override-Block für `tests/**` (lockert Naming-Regeln für Testklassen/-methoden).

## Known Issues

- `NU1903`: `Microsoft.OpenApi` 2.0.0 (transitiv über `Microsoft.AspNetCore.OpenApi` 10.0.9 in `TipSplitter.Api`) hat eine bekannte High-Severity-Sicherheitslücke (GHSA-v5pm-xwqc-g5wc, gepatcht ab 2.7.5). Fix wurde vorgeschlagen, vom Nutzer aber vorerst zurückgestellt — kein Blocker für diese Spec, aber vor einem Produktiv-Einsatz zu beheben.

## Out of Scope

- Fachliche Berechnungslogik, HTTP-Endpunkte, Request-/Response-DTOs.
- CI-Pipeline (GitHub Actions).
- Deployment/Hosting-Konfiguration.
