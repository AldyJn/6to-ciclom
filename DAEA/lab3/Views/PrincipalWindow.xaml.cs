using System.Windows;
using lab3.ViewModels;

namespace lab3.Views
{
    public partial class PrincipalWindow : Window
    {
        public PrincipalWindow()
        {
            InitializeComponent();
            DataContext = new PrincipalViewModel();
        }
    }
}
