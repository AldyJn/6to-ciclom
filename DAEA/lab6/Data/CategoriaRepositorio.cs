using System;
using System.Collections.Generic;
using System.Data;
using lab6.Models;
using Microsoft.Data.SqlClient;

namespace lab6.Data
{
    /// <summary>
    /// CRUD de categorias contra los procedimientos almacenados de NeptunoDB.
    /// Insertar, Actualizar y Eliminar usan ExecuteNonQuery y devuelven la
    /// cantidad de filas afectadas. Eliminar es una baja logica (Activo = 0).
    /// </summary>
    public class CategoriaRepositorio
    {
        public List<Categoria> Listar()
        {
            var lista = new List<Categoria>();

            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_CategoriasListar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read()) lista.Add(Leer(lector));
                }
            }

            return lista;
        }

        public int Insertar(Categoria categoria)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_CategoriaInsertar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreCategoria", categoria.NombreCategoria);
                cmd.Parameters.AddWithValue("@Descripcion", Bd.Valor(categoria.Descripcion));

                var nuevoId = new SqlParameter("@NuevoID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(nuevoId);

                cn.Open();
                var filas = cmd.ExecuteNonQuery();      // INSERT

                if (nuevoId.Value != DBNull.Value) categoria.CategoriaID = (int)nuevoId.Value;
                return filas;
            }
        }

        public int Actualizar(Categoria categoria)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_CategoriaActualizar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoriaID", categoria.CategoriaID);
                cmd.Parameters.AddWithValue("@NombreCategoria", categoria.NombreCategoria);
                cmd.Parameters.AddWithValue("@Descripcion", Bd.Valor(categoria.Descripcion));

                cn.Open();
                return cmd.ExecuteNonQuery();           // UPDATE
            }
        }

        /// <summary>Baja logica: el procedimiento ejecuta UPDATE ... SET Activo = 0.</summary>
        public int Eliminar(int categoriaId)
        {
            using (var cn = new SqlConnection(Conexion.Cadena))
            using (var cmd = new SqlCommand("dbo.usp_CategoriaEliminar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoriaID", categoriaId);

                cn.Open();
                return cmd.ExecuteNonQuery();           // UPDATE Activo = 0
            }
        }

        private static Categoria Leer(IDataRecord registro) => new Categoria
        {
            CategoriaID     = Bd.Entero(registro, "CategoriaID"),
            NombreCategoria = Bd.Texto(registro, "NombreCategoria"),
            Descripcion     = Bd.Texto(registro, "Descripcion"),
            Activo          = Bd.Booleano(registro, "Activo")
        };
    }
}
