using System.Windows;

namespace lab2.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Ingresar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Password;

            if (usuario.Length == 0 || password.Length == 0)
            {
                lblError.Text = "Ingrese usuario y contraseña.";
                return;
            }

            if (usuario != "admin" || password != "123")
            {
                lblError.Text = "Usuario o contraseña incorrectos.";
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            var shell = new ShellWindow();
            Application.Current.MainWindow = shell;
            shell.Show();
            Close();
        }
    }
}
