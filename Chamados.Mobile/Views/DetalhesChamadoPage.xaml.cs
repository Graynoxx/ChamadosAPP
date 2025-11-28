using Microsoft.Maui.Controls;
using Chamados.Core.ViewModels;

namespace Chamados.Mobile.Views
{
    [QueryProperty(nameof(ChamadoId), "ChamadoId")]
    public partial class DetalhesChamadoPage : ContentPage
    {
        private int _chamadoId;
        public int ChamadoId
        {
            get => _chamadoId;
            set
            {
                _chamadoId = value;
                // Em um app real, o LoadChamadoAsync seria chamado aqui.
                // ((DetalhesChamadoViewModel)BindingContext).LoadChamadoAsync(value);
            }
        }

        public DetalhesChamadoPage()
        {
            InitializeComponent();
            // O BindingContext é definido no XAML.
        }
    }
}
