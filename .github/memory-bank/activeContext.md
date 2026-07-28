# Aktiver Kontext

Last updated: 2026-07-28
Current branch: (unbekannt — im Zweifel `main`)
Current phase: Onboarding abgeschlossen → warte auf erste Spec / erstes TDD-Ticket

## Now

SDD-Grundgerüst ist bootstrapped. Als Nächstes soll die erste Spec für den **Tip-Split-Endpunkt** (z. B. `POST /split`) angelegt werden, gefolgt von der Anlage von Projekt-Skelett + `.editorconfig`.

## Active Spec

- Spec: — (noch keine)
- Aktueller Task: erste Spec via `/specify` erstellen
- Akzeptanzkriterien im Fokus: t. b. d. mit der ersten Spec

## Changed Recently

- 2026-07-28: `/setupSpecs` durchgeführt — `DocLanguage=German`, Projektziel „Tip Splitter“ gesetzt, Style- und Workflow-Präferenzen (TDD Baby-Steps, `dotnet format`- und `dotnet test`-Gate) in `.github/copilot-instructions.md` ergänzt.
- 2026-07-28: Architektur-Snapshot mit vorgeschlagenem Layout (`TipSplitter.Api` / `.Application` / `.Domain` + `TipSplitter.Tests`) befüllt — Status: **geplant, noch nicht angelegt**.
- 2026-07-28: Memory Bank (`projectbrief.md`, `techContext.md`, `systemPatterns.md`) auf Deutsch mit projektspezifischem Inhalt versehen.

## Decisions in Flight

- Konkrete Projekt- und Ordnernamen aus dem Architektur-Snapshot sind ein **Vorschlag** und werden mit der ersten Implementierungs-Spec bestätigt.

## Blockers / Questions

- Wie soll der HTTP-Endpunkt genau aussehen (Route, Verb, Request-/Response-Format)? → klären in der ersten Spec.
- Rundungsregel für Beträge pro Person (kaufmännisch? auf Cent? Rest an eine Person?) → klären in der ersten Spec.

## Next

1. `/specify` für den ersten Use Case (Tip-Split-Endpunkt) ausführen.
2. In der ersten Implementierungs-Runde: `.editorconfig` (nach .NET-Best-Practices) und Projekt-Skelett anlegen.
3. GitHub-Actions-CI-Workflow (`dotnet format --verify-no-changes`, `dotnet build`, `dotnet test`) einrichten.

## Validation

- Done: SDD-Bootstrap (Memory Bank + Instructions + Architektur-Snapshot Entwurf).
- Pending: `.editorconfig`, Projekt-Skelett, erste Spec, erster roter TDD-Test, CI-Workflow.
- Known issues: keine.
