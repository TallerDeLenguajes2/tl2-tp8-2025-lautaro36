using System.ComponentModel.DataAnnotations;

namespace TiendaElectronica.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Debe ingresar su nombre de usuario.")]
    public string? Username {get ; set;}

    [Required(ErrorMessage = "Debe ingresar su contraseña.")]
    public string? Password {get ; set;}
    public string? ErrorMessage {get ; set;}
    public LoginViewModel()
    {
    }
}