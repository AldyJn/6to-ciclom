USE NeptunoDB;
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProductosListar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProductoID, p.NombreProducto, p.ProveedorID, pr.CompaniaNombre AS Proveedor,
           p.CategoriaID, c.NombreCategoria AS Categoria, p.CantidadPorUnidad, p.PrecioUnidad,
           p.UnidadesEnExistencia, p.UnidadesEnPedido, p.NivelDeReorden, p.Descontinuado
    FROM dbo.Productos p
    LEFT JOIN dbo.Proveedores pr ON pr.ProveedorID = p.ProveedorID
    LEFT JOIN dbo.Categorias c ON c.CategoriaID = p.CategoriaID
    ORDER BY p.ProductoID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProductoObtener @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.Productos WHERE ProductoID = @ProductoID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProductoInsertar
    @NombreProducto NVARCHAR(60), @ProveedorID INT = NULL, @CategoriaID INT = NULL,
    @CantidadPorUnidad NVARCHAR(30) = NULL, @PrecioUnidad DECIMAL(10,2) = 0,
    @UnidadesEnExistencia SMALLINT = 0, @UnidadesEnPedido SMALLINT = 0,
    @NivelDeReorden SMALLINT = 0, @Descontinuado BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Productos (NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad, PrecioUnidad,
                               UnidadesEnExistencia, UnidadesEnPedido, NivelDeReorden, Descontinuado)
    VALUES (@NombreProducto, @ProveedorID, @CategoriaID, @CantidadPorUnidad, @PrecioUnidad,
            @UnidadesEnExistencia, @UnidadesEnPedido, @NivelDeReorden, @Descontinuado);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ProductoID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProductoActualizar
    @ProductoID INT, @NombreProducto NVARCHAR(60), @ProveedorID INT = NULL, @CategoriaID INT = NULL,
    @CantidadPorUnidad NVARCHAR(30) = NULL, @PrecioUnidad DECIMAL(10,2) = 0,
    @UnidadesEnExistencia SMALLINT = 0, @UnidadesEnPedido SMALLINT = 0,
    @NivelDeReorden SMALLINT = 0, @Descontinuado BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Productos
    SET NombreProducto = @NombreProducto, ProveedorID = @ProveedorID, CategoriaID = @CategoriaID,
        CantidadPorUnidad = @CantidadPorUnidad, PrecioUnidad = @PrecioUnidad,
        UnidadesEnExistencia = @UnidadesEnExistencia, UnidadesEnPedido = @UnidadesEnPedido,
        NivelDeReorden = @NivelDeReorden, Descontinuado = @Descontinuado
    WHERE ProductoID = @ProductoID;
    SELECT @@ROWCOUNT AS Filas;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProductoEliminar @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Productos WHERE ProductoID = @ProductoID;
    SELECT @@ROWCOUNT AS Filas;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_CategoriasListar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID, NombreCategoria, Descripcion FROM dbo.Categorias ORDER BY CategoriaID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_CategoriaObtener @CategoriaID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID, NombreCategoria, Descripcion FROM dbo.Categorias WHERE CategoriaID = @CategoriaID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_CategoriaInsertar @NombreCategoria NVARCHAR(30), @Descripcion NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Categorias (NombreCategoria, Descripcion) VALUES (@NombreCategoria, @Descripcion);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS CategoriaID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_CategoriaActualizar
    @CategoriaID INT, @NombreCategoria NVARCHAR(30), @Descripcion NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Categorias SET NombreCategoria = @NombreCategoria, Descripcion = @Descripcion
    WHERE CategoriaID = @CategoriaID;
    SELECT @@ROWCOUNT AS Filas;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_CategoriaEliminar @CategoriaID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Categorias WHERE CategoriaID = @CategoriaID;
    SELECT @@ROWCOUNT AS Filas;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProveedoresListar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto, Direccion, Ciudad,
           CodigoPostal, Pais, Telefono, Fax
    FROM dbo.Proveedores ORDER BY ProveedorID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProveedorObtener @ProveedorID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.Proveedores WHERE ProveedorID = @ProveedorID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProveedorInsertar
    @CompaniaNombre NVARCHAR(60), @NombreContacto NVARCHAR(40) = NULL, @CargoContacto NVARCHAR(40) = NULL,
    @Direccion NVARCHAR(80) = NULL, @Ciudad NVARCHAR(30) = NULL, @CodigoPostal NVARCHAR(10) = NULL,
    @Pais NVARCHAR(30) = NULL, @Telefono NVARCHAR(24) = NULL, @Fax NVARCHAR(24) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Proveedores (CompaniaNombre, NombreContacto, CargoContacto, Direccion, Ciudad,
                                 CodigoPostal, Pais, Telefono, Fax)
    VALUES (@CompaniaNombre, @NombreContacto, @CargoContacto, @Direccion, @Ciudad,
            @CodigoPostal, @Pais, @Telefono, @Fax);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ProveedorID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProveedorActualizar
    @ProveedorID INT, @CompaniaNombre NVARCHAR(60), @NombreContacto NVARCHAR(40) = NULL,
    @CargoContacto NVARCHAR(40) = NULL, @Direccion NVARCHAR(80) = NULL, @Ciudad NVARCHAR(30) = NULL,
    @CodigoPostal NVARCHAR(10) = NULL, @Pais NVARCHAR(30) = NULL, @Telefono NVARCHAR(24) = NULL,
    @Fax NVARCHAR(24) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Proveedores
    SET CompaniaNombre = @CompaniaNombre, NombreContacto = @NombreContacto, CargoContacto = @CargoContacto,
        Direccion = @Direccion, Ciudad = @Ciudad, CodigoPostal = @CodigoPostal, Pais = @Pais,
        Telefono = @Telefono, Fax = @Fax
    WHERE ProveedorID = @ProveedorID;
    SELECT @@ROWCOUNT AS Filas;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProveedorEliminar @ProveedorID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Proveedores WHERE ProveedorID = @ProveedorID;
    SELECT @@ROWCOUNT AS Filas;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProveedoresBuscar
    @NombreContacto NVARCHAR(40) = NULL, @Ciudad NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto, Direccion, Ciudad,
           CodigoPostal, Pais, Telefono, Fax
    FROM dbo.Proveedores
    WHERE (@NombreContacto IS NULL OR @NombreContacto = N'' OR NombreContacto LIKE N'%' + @NombreContacto + N'%')
      AND (@Ciudad IS NULL OR @Ciudad = N'' OR Ciudad LIKE N'%' + @Ciudad + N'%')
    ORDER BY ProveedorID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_PedidosListar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PedidoID, p.ClienteID, c.Empresa AS Cliente, p.EmpleadoID,
           e.Nombre + N' ' + e.Apellidos AS Empleado, p.FechaPedido, p.FechaRequerida, p.FechaEnvio,
           p.TransportistaID, t.CompaniaNombre AS Transportista, p.Destinatario, p.CiudadDestino, p.PaisDestino
    FROM dbo.Pedidos p
    LEFT JOIN dbo.Clientes c ON c.ClienteID = p.ClienteID
    LEFT JOIN dbo.Empleados e ON e.EmpleadoID = p.EmpleadoID
    LEFT JOIN dbo.Transportistas t ON t.TransportistaID = p.TransportistaID
    ORDER BY p.PedidoID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_PedidoObtener @PedidoID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.Pedidos WHERE PedidoID = @PedidoID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_PedidoInsertar
    @ClienteID INT = NULL, @EmpleadoID INT = NULL, @FechaPedido DATE, @FechaRequerida DATE = NULL,
    @FechaEnvio DATE = NULL, @TransportistaID INT = NULL, @Destinatario NVARCHAR(60) = NULL,
    @CiudadDestino NVARCHAR(30) = NULL, @PaisDestino NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Pedidos (ClienteID, EmpleadoID, FechaPedido, FechaRequerida, FechaEnvio,
                             TransportistaID, Destinatario, CiudadDestino, PaisDestino)
    VALUES (@ClienteID, @EmpleadoID, @FechaPedido, @FechaRequerida, @FechaEnvio,
            @TransportistaID, @Destinatario, @CiudadDestino, @PaisDestino);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS PedidoID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_PedidoActualizar
    @PedidoID INT, @ClienteID INT = NULL, @EmpleadoID INT = NULL, @FechaPedido DATE,
    @FechaRequerida DATE = NULL, @FechaEnvio DATE = NULL, @TransportistaID INT = NULL,
    @Destinatario NVARCHAR(60) = NULL, @CiudadDestino NVARCHAR(30) = NULL, @PaisDestino NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Pedidos
    SET ClienteID = @ClienteID, EmpleadoID = @EmpleadoID, FechaPedido = @FechaPedido,
        FechaRequerida = @FechaRequerida, FechaEnvio = @FechaEnvio, TransportistaID = @TransportistaID,
        Destinatario = @Destinatario, CiudadDestino = @CiudadDestino, PaisDestino = @PaisDestino
    WHERE PedidoID = @PedidoID;
    SELECT @@ROWCOUNT AS Filas;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_PedidoEliminar @PedidoID INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        DELETE FROM dbo.DetallePedidos WHERE PedidoID = @PedidoID;
        DELETE FROM dbo.Pedidos WHERE PedidoID = @PedidoID;
        DECLARE @Filas INT = @@ROWCOUNT;
    COMMIT TRANSACTION;
    SELECT @Filas AS Filas;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_DetallePedidosPorFechas @FechaDesde DATE, @FechaHasta DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PedidoID, p.FechaPedido, c.Empresa AS Cliente, pr.NombreProducto AS Producto,
           d.PrecioUnidad, d.Cantidad, d.Descuento,
           CAST(d.PrecioUnidad * d.Cantidad * (1 - d.Descuento) AS DECIMAL(12,2)) AS Subtotal
    FROM dbo.DetallePedidos d
    INNER JOIN dbo.Pedidos p ON p.PedidoID = d.PedidoID
    INNER JOIN dbo.Productos pr ON pr.ProductoID = d.ProductoID
    LEFT JOIN dbo.Clientes c ON c.ClienteID = p.ClienteID
    WHERE p.FechaPedido BETWEEN @FechaDesde AND @FechaHasta
    ORDER BY p.FechaPedido, p.PedidoID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ClientesCombo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ClienteID AS Id, Empresa AS Nombre FROM dbo.Clientes ORDER BY Empresa;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_EmpleadosCombo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmpleadoID AS Id, Nombre + N' ' + Apellidos AS Nombre FROM dbo.Empleados ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_TransportistasCombo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TransportistaID AS Id, CompaniaNombre AS Nombre FROM dbo.Transportistas ORDER BY CompaniaNombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_ProveedoresCombo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID AS Id, CompaniaNombre AS Nombre FROM dbo.Proveedores ORDER BY CompaniaNombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_CategoriasCombo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID AS Id, NombreCategoria AS Nombre FROM dbo.Categorias ORDER BY NombreCategoria;
END
GO
