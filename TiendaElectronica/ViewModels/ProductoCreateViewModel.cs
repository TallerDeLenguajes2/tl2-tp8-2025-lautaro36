using System.ComponentModel.DataAnnotations;
using TiendaElectronica.Models;

namespace TiendaElectronica.ViewModels;

public class ProductoCreateViewModel //se usa en create.cshtml
{
    [Required(ErrorMessage = "La descripcion es obligatoria")]
    [StringLength(100, ErrorMessage = "La descripcion debe tener de 10 a 100 caracteres", MinimumLength = 10)]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(100, 9999999, ErrorMessage = "El {0} debe estar entre {1} y {2}.")]
    public int? Precio { get; set; } //lo hago int? en vez de int para que el campo del form acepte null, y asi, se muestre el errorMessage del parametro required
    public ProductoCreateViewModel() { }
    public ProductoCreateViewModel(string? descripcion, int precio)
    {
        Descripcion = descripcion;
        Precio = precio;
    }
}

//tp10?