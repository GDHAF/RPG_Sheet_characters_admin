using System.ComponentModel.DataAnnotations;

namespace RPG_Sheet_characters_admin.Models
{
    public class LoginModel
    {

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public string Usuario { get; set; } = string.Empty;
    }
}
