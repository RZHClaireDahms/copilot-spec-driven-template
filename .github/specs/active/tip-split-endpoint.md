**Status:** Draft

# Tip-Split-Endpunkt: Faire Aufteilung einer Restaurant-Rechnung

## Summary

Ein kleiner HTTP-Service, der eine Restaurant-Rechnung fair auf mehrere Personen aufteilt. Eingabe: Rechnungsbetrag, Trinkgeld-Prozentsatz, Anzahl Personen. Ausgabe: Betrag pro Person (inkl. Trinkgeld, auf 2 Nachkommastellen gerundet) und Gesamt-Trinkgeldbetrag. Der Service ist zustandslos und liefert das Ergebnis in unter einer Sekunde.

## Problem / Motivation

Eine Gruppe am Restauranttisch soll in Sekunden wissen, was jeder zahlt, ohne im Kopf zu rechnen. Nach dem Solution-Bootstrap (`Api → Application → Domain` steht) fehlt jetzt der erste fachliche Use Case: die eigentliche Split-Berechnung inklusive HTTP-Endpunkt. Diese Spec ist der Startpunkt für die TDD-Baby-Steps (erst Domain-Berechnung, dann Application-Service, dann Controller).

## Goals / Non-goals

**Goals**

- Reine, seiteneffektfreie Domänen-Logik in `TipSplitter.Domain`, die aus (`billAmount`, `tipPercent`, `peopleCount`) einen fairen Split berechnet.
- Application-Service in `TipSplitter.Application`, der die Domänen-Logik orchestriert und ein DTO für die Antwort bereitstellt.
- HTTP-Endpunkt in `TipSplitter.Api` (Controller-basiert), der Eingaben entgegennimmt, validiert und das Ergebnis als JSON zurückgibt.
- Faire Rundung ohne verlorene Cents: die Summe der pro-Person-Beträge ergibt exakt den Gesamtbetrag (Rechnung + Trinkgeld).
- Antwortzeit deutlich unter 1 s pro Request (typischerweise wenige Millisekunden).

**Non-goals**

- Kein Login / keine Authentifizierung / keine Autorisierung.
- Keine Persistenz (weder Datenbank noch Datei).
- Keine Währungsumrechnung, keine Multi-Currency-Behandlung — alle Beträge in einer einzigen (impliziten) Währungseinheit mit 2 Nachkommastellen.
- Keine Aufteilung nach unterschiedlichen Bestellungen pro Person („Item-based Split“).
- Keine UI / kein Frontend.
- Kein CI-Workflow (separate Spec/Task).

## Requirements

### 1. Domäne (`TipSplitter.Domain`)

- Reine Funktion / Service, keine Abhängigkeit auf ASP.NET Core oder Application.
- Eingaben:
  - `billAmount` (Rechnungsbetrag ohne Trinkgeld), `decimal`, `>= 0`. **`0` ist ausdrücklich erlaubt** und ergibt `0.00` pro Person.
  - `tipPercent` (Trinkgeld-Prozentsatz), `decimal`, `>= 0` (z. B. `10` für 10 %). **`0` ist ausdrücklich erlaubt**; negative Werte sind nicht zulässig.
  - `peopleCount` (Anzahl Personen), `int`, `>= 1`. **`0` und negative Werte sind nicht zulässig** — es wird nichts berechnet, sondern eine klare Fehlermeldung zurückgegeben.
- Berechnung:
  1. `tipAmount = round(billAmount * tipPercent / 100, 2, kaufmännisch)`.
  2. `total = billAmount + tipAmount`.
  3. `basePerPerson = floor(total * 100 / peopleCount) / 100` (kleinster gleicher Cent-Betrag).
  4. `remainderCents = round(total * 100) - basePerPerson * 100 * peopleCount`.
  5. **Der gesamte Restbetrag (`remainderCents / 100`) wird der ersten Person zugeschlagen**, alle anderen Personen zahlen `basePerPerson`. So geht die Summe exakt auf.
- Rückgabe: `tipAmount`, `total`, `perPerson` (Liste von `peopleCount` Beträgen, jeweils auf 2 Nachkommastellen; Reihenfolge stabil, Index 0 = „erste Person“).
- Ungültige Eingaben führen zu einer expliziten Domain-Exception mit klarer, für die API-Schicht auswertbarer Fehlermeldung (kein „silent default“, keine Berechnung):
  - `peopleCount <= 0` → Fehler „Anzahl Personen muss mindestens 1 sein.“
  - `tipPercent < 0` → Fehler „Trinkgeld-Prozentsatz darf nicht negativ sein.“
  - `billAmount < 0` → Fehler „Rechnungsbetrag darf nicht negativ sein.“

