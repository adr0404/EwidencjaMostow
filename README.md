# Ewidencja Mostów 🌉

Aplikacja WPF do zarządzania ewidencją mostów, kładek oraz rejestrowania przeglądów i inspektorów budowlanych. Projekt wykorzystuje Entity Framework do komunikacji z bazą danych.

## 🛠️ Wymagania środowiskowe
* Visual Studio (z zainstalowanym .NET desktop development)
* Microsoft SQL Server

## 🚀 Uruchomienie projektu

### 1. Inicjalizacja bazy danych
Struktura bazy oraz dane testowe znajdują się w dołączonym skrypcie SQL:
* Uruchom skrypt **`init.sql`** na swojej lokalnej instancji SQL Server (np. z poziomu SSMS). Skrypt utworzy bazę `EwidencjaMostow` i zasili tabele początkowymi rekordami.

### 2. Konfiguracja połączenia
* Domyślnie aplikacja próbuje łączyć się z lokalnym serwerem SQL. W razie potrzeby zaktualizuj *Connection String* w konfiguracji kontekstu bazy danych (`EwidencjaMostowContext`).

### 3. Budowanie i start
* Otwórz rozwiązanie **`MostyApp.sln`** w Visual Studio, skompiluj projekt i uruchom aplikację (`F5`).
