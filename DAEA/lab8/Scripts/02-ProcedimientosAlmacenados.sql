USE BibliotecaDB;
GO

CREATE OR ALTER PROCEDURE usp_Libros_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId, a.Nombre AS AutorNombre, l.Ejemplares, l.Activo
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1
    ORDER BY l.Titulo;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_BuscarPorTitulo
    @Titulo NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId, a.Nombre AS AutorNombre, l.Ejemplares, l.Activo
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1 AND l.Titulo LIKE '%' + @Titulo + '%'
    ORDER BY l.Titulo;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_ObtenerPorId
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId, a.Nombre AS AutorNombre, l.Ejemplares, l.Activo
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.LibroId = @LibroId AND l.Activo = 1;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_Insertar
    @Titulo     NVARCHAR(150),
    @ISBN       NVARCHAR(20),
    @AutorId    INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Libros (Titulo, ISBN, AutorId, Ejemplares, Activo)
    VALUES (@Titulo, @ISBN, @AutorId, @Ejemplares, 1);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_Actualizar
    @LibroId    INT,
    @Titulo     NVARCHAR(150),
    @ISBN       NVARCHAR(20),
    @AutorId    INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Libros
    SET Titulo = @Titulo, ISBN = @ISBN, AutorId = @AutorId, Ejemplares = @Ejemplares
    WHERE LibroId = @LibroId AND Activo = 1;
    SELECT @@ROWCOUNT;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_Eliminar
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Libros SET Activo = 0 WHERE LibroId = @LibroId AND Activo = 1;
    SELECT @@ROWCOUNT;
END
GO

CREATE OR ALTER PROCEDURE usp_Socios_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SocioId, DNI, Nombre, Email, Activo
    FROM Socios
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE usp_Socios_Insertar
    @DNI    NVARCHAR(8),
    @Nombre NVARCHAR(100),
    @Email  NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Socios (DNI, Nombre, Email, Activo)
    VALUES (@DNI, @Nombre, @Email, 1);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

CREATE OR ALTER PROCEDURE usp_Autores_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AutorId, Nombre, Nacionalidad
    FROM Autores
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE usp_Prestamos_ReportePorFechas
    @Desde DATE,
    @Hasta DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PrestamoId, s.Nombre AS Socio, l.Titulo AS Libro,
           p.FechaPrestamo, p.FechaLimite, p.Estado
    FROM Prestamos p
    INNER JOIN DetallePrestamo d ON d.PrestamoId = p.PrestamoId
    INNER JOIN Libros l ON l.LibroId = d.LibroId
    INNER JOIN Socios s ON s.SocioId = p.SocioId
    WHERE p.FechaPrestamo BETWEEN @Desde AND @Hasta
    ORDER BY p.FechaPrestamo, p.PrestamoId, l.Titulo;
END
GO