### 2. Application-Service (`TipSplitter.Application`)

- Referenziert nur `Domain`, keine ASP.NET-Core-Abhängigkeit.
- Nimmt ein einfaches Eingabe-DTO/Record entgegen, ruft die Domänen-Berechnung auf und mappt auf ein Antwort-DTO/Record.
- Reicht Domain-Exceptions durch (oder wickelt sie in eine anwendungsspezifische Exception) — die Übersetzung in HTTP-Statuscodes ist Aufgabe des Api-Layers.

### 3. HTTP-API (`TipSplitter.Api`)

- **Endpunkt (Vorschlag):** `POST /api/split`
- **Request-Body (JSON):**

  ```json
  {
    "billAmount": 87.5,
    "tipPercent": 10,
    "peopleCount": 3
  }
  ```

- **Response-Body (JSON, HTTP 200):**

  ```json
  {
    "billAmount": 87.5,
    "tipPercent": 10,
    "peopleCount": 3,
    "tipAmount": 8.75,
    "totalAmount": 96.25,
    "perPerson": [32.09, 32.08, 32.08]
  }
  ```

  Der Restcent von `0.01` liegt gemäß Rundungsregel bei der ersten Person (Index 0). Bei größeren Rest-Cent-Mengen liegen alle bei Index 0 (siehe Beispiel `100 / 0 % / 7 Personen` in den Acceptance Criteria).

- **Fehlerfälle → HTTP 400** mit maschinenlesbarem Fehlerobjekt (ProblemDetails o. Ä.):
  - `billAmount < 0`
  - `tipPercent < 0`
  - `peopleCount < 1`
  - fehlende / nicht parsebare Felder
- Der Controller ist dünn: Validierung + Delegation an den Application-Service + Mapping auf HTTP.

### 4. Nicht-funktionale Anforderungen

- **Performance:** Response-Zeit p95 < 100 ms auf einem üblichen Entwickler-Rechner; harte Obergrenze < 1 s wie im Ziel formuliert.
- **Genauigkeit:** Alle Beträge in `decimal`; niemals `float`/`double` für Geld.
- **Determinismus:** Gleiche Eingaben ⇒ exakt gleiche Ausgabe (keine zufällige Restverteilung).

## Acceptance Criteria (testable)

Kriterien sind so formuliert, dass jedes einzelne per TDD-Baby-Step als eigener Test entstehen kann.

### Domäne

- [ ] Bei `billAmount = 100`, `tipPercent = 0`, `peopleCount = 4` liefert die Berechnung `tipAmount = 0.00`, `totalAmount = 100.00`, `perPerson = [25.00, 25.00, 25.00, 25.00]`.
- [ ] Bei `billAmount = 100`, `tipPercent = 10`, `peopleCount = 4` liefert die Berechnung `tipAmount = 10.00`, `totalAmount = 110.00`, `perPerson = [27.50, 27.50, 27.50, 27.50]`.
- [ ] Bei `billAmount = 100`, `tipPercent = 0`, `peopleCount = 3` gilt `perPerson = [33.34, 33.33, 33.33]` — Rest-Cent liegt bei der ersten Person, `sum(perPerson) == 100.00`.
- [ ] Bei `billAmount = 100`, `tipPercent = 0`, `peopleCount = 7` gilt `perPerson = [14.32, 14.28, 14.28, 14.28, 14.28, 14.28, 14.28]` — die 4 Rest-Cents liegen **vollständig** bei der ersten Person (`14.28 + 0.04`), `sum(perPerson) == 100.00`.
- [ ] Bei `billAmount = 87.50`, `tipPercent = 10`, `peopleCount = 3` gilt `perPerson = [32.09, 32.08, 32.08]`, `sum(perPerson) == totalAmount == 96.25`.
- [ ] Für jede beliebige gültige Kombination gilt: `sum(perPerson) == totalAmount` (kein verlorener Cent).
- [ ] Für jede beliebige gültige Kombination trägt **Index 0** den vollen Rest: `perPerson[0] == basePerPerson + remainderCents/100`, alle übrigen Einträge sind exakt `basePerPerson`.
- [ ] Jeder Wert in `perPerson` hat genau 2 Nachkommastellen (bzw. ist als `decimal` mit Scale 2 darstellbar).
- [ ] Bei `billAmount = 0`, `tipPercent = 0`, `peopleCount = 4` gilt `tipAmount = 0.00`, `totalAmount = 0.00`, `perPerson = [0.00, 0.00, 0.00, 0.00]` (keine Exception).
- [ ] Bei `billAmount = 0`, `tipPercent = 15`, `peopleCount = 3` gilt `tipAmount = 0.00`, `totalAmount = 0.00`, `perPerson = [0.00, 0.00, 0.00]` (keine Exception).
- [ ] Bei `billAmount = 50`, `tipPercent = 0`, `peopleCount = 2` gilt `tipAmount = 0.00`, `totalAmount = 50.00`, `perPerson = [25.00, 25.00]` (0 % ist erlaubt).
- [ ] `peopleCount = 0` wirft eine Domain-Exception mit Meldung „Anzahl Personen muss mindestens 1 sein.“
- [ ] `peopleCount = -1` wirft eine Domain-Exception mit derselben Meldung.
- [ ] `billAmount = -0.01` wirft eine Domain-Exception mit Meldung „Rechnungsbetrag darf nicht negativ sein.“
- [ ] `tipPercent = -0.01` wirft eine Domain-Exception mit Meldung „Trinkgeld-Prozentsatz darf nicht negativ sein.“

