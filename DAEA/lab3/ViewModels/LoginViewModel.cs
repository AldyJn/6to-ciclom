using System;
using System.Windows.Input;
using lab3.Data;

namespace lab3.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly UsuarioRepositorio _repositorio = new UsuarioRepositorio();
        private string _username;
        private string _mensaje;

        public LoginViewModel()
        {
            IngresarCommand = new Comando(Ingresar);
        }

        public string Username
        {
            get => _username;
            set { _username = value; Notificar(); }
        }

        public string Password { get; set; }

        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; Notificar(); }
        }

        public ICommand IngresarCommand { get; }

        public Action Ingresado { get; set; }

        private void Ingresar()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                Mensaje = "Ingrese usuario y contraseña";
                return;
            }

            var usuario = _repositorio.Validar(Username, Password);

            if (usuario == null)
            {
                Mensaje = "Usuario o contraseña incorrectos";
                return;
            }

            Sesion.Usuario = usuario;
            Ingresado?.Invoke();
        }
    }
}
