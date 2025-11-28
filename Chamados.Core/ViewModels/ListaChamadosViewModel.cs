using Chamados.Core.Models;
using Chamados.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Chamados.Core.ViewModels
{
    public class ListaChamadosViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Chamado> Chamados { get; } = new ObservableCollection<Chamado>();

        public ListaChamadosViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task LoadChamadosAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Chamados.Clear();
                var chamadosList = await _apiService.GetChamadosAsync();
                if (chamadosList != null)
                {
                    foreach (var chamado in chamadosList)
                    {
                        Chamados.Add(chamado);
                    }
                }
            }
            catch (Exception ex)
            {
                // Tratar erro
                Console.WriteLine($"Erro ao carregar chamados: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
