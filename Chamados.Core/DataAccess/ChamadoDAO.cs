using Npgsql;
using System;
using System.Collections.Generic;
using Chamados.Core.Models;

namespace Chamados.Core.DataAccess
{
    public class ChamadoDAO
    {
        private readonly string _connectionString;

        public ChamadoDAO(string? connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // 1. LISTAR TODOS
        public List<Chamado> ListarTodos()
        {
            List<Chamado> chamados = new List<Chamado>();
            string sql = "SELECT id, usuario_id, titulo, descricao, status, data_criacao, categoria FROM chamados ORDER BY data_criacao DESC";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            chamados.Add(MapToChamado(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar chamados: {ex.Message}", ex);
            }

            return chamados;
        }

        // 2. INSERIR (CRIAR)
        public void Inserir(Chamado chamado)
        {
            string sql = "INSERT INTO chamados (usuario_id, titulo, descricao, status, data_criacao, categoria) " +
                         "VALUES (@usuario_id, @titulo, @descricao, @status, @data_criacao, @categoria)";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("usuario_id", chamado.UsuarioId); 
                        cmd.Parameters.AddWithValue("titulo", chamado.Titulo.Trim());
                        cmd.Parameters.AddWithValue("descricao", chamado.Descricao.Trim());
                        cmd.Parameters.AddWithValue("status", "Aberto");
                        cmd.Parameters.AddWithValue("data_criacao", DateTime.Now);
                        cmd.Parameters.AddWithValue("categoria", chamado.Categoria);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir chamado: {ex.Message}", ex);
            }
        }

        // 3. BUSCAR POR ID
        public Chamado? BuscarPorId(int id)
        {
            string sql = "SELECT id, usuario_id, titulo, descricao, status, data_criacao, categoria FROM chamados WHERE id = @id";
            Chamado? chamado = null;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                chamado = MapToChamado(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar chamado ID {id}: {ex.Message}", ex);
            }
            return chamado;
        }

        // 4. ATUALIZAR (EDITAR)
        public void Atualizar(Chamado chamado)
        {
            string sql = @"
                UPDATE chamados SET
                    titulo = @titulo,
                    descricao = @descricao,
                    categoria = @categoria
                WHERE id = @id";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("titulo", chamado.Titulo.Trim());
                        cmd.Parameters.AddWithValue("descricao", chamado.Descricao.Trim());
                        cmd.Parameters.AddWithValue("categoria", chamado.Categoria);
                        cmd.Parameters.AddWithValue("id", chamado.Id); 

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar atualizar o chamado ID {chamado.Id}: {ex.Message}", ex);
            }
        }

        // 5. ATUALIZAR STATUS
        public bool AtualizarStatus(int idChamado, string novoStatus)
        {
            string sql = "UPDATE chamados SET status = @novoStatus WHERE id = @id";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("novoStatus", novoStatus);
                        cmd.Parameters.AddWithValue("id", idChamado);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar status do chamado ID {idChamado}: {ex.Message}", ex);
            }
        }
        
        // 6. EXCLUIR (DELETAR)
        public void Excluir(int id)
        {
            string sql = "DELETE FROM chamados WHERE id = @id";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception($"Chamado ID {id} não encontrado no banco de dados para exclusão.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar excluir o chamado ID {id}: {ex.Message}", ex);
            }
        }

        private Chamado MapToChamado(NpgsqlDataReader reader)
        {
            return new Chamado
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id")),
                Titulo = reader.GetString(reader.GetOrdinal("titulo")),
                Descricao = reader.GetString(reader.GetOrdinal("descricao")),
                Categoria = reader.GetString(reader.GetOrdinal("categoria")),
                Status = reader.GetString(reader.GetOrdinal("status")),
                DataCriacao = reader.GetDateTime(reader.GetOrdinal("data_criacao"))
            };
        }
    }
}
