using TiendaElectronica.Models;
using TiendaElectronica.ViewModels;

public class DetalleIndexViewModel //se usa en presupuestoIndexViewModel, ninguna view lo usa especificamente sino al viewModel que lo contiene
{
    public ProductoViewModel Producto { get; set; }
    public int Cantidad { get; set; }

    public DetalleIndexViewModel(PresupuestoDetalle detalle)
    {
        Producto = new ProductoViewModel(detalle._Producto);
        Cantidad = detalle.Cantidad;
    }//recibo un presupuestoDetalle para facilitar el mapeo de presupuesto model a presupuesto view model

    
    public int GetPrecio()
    {
        return Producto.Precio;
    }
}