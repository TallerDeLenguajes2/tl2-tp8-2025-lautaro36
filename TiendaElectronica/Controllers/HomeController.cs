using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TiendaElectronica.Models;
using miAuthService = TiendaElectronica.Repositorios.IAuthenticationService;

namespace TiendaElectronica.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly miAuthService _authenticationService;

    public HomeController(ILogger<HomeController> logger, miAuthService authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
    }

    public IActionResult Index()
    {
        if (!_authenticationService.IsAuthenticated())
        {
            return RedirectToAction("Login", "Account");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
