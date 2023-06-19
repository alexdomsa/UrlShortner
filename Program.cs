using UrlShortner.Data;
using UrlShortner.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("UrlShortnerConnection");
builder.Services.AddDbContext<UrlShortnerContext>(options => options.UseSqlServer(connectionString));

var authConnectionString = builder.Configuration.GetConnectionString("UrlShortnerIdentityDbContextConnection");
builder.Services.AddDbContext<UrlShortnerIdentityDbContext>(options => options.UseSqlServer(authConnectionString)); 
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UrlShortnerIdentityDbContext>();

builder.Services.AddScoped<UrlService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
