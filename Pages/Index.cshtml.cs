using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http.Extensions;
using UrlShortner.Models;
using UrlShortner.Services;

namespace UrlShortner.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UrlService _service;

    [BindProperty]
    public Url UrlData { get; set; } = default!;

    [BindProperty]
    public string? ShortenedUrl { get; set; }

    public IndexModel(ILogger<IndexModel> logger, UrlService service)
    {
        _logger = logger;
        _service = service;
    }

    public void OnGet()
    {

    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid || UrlData == null)
        {
            return Page();
        }

        var registeredUrl = _service.RegisterUrl(UrlData);

        ShortenedUrl = $"{HttpContext.Request.GetDisplayUrl()}{registeredUrl.ShortIdentifier}";

        return Page();
    }
}
