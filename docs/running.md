# Uruchamianie

> [!IMPORTANT]  
> Do uruchomienia serwera API wymagany jest [ASP.NET Core Runtime 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0#runtime-aspnetcore-10.0.10).

Serwer API można uruchomić w dwóch środowiskach:

- produkcyjny (production)
- rozwojowy (development)

## Środowisko produkcyjne (production)

1. Pobierz najnowsze wydanie serwera API [klikając tutaj](https://github.com/heapyxyz/kamsoft-recrutation/releases/latest). Są 4 różne wersje wydania do pobrania:

- Windows:
  - `KamsoftApi-win-x64.zip`
  - `KamsoftApi-sc-win-x64.zip` (wydanie self-contained - nie wymaga **ASP.NET Core Runtime 10.0**)
- Linux:
  - `KamsoftApi-linux-x64.tar.gz`
  - `KamsoftApi-sc-linux-x64.tar.gz` (wydanie self-contained - nie wymaga **ASP.NET Core Runtime 10.0**)

2. Wypakuj pobrane wydanie.
3. Uruchom plik `Api.exe` (w przypadku systemu Linux plik `Api`). Serwer API jest teraz włączony pod adresem http://localhost:5000. Strona główna zawiera SwaggerUI - narzędzie do generowania dokumentacji oraz testowania endpointów.

## Środowisko rozwojowe (development)

1. Sklonuj cały projekt za pomocą:

```
git clone https://github.com/heapyxyz/kamsoft-recrutation
```

2. Wejdź w folder projektu (`kamsoft-recrutation`).
3. Użyj komendy `dotnet run --project Api` w terminalu. Serwer API jest teraz włączony pod adresem http://localhost:5147. Strona główna zawiera SwaggerUI - narzędzie do generowania dokumentacji oraz testowania endpointów.
