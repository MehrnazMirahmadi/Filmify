using System.ComponentModel.DataAnnotations;
namespace Filmify.UI.Models;
public class RegisterViewModel
{
    [Required(ErrorMessage = "name is requier")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email is requeir")]
    [EmailAddress(ErrorMessage = "Email not valid")]
    public string Email { get; set; }

    [Required(ErrorMessage = "password is requeir")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool Agree { get; set; } // Privacy Policy
}
