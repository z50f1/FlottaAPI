using System.ComponentModel.DataAnnotations;

namespace Flotta_API.DTOs;

public class LoginDTO
{
    [Required]
    public string LoginName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
