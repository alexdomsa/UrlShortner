using UrlShortner.Models;
using UrlShortner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace UrlShortner.Pages;

[Authorize]
public class RedirectUrlModel : PageModel
{
    private readonly ILogger<RedirectUrlModel> _logger;
    private readonly UrlService _service;

    public RedirectUrlModel(ILogger<RedirectUrlModel> logger, UrlService service)
    {
        _logger = logger;
        _service = service;
    }

    public IActionResult OnGet(string shortIdentifier)
    {
        var url = _service.GetUrlByShortIdentifier(shortIdentifier,
            User.FindFirstValue(ClaimTypes.NameIdentifier)
        );

        if (url is null)
        {
            return NotFound();
        }

        return Redirect(url.OriginalUrl);
    }
}
