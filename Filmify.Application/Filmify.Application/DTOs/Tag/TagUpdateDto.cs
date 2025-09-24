using System.ComponentModel.DataAnnotations;

namespace Filmify.Application.DTOs.Tag;
public class TagUpdateDto
{
   
    [Required]
    public string TagText { get; set; } = null!;
}