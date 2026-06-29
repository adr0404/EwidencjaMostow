# Ewidencja Mostów 🌉

Aplikacja WPF do zarządzania ewidencją mostów, kładek oraz rejestrowania przeglądów i inspektorów budowlanych. Projekt wykorzystuje Entity Framework do komunikacji z bazą danych.

## 🛠️ Wymagania środowiskowe
* Visual Studio (z zainstalowanym .NET desktop development)
* SDK dla **.NET 9.0** (lub wyższej)
* Microsoft SQL Server

## 🚀 Uruchomienie projektu

### 1. Inicjalizacja bazy danych
Struktura bazy oraz dane testowe znajdują się w dołączonym skrypcie SQL:
* Uruchom skrypt **`init.sql`** na swojej lokalnej instancji SQL Server (np. z poziomu SSMS). Skrypt utworzy bazę `EwidencjaMostow` i zasili tabele początkowymi rekordami.

### 2. Konfiguracja połączenia
Domyślnie aplikacja próbuje łączyć się z domyślną lokalną instancją serwera SQL (`Server=.`).
* Jeśli Twój serwer ma inną nazwę (np. `.\SQLEXPRESS` lub `localhost\MSSQLSERVER01`), otwórz plik **`Models/EwidencjaMostowContext.cs`**.
* Znajdź metodę `OnConfiguring` (około 30. linijki) i zaktualizuj wpis w funkcji `UseSqlServer`:
  ```csharp
  // Przykład zmiany nazwy serwera:
  optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=EwidencjaMostow;Trusted_Connection=True;Encrypt=False;");
