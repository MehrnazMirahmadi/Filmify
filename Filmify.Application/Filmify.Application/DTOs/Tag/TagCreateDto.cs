using System.ComponentModel.DataAnnotations;

namespace Filmify.Application.DTOs.Tag;

public class TagCreateDto
{
    [Required]
    public string TagText { get; set; } = null!;
}