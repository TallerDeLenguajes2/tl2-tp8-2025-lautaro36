using TiendaElectronica.Models;

namespace TiendaElectronica.ViewModels;

public class PresupuestoCreateViewModels
{
    public int IdPresupuesto { get; set; }
    public string? NombreDestinatario { get; set; }
    public DateOnly FechaCreacion { get; set; }
    public List<Producto> ListadoProductos { get; set; } = new List<Producto>();
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
}