using System.Windows;
using lab3.ViewModels;

namespace lab3.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _modelo = new LoginViewModel();

        public LoginWindow()
        {
            InitializeComponent();
            _modelo.Ingresado = Abrir;
            DataContext = _modelo;
        }

        private void Clave_PasswordChanged(object remitente, RoutedEventArgs e)
        {
            _modelo.Password = clave.Password;
        }

        private void Abrir()
        {
            new PrincipalWindow().Show();
            Close();
        }
    }
}
