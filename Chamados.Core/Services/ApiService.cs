using System.Net.Http.Json;
using Chamados.Core.Models;

namespace Chamados.Core.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5000/api/"; // URL da Web API

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        // --- Usuário ---
        public async Task<Usuario?> LoginAsync(string email, string senha)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", new { Email = email, Senha = senha });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Usuario>();
            }
            return null;
        }

        // --- Chamados ---
        public async Task<List<Chamado>?> GetChamadosAsync()
        {
            var response = await _httpClient.GetAsync("chamados");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Chamado>>();
            }
            return new List<Chamado>();
        }

        public async Task<Chamado?> GetChamadoByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"chamados/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Chamado>();
            }
            return null;
        }

        public async Task<Chamado?> CreateChamadoAsync(Chamado chamado)
        {
            var response = await _httpClient.PostAsJsonAsync("chamados", chamado);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Chamado>();
            }
            return null;
        }

        // --- Comentários ---
        public async Task<bool> AddComentarioAsync(int chamadoId, Comentario comentario)
        {
            var response = await _httpClient.PostAsJsonAsync($"chamados/{chamadoId}/comentarios", comentario);
            return response.IsSuccessStatusCode;
        }
    }
}
