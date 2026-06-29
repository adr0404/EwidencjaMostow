USE master;
GO

-- Jeœli baza ju¿ istnieje, to j¹ usuwamy
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'EwidencjaMostow')
BEGIN
    ALTER DATABASE EwidencjaMostow SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EwidencjaMostow;
END
GO

-- Tworzymy now¹ bazê
CREATE DATABASE EwidencjaMostow;
GO

USE EwidencjaMostow;
GO

CREATE TABLE SLTypyKonstrukcji (
    TypKonstrukcjiID INT IDENTITY(1,1) PRIMARY KEY,
    NazwaTypu VARCHAR(100) NOT NULL
);

CREATE TABLE SLTypyPrzegladow (
    TypPrzegladuID INT IDENTITY(1,1) PRIMARY KEY,
    NazwaTypu VARCHAR(100) NOT NULL,
    WaznoscWMiesiacach INT NOT NULL CHECK (WaznoscWMiesiacach > 0),
    KosztEwidencyjny DECIMAL(10,2) NOT NULL CHECK (KosztEwidencyjny >= 0)
);

CREATE TABLE SLKategorieUprawnien (
    KategoriaUprawnienID INT IDENTITY(1,1) PRIMARY KEY,
    NazwaKategorii VARCHAR(100) NOT NULL
);

CREATE TABLE Obiekty (
    ObiektID INT IDENTITY(1,1) PRIMARY KEY,
    NazwaObiektu VARCHAR(150) NOT NULL,
    TypKonstrukcjiID INT NOT NULL,
    OpisLokalizacji VARCHAR(300) NOT NULL,
    StatusEksploatacyjny VARCHAR(50) NOT NULL
    CHECK (StatusEksploatacyjny IN ('Otwarty', 'Ograniczenie tona¿owe', 'Wy³¹czony z ruchu')),
    NosnoscEwidencyjna DECIMAL(8,2) NOT NULL CHECK (NosnoscEwidencyjna >= 0),

    CONSTRAINT UQ_Obiekt_NazwaLokalizacja UNIQUE (NazwaObiektu, OpisLokalizacji),

    CONSTRAINT FK_Obiekty_TypKonstrukcji FOREIGN KEY (TypKonstrukcjiID)
    REFERENCES SLTypyKonstrukcji(TypKonstrukcjiID)
);

CREATE TABLE Inspektorzy (
    InspektorID INT IDENTITY(1,1) PRIMARY KEY,
    Imie VARCHAR(50) NOT NULL,
    Nazwisko VARCHAR(50) NOT NULL,
    NrUprawnien VARCHAR(50) NOT NULL UNIQUE,
    KategoriaUprawnienID INT NOT NULL,
    Telefon VARCHAR(15) NOT NULL,
    Email VARCHAR(100) NOT NULL CHECK (Email LIKE '%@%'),

    CONSTRAINT FK_Inspektorzy_Kategoria FOREIGN KEY (KategoriaUprawnienID)
    REFERENCES SLKategorieUprawnien(KategoriaUprawnienID)
);

CREATE TABLE Przeglady (
    PrzegladID INT IDENTITY(1,1) PRIMARY KEY,
    ObiektID INT NOT NULL,
    InspektorID INT NOT NULL,
    TypPrzegladuID INT NOT NULL,
    DataWykonania DATE NOT NULL,
    DataWaznosci DATE NOT NULL,
    StanTechniczny INT NOT NULL CHECK (StanTechniczny BETWEEN 1 AND 5),
    ZatwierdzonaNosnosc DECIMAL(8,2) NOT NULL CHECK (ZatwierdzonaNosnosc >= 0),
    KosztRozliczeniowy DECIMAL(10,2) NOT NULL CHECK (KosztRozliczeniowy >= 0),
    DataWpisuDoSystemu DATETIME NOT NULL DEFAULT GETDATE(),
    Uwagi VARCHAR(500) NULL,

    CONSTRAINT CHK_DatyPrzegladu CHECK (DataWaznosci > DataWykonania),

    CONSTRAINT FK_Przeglady_Obiekt FOREIGN KEY (ObiektID)
    REFERENCES Obiekty(ObiektID),
    CONSTRAINT FK_Przeglady_Inspektor FOREIGN KEY (InspektorID)
    REFERENCES Inspektorzy(InspektorID),
    CONSTRAINT FK_Przeglady_Typ FOREIGN KEY (TypPrzegladuID)
    REFERENCES SLTypyPrzegladow(TypPrzegladuID)
);
GO

INSERT INTO SLTypyKonstrukcji (NazwaTypu) VALUES 
('Stalowa'), 
('¯elbetowa'), 
('Drewniana'), 
('Sprê¿ona');

INSERT INTO SLTypyPrzegladow (NazwaTypu, WaznoscWMiesiacach, KosztEwidencyjny) VALUES 
('Roczny podstawowy', 12, 1500.00), 
('Piêcioletni rozszerzony', 60, 5000.00), 
('DoraŸny (poawaryjny)', 6, 2500.00);

INSERT INTO SLKategorieUprawnien (NazwaKategorii) VALUES 
('Mostowa do kierowania robotami bez ograniczeñ'), 
('Mostowa do projektowania bez ograniczeñ'), 
('Budowlana ogólna');

-- PRZYK£ADOWE DANE TESTOWE (G£ÓWNE TABELE)

INSERT INTO Obiekty (NazwaObiektu, TypKonstrukcjiID, OpisLokalizacji, StatusEksploatacyjny, NosnoscEwidencyjna) VALUES 
('Most Grunwaldzki', 1, 'Wroc³aw, rzeka Odra, centrum', 'Otwarty', 40.00),
('K³adka Zwierzyniecka', 3, 'Wroc³aw, obok ZOO', 'Ograniczenie tona¿owe', 3.50),
('Wiadukt Kolejowy', 2, 'Kraków, ul. Grzegórzecka', 'Otwarty', 50.00);

INSERT INTO Inspektorzy (Imie, Nazwisko, NrUprawnien, KategoriaUprawnienID, Telefon, Email) VALUES 
('Jan', 'Kowalski', 'MAP/0123/PWOM/10', 1, '123456789', 'jan.kowalski@mosty.pl'),
('Anna', 'Nowak', 'MAP/4567/PWOM/15', 2, '987654321', 'anna.nowak@mosty.pl');

INSERT INTO Przeglady (ObiektID, InspektorID, TypPrzegladuID, DataWykonania, DataWaznosci, StanTechniczny, ZatwierdzonaNosnosc, KosztRozliczeniowy, Uwagi) VALUES 
(1, 1, 2, '2023-05-10', '2028-05-10', 4, 40.00, 5200.00, 'Stan dobry, widoczne drobne ogniska korozji.'),
(2, 2, 1, '2024-02-15', '2025-02-15', 3, 3.50, 1600.00, 'Konieczna wymiana desek pok³adu w przysz³ym roku.'),
(3, 1, 1, '2023-11-20', '2024-11-20', 5, 50.00, 1500.00, 'Stan idealny, brak uwag.');
GO