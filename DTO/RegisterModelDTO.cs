using System.ComponentModel.DataAnnotations;

namespace MegaLivros.DTO;

public class RegisterModelDTO
{
    [Required(ErrorMessage = "User name is required")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "User name is required")]
    public string? Password { get; set; }

    [EmailAddress] 
    [Required(ErrorMessage = "User name is required")]
    public string? Email { get; set; }
}
