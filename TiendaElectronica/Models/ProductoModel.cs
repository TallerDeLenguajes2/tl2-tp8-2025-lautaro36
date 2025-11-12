using TiendaElectronica.ViewModels;

namespace TiendaElectronica.Models;

public class Producto
{
    public int IdProducto { get; set; }
    public string? Descripcion { get; set; }
    public int Precio { get; set; }
    public Producto()
    {
    }
    public Producto(int id, string descripcion, int precio)
    {
        IdProducto = id;
        Descripcion = descripcion;
        Precio = precio;
    }

    public Producto(ProductoViewModel viewModel) //preguntar si es correcto tener un mapeador en el modelo (tp9). si es pero en proyectos grandes se usan mappers para verdadero aislamiento
    {
        IdProducto = viewModel.IdProducto;
        Descripcion = viewModel.Descripcion;
        Precio = viewModel.Precio;
    }
    public Producto(ProductoCreateViewModel viewModel) //constructor para create
    {
        Descripcion = viewModel.Descripcion;
        Precio = Convert.ToInt32(viewModel.Precio);
    }

}