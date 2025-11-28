using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Chamados.Core.Models
{
    public class Chamado
    {
        public int Id { get; set; } 
        public int UsuarioId { get; set; } // Renomeado de IdUsuario para UsuarioId para consistência
        
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título não pode exceder 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;
        
        public string Categoria { get; set; } = "Geral"; 

        public string Status { get; set; } = "Aberto"; // Aberto, Em Andamento, Resolvido
        public DateTime DataCriacao { get; set; } = DateTime.Now; // Renomeado de DataAbertura para DataCriacao para consistência
        
        // Propriedade para exibição no mobile
        public string DataCriacaoFormatada => DataCriacao.ToString("dd/MM/yyyy");

        // Adicionado para o design mobile (Detalhes do Chamado)
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
