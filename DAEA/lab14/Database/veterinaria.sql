IF DB_ID('VeterinariaDb') IS NOT NULL
BEGIN
    ALTER DATABASE VeterinariaDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE VeterinariaDb;
END
GO

CREATE DATABASE VeterinariaDb;
GO

USE VeterinariaDb;
GO

CREATE TABLE Propietarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Documento NVARCHAR(20) NOT NULL,
    Telefono NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT UQ_Propietarios_Documento UNIQUE (Documento)
);
GO

CREATE TABLE Mascotas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    Especie NVARCHAR(30) NOT NULL,
    Raza NVARCHAR(60) NULL,
    FechaNacimiento DATE NULL,
    Peso DECIMAL(6,2) NOT NULL,
    Microchip NVARCHAR(30) NULL,
    Activa BIT NOT NULL DEFAULT 1,
    PropietarioId INT NOT NULL,
    CONSTRAINT FK_Mascotas_Propietarios FOREIGN KEY (PropietarioId) REFERENCES Propietarios(Id)
);
GO

CREATE UNIQUE INDEX UX_Mascotas_Microchip ON Mascotas(Microchip) WHERE Microchip IS NOT NULL;
GO

INSERT INTO Propietarios (Nombre, Documento, Telefono, Email) VALUES
('Carlos Ramirez', '45871236', '987654321', 'carlos.ramirez@mail.com'),
('Lucia Torres', '70123456', '956321478', 'lucia.torres@mail.com'),
('Miguel Paredes', '41236985', '912345678', NULL),
('Ana Flores', '73659841', '934567812', 'ana.flores@mail.com');
GO

INSERT INTO Mascotas (Nombre, Especie, Raza, FechaNacimiento, Peso, Microchip, Activa, PropietarioId) VALUES
('Firulais', 'Perro', 'Labrador', '2020-03-15', 28.50, 'MC-0001', 1, 1),
('Michi', 'Gato', 'Siames', '2021-07-02', 4.20, 'MC-0002', 1, 1),
('Rocky', 'Perro', 'Bulldog', '2019-11-20', 22.00, 'MC-0003', 1, 2),
('Luna', 'Gato', 'Persa', '2022-01-10', 3.80, NULL, 1, 3),
('Kiwi', 'Ave', 'Canario', '2023-05-05', 0.03, NULL, 1, 4),
('Toby', 'Perro', 'Beagle', '2018-09-30', 12.40, 'MC-0004', 0, 2);
GO
