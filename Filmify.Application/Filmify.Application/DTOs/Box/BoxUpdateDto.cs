using System.ComponentModel.DataAnnotations;

namespace Filmify.Application.DTOs.Box;

public class BoxUpdateDto
{
    [Required]
    public string BoxName { get; set; } = null!;
    [Required]
    public string Slug { get; set; } = null!;
}
