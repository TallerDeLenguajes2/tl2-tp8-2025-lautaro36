using TiendaElectronica.Models;

namespace TiendaElectronica.Models;

public class PresupuestoDetalle
{
    public Producto _Producto { get; set; }
    public int Cantidad { get; set; }

    public PresupuestoDetalle(Producto producto, int cantidad)
    {
        Cantidad = cantidad;
        _Producto = producto;
    }

    public int GetPrecio()
    {
        return _Producto.Precio;
    }
}