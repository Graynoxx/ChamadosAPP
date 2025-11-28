using Microsoft.AspNetCore.Mvc;
using Chamados.Core.DataAccess;
using Chamados.Core.Models;

namespace Chamados.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioDAO _usuarioDAO;

        public AuthController(UsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
            {
                return BadRequest(new { Message = "Email e senha são obrigatórios." });
            }

            try
            {
                var usuario = _usuarioDAO.Autenticar(request.Email, request.Senha);

                if (usuario == null)
                {
                    return Unauthorized(new { Message = "Credenciais inválidas." });
                }

                // Em um cenário real, um token JWT seria gerado aqui.
                // Para simplificar, retornamos o objeto Usuario (sem a senha/hash).
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                // Logar a exceção
                return StatusCode(500, new { Message = "Erro interno do servidor durante a autenticação." });
            }
        }
    }

    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}
