using Chamados.Core.Models;
using Chamados.Core.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chamados.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public LoginViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _senha = string.Empty;
        public string Senha
        {
            get => _senha;
            set => SetProperty(ref _senha, value);
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public async Task<Usuario?> LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
            {
                ErrorMessage = "Preencha todos os campos.";
                return null;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var usuario = await _apiService.LoginAsync(Email, Senha);
                if (usuario != null)
                {
                    // Lógica de armazenamento de sessão/token seria aqui
                    return usuario;
                }
                else
                {
                    ErrorMessage = "Credenciais inválidas.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao tentar logar: {ex.Message}";
                return null;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
