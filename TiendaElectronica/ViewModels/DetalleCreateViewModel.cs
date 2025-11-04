using TiendaElectronica.Models;

namespace TiendaElectronica.ViewModels;

public class DetalleCreateViewModel
{

    public int IdPresupuesto { get; set; }
    public List<Producto> ListadoProductos { get; set; } = new List<Producto>();

    public DetalleCreateViewModel(int idPresupuesto, List<Producto> productos)
    {
        ListadoProductos = productos;
        IdPresupuesto = idPresupuesto;
    }
}