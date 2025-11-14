using System.ComponentModel.DataAnnotations;
using TiendaElectronica.Models;

namespace TiendaElectronica.ViewModels;

public class DetalleCreateViewModel //se usa en createDetalle.cshtml
//modifico la clase y agrego idproducto y cantidad, dado que lo correcto es enviarle al metodo post el viewmodel con los datos y no 
//cada dato por separado. antes mi clase solo tenia los dos primeros campos porque los necesitaba para armar el form en la vista, el 
//idProd y la cantidad eran enviados a traves de la query sueltos. eso no me permitia hacer las validaciones correspondientes.
{
    public int IdPresupuesto { get; set; }
    public List<Producto> ListadoProductos { get; set; } = new List<Producto>();

    [Required(ErrorMessage = "Elegir un producto es obligatorio.")]
    public int? IdProducto { get; set; }

    [Required(ErrorMessage = "La cantidad del producto es obligatoria")]
    [Range(1, 50, ErrorMessage = "La {0} debe ser mayor a {1} y menor a {2}")]
    public int? Cantidad { get; set; }
    
    public DetalleCreateViewModel() { }
    public DetalleCreateViewModel(int idPresupuesto, List<Producto> productos)
    {
        ListadoProductos = productos;
        IdPresupuesto = idPresupuesto;
    }
}