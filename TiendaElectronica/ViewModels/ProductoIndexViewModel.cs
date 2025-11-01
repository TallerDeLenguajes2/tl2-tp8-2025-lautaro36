using TiendaElectronica.Models;

namespace  TiendaElectronica.ViewModels;

public class ProductoIndexViewModel //usando ViewModels en lugar de usar directamente el modelo, solo se exponen los tres campos necesarios. Es más seguro, flexible y mantenible
{
    public int IdProducto { get; set; }
    public string? Descripcion { get; set; }
    public int Precio { get; set; }
    public ProductoIndexViewModel(Producto producto)
    {
        IdProducto = producto.IdProducto;
        Descripcion = producto.Descripcion;
        Precio = producto.Precio;
    }//recibo direcctamente el producto para facilitar el mapeo y hacerlo mas limpio a la hora de hacer el mapeo desde presuppuesto model a presupuesto view model
}