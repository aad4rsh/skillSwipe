using Microsoft.EntityFrameworkCore;
using skillSewa.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

// Web application builder start gareko
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// MVC (Model-View-Controller) service add gareko
builder.Services.AddControllersWithViews();

// Database context add gareko (SQL Server use garne)
builder.Services.AddDbContext<SkillSwapContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SkillSwapContext") ?? throw new InvalidOperationException("Connection string 'SkillSwapContext' not found.")));

// Authentication service add gareko
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// App build gareko
var app = builder.Build();

// Configure the HTTP request pipeline.
// Development mode ma chaina bhane error handler use garne
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

// Https redirection ra routing enable gareko
app.UseHttpsRedirection();
app.UseRouting();

// Authorization ra Authentication enable gareko
app.UseAuthentication();
app.UseAuthorization();

// Static file haru serve garna (css, js, images)
app.MapStaticAssets();

// Default controller route set gareko (Home/Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// App run gareko
app.Run();
