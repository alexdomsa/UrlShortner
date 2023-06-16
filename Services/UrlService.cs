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

    public Url? GetUrlByShortIdentifier(string shortIdentifier)
    {
        return _context.Urls
            .AsNoTracking()
            .SingleOrDefault(item => item.ShortIdentifier == shortIdentifier);
    }

    public Url? RegisterUrl(Url url)
    {
        var shortIdentifier = ComputeShortIdentifier(url.OriginalUrl);

        var alreadyRegisteredUrl = GetUrlByShortIdentifier(shortIdentifier);
        if (alreadyRegisteredUrl != null)
        {
            return alreadyRegisteredUrl;
        }

        url.ShortIdentifier = shortIdentifier;
        _context.Urls.Add(url);
        _context.SaveChanges();

        return url;
    }

    private string ComputeShortIdentifier(string originalUrl)
    {
        var urlBytes = Encoding.UTF8.GetBytes(originalUrl);
        var hashBytes = _hashingService.ComputeHash(urlBytes);
        return Convert.ToHexString(hashBytes);
    }
}