using System.Collections.Generic;
using System.Windows.Input;
using lab3.Data;
using lab3.Models;

namespace lab3.ViewModels
{
    public class AulasObjetosViewModel : ViewModelBase
    {
        private readonly AulaRepositorio _repositorio = new AulaRepositorio();
        private List<Aula> _aulas;
        private string _filtro;

        public AulasObjetosViewModel()
        {
            Aulas = _repositorio.Listar();
            BuscarCommand = new Comando(Buscar);
        }

        public List<Aula> Aulas
        {
            get => _aulas;
            set { _aulas = value; Notificar(); }
        }

        public string Filtro
        {
            get => _filtro;
            set { _filtro = value; Notificar(); }
        }

        public ICommand BuscarCommand { get; }

        private void Buscar()
        {
            Aulas = string.IsNullOrWhiteSpace(Filtro)
                ? _repositorio.Listar()
                : _repositorio.BuscarPorNombre(Filtro);
        }
    }
}
