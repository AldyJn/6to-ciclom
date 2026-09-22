/* ============================================================
   NeptunoDB
   ------------------------------------------------------------
   Paso 2 de 3: campo de estado para eliminacion logica
   ------------------------------------------------------------
   Se agrega la columna Activo BIT NOT NULL DEFAULT 1 a las
   tablas Productos, Categorias, Proveedores y Pedidos.

   Activo = 1  ->  registro vigente (visible en los listados)
   Activo = 0  ->  registro dado de baja (nunca se borra la fila)
   ============================================================ */

USE NeptunoDB;
GO

-- Opciones recomendadas al ejecutar el script desde sqlcmd
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ------------------------------------------------------------
   Categorias
   ------------------------------------------------------------ */
IF COL_LENGTH('dbo.Categorias', 'Activo') IS NULL
BEGIN
    ALTER TABLE dbo.Categorias
        ADD Activo BIT NOT NULL CONSTRAINT DF_Categorias_Activo DEFAULT (1);
END
GO

/* ------------------------------------------------------------
   Proveedores
   ------------------------------------------------------------ */
IF COL_LENGTH('dbo.Proveedores', 'Activo') IS NULL
BEGIN
    ALTER TABLE dbo.Proveedores
        ADD Activo BIT NOT NULL CONSTRAINT DF_Proveedores_Activo DEFAULT (1);
END
GO

/* ------------------------------------------------------------
   Productos
   ------------------------------------------------------------ */
IF COL_LENGTH('dbo.Productos', 'Activo') IS NULL
BEGIN
    ALTER TABLE dbo.Productos
        ADD Activo BIT NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT (1);
END
GO

/* ------------------------------------------------------------
   Pedidos
   ------------------------------------------------------------ */
IF COL_LENGTH('dbo.Pedidos', 'Activo') IS NULL
BEGIN
    ALTER TABLE dbo.Pedidos
        ADD Activo BIT NOT NULL CONSTRAINT DF_Pedidos_Activo DEFAULT (1);
END
GO

/* ------------------------------------------------------------
   Los registros que ya existian quedan como vigentes
   ------------------------------------------------------------ */
UPDATE dbo.Categorias  SET Activo = 1 WHERE Activo IS NULL;
UPDATE dbo.Proveedores SET Activo = 1 WHERE Activo IS NULL;
UPDATE dbo.Productos   SET Activo = 1 WHERE Activo IS NULL;
UPDATE dbo.Pedidos     SET Activo = 1 WHERE Activo IS NULL;
GO

/* ------------------------------------------------------------
   Indices de apoyo: todos los listados consultan por Activo
   ------------------------------------------------------------ */
DROP INDEX IF EXISTS IX_Categorias_Activo  ON dbo.Categorias;
DROP INDEX IF EXISTS IX_Proveedores_Activo ON dbo.Proveedores;
DROP INDEX IF EXISTS IX_Productos_Activo   ON dbo.Productos;
DROP INDEX IF EXISTS IX_Pedidos_Activo     ON dbo.Pedidos;
GO

CREATE INDEX IX_Categorias_Activo  ON dbo.Categorias  (Activo);
CREATE INDEX IX_Proveedores_Activo ON dbo.Proveedores (Activo);
CREATE INDEX IX_Productos_Activo   ON dbo.Productos   (Activo);
CREATE INDEX IX_Pedidos_Activo     ON dbo.Pedidos     (Activo);
GO

/* ------------------------------------------------------------
   Validar la columna agregada
   ------------------------------------------------------------ */
SELECT
    t.name  AS Tabla,
    c.name  AS Columna,
    ty.name AS Tipo,
    OBJECT_DEFINITION(c.default_object_id) AS ValorPorDefecto
FROM sys.columns c
INNER JOIN sys.tables t  ON t.object_id = c.object_id
INNER JOIN sys.types  ty ON ty.user_type_id = c.user_type_id
WHERE c.name = 'Activo'
  AND t.name IN ('Productos', 'Categorias', 'Proveedores', 'Pedidos')
ORDER BY t.name;
GO
