using System.ComponentModel.DataAnnotations;
using TiendaElectronica.Models;

namespace  TiendaElectronica.ViewModels;

public class ProductoUpdateViewModel
{ 
    public int IdProducto { get; set; }
    
    [Required(ErrorMessage = "La descripcion es obligatoria")]
    [StringLength(100, ErrorMessage = "La descripcion debe tener de 10 a 100 caracteres", MinimumLength = 10)]
    public string? Descripcion { get; set; }
    
    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(100, 9999999, 
        ErrorMessage = "El {0} debe estar entre {1} y {2}.")]
    public int? Precio { get; set; }

    public ProductoUpdateViewModel() { }

    public ProductoUpdateViewModel(int id, string descripcion, int precio)
    {
        IdProducto = id;
        Descripcion = descripcion;
        Precio = precio;
    }
}