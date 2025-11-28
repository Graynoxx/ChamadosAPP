using Chamados.Core.Models;
using Chamados.Core.Services;
using System.Threading.Tasks;

namespace Chamados.Core.ViewModels
{
    public class NovoChamadoViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public NovoChamadoViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        private string _titulo = string.Empty;
        public string Titulo
        {
            get => _titulo;
            set => SetProperty(ref _titulo, value);
        }

        private string _descricao = string.Empty;
        public string Descricao
        {
            get => _descricao;
            set => SetProperty(ref _descricao, value);
        }

        private string _categoria = string.Empty;
        public string Categoria
        {
            get => _categoria;
            set => SetProperty(ref _categoria, value);
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public List<string> CategoriasDisponiveis { get; } = new List<string>
        {
            "Suporte Técnico",
            "Financeiro",
            "Dúvida",
            "Sugestão"
        };

        public async Task<Chamado?> EnviarChamadoAsync(int usuarioId)
        {
            if (string.IsNullOrWhiteSpace(Titulo) || string.IsNullOrWhiteSpace(Descricao) || string.IsNullOrWhiteSpace(Categoria))
            {
                ErrorMessage = "Todos os campos são obrigatórios.";
                return null;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var novoChamado = new Chamado
                {
                    Titulo = Titulo,
                    Descricao = Descricao,
                    Categoria = Categoria,
                    UsuarioId = usuarioId,
                    Status = "ABERTO", // Status inicial
                    DataCriacao = DateTime.Now
                };

                var chamadoCriado = await _apiService.CreateChamadoAsync(novoChamado);
                
                if (chamadoCriado != null)
                {
                    // Limpar campos após sucesso
                    Titulo = string.Empty;
                    Descricao = string.Empty;
                    Categoria = string.Empty;
                }
                else
                {
                    ErrorMessage = "Falha ao criar o chamado. Tente novamente.";
                }

                return chamadoCriado;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao enviar chamado: {ex.Message}";
                return null;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
