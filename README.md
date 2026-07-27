# Generyczny parser danych przesyłanych przez API (.NET / C#)

## Uruchamianie

Jeśli chcesz uruchomić ten serwer API, [kliknij tutaj](./docs/running.md).

## Cel zadania

Celem zadania jest weryfikacja umiejętności projektowania bezpiecznych i rozszerzalnych punktów końcowych API, operowania na typach MIME oraz praktycznej implementacji logiki parsującej dla formatów o zmiennej strukturze przy użyciu języka C#.

## Opis zadania

Twoim zadaniem jest stworzenie endpointu HTTP w technologii [ASP.NET](http://asp.net/) Core Web API / Minimal APIs, który przyjmuje ustandaryzowany ładunek (payload) w formacie JSON, dekoduje i przetwarza przesłane dane, a następnie zwraca wynik w ujednoliconej formie.

## Wymagania techniczne

- Technologia: C# (.NET 8 lub nowszy).
- Metoda i ścieżka: endpoint działa jako `POST /api/v1/parse-content`.
- Format wejściowy: żądanie musi zawierać nagłówek `Content-Type: application/json`.
- Payload powinien przyjmować następujący format:
  - `type` — określa typ zawartości znajdującej się w polu content (rekomendowane użycie typu enum).
  - `content` — zawiera surowe dane zakodowane algorytmem Base64.

```json
{
  "type": "CSV" | "INTERNAL_JSON",
  "content": "..."
}
```

## Logika biznesowa (implementacja parsera)

Po odebraniu żądania aplikacja powinna wykonać następujące kroki:

1. Zweryfikować, czy przesłany type jest obsługiwany (w przypadku błędu zwrócić odpowiedni kod HTTP, np. `400 Bad Request`).
2. Zdekodować ciąg znaków z Base64 do postaci zwykłego tekstu (string).
3. W zależności od typu wykonać parsowanie danych:

- Dla typu **CSV**: sparsować tekst jako wartości rozdzielane przecinkami i przekształcić je do kolekcji obiektów (można użyć biblioteki zewnętrznej lub napisać własną logikę).
- Dla typu **INTERNAL_JSON**: przeprowadzić walidację i deserializację wewnętrznego formatu JSON (za pomocą `System.Text.Json` lub `Newtonsoft.Json`).

4. Zwrócić odpowiedź w formacie JSON, która zawiera status operacji, liczbę przetworzonych wierszy/obiektów oraz sparsowane dane w ujednoliconej strukturze.

## Sposób oddania zadania

1. Projekt musi zostać opublikowany w publicznym systemie kontroli wersji (np. GitHub, GitLab, Bitbucket).
2. Jako rozwiązanie należy dostarczyć bezpośredni link do repozytorium z kodem źródłowym rozwiązania (plik `.sln` oraz projekty)
3. W pliku `README.md` powinna znaleźć się krótka instrukcja, jak uruchomić aplikację lokalnie (np. za pomocą `dotnet run`).
