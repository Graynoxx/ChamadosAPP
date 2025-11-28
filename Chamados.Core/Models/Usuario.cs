using System.ComponentModel.DataAnnotations;

namespace Chamados.Core.Models
{
    public class Usuario
    {
        public int Id { get; set; } 
        
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; } = string.Empty;
        
        // A senha não será armazenada no modelo, apenas usada para login
        public string Senha { get; set; } = string.Empty; 
        
        // Hash da senha armazenado no banco de dados
        public string SenhaHash { get; set; } = string.Empty;
    }
}
