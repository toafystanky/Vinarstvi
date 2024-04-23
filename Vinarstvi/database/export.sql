-- VINARSTVI MANAGER - Franziska Wolf
-- franziska.wolf@windowslive.com
-- 731731270

START TRANSACTION;


-- Vytvoření databáze
CREATE DATABASE IF NOT EXISTS Vinarstvi;
USE Vinarstvi;

--

-- Tabulka Vinařství
CREATE TABLE Vinarstvi (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nazev VARCHAR(255) NOT NULL UNIQUE,
    adresa VARCHAR(255) UNIQUE,
    telefon VARCHAR(255) UNIQUE,
    email VARCHAR(255) UNIQUE
);

-- Tabulka Vinice
CREATE TABLE Vinice (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nazev VARCHAR(255) NOT NULL UNIQUE,
    rozloha FLOAT,
    rok_zalozeni DATETIME,
    vinarstvi_id INT,
    FOREIGN KEY (vinarstvi_id) REFERENCES Vinarstvi(id) ON DELETE CASCADE
);

-- Tabulka Odběratel
CREATE TABLE Odberatel (
    id INT PRIMARY KEY AUTO_INCREMENT,
    jmeno VARCHAR(255) NOT NULL,
    prijmeni VARCHAR(255) NOT NULL,
    adresa VARCHAR(255),
    email VARCHAR(255) UNIQUE,
    telefon VARCHAR(255) UNIQUE
);


-- Tabulka Víno
CREATE TABLE Vino (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nazev VARCHAR(255) NOT NULL UNIQUE,
    rocnik INT,
    mnozstvi DECIMAL(10,2),
    cena DECIMAL(10,2),
    vinice_id INT,
    FOREIGN KEY (vinice_id) REFERENCES Vinice(id) ON DELETE CASCADE
);

-- Tabulka Odrůda
CREATE TABLE Odruda (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nazev VARCHAR(255) NOT NULL UNIQUE,
    barva ENUM('Bílé', 'Červené', 'Růžové'),
    popis TEXT
);

-- Vazební tabulka pro M:N vazbu mezi Víno a Odrůda
CREATE TABLE Vino_Odruda (
    vino_id INT,
    odruda_id INT,
    PRIMARY KEY (vino_id, odruda_id),
    FOREIGN KEY (vino_id) REFERENCES Vino(id) ON DELETE CASCADE,
    FOREIGN KEY (odruda_id) REFERENCES Odruda(id) ON DELETE CASCADE
);

-- Tabulka Zakázka
CREATE TABLE Zakazka (
    id INT PRIMARY KEY AUTO_INCREMENT,
    datum_objednani DATETIME,
    cena DECIMAL(10,2),
    vino_id INT,
    odberatel_id INT,
    FOREIGN KEY (vino_id) REFERENCES Vino(id) ON DELETE CASCADE,
    FOREIGN KEY (odberatel_id) REFERENCES Odberatel(id) ON DELETE CASCADE
);


-- Databázový pohled 1: Seznam vín s jejich odruhami
CREATE VIEW View_Vino_Odruda AS
SELECT Vino.id AS Vinoid, Vino.Nazev AS NazevVina, GROUP_CONCAT(Odruda.Nazev) AS Odrudy
FROM Vino
LEFT JOIN Vino_Odruda ON Vino.id = Vino_Odruda.Vino_id
LEFT JOIN Odruda ON Vino_Odruda.odruda_id = Odruda.id
GROUP BY Vino.id;

-- Databázový pohled 2: Seznam vín a jejich zakázek
CREATE VIEW View_Vino_Zakazka AS
SELECT Vino.id AS Vinoid, Vino.Nazev AS NazevVina, COUNT(Zakazka.id) AS PocetZakazek
FROM Vino
LEFT JOIN Zakazka ON Vino.id = Zakazka.Vino_id
GROUP BY Vino.id;

-- Insert data into Vinarstvi
INSERT INTO Vinarstvi (nazev, adresa, telefon, email) VALUES
    ('Chateau Bordeaux', '123 Vineyard Lane, Bordeaux', '+33 1 23 45 67 89', 'info@chateaubordeaux.com'),
    ('Napa Valley Estates', '456 Winery Road, Napa Valley', '+1 555 123 4567', 'info@napavalleyestates.com'),
    ('Tuscany Vineyards', '789 Grapes Street, Tuscany', '+39 055 123 4567', 'info@tuscanyvineyards.com'),
    ('Rioja Cellars', '101 Tempranillo Avenue, Rioja', '+34 123 456 789', 'info@riojacellars.com'),
    ('Hunter Valley Wines', '246 Shiraz Lane, Hunter Valley', '+21 2 9876 5432', 'info@huntervalleywines.com'),
     ('Vineyard Château', '789 Grapes Avenue, Bordeaux', '+23 1 23 45 67 89', 'info@vineyardchateau.com'),
    ('Sonoma Valley Wines', '101 Cabernet Street, Sonoma Valley', '+1 555 987 6543', 'info@sonomawines.com'),
    ('Chianti Heritage', '456 Sangiovese Road, Tuscany', '+39 055 987 6543', 'info@chiantiheritage.com'),
    ('Spanish Flavors Winery', '789 Tempranillo Lane, Rioja', '+34 987 654 321', 'info@spanishflavors.com'),
    ('Barossa Valley Cellars', '246 Shiraz Street, Barossa Valley', '+56 2 1234 5678', 'info@barossacellars.com');

-- Insert data into Vinice
INSERT INTO Vinice (nazev, rozloha, rok_zalozeni, vinarstvi_id) VALUES
    ('Vineyard A', 50.5, '2015-01-01', 1),
    ('Vineyard B', 30.2, '2010-01-01', 2),
    ('Vineyard C', 40.8, '2012-01-01', 3),
    ('Vineyard D', 60.0, '2018-01-01', 4),
    ('Vineyard E', 35.5, '2016-01-01', 5),
     ('Vineyard F', 45.3, '2014-01-01', 1),
    ('Vineyard G', 55.7, '2013-01-01', 2),
    ('Vineyard H', 33.6, '2011-01-01', 3),
    ('Vineyard I', 28.9, '2017-01-01', 4),
    ('Vineyard J', 50.0, '2019-01-01', 5);

-- Insert data into Odberatel
INSERT INTO Odberatel (jmeno, prijmeni, adresa, email, telefon) VALUES
    ('John', 'Doe', '123 Main Street, Cityville', 'john.doe@example.com', '+1 525 111 2233'),
    ('Jane', 'Smith', '456 Vineyard Street, Winetown', 'jane.smith@example.com', '+44 20 1234 5678'),
    ('Mario', 'Rossi', '789 Tuscany Square, Florence', 'mario.rossi@example.com', '+31 333 123 4567'),
    ('Elena', 'Gomez', '101 Rioja Boulevard, Logroño', 'elena.gomez@example.com', '+34 666 789 0123'),
    ('Olivia', 'Johnson', '246 Grapevine Road, Hunter Valley', 'olivia.johnson@example.com', '+61 2 1176 5432'),
        ('Michael', 'Williams', '789 Red Wine Lane, Bordeaux', 'michael.williams@example.com', '+33 1 11 22 33 44'),
    ('Sophie', 'Miller', '101 White Wine Street, Sonoma Valley', 'sophie.miller@example.com', '+1 555 555 5515'),
    ('Luigi', 'Bianchi', '456 Vine Street, Tuscany', 'luigi.bianchi@example.com', '+39 333 567 8901'),
    ('Isabella', 'Martinez', '789 Grape Square, Rioja', 'isabella.martinez@example.com', '+31 666 123 4567'),
    ('Jack', 'Thompson', '246 Cellar Road, Barossa Valley', 'jack.thompson@example.com', '+61 2 9876 5432');



-- Insert data into Vino
INSERT INTO Vino (nazev, rocnik, mnozstvi, cena, vinice_id) VALUES
    ('Merlot Reserve', 2017, 100.5, 25.99, 1),
    ('Cabernet Sauvignon', 2019, 75.2, 32.49, 2),
    ('Chianti Classico', 2016, 120.8, 19.99, 3),
    ('Rioja Gran Reserva', 2015, 90.0, 45.99, 4),
    ('Shiraz Special Blend', 2018, 55.5, 28.75, 5),
        ('Cabernet Franc', 2016, 88.3, 28.99, 1),
    ('Chardonnay Reserve', 2021, 65.8, 22.49, 2),
    ('Super Tuscan', 2014, 110.2, 35.99, 3),
    ('Garnacha Rosé', 2020, 75.0, 18.75, 4),
    ('Barossa Shiraz', 2017, 40.5, 42.50, 5);

-- Insert data into Odruda
INSERT INTO Odruda (nazev, barva, popis) VALUES
    ('Merlot', 'Červené', 'A red wine grape variety with deep color and soft tannins.'),
    ('Cabernet Sauvignon', 'Červené', 'Known for its robust flavor and aging potential.'),
    ('Sangiovese', 'Bílé', 'Principal grape variety used to make Chianti wines.'),
    ('Tempranillo', 'Červené', 'Main grape variety in Rioja wines.'),
    ('Shiraz', 'Růžové', 'A dark-skinned grape variety with bold flavors.'),
      ('Cabernet Franc', 'Červené', 'Known for its peppery and herbaceous notes.'),
    ('Chardonnay', 'Bílé', 'A versatile white wine with buttery and oaky flavors.'),
    ('Sangiovese Merlot Blend', 'Červené', 'A delightful blend showcasing the best of both grapes.'),
    ('Garnacha', 'Růžové', 'A refreshing and fruity rosé wine.'),
    ('Barossa Shiraz', 'Červené', 'Rich and full-bodied red wine with bold flavors.');

-- Insert data into Vino_Odruda
INSERT INTO Vino_Odruda (vino_id, odruda_id) VALUES
    (1, 1),
    (2, 2),
    (3, 3),
    (4, 4),
    (5, 5),
     (6, 1),
    (7, 2),
    (8, 3),
    (9, 4),
    (10, 5);

-- Insert data into Zakazka
INSERT INTO Zakazka (datum_objednani, cena, vino_id, odberatel_id) VALUES
    ('2021-03-10', 150.50, 1, 1),
    ('2022-05-18', 120.75, 2, 2),
    ('2020-12-05', 95.20, 2, 3),
    ('2021-08-30', 200.00, 4, 4),
    ('2022-02-14', 80.50, 4, 5),
     ('2022-01-25', 120.00, 6, 6),
    ('2023-04-12', 85.50, 2, 7),
    ('2021-11-30', 150.75, 2, 8),
    ('2022-07-15', 65.20, 9, 9),
    ('2023-03-01', 95.00, 4, 10);

-- Ensure your other tables have appropriate data as needed.

-- Check your views to see if they are populated correctly
SELECT * FROM View_Vino_Odruda;
SELECT * FROM View_Vino_Zakazka;

COMMIT;



