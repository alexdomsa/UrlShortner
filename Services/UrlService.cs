using UrlShortner.Models;
using UrlShortner.Data;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

namespace UrlShortner.Services;

public class UrlService
{
    private readonly UrlShortnerContext _context;
    private readonly MD5 _hashingService;

    public UrlService(UrlShortnerContext context)
    {
        _context = context;
        _hashingService = MD5.Create();
    }

    public IEnumerable<Url> GetAll()
    {
        return _context.Urls
            .AsNoTracking()
            .ToList();
    }

    public Url? GetUrlByShortIdentifier(string shortIdentifier, string userId)
    {
        return _context.Urls
            .AsNoTracking()
            .SingleOrDefault(item => item.ShortIdentifier == shortIdentifier && item.UserId == userId);
    }

    public Url RegisterUrl(Url url, string userId)
    {
        var shortIdentifier = ComputeShortIdentifier(url.OriginalUrl, userId);

        var alreadyRegisteredUrl = GetUrlByShortIdentifier(shortIdentifier, userId);
        if (alreadyRegisteredUrl != null)
        {
            return alreadyRegisteredUrl;
        }

        url.ShortIdentifier = shortIdentifier;
        url.UserId = userId;
        _context.Urls.Add(url);
        _context.SaveChanges();

        return url;
    }

    private string ComputeShortIdentifier(string originalUrl, string userId)
    {
        // use a combined value for computing shortIdentifier, so that different ussers
        // can perform the shortening for the same URL
        var urlBytes = Encoding.UTF8.GetBytes($"{originalUrl}{userId}");
        var hashBytes = _hashingService.ComputeHash(urlBytes);

        return Convert.ToHexString(hashBytes).ToLower();
    }
}