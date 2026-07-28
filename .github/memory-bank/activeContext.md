# Aktiver Kontext

Last updated: 2026-07-28
Current branch: (unbekannt — im Zweifel `main`)
Current phase: Erster fachlicher Use Case (Tip-Split-Endpunkt) implementiert und abgeschlossen → bereit für nächste Spec

## Now

`tip-split-endpoint.md` ist vollständig umgesetzt (Domain, Application, Api) und nach `.github/specs/done/` verschoben. 19/19 Tests grün, `dotnet format --verify-no-changes` sauber. Als Nächstes: neue Spec (`/specify`) oder Follow-ups (CI-Workflow, NU1903-Fix, OpenAPI-Sichtbarkeit) angehen.

## Active Spec

- Spec: — (keine aktiv; `tip-split-endpoint.md` ist nach `done/` verschoben, Status: Implemented)
- Aktueller Task: keiner offen; wartet auf nächste Nutzeranweisung (`/specify` für neuen Use Case oder Follow-up-Task).
- Umgesetzter HTTP-Contract: `POST /split`, Request `{amount, tipPercent, people}`, Response `{perPerson, totalTip}` (kein Echo der Eingaben).

## Changed Recently

- 2026-07-28: `tip-split-endpoint.md` vollständig via TDD-Baby-Steps umgesetzt (Phasen A–D). Domain: `TipSplitCalculator` (Berechnung, kaufmännische Rundung, Restcent-Verteilung an Index 0, Validierung via `TipSplitValidationException`). Application: `TipSplitService`/`TipSplitRequest`/`TipSplitResponse` delegieren an die Domain. Api: `SplitController` (`POST /split`), `SplitRequest`/`SplitResponse` mit nullable Feldern für Pflichtfeld-Validierung, `TipSplitValidationException` → HTTP 400 via `ProblemDetails`. 19/19 Tests grün (`ToolchainSmokeTests` + 11 Domain- + 2 Application- + 6 Api-Tests via `WebApplicationFactory<Program>`), `dotnet format --verify-no-changes` sauber.
- 2026-07-28: `TipSplitter.Tests.csproj` um `Microsoft.AspNetCore.Mvc.Testing` und eine Projektreferenz auf `TipSplitter.Api` erweitert; `Program.cs` um `public partial class Program;` ergänzt (Voraussetzung für `WebApplicationFactory<Program>`).
- 2026-07-28: Spec nach `done/` verschoben (Status: Implemented), Acceptance Criteria abgehakt, Resolved Decisions/Open Questions aktualisiert. Architektur-Snapshot in `.github/copilot-instructions.md` sowie `systemPatterns.md`/`techContext.md` synchronisiert (Modul-/Namensdetails, Api-Integrationstest-Infrastruktur).
- 2026-07-28: `/plan` für `tip-split-endpoint.md` erstellt; Nutzer hat HTTP-Contract-Abweichungen bestätigt (Route `/split` statt `/api/split`, Felder `amount`/`tipPercent`/`people`, Response nur `perPerson`+`totalTip`).
- 2026-07-28: `/setupSpecs` durchgeführt — `DocLanguage=German`, Projektziel „Tip Splitter“ gesetzt, Style- und Workflow-Präferenzen (TDD Baby-Steps, `dotnet format`- und `dotnet test`-Gate) in `.github/copilot-instructions.md` ergänzt.
- 2026-07-28: Spec `solution-bootstrap.md` erstellt, umgesetzt und nach `.github/specs/done/` verschoben (Status: Implemented).
- 2026-07-28: Solution `TipSplitter.slnx` mit vier Projekten angelegt: `src/TipSplitter.Domain`, `src/TipSplitter.Application`, `src/TipSplitter.Api` (Controller-basiert), `tests/TipSplitter.Tests` (xUnit + Shouldly). Referenzen gemäß Architektur-Boundaries gesetzt (`Api → Application, Domain`; `Application → Domain`; `Tests → Domain, Application`).
- 2026-07-28: Root-`.editorconfig` nach .NET-Best-Practices angelegt (inkl. Override-Block für `tests/**`); `dotnet format --verify-no-changes` läuft grün.
- 2026-07-28: `dotnet build` und `dotnet test` laufen grün (1 Platzhalter-Test `ToolchainSmokeTests` mit Shouldly-Assertion).
- 2026-07-28: Architektur-Snapshot in `.github/copilot-instructions.md` und `systemPatterns.md`/`techContext.md` von „planned“ auf „scaffolded/umgesetzt“ aktualisiert; Build/Run/Test-Kommandos konkretisiert.

## Decisions in Flight

- Keine offenen Architektur-Entscheidungen — HTTP-Contract der Tip-Split-Spec ist entschieden und umgesetzt (siehe `done/tip-split-endpoint.md`, Resolved Decisions).

## Blockers / Questions

- `NU1903`-Sicherheitswarnung (`Microsoft.OpenApi` 2.0.0, transitiv über `Microsoft.AspNetCore.OpenApi` 10.0.9 in `TipSplitter.Api`) ist noch offen — Fix (Pin auf 2.7.5) wurde vorgeschlagen, vom Nutzer aber zurückgestellt. Kein Blocker für weitere Arbeit, aber vor Produktiv-Einsatz zu klären.
- Obergrenzen für `amount`/`people` (z. B. Maximalwerte) sind bewusst nicht validiert — offen für eine künftige Spec.
- OpenAPI/Swagger-Sichtbarkeit des `/split`-Endpunkts wurde nicht verifiziert — offen für eine künftige Task.

## Next

1. Nächste fachliche Spec via `/specify` (falls weitere Use Cases geplant sind) oder Follow-up-Tasks priorisieren.
2. GitHub-Actions-CI-Workflow (`dotnet format --verify-no-changes`, `dotnet build`, `dotnet test`) einrichten.
3. `NU1903`-Fix (Microsoft.OpenApi auf 2.7.5 pinnen) bei Gelegenheit nachholen.
4. Optional: OpenAPI-Sichtbarkeit für `/split` prüfen/dokumentieren, Obergrenzen für Eingaben klären.

## Validation

- Done: SDD-Bootstrap (Memory Bank + Instructions + Architektur-Snapshot); Solution-Bootstrap (Projekte, Referenzen, `.editorconfig`, xUnit+Shouldly); Tip-Split-Endpunkt (Domain + Application + Api, 19/19 Tests grün, Formatter sauber) — beide Specs in `done/`.
- Pending: CI-Workflow, NU1903-Fix, OpenAPI-Sichtbarkeit, Obergrenzen-Validierung, ggf. weitere fachliche Specs.
- Known issues: NU1903 (Microsoft.OpenApi 2.0.0, siehe Blockers).
