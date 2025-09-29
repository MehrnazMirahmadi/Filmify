using System.ComponentModel.DataAnnotations;
namespace Filmify.UI.Models;
public class LoginViewModel
{
    [Required(ErrorMessage = "Email is requier")]
    [EmailAddress(ErrorMessage = "invalid email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "password is requier")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
