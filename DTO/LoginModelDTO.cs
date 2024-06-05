using System.ComponentModel.DataAnnotations;

namespace MegaLivros.DTO;

public class LoginModelDTO
{
    [Required(ErrorMessage ="User name is required")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "User name is required")]
    public string? Password { get; set; }
}
