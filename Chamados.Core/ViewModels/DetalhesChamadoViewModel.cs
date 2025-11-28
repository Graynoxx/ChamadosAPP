using Chamados.Core.Models;
using Chamados.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Chamados.Core.ViewModels
{
    public class DetalhesChamadoViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public DetalhesChamadoViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        private Chamado? _chamado;
        public Chamado? Chamado
        {
            get => _chamado;
            set => SetProperty(ref _chamado, value);
        }

        public ObservableCollection<Comentario> Comentarios { get; } = new ObservableCollection<Comentario>();

        private string _novoComentarioTexto = string.Empty;
        public string NovoComentarioTexto
        {
            get => _novoComentarioTexto;
            set => SetProperty(ref _novoComentarioTexto, value);
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public async Task LoadChamadoAsync(int chamadoId)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                Chamado = await _apiService.GetChamadoByIdAsync(chamadoId);
                
                if (Chamado != null && Chamado.Comentarios != null)
                {
                    Comentarios.Clear();
                    foreach (var comentario in Chamado.Comentarios)
                    {
                        Comentarios.Add(comentario);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao carregar detalhes do chamado: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> AdicionarComentarioAsync(int usuarioId, string nomeUsuario)
        {
            if (Chamado == null || string.IsNullOrWhiteSpace(NovoComentarioTexto))
            {
                ErrorMessage = "O chamado não foi carregado ou o comentário está vazio.";
                return false;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var novoComentario = new Comentario
                {
                    ChamadoId = Chamado.Id,
                    UsuarioId = usuarioId,
                    Texto = NovoComentarioTexto,
                    DataCriacao = DateTime.Now,
                    NomeUsuario = nomeUsuario
                };

                var sucesso = await _apiService.AddComentarioAsync(Chamado.Id, novoComentario);

                if (sucesso)
                {
                    Comentarios.Add(novoComentario);
                    NovoComentarioTexto = string.Empty;
                    return true;
                }
                else
                {
                    ErrorMessage = "Falha ao adicionar o comentário.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao adicionar comentário: {ex.Message}";
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
