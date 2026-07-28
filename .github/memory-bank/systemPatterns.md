# System-Patterns

## Architektur-Stil

- **Einfaches Schichtenmodell** für eine kleine HTTP-API — **umgesetzt** als Solution `TipSplitter.slnx` mit vier Projekten:
  - **Presentation** (`src/TipSplitter.Api`) — ASP.NET-Core-Controller, Request-/Response-DTOs, DI-Verdrahtung, Fehler-Mapping.
  - **Application** (`src/TipSplitter.Application`) — Use-Case-Orchestrierung für die Aufteilung; kennt weder HTTP noch ASP.NET Core.
  - **Domain** (`src/TipSplitter.Domain`) — reine Rechenlogik (z. B. `TipSplitCalculator`), Value Objects, keinerlei Framework-Abhängigkeiten.
  - **Tests** (`tests/TipSplitter.Tests`) — ein gemeinsames xUnit+Shouldly-Testprojekt für Domain + Application.
- Datenfluss ist strikt einwegs: `Api → Application → Domain`. Rückwärtige Abhängigkeiten sind nicht erlaubt (über Projekt-Referenzen technisch erzwungen).

## Zentrale Entscheidungen

- **TDD Baby-Steps mit Freigabe-Gate:** Der Test wird zuerst geschrieben und muss initial mit `NotImplementedException` (o. ä.) fehlschlagen. Danach **Stop**, bis der Nutzer ein explizites „go“ gibt. Implementierung nur so weit, dass **genau dieser eine Test** grün wird.
- **Kein Kommentar-Rauschen** im generierten Code (außer explizit gewünscht).
- **Formatter- und Test-Gate nach jedem Task:** `dotnet format` und `dotnet test` sind Pflicht, Warnings/Errors werden im selben Change-Set gefixt.
- **Stateless Service:** keine Persistenz, kein Zustand über Requests hinaus — vereinfacht Tests, Deployment und Reasoning.
- **Controller-basiert** statt Minimal APIs (bewusste Entscheidung des Nutzers).

## Patterns

- **DTO an der Presentation-Grenze:** Controller nehmen ein Request-DTO entgegen und liefern ein Response-DTO zurück; Domain-Typen verlassen die Application-Schicht nicht.
- **Reine Domain-Funktionen:** `TipSplitCalculator` und verwandte Bausteine sind deterministisch, ohne I/O, ohne Zeit-/Zufalls-Abhängigkeit — damit trivial per Unit-Test abdeckbar.
- **Validierung nah am Rand:** Eingangsvalidierung (z. B. Personenanzahl > 0, Beträge ≥ 0) geschieht möglichst früh (Controller oder Application-Grenze), Domain rechnet auf validen Eingaben.
- **Fehler als klare HTTP-Antworten:** Ungültige Eingaben führen zu `400 Bad Request` mit eindeutiger Problembeschreibung; keine 500er für erwartbare Fehleingaben.
