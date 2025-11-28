using BCrypt.Net; 
using Npgsql;
using Chamados.Core.Models;

namespace Chamados.Core.DataAccess
{
    public class UsuarioDAO
    {
        private readonly string _connectionString;

        public UsuarioDAO(string? connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public Usuario? Autenticar(string email, string senha)
        {
            string sql = "SELECT id, nome, email, senha_hash FROM usuarios WHERE email = @email";

            string emailLimpo = email.Trim();
            string senhaLimpa = senha.Trim();

            Usuario? usuario = null;

            try
            {
                using (NpgsqlConnection conexao = new NpgsqlConnection(_connectionString))
                {
                    conexao.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("email", emailLimpo);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashArmazenado = reader["senha_hash"]?.ToString() ?? string.Empty;

                                // 1. VERIFICAÇÃO DO BCrypt
                                if (BCrypt.Net.BCrypt.Verify(senhaLimpa, hashArmazenado))
                                {
                                    // Login OK!
                                    usuario = new Usuario
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                                        Nome = reader["nome"]?.ToString() ?? string.Empty,
                                        Email = reader["email"]?.ToString() ?? string.Empty,
                                        Senha = string.Empty // Nunca retorne a senha/hash
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao acessar o banco de dados durante a autenticação.", ex);
            }

            return usuario;
        }
        
        // Método para registrar um novo usuário (necessário para um app funcional)
        public void Registrar(Usuario usuario, string senha)
        {
            // 1. Gerar o hash da senha
            string senhaHash = BCrypt.Net.BCrypt.HashPassword(senha.Trim());
            
            // 2. Inserir no banco de dados
            string sql = "INSERT INTO usuarios (nome, email, senha_hash) VALUES (@nome, @email, @senha_hash)";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("nome", usuario.Nome.Trim());
                        cmd.Parameters.AddWithValue("email", usuario.Email.Trim());
                        cmd.Parameters.AddWithValue("senha_hash", senhaHash);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao registrar usuário: {ex.Message}", ex);
            }
        }
    }
}