### Application

- [ ] Der Application-Service liefert für gültige Eingaben ein Antwort-DTO, dessen Felder mit dem Domain-Ergebnis übereinstimmen.
- [ ] Ungültige Eingaben schlagen mit einer erkennbaren Exception fehl, die vom Api-Layer in HTTP 400 übersetzt werden kann.

### API

- [ ] `POST /api/split` mit gültigem JSON antwortet mit HTTP 200 und dem oben spezifizierten Response-Schema.
- [ ] `POST /api/split` mit `peopleCount = 0` antwortet mit HTTP 400.
- [ ] `POST /api/split` mit `billAmount = -1` antwortet mit HTTP 400.
- [ ] `POST /api/split` mit `tipPercent = -5` antwortet mit HTTP 400.
- [ ] `POST /api/split` mit fehlendem Feld antwortet mit HTTP 400.
- [ ] Eine typische Anfrage wird in < 1 s beantwortet (Smoke-Test reicht; kein formelles Lasttest-Setup).

### Quality Gates

- [ ] `dotnet build` läuft grün.
- [ ] `dotnet test` läuft grün (alle neuen Tests aus dieser Spec + bestehender `ToolchainSmokeTests`).
- [ ] `dotnet format --verify-no-changes` meldet keine Änderungen/Warnungen.

## Resolved Decisions

- **Validierung** (2026-07-28): `peopleCount <= 0` ist unzulässig und führt zu einer klaren Fehlermeldung — es wird nichts berechnet. `tipPercent < 0` ist unzulässig; `tipPercent == 0` ist erlaubt. `billAmount == 0` ist erlaubt und ergibt `0.00` pro Person; `billAmount < 0` ist unzulässig.
- **Rundungsstrategie** (2026-07-28): Überzählige Rundungs-Cents werden **vollständig der ersten Person (Index 0)** zugeschlagen, damit die Summe exakt aufgeht.

## Open Questions

- **Route/Verb:** Ist `POST /api/split` in Ordnung, oder soll ein anderer Pfad (`/tips/split`, `/split`, `/api/v1/split` …) verwendet werden?
- **Rundung des Trinkgelds:** Vorgeschlagen ist kaufmännische Rundung auf 2 Nachkommastellen. Alternativ: exakt (unrunded) und erst am Ende runden. Bestätigen?
- **Trinkgeld-Basis:** Prozent bezieht sich auf `billAmount` (netto). Passt das, oder gibt es Fälle mit „Trinkgeld auf inkl. Steuer/Service“?
- **Obergrenzen:** Gibt es sinnvolle obere Grenzen (z. B. `peopleCount <= 1000`, `billAmount <= 1_000_000`), die als Validierung erzwungen werden sollen?
- **Fehlerformat:** ProblemDetails (RFC 7807) oder ein eigenes einfaches Fehler-DTO?
- **OpenAPI/Swagger:** Soll der Endpunkt im bereits vorhandenen OpenAPI-Setup (`Microsoft.AspNetCore.OpenApi`) sichtbar/dokumentiert sein? (Vermutlich ja, gehört aber ggf. in eine eigene Task.)

## Out of Scope

- Persistenz jeglicher Art (kein Speichern von Requests/Ergebnissen).
- Authentifizierung / Autorisierung / Rate-Limiting.
- Währungsumrechnung, Locale-abhängige Formatierung, Zahl-Formate mit Komma vs. Punkt.
- Aufteilung nach individuellen Bestellungen pro Person („Item-based Split“, Steuern pro Position).
- Frontend/UI.
- CI-Workflow, Deployment, Hosting-Konfiguration (separate Specs/Tasks).
- Fix der offenen `NU1903`-Warnung (`Microsoft.OpenApi` 2.0.0) — bleibt bewusst zurückgestellt und ist nicht Teil dieser Spec.
