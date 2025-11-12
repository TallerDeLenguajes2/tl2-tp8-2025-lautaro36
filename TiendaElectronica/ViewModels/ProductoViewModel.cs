using TiendaElectronica.Models;

namespace  TiendaElectronica.ViewModels;

public class ProductoViewModel //se usa en index y update. pronto a usarse solo en index porque si el sistema crece puede volverse problematico
{ //cambio de nombre de ProductoIndexViewModel porque este es mas general y representativo, ademas se reutiliza en todas las vistas de producto
    public int IdProducto { get; set; }
    public string? Descripcion { get; set; }
    public int Precio { get; set; }

    public ProductoViewModel() { }

    public ProductoViewModel(int id, string descripcion, int precio)
    {
        IdProducto = id;
        Descripcion = descripcion;
        Precio = precio;
    }

    public ProductoViewModel(Producto producto)
    {
        IdProducto = producto.IdProducto;
        Descripcion = producto.Descripcion;
        Precio = producto.Precio;
    }//recibo direcctamente el producto para facilitar el mapeo y hacerlo mas limpio a la hora de hacer el mapeo desde presuppuesto model a presupuesto view model
}