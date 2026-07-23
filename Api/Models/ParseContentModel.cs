using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class ParseContentModel
{
    [Required(ErrorMessage = "Field 'type' is required")]
    public ContentType? Type { get; set; }

    [Required(ErrorMessage = "Field 'content' is required")]
    public string Content { get; set; } = string.Empty;
}