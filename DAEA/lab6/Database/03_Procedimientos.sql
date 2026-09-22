/* ============================================================
   NeptunoDB
   ------------------------------------------------------------
   Paso 3 de 3: procedimientos almacenados
   ------------------------------------------------------------
   Convenciones usadas en todo el script:

   1. Los procedimientos de LECTURA usan SET NOCOUNT ON porque
      devuelven un conjunto de resultados y no interesa el
      contador de filas.

   2. Los procedimientos de ESCRITURA (insertar / actualizar /
      eliminar) NO usan SET NOCOUNT ON a proposito: de esa forma
      ExecuteNonQuery() en la capa ADO .NET recibe la cantidad
      real de filas afectadas y la aplicacion puede avisar al
      usuario si la operacion no impacto ningun registro.

   3. Eliminar NUNCA significa DELETE. Los procedimientos
      usp_*Eliminar ejecutan UPDATE ... SET Activo = 0 y todos
      los listados agregan el filtro Activo = 1.
   ============================================================ */

USE NeptunoDB;
GO

-- Requerido por los indices filtrados y por los procedimientos almacenados
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ============================================================
   CATEGORIAS  -  CRUD
   ============================================================ */

DROP PROCEDURE IF EXISTS dbo.usp_CategoriasListar;
GO
CREATE PROCEDURE dbo.usp_CategoriasListar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.CategoriaID,
        c.NombreCategoria,
        c.Descripcion,
        c.Activo
    FROM dbo.Categorias c
    WHERE c.Activo = 1              -- eliminacion logica: solo vigentes
    ORDER BY c.NombreCategoria;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_CategoriaObtener;
GO
CREATE PROCEDURE dbo.usp_CategoriaObtener
    @CategoriaID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.CategoriaID,
        c.NombreCategoria,
        c.Descripcion,
        c.Activo
    FROM dbo.Categorias c
    WHERE c.CategoriaID = @CategoriaID
      AND c.Activo = 1;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_CategoriaInsertar;
GO
CREATE PROCEDURE dbo.usp_CategoriaInsertar
    @NombreCategoria NVARCHAR(30),
    @Descripcion     NVARCHAR(200) = NULL,
    @NuevoID         INT OUTPUT
AS
BEGIN
    IF LTRIM(RTRIM(ISNULL(@NombreCategoria, ''))) = ''
    BEGIN
        RAISERROR(N'El nombre de la categoria es obligatorio.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.Categorias (NombreCategoria, Descripcion, Activo)
    VALUES (@NombreCategoria, @Descripcion, 1);

    SET @NuevoID = SCOPE_IDENTITY();
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_CategoriaActualizar;
GO
CREATE PROCEDURE dbo.usp_CategoriaActualizar
    @CategoriaID     INT,
    @NombreCategoria NVARCHAR(30),
    @Descripcion     NVARCHAR(200) = NULL
AS
BEGIN
    IF LTRIM(RTRIM(ISNULL(@NombreCategoria, ''))) = ''
    BEGIN
        RAISERROR(N'El nombre de la categoria es obligatorio.', 16, 1);
        RETURN;
    END

    UPDATE dbo.Categorias
    SET NombreCategoria = @NombreCategoria,
        Descripcion     = @Descripcion
    WHERE CategoriaID = @CategoriaID
      AND Activo = 1;               -- no se edita un registro dado de baja
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_CategoriaEliminar;
GO
CREATE PROCEDURE dbo.usp_CategoriaEliminar
    @CategoriaID INT
AS
BEGIN
    /* ELIMINACION LOGICA: la fila permanece en la tabla,
       solo cambia su estado a Activo = 0.                     */
    UPDATE dbo.Categorias
    SET Activo = 0
    WHERE CategoriaID = @CategoriaID
      AND Activo = 1;
END
GO

/* ============================================================
   PROVEEDORES  -  CRUD + busqueda
   ============================================================ */

DROP PROCEDURE IF EXISTS dbo.usp_ProveedoresListar;
GO
CREATE PROCEDURE dbo.usp_ProveedoresListar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.ProveedorID,
        p.CompaniaNombre,
        p.NombreContacto,
        p.CargoContacto,
        p.Direccion,
        p.Ciudad,
        p.CodigoPostal,
        p.Pais,
        p.Telefono,
        p.Fax,
        p.Activo
    FROM dbo.Proveedores p
    WHERE p.Activo = 1
    ORDER BY p.CompaniaNombre;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_ProveedorObtener;
GO
CREATE PROCEDURE dbo.usp_ProveedorObtener
    @ProveedorID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.ProveedorID,
        p.CompaniaNombre,
        p.NombreContacto,
        p.CargoContacto,
        p.Direccion,
        p.Ciudad,
        p.CodigoPostal,
        p.Pais,
        p.Telefono,
        p.Fax,
        p.Activo
    FROM dbo.Proveedores p
    WHERE p.ProveedorID = @ProveedorID
      AND p.Activo = 1;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_ProveedorInsertar;
GO
CREATE PROCEDURE dbo.usp_ProveedorInsertar
    @CompaniaNombre NVARCHAR(60),
    @NombreContacto NVARCHAR(40) = NULL,
    @CargoContacto  NVARCHAR(40) = NULL,
    @Direccion      NVARCHAR(80) = NULL,
    @Ciudad         NVARCHAR(30) = NULL,
    @CodigoPostal   NVARCHAR(10) = NULL,
    @Pais           NVARCHAR(30) = NULL,
    @Telefono       NVARCHAR(24) = NULL,
    @Fax            NVARCHAR(24) = NULL,
    @NuevoID        INT OUTPUT
AS
BEGIN
    IF LTRIM(RTRIM(ISNULL(@CompaniaNombre, ''))) = ''
    BEGIN
        RAISERROR(N'El nombre de la compania es obligatorio.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.Proveedores
        (CompaniaNombre, NombreContacto, CargoContacto, Direccion, Ciudad,
         CodigoPostal, Pais, Telefono, Fax, Activo)
    VALUES
        (@CompaniaNombre, @NombreContacto, @CargoContacto, @Direccion, @Ciudad,
         @CodigoPostal, @Pais, @Telefono, @Fax, 1);

    SET @NuevoID = SCOPE_IDENTITY();
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_ProveedorActualizar;
GO
CREATE PROCEDURE dbo.usp_ProveedorActualizar
    @ProveedorID    INT,
    @CompaniaNombre NVARCHAR(60),
    @NombreContacto NVARCHAR(40) = NULL,
    @CargoContacto  NVARCHAR(40) = NULL,
    @Direccion      NVARCHAR(80) = NULL,
    @Ciudad         NVARCHAR(30) = NULL,
    @CodigoPostal   NVARCHAR(10) = NULL,
    @Pais           NVARCHAR(30) = NULL,
    @Telefono       NVARCHAR(24) = NULL,
    @Fax            NVARCHAR(24) = NULL
AS
BEGIN
    IF LTRIM(RTRIM(ISNULL(@CompaniaNombre, ''))) = ''
    BEGIN
        RAISERROR(N'El nombre de la compania es obligatorio.', 16, 1);
        RETURN;
    END

    UPDATE dbo.Proveedores
    SET CompaniaNombre = @CompaniaNombre,
        NombreContacto = @NombreContacto,
        CargoContacto  = @CargoContacto,
        Direccion      = @Direccion,
        Ciudad         = @Ciudad,
        CodigoPostal   = @CodigoPostal,
        Pais           = @Pais,
        Telefono       = @Telefono,
        Fax            = @Fax
    WHERE ProveedorID = @ProveedorID
      AND Activo = 1;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_ProveedorEliminar;
GO
CREATE PROCEDURE dbo.usp_ProveedorEliminar
    @ProveedorID INT
AS
BEGIN
    /* ELIMINACION LOGICA */
    UPDATE dbo.Proveedores
    SET Activo = 0
    WHERE ProveedorID = @ProveedorID
      AND Activo = 1;
END
GO

/* ------------------------------------------------------------
   Punto 7: listado de proveedores buscando por NombreContacto
            y Ciudad. Solo registros con Activo = 1.
            Los dos parametros son opcionales: si llegan vacios
            o NULL el filtro correspondiente no se aplica.
   ------------------------------------------------------------ */
DROP PROCEDURE IF EXISTS dbo.usp_ProveedoresBuscar;
GO
CREATE PROCEDURE dbo.usp_ProveedoresBuscar
    @NombreContacto NVARCHAR(40) = NULL,
    @Ciudad         NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.ProveedorID,
        p.CompaniaNombre,
        p.NombreContacto,
        p.CargoContacto,
        p.Direccion,
        p.Ciudad,
        p.CodigoPostal,
        p.Pais,
        p.Telefono,
        p.Fax,
        p.Activo
    FROM dbo.Proveedores p
    WHERE p.Activo = 1              -- excluye los dados de baja
      AND (@NombreContacto IS NULL OR LTRIM(RTRIM(@NombreContacto)) = ''
           OR p.NombreContacto LIKE N'%' + @NombreContacto + N'%')
      AND (@Ciudad IS NULL OR LTRIM(RTRIM(@Ciudad)) = ''
           OR p.Ciudad LIKE N'%' + @Ciudad + N'%')
    ORDER BY p.CompaniaNombre;
END
GO

/* ============================================================
   PRODUCTOS  -  CRUD
   ============================================================ */

DROP PROCEDURE IF EXISTS dbo.usp_ProductosListar;
GO
CREATE PROCEDURE dbo.usp_ProductosListar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pr.ProductoID,
        pr.NombreProducto,
        pr.ProveedorID,
        pv.CompaniaNombre  AS Proveedor,
        pr.CategoriaID,
        ct.NombreCategoria AS Categoria,
        pr.CantidadPorUnidad,
        pr.PrecioUnidad,
        pr.UnidadesEnExistencia,
        pr.UnidadesEnPedido,
        pr.NivelDeReorden,
        pr.Descontinuado,
        pr.Activo
    FROM dbo.Productos pr
    LEFT JOIN dbo.Proveedores pv ON pv.ProveedorID = pr.ProveedorID
    LEFT JOIN dbo.Categorias  ct ON ct.CategoriaID = pr.CategoriaID
    WHERE pr.Activo = 1
    ORDER BY pr.NombreProducto;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_ProductoObtener;
GO
CREATE PROCEDURE dbo.usp_ProductoObtener
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pr.ProductoID,
        pr.NombreProducto,
        pr.ProveedorID,
        pv.CompaniaNombre  AS Proveedor,
        pr.CategoriaID,
        ct.NombreCategoria AS Categoria,
        pr.CantidadPorUnidad,
        pr.PrecioUnidad,
        pr.UnidadesEnExistencia,
        pr.UnidadesEnPedido,
        pr.NivelDeReorden,
        pr.Descontinuado,
        pr.Activo
    FROM dbo.Productos pr
    LEFT JOIN dbo.Proveedores pv ON pv.ProveedorID = pr.ProveedorID
    LEFT JOIN dbo.Categorias  ct ON ct.CategoriaID = pr.CategoriaID
    WHERE pr.ProductoID = @ProductoID
      AND pr.Activo = 1;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_ProductoInsertar;
GO
CREATE PROCEDURE dbo.usp_ProductoInsertar
    @NombreProducto       NVARCHAR(60),
    @ProveedorID          INT           = NULL,
    @CategoriaID          INT           = NULL,
    @CantidadPorUnidad    NVARCHAR(30)  = NULL,
    @PrecioUnidad         DECIMAL(10,2) = 0,
    @UnidadesEnExistencia SMALLINT      = 0,
    @UnidadesEnPedido     SMALLINT      = 0,
    @NivelDeReorden       SMALLINT      = 0,
    @Descontinuado        BIT           = 0,
    @NuevoID              INT OUTPUT
AS
BEGIN
    IF LTRIM(RTRIM(ISNULL(@NombreProducto, ''))) = ''
    BEGIN
        RAISERROR(N'El nombre del producto es obligatorio.', 16, 1);
        RETURN;
    END

    IF @PrecioUnidad < 0
    BEGIN
        RAISERROR(N'El precio unitario no puede ser negativo.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.Productos
        (NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad, PrecioUnidad,
         UnidadesEnExistencia, UnidadesEnPedido, NivelDeReorden, Descontinuado, Activo)
    VALUES
        (@NombreProducto, @ProveedorID, @CategoriaID, @CantidadPorUnidad, @PrecioUnidad,
         @UnidadesEnExistencia, @UnidadesEnPedido, @NivelDeReorden, @Descontinuado, 1);

    SET @NuevoID = SCOPE_IDENTITY();
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_ProductoActualizar;
GO
CREATE PROCEDURE dbo.usp_ProductoActualizar
    @ProductoID           INT,
    @NombreProducto       NVARCHAR(60),
    @ProveedorID          INT           = NULL,
    @CategoriaID          INT           = NULL,
    @CantidadPorUnidad    NVARCHAR(30)  = NULL,
    @PrecioUnidad         DECIMAL(10,2) = 0,
    @UnidadesEnExistencia SMALLINT      = 0,
    @UnidadesEnPedido     SMALLINT      = 0,
    @NivelDeReorden       SMALLINT      = 0,
    @Descontinuado        BIT           = 0
AS
BEGIN
    IF LTRIM(RTRIM(ISNULL(@NombreProducto, ''))) = ''
    BEGIN
        RAISERROR(N'El nombre del producto es obligatorio.', 16, 1);
        RETURN;
    END

    IF @PrecioUnidad < 0
    BEGIN
        RAISERROR(N'El precio unitario no puede ser negativo.', 16, 1);
        RETURN;
    END

    UPDATE dbo.Productos
    SET NombreProducto       = @NombreProducto,
        ProveedorID          = @ProveedorID,
        CategoriaID          = @CategoriaID,
        CantidadPorUnidad    = @CantidadPorUnidad,
        PrecioUnidad         = @PrecioUnidad,
        UnidadesEnExistencia = @UnidadesEnExistencia,
        UnidadesEnPedido     = @UnidadesEnPedido,
        NivelDeReorden       = @NivelDeReorden,
        Descontinuado        = @Descontinuado
    WHERE ProductoID = @ProductoID
      AND Activo = 1;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_ProductoEliminar;
GO
CREATE PROCEDURE dbo.usp_ProductoEliminar
    @ProductoID INT
AS
BEGIN
    /* ELIMINACION LOGICA: no se ejecuta DELETE, de esa forma el
       historial de DetallePedidos sigue siendo consistente.     */
    UPDATE dbo.Productos
    SET Activo = 0
    WHERE ProductoID = @ProductoID
      AND Activo = 1;
END
GO

/* ============================================================
   PEDIDOS  -  CRUD
   ============================================================ */

DROP PROCEDURE IF EXISTS dbo.usp_PedidosListar;
GO
CREATE PROCEDURE dbo.usp_PedidosListar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pe.PedidoID,
        pe.ClienteID,
        cl.Empresa AS Cliente,
        pe.EmpleadoID,
        em.Nombre + N' ' + em.Apellidos AS Empleado,
        pe.FechaPedido,
        pe.FechaRequerida,
        pe.FechaEnvio,
        pe.TransportistaID,
        tr.CompaniaNombre AS Transportista,
        pe.Destinatario,
        pe.CiudadDestino,
        pe.PaisDestino,
        pe.Activo
    FROM dbo.Pedidos pe
    LEFT JOIN dbo.Clientes       cl ON cl.ClienteID       = pe.ClienteID
    LEFT JOIN dbo.Empleados      em ON em.EmpleadoID      = pe.EmpleadoID
    LEFT JOIN dbo.Transportistas tr ON tr.TransportistaID = pe.TransportistaID
    WHERE pe.Activo = 1
    ORDER BY pe.FechaPedido DESC, pe.PedidoID DESC;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_PedidoObtener;
GO
CREATE PROCEDURE dbo.usp_PedidoObtener
    @PedidoID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pe.PedidoID,
        pe.ClienteID,
        cl.Empresa AS Cliente,
        pe.EmpleadoID,
        em.Nombre + N' ' + em.Apellidos AS Empleado,
        pe.FechaPedido,
        pe.FechaRequerida,
        pe.FechaEnvio,
        pe.TransportistaID,
        tr.CompaniaNombre AS Transportista,
        pe.Destinatario,
        pe.CiudadDestino,
        pe.PaisDestino,
        pe.Activo
    FROM dbo.Pedidos pe
    LEFT JOIN dbo.Clientes       cl ON cl.ClienteID       = pe.ClienteID
    LEFT JOIN dbo.Empleados      em ON em.EmpleadoID      = pe.EmpleadoID
    LEFT JOIN dbo.Transportistas tr ON tr.TransportistaID = pe.TransportistaID
    WHERE pe.PedidoID = @PedidoID
      AND pe.Activo = 1;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_PedidoInsertar;
GO
CREATE PROCEDURE dbo.usp_PedidoInsertar
    @ClienteID       INT = NULL,
    @EmpleadoID      INT = NULL,
    @FechaPedido     DATE,
    @FechaRequerida  DATE = NULL,
    @FechaEnvio      DATE = NULL,
    @TransportistaID INT  = NULL,
    @Destinatario    NVARCHAR(60) = NULL,
    @CiudadDestino   NVARCHAR(30) = NULL,
    @PaisDestino     NVARCHAR(30) = NULL,
    @NuevoID         INT OUTPUT
AS
BEGIN
    IF @FechaRequerida IS NOT NULL AND @FechaRequerida < @FechaPedido
    BEGIN
        RAISERROR(N'La fecha requerida no puede ser anterior a la fecha del pedido.', 16, 1);
        RETURN;
    END

    IF @FechaEnvio IS NOT NULL AND @FechaEnvio < @FechaPedido
    BEGIN
        RAISERROR(N'La fecha de envio no puede ser anterior a la fecha del pedido.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.Pedidos
        (ClienteID, EmpleadoID, FechaPedido, FechaRequerida, FechaEnvio,
         TransportistaID, Destinatario, CiudadDestino, PaisDestino, Activo)
    VALUES
        (@ClienteID, @EmpleadoID, @FechaPedido, @FechaRequerida, @FechaEnvio,
         @TransportistaID, @Destinatario, @CiudadDestino, @PaisDestino, 1);

    SET @NuevoID = SCOPE_IDENTITY();
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_PedidoActualizar;
GO
CREATE PROCEDURE dbo.usp_PedidoActualizar
    @PedidoID        INT,
    @ClienteID       INT = NULL,
    @EmpleadoID      INT = NULL,
    @FechaPedido     DATE,
    @FechaRequerida  DATE = NULL,
    @FechaEnvio      DATE = NULL,
    @TransportistaID INT  = NULL,
    @Destinatario    NVARCHAR(60) = NULL,
    @CiudadDestino   NVARCHAR(30) = NULL,
    @PaisDestino     NVARCHAR(30) = NULL
AS
BEGIN
    IF @FechaRequerida IS NOT NULL AND @FechaRequerida < @FechaPedido
    BEGIN
        RAISERROR(N'La fecha requerida no puede ser anterior a la fecha del pedido.', 16, 1);
        RETURN;
    END

    IF @FechaEnvio IS NOT NULL AND @FechaEnvio < @FechaPedido
    BEGIN
        RAISERROR(N'La fecha de envio no puede ser anterior a la fecha del pedido.', 16, 1);
        RETURN;
    END

    UPDATE dbo.Pedidos
    SET ClienteID       = @ClienteID,
        EmpleadoID      = @EmpleadoID,
        FechaPedido     = @FechaPedido,
        FechaRequerida  = @FechaRequerida,
        FechaEnvio      = @FechaEnvio,
        TransportistaID = @TransportistaID,
        Destinatario    = @Destinatario,
        CiudadDestino   = @CiudadDestino,
        PaisDestino     = @PaisDestino
    WHERE PedidoID = @PedidoID
      AND Activo = 1;
END
GO

DROP PROCEDURE IF EXISTS dbo.usp_PedidoEliminar;
GO
CREATE PROCEDURE dbo.usp_PedidoEliminar
    @PedidoID INT
AS
BEGIN
    /* ELIMINACION LOGICA: el pedido y su detalle se conservan;
       al quedar Activo = 0 dejan de aparecer en los listados
       y en el reporte por intervalo de fechas.                 */
    UPDATE dbo.Pedidos
    SET Activo = 0
    WHERE PedidoID = @PedidoID
      AND Activo = 1;
END
GO

/* ------------------------------------------------------------
   Punto 8: listado de detalles de pedidos con INNER JOIN a
            Pedidos, filtrado por intervalo de fechas y
            excluyendo los pedidos con Activo = 0.
   ------------------------------------------------------------ */
DROP PROCEDURE IF EXISTS dbo.usp_DetallePedidosPorFechas;
GO
CREATE PROCEDURE dbo.usp_DetallePedidosPorFechas
    @FechaInicio DATE,
    @FechaFin    DATE
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaFin < @FechaInicio
    BEGIN
        RAISERROR(N'La fecha final no puede ser anterior a la fecha inicial.', 16, 1);
        RETURN;
    END

    SELECT
        pe.PedidoID,
        pe.FechaPedido,
        cl.Empresa         AS Cliente,
        pe.Destinatario,
        pe.CiudadDestino,
        dp.ProductoID,
        pr.NombreProducto  AS Producto,
        ct.NombreCategoria AS Categoria,
        dp.PrecioUnidad,
        dp.Cantidad,
        dp.Descuento,
        CAST(dp.PrecioUnidad * dp.Cantidad * (1 - dp.Descuento) AS DECIMAL(12,2)) AS Subtotal
    FROM dbo.DetallePedidos dp
    INNER JOIN dbo.Pedidos   pe ON pe.PedidoID   = dp.PedidoID     -- inner join pedido
    INNER JOIN dbo.Productos pr ON pr.ProductoID = dp.ProductoID
    LEFT  JOIN dbo.Categorias ct ON ct.CategoriaID = pr.CategoriaID
    LEFT  JOIN dbo.Clientes   cl ON cl.ClienteID   = pe.ClienteID
    WHERE pe.Activo = 1                                            -- excluye pedidos dados de baja
      AND pe.FechaPedido BETWEEN @FechaInicio AND @FechaFin         -- intervalo de fechas
    ORDER BY pe.FechaPedido, pe.PedidoID, pr.NombreProducto;
END
GO

/* ============================================================
   Validar los procedimientos creados
   ============================================================ */
SELECT name AS Procedimiento, create_date AS Creado
FROM sys.procedures
WHERE name LIKE 'usp_%'
ORDER BY name;
GO
