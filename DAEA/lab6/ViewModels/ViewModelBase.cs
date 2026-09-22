using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace lab6.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notificar([CallerMemberName] string propiedad = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
        }

        /// <summary>Concuerda el numero con el sustantivo del mensaje.</summary>
        protected static string Contar(int cantidad, string singular, string plural)
            => cantidad + " " + (cantidad == 1 ? singular : plural);
    }
}
