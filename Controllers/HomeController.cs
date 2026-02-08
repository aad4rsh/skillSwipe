using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using skillSewa.Models;

namespace skillSewa.Controllers;

public class HomeController : Controller
{
    // Home page dekhaune method
    public IActionResult Index()
    {
        return View();
    }

    // Privacy policy page dekhaune method
    public IActionResult Privacy()
    {
        return View();
    }

    // Error aayo bhane yo page dekhaune
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
