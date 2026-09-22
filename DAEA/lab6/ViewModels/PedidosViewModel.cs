using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using lab6.Data;
using lab6.Models;
using Microsoft.Data.SqlClient;

namespace lab6.ViewModels
{
    /// <summary>
    /// Mantenimiento de pedidos. Igual que el resto de mantenimientos:
    /// ExecuteNonQuery para alta, edicion y baja logica.
    /// </summary>
    public class PedidosViewModel : ViewModelBase
    {
        private readonly PedidoRepositorio _repositorio = new PedidoRepositorio();
        private Pedido _seleccionado;
        private Pedido _editado = new Pedido { FechaPedido = DateTime.Today };
        private bool _esNuevo = true;
        private string _mensaje;

        public PedidosViewModel()
        {
            Pedidos = new ObservableCollection<Pedido>();

            var catalogo = new CatalogoRepositorio();
            Clientes = catalogo.Clientes();
            Empleados = catalogo.Empleados();
            Transportistas = catalogo.Transportistas();

            NuevoCommand    = new Comando(Nuevo);
            GuardarCommand  = new Comando(Guardar);
            EliminarCommand = new Comando(Eliminar, () => !_esNuevo);

            Refrescar();
        }

        public ObservableCollection<Pedido> Pedidos { get; }

        public List<Cliente> Clientes { get; }

        public List<Empleado> Empleados { get; }

        public List<Transportista> Transportistas { get; }

        public Pedido Seleccionado
        {
            get => _seleccionado;
            set
            {
                _seleccionado = value;
                Notificar();

                if (value == null) return;

                _esNuevo = false;
                Editado = new Pedido
                {
                    PedidoID        = value.PedidoID,
                    ClienteID       = value.ClienteID,
                    EmpleadoID      = value.EmpleadoID,
                    FechaPedido     = value.FechaPedido,
                    FechaRequerida  = value.FechaRequerida,
                    FechaEnvio      = value.FechaEnvio,
                    TransportistaID = value.TransportistaID,
                    Destinatario    = value.Destinatario,
                    CiudadDestino   = value.CiudadDestino,
                    PaisDestino     = value.PaisDestino,
                    Activo          = value.Activo
                };
                Mensaje = "Editando el pedido " + value.PedidoID;
                Notificar(nameof(Titulo));
            }
        }

        public Pedido Editado
        {
            get => _editado;
            set { _editado = value; Notificar(); }
        }

        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; Notificar(); }
        }

        public string Titulo => _esNuevo ? "Nuevo pedido" : "Editar pedido";

        public ICommand NuevoCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }

        private void Refrescar()
        {
            try
            {
                Pedidos.Clear();
                foreach (var pedido in _repositorio.Listar()) Pedidos.Add(pedido);
                Mensaje = Contar(Pedidos.Count, "pedido activo", "pedidos activos");
            }
            catch (SqlException ex)
            {
                Mensaje = "Error al listar: " + ex.Message;
            }
        }

        private void Nuevo()
        {
            _esNuevo = true;
            Seleccionado = null;
            Editado = new Pedido
            {
                FechaPedido = DateTime.Today,
                FechaRequerida = DateTime.Today.AddDays(10),
                PaisDestino = "Peru",
                Activo = true
            };
            Mensaje = "Registre un nuevo pedido";
            Notificar(nameof(Titulo));
        }

        private void Guardar()
        {
            if (Editado.ClienteID == null)
            {
                Mensaje = "Seleccione el cliente del pedido";
                return;
            }

            if (Editado.FechaRequerida.HasValue && Editado.FechaRequerida < Editado.FechaPedido)
            {
                Mensaje = "La fecha requerida no puede ser anterior a la fecha del pedido";
                return;
            }

            if (Editado.FechaEnvio.HasValue && Editado.FechaEnvio < Editado.FechaPedido)
            {
                Mensaje = "La fecha de envio no puede ser anterior a la fecha del pedido";
                return;
            }

            try
            {
                // ExecuteNonQuery devuelve las filas afectadas por el INSERT o el UPDATE
                var filas = _esNuevo
                    ? _repositorio.Insertar(Editado)
                    : _repositorio.Actualizar(Editado);

                if (filas == 0)
                {
                    Mensaje = "Ningun registro fue modificado";
                    return;
                }

                var aviso = _esNuevo
                    ? "Pedido registrado con el ID " + Editado.PedidoID
                    : "Pedido " + Editado.PedidoID + " actualizado";

                Refrescar();
                Nuevo();
                Mensaje = aviso;
            }
            catch (SqlException ex)
            {
                Mensaje = "Error al guardar: " + ex.Message;
            }
        }

        private void Eliminar()
        {
            if (_esNuevo || Editado.PedidoID == 0)
            {
                Mensaje = "Seleccione un pedido de la lista";
                return;
            }

            var respuesta = MessageBox.Show(
                "Se dara de baja el pedido " + Editado.PedidoID +
                ".\nEl pedido y su detalle se conservan, solo pasa a Activo = 0.",
                "Confirmar baja logica", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (respuesta != MessageBoxResult.Yes) return;

            try
            {
                // Baja logica mediante ExecuteNonQuery (UPDATE ... SET Activo = 0)
                var filas = _repositorio.Eliminar(Editado.PedidoID);

                var aviso = filas > 0
                    ? "Pedido " + Editado.PedidoID + " dado de baja (Activo = 0)"
                    : "El pedido ya estaba dado de baja";

                Refrescar();
                Nuevo();
                Mensaje = aviso;
            }
            catch (SqlException ex)
            {
                Mensaje = "Error al eliminar: " + ex.Message;
            }
        }
    }
}
