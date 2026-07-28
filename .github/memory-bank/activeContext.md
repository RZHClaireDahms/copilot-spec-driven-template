# Aktiver Kontext

Last updated: 2026-07-28
Current branch: (unbekannt — im Zweifel `main`)
Current phase: Solution-Bootstrap abgeschlossen → bereit für erste fachliche Spec (TDD)

## Now

Solution + Projektstruktur (`Api`/`Application`/`Domain`/`Tests`) stehen, bauen und testen grün. Als Nächstes: erste fachliche Spec für den Tip-Split-Endpunkt via `/specify`.

## Active Spec

- Spec: `.github/specs/active/tip-split-endpoint.md` (Status: In Progress)
- Aktueller Task: TDD-Baby-Steps gemäß `/plan` — Test **A1** (`TipSplitCalculatorTests.Split_100Bill_0PercentTip_4People_ReturnsEqualShares`) ist geschrieben und rot (`NotImplementedException`). Warte auf „go“ vom Nutzer für die minimale Implementierung.
- Bestätigter HTTP-Contract: `POST /split`, Request `{amount, tipPercent, people}`, Response `{perPerson, totalTip}` (kein Echo der Eingaben).
- Akzeptanzkriterien im Fokus: Domäne-Abschnitt der Spec (Phase A des Plans), danach Application (Phase B), dann Api (Phase C).

## Changed Recently

- 2026-07-28: `/plan` für `tip-split-endpoint.md` erstellt; Nutzer hat HTTP-Contract-Abweichungen bestätigt (Route `/split` statt `/api/split`, Felder `amount`/`tipPercent`/`people`, Response nur `perPerson`+`totalTip`). Spec aktualisiert (Resolved Decisions, Requirements, Acceptance Criteria) und von `backlog/` nach `active/` verschoben.
- 2026-07-28: Erster TDD-Baby-Step gestartet — `TipSplitCalculator.Split(...)` in `TipSplitter.Domain` als Skeleton mit `NotImplementedException` angelegt, Test A1 geschrieben und als rot verifiziert (`dotnet test`).
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

1. „go“ vom Nutzer abwarten, dann minimale Implementierung für Test A1 (`TipSplitCalculator.Split` happy path even).
2. Weitere Domain-Baby-Steps (A2–A11) gemäß Plan, dann Application (B1–B2), dann Api (C1–C6).
3. GitHub-Actions-CI-Workflow (`dotnet format --verify-no-changes`, `dotnet build`, `dotnet test`) einrichten.
4. `NU1903`-Fix (Microsoft.OpenApi auf 2.7.5 pinnen) bei Gelegenheit nachholen.

## Validation

- Done: SDD-Bootstrap (Memory Bank + Instructions + Architektur-Snapshot); Solution-Bootstrap (Projekte, Referenzen, `.editorconfig`, xUnit+Shouldly) — Spec in `done/`.
- Pending: erste fachliche Spec, erster roter TDD-Test, CI-Workflow, NU1903-Fix.
- Known issues: NU1903 (Microsoft.OpenApi 2.0.0, siehe Blockers).
