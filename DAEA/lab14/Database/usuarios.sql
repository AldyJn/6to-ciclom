USE VeterinariaDb;
GO

IF OBJECT_ID('dbo.Usuarios') IS NOT NULL
    DROP TABLE dbo.Usuarios;
GO

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Correo NVARCHAR(100) NOT NULL,
    ClaveHash NVARCHAR(100) NOT NULL,
    Rol NVARCHAR(30) NOT NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT UQ_Usuarios_Correo UNIQUE (Correo),
    CONSTRAINT CK_Usuarios_Rol CHECK (Rol IN ('Recepcionista', 'Veterinario', 'Asistente'))
);
GO

INSERT INTO Usuarios (Correo, ClaveHash, Rol) VALUES
('recepcion@veterinaria.pe',   '$2a$11$LqqykmzCZ17Kwjw3wQ/oQuj/VxbF6pCO5FFTZGBOeTlTq47QkaMta', 'Recepcionista'),
('veterinario@veterinaria.pe', '$2a$11$NtnNNK.N9FEvirjjKfiQ4OcJAHqGocPR.pHmBrcy.dcOgLqZvva3m', 'Veterinario'),
('asistente@veterinaria.pe',   '$2a$11$jfQaPCxDKuvLx6NSifm6duzWahJMShebg45TmCK7e5WLlXsh203Em', 'Asistente');
GO
