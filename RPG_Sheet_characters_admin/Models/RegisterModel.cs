using System.ComponentModel.DataAnnotations;

namespace RPG_Sheet_characters_admin.Models
{
    public class RegisterModel
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
        [Compare(nameof(Senha))]
        public string ConfirmSenha { get; set; } = string.Empty;

        [Required]
        public string Usuario { get; set; } = string.Empty;
    }
}
