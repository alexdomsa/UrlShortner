using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Models;

public class Url
{
    public int Id { get; set; }

    [Required]
    [Url]
    [Display(Name = "URL")]
    public string OriginalUrl { get; set; } = default!;

    [Required]
    public string? ShortIdentifier { get; set; }

    [Required]
    public string? UserId { get; set; }
}