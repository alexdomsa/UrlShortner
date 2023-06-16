using Microsoft.EntityFrameworkCore;
using UrlShortner.Models;

namespace UrlShortner.Data;

public class UrlShortnerContext : DbContext
{
    public UrlShortnerContext(DbContextOptions<UrlShortnerContext> options) : base(options) { }

    public DbSet<Url> Urls => Set<Url>();
}