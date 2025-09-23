using System.ComponentModel.DataAnnotations;
namespace Filmify.UI.Models;
public class RegisterViewModel
{
    [Required(ErrorMessage = "نام کامل الزامی است")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
    public string Email { get; set; }

    [Required(ErrorMessage = "پسورد الزامی است")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool Agree { get; set; } // Privacy Policy
}
