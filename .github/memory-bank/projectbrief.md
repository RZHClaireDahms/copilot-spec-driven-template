# Projekt-Steckbrief

## Mission

**Tip Splitter** ist ein kleiner HTTP-Service, der eine Restaurant-Rechnung fair auf mehrere Personen aufteilt.

- **Eingaben:** Rechnungsbetrag, Trinkgeld-Prozentsatz, Personenanzahl
- **Ausgaben:** Betrag pro Person und Gesamt-Trinkgeld
- **Bewusst außen vor:** keine Datenbank, kein Frontend, kein Login/Authentifizierung

## Primäre Nutzer

- Restaurantgäste (indirekt — der Service wird von einem Client konsumiert, den Gäste am Tisch nutzen)
- Konsumenten der API im weiteren Sinne: kleine Tools/Skripte, die eine faire Aufteilung berechnen wollen

## Erfolgskriterien

- Die API liefert für valide Eingaben eine korrekte Aufteilung (Betrag pro Person + Gesamt-Trinkgeld) mit sauberer Rundung.
- Ungültige Eingaben (z. B. 0 Personen, negativer Betrag, negativer Prozentsatz) werden mit einer klaren HTTP-Fehlerantwort abgewiesen.
- Alle funktionalen Anforderungen sind per **TDD in Baby-Steps** entstanden: zu jeder Verhaltenszeile existiert genau ein Test, der zuerst rot war.
- `dotnet format` läuft warnungs- und fehlerfrei; `dotnet test` ist grün.
- Kein persistenter Zustand, keine externen Abhängigkeiten außerhalb .NET-Standard-/ASP.NET-Core-Paketen.
