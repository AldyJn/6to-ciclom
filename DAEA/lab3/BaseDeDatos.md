# Base de datos ReservasDB

Script de creacion de la base de datos, las tablas y los datos de prueba.
Ejecutarlo en SQL Server antes de iniciar la aplicacion.

```sql
IF DB_ID('ReservasDB') IS NOT NULL
BEGIN
    ALTER DATABASE ReservasDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ReservasDB;
END
GO

CREATE DATABASE ReservasDB;
GO

USE ReservasDB;
GO

CREATE TABLE Usuarios (
    UsuarioId       INT IDENTITY(1,1) PRIMARY KEY,
    Username        VARCHAR(50) NOT NULL UNIQUE,
    Password        VARCHAR(50) NOT NULL,
    NombreCompleto  VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Aulas (
    AulaId     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre     VARCHAR(50) NOT NULL,
    Capacidad  INT NOT NULL
);
GO

CREATE TABLE Reservas (
    ReservaId  INT IDENTITY(1,1) PRIMARY KEY,
    AulaId     INT NOT NULL,
    UsuarioId  INT NOT NULL,
    Fecha      DATE NOT NULL,
    Hora       TIME NOT NULL,
    Motivo     VARCHAR(200) NOT NULL,
    CONSTRAINT FK_Reservas_Aulas FOREIGN KEY (AulaId) REFERENCES Aulas(AulaId),
    CONSTRAINT FK_Reservas_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
);
GO

INSERT INTO Usuarios (Username, Password, NombreCompleto) VALUES
('admin',    'admin123', 'Administrador del Sistema'),
('jperez',   '1234',     'Juan Perez Quispe'),
('mgarcia',  '1234',     'Maria Garcia Rojas'),
('lramos',   '1234',     'Luis Ramos Torres'),
('acastro',  '1234',     'Ana Castro Vega'),
('rmendoza', '1234',     'Ricardo Mendoza Diaz'),
('cvargas',  '1234',     'Carmen Vargas Lopez'),
('dsalazar', '1234',     'Diego Salazar Nunez'),
('pflores',  '1234',     'Patricia Flores Ruiz'),
('ehuaman',  '1234',     'Eduardo Huaman Soto');
GO

INSERT INTO Aulas (Nombre, Capacidad) VALUES
('Aula A-101', 30),
('Aula A-102', 30),
('Aula A-103', 25),
('Aula B-201', 40),
('Aula B-202', 40),
('Aula B-203', 35),
('Laboratorio C-301', 20),
('Laboratorio C-302', 20),
('Laboratorio C-303', 24),
('Auditorio Principal', 120),
('Sala de Computo 1', 28),
('Sala de Computo 2', 28),
('Sala de Reuniones', 12),
('Taller de Electronica', 18),
('Taller de Mecanica', 22);
GO

INSERT INTO Reservas (AulaId, UsuarioId, Fecha, Hora, Motivo) VALUES
(1,  1, '2026-09-01', '08:00', 'Clase de Programacion I'),
(2,  2, '2026-09-01', '10:00', 'Clase de Base de Datos'),
(3,  3, '2026-09-01', '14:00', 'Asesoria de proyectos'),
(4,  4, '2026-09-02', '08:00', 'Examen parcial de Redes'),
(5,  5, '2026-09-02', '11:00', 'Clase de Algoritmos'),
(6,  6, '2026-09-02', '16:00', 'Reunion de docentes'),
(7,  7, '2026-09-03', '09:00', 'Laboratorio de Sistemas'),
(8,  8, '2026-09-03', '13:00', 'Practica de Redes'),
(9,  9, '2026-09-03', '15:00', 'Laboratorio de Software'),
(10, 10,'2026-09-04', '10:00', 'Charla de induccion'),
(11, 1, '2026-09-04', '08:00', 'Taller de Ofimatica'),
(12, 2, '2026-09-04', '12:00', 'Practica calificada'),
(13, 3, '2026-09-05', '09:00', 'Reunion de coordinacion'),
(14, 4, '2026-09-05', '14:00', 'Practica de Electronica'),
(15, 5, '2026-09-05', '16:00', 'Practica de Mecanica'),
(1,  6, '2026-09-08', '08:00', 'Clase de Programacion II'),
(2,  7, '2026-09-08', '10:00', 'Clase de Estructuras de Datos'),
(3,  8, '2026-09-08', '15:00', 'Tutoria academica'),
(4,  9, '2026-09-09', '08:00', 'Sustentacion de proyectos'),
(5,  10,'2026-09-09', '11:00', 'Clase de Matematica'),
(6,  1, '2026-09-09', '17:00', 'Capacitacion docente'),
(7,  2, '2026-09-10', '09:00', 'Laboratorio de Hardware'),
(8,  3, '2026-09-10', '13:00', 'Practica de Servidores'),
(9,  4, '2026-09-10', '15:00', 'Laboratorio de Testing'),
(10, 5, '2026-09-11', '10:00', 'Ceremonia de bienvenida'),
(11, 6, '2026-09-11', '08:00', 'Examen de Ofimatica'),
(12, 7, '2026-09-11', '12:00', 'Taller de Excel avanzado'),
(13, 8, '2026-09-12', '09:00', 'Reunion con jefatura'),
(14, 9, '2026-09-12', '14:00', 'Proyecto de Electronica'),
(15, 10,'2026-09-12', '16:00', 'Mantenimiento de equipos');
GO
```
