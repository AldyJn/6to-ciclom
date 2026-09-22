using System.Windows;
using lab6.ViewModels;

namespace lab6.Views
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
