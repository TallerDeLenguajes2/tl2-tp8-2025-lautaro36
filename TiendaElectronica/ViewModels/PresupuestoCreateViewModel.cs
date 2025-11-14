using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TiendaElectronica.Models;

namespace TiendaElectronica.ViewModels;

public class PresupuestoCreateViewModel //se  usa en create.cstml
{
    public int IdPresupuesto { get; set; }

    [DisplayName("Nombre")]
    [Required(ErrorMessage = "El nombre del destinatario es obligatorio.")]
    [StringLength(100, ErrorMessage = "El {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 10)]
    public string? NombreDestinatario { get; set; }

    [Required(ErrorMessage = "La fecha de creacion es obligatoria.")]
    public DateOnly? FechaCreacion { get; set; }


    public List<Producto> ListadoProductos { get; set; } = new List<Producto>();
    
    [Required(ErrorMessage = "Elegir un producto es obligatorio.")]   
    public int? IdProducto { get; set; }

    [Required(ErrorMessage = "La cantidad del producto es obligatoria")]
    [Range(1, 50, ErrorMessage = "La {0} debe ser mayor a {1} y menor a {2}")]
    public int? Cantidad { get; set; }
}