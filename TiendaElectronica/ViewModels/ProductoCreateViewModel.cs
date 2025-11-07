using TiendaElectronica.Models;

namespace TiendaElectronica.ViewModels;

public class ProductoCreateViewModel
{
    public string? Descripcion { get; set; }
    public int Precio { get; set; }
    public ProductoCreateViewModel(){}
    public ProductoCreateViewModel(string? descripcion, int precio)
    {
        Descripcion = descripcion;
        Precio = precio;
    }
}