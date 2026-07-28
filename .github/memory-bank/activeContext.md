# Aktiver Kontext

Last updated: 2026-07-28
Current branch: (unbekannt — im Zweifel `main`)
Current phase: Solution-Bootstrap abgeschlossen → bereit für erste fachliche Spec (TDD)

## Now

Solution + Projektstruktur (`Api`/`Application`/`Domain`/`Tests`) stehen, bauen und testen grün. Als Nächstes: erste fachliche Spec für den Tip-Split-Endpunkt via `/specify`.

## Active Spec

- Spec: — (keine aktiv; `solution-bootstrap.md` ist nach `done/` verschoben)
- Aktueller Task: `/specify` für den ersten fachlichen Use Case (Tip-Split-Endpunkt) ausführen
- Akzeptanzkriterien im Fokus: t. b. d. mit der nächsten Spec

## Changed Recently

- 2026-07-28: `/setupSpecs` durchgeführt — `DocLanguage=German`, Projektziel „Tip Splitter“ gesetzt, Style- und Workflow-Präferenzen (TDD Baby-Steps, `dotnet format`- und `dotnet test`-Gate) in `.github/copilot-instructions.md` ergänzt.
- 2026-07-28: Spec `solution-bootstrap.md` erstellt, umgesetzt und nach `.github/specs/done/` verschoben (Status: Implemented).
- 2026-07-28: Solution `TipSplitter.slnx` mit vier Projekten angelegt: `src/TipSplitter.Domain`, `src/TipSplitter.Application`, `src/TipSplitter.Api` (Controller-basiert), `tests/TipSplitter.Tests` (xUnit + Shouldly). Referenzen gemäß Architektur-Boundaries gesetzt (`Api → Application, Domain`; `Application → Domain`; `Tests → Domain, Application`).
- 2026-07-28: Root-`.editorconfig` nach .NET-Best-Practices angelegt (inkl. Override-Block für `tests/**`); `dotnet format --verify-no-changes` läuft grün.
- 2026-07-28: `dotnet build` und `dotnet test` laufen grün (1 Platzhalter-Test `ToolchainSmokeTests` mit Shouldly-Assertion).
- 2026-07-28: Architektur-Snapshot in `.github/copilot-instructions.md` und `systemPatterns.md`/`techContext.md` von „planned“ auf „scaffolded/umgesetzt“ aktualisiert; Build/Run/Test-Kommandos konkretisiert.

## Decisions in Flight

- Keine offenen Architektur-Entscheidungen aus dem Solution-Bootstrap mehr — beide Open Questions der Spec wurden entschieden (ein gemeinsames Testprojekt, ein Root-`.editorconfig`).

## Blockers / Questions

- `NU1903`-Sicherheitswarnung (`Microsoft.OpenApi` 2.0.0, transitiv über `Microsoft.AspNetCore.OpenApi` 10.0.9 in `TipSplitter.Api`) ist noch offen — Fix (Pin auf 2.7.5) wurde vorgeschlagen, vom Nutzer aber zurückgestellt. Kein Blocker für weitere Arbeit, aber vor Produktiv-Einsatz zu klären.
- Wie soll der HTTP-Endpunkt genau aussehen (Route, Verb, Request-/Response-Format)? → klären in der nächsten fachlichen Spec.
- Rundungsregel für Beträge pro Person (kaufmännisch? auf Cent? Rest an eine Person?) → klären in der nächsten fachlichen Spec.

## Next

1. `/specify` für den ersten fachlichen Use Case (Tip-Split-Endpunkt) ausführen.
2. Danach `/plan` bzw. direktes TDD: erster fehlschlagender Test mit `NotImplementedException`, dann Stopp bis „go“.
3. GitHub-Actions-CI-Workflow (`dotnet format --verify-no-changes`, `dotnet build`, `dotnet test`) einrichten.
4. `NU1903`-Fix (Microsoft.OpenApi auf 2.7.5 pinnen) bei Gelegenheit nachholen.

## Validation

- Done: SDD-Bootstrap (Memory Bank + Instructions + Architektur-Snapshot); Solution-Bootstrap (Projekte, Referenzen, `.editorconfig`, xUnit+Shouldly) — Spec in `done/`.
- Pending: erste fachliche Spec, erster roter TDD-Test, CI-Workflow, NU1903-Fix.
- Known issues: NU1903 (Microsoft.OpenApi 2.0.0, siehe Blockers).
