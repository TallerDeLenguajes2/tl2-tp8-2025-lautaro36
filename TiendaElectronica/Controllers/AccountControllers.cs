using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TiendaElectronica.Models;
using TiendaElectronica.ViewModels;
using TiendaElectronica.Repositorios;
using miAuthService = TiendaElectronica.Repositorios.IAuthenticationService; //habia una ambiguedad entre mi servicio 'TiendaElectronica.Repositorios.IAuthenticationService' y 'Microsoft.AspNetCore.Authentication.IAuthenticationService'

namespace TiendaElectronica.Controllers;

public class AccountController : Controller
{
    private readonly miAuthService _authenticationService;

    public AccountController(miAuthService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if(_authenticationService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Home");
        }
        return View(new LoginViewModel());
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel viewModel)
    {
        if(_authenticationService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Home");
        }
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        bool resultado = _authenticationService.Login(viewModel.Username, viewModel.Password);
        if(!resultado) 
        {
            viewModel.ErrorMessage = "Credenciales no validas";
            return View(viewModel);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        _authenticationService.Logout();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult DeniedAccess()
    {
        return View("DeniedAccess");
    }
}