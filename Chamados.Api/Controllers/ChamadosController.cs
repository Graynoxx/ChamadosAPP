using Microsoft.AspNetCore.Mvc;
using Chamados.Core.DataAccess;
using Chamados.Core.Models;

namespace Chamados.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChamadosController : ControllerBase
    {
        private readonly ChamadoDAO _chamadoDAO;
        // Em um projeto real, teríamos um ComentarioDAO aqui.
        // Como o projeto original não tinha, vamos simular a inclusão de comentários.

        public ChamadosController(ChamadoDAO chamadoDAO)
        {
            _chamadoDAO = chamadoDAO;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var chamados = _chamadoDAO.ListarTodos();
                return Ok(chamados);
            }
            catch (Exception ex)
            {
                // Logar a exceção
                return StatusCode(500, new { Message = "Erro ao listar chamados." });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var chamado = _chamadoDAO.BuscarPorId(id);
                if (chamado == null)
                {
                    return NotFound(new { Message = $"Chamado ID {id} não encontrado." });
                }
                
                // Simulação de comentários para o design mobile
                // Em um projeto real, você buscaria os comentários do banco de dados.
                chamado.Comentarios.Add(new Comentario { Id = 1, ChamadoId = id, UsuarioId = 1, NomeUsuario = "Suporte", Texto = "Recebemos sua solicitação e já estamos analisando.", DataCriacao = DateTime.Now.AddHours(-1) });
                chamado.Comentarios.Add(new Comentario { Id = 2, ChamadoId = id, UsuarioId = 2, NomeUsuario = "Você", Texto = "Olá, abri este chamado para...", DataCriacao = DateTime.Now.AddHours(-2) });

                return Ok(chamado);
            }
            catch (Exception ex)
            {
                // Logar a exceção
                return StatusCode(500, new { Message = $"Erro ao buscar chamado ID {id}." });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Chamado chamado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // O ID do usuário deve vir do contexto de autenticação em um app real.
                // Aqui, assumimos que o objeto Chamado já tem o UsuarioId preenchido pelo cliente.
                _chamadoDAO.Inserir(chamado);
                return CreatedAtAction(nameof(Get), new { id = chamado.Id }, chamado);
            }
            catch (Exception ex)
            {
                // Logar a exceção
                return StatusCode(500, new { Message = "Erro ao criar chamado." });
            }
        }

        [HttpPost("{chamadoId}/comentarios")]
        public IActionResult AddComentario(int chamadoId, [FromBody] Comentario comentario)
        {
            if (chamadoId != comentario.ChamadoId)
            {
                return BadRequest(new { Message = "ID do chamado inconsistente." });
            }

            // Em um projeto real, você inseriria o comentário no banco de dados.
            // Aqui, apenas simulamos o sucesso.
            try
            {
                // _comentarioDAO.Inserir(comentario);
                return Ok(new { Message = "Comentário adicionado com sucesso." });
            }
            catch (Exception ex)
            {
                // Logar a exceção
                return StatusCode(500, new { Message = "Erro ao adicionar comentário." });
            }
        }
    }
}
