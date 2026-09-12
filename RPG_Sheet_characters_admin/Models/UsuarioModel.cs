using System.ComponentModel.DataAnnotations;

namespace RPG_Sheet_characters_admin.Models
{
    public class UsuarioModel
    {
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public string Usuario { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;

    }
}
