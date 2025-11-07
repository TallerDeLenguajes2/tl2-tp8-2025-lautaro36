using TiendaElectronica.ViewModels;

namespace TiendaElectronica.Models;

public class Presupuesto
{
    public int IdPresupuesto { get; set; }
    public string? NombreDestinatario { get; set; }
    public DateOnly FechaCreacion { get; set; }
    public List<PresupuestoDetalle> ListadoDetalles { get; set; } = new List<PresupuestoDetalle>();

    public Presupuesto(){}
    public Presupuesto(int id, string nombre, DateOnly fecha)
    {
        IdPresupuesto = id;
        NombreDestinatario = nombre;
        FechaCreacion = fecha;
    }

    public Presupuesto(PresupuestoViewModel viewModel) //en un sistema con verdadero aislamiento  se usa un clase mapper, que solo mapea de un modelo a las viewmodels y viceversa
    {
        IdPresupuesto = viewModel.IdPresupuesto;
        NombreDestinatario = viewModel.NombreDestinatario;
        FechaCreacion = viewModel.FechaCreacion;
    }

    public void agregarDetalles(Producto producto, int cantidad)
    {
        ListadoDetalles.Add(new PresupuestoDetalle(producto, cantidad));
    }

    public int MontoPresupuesto()
    {
        int resultado = 0;
        foreach (var detalle in ListadoDetalles)
        {
            resultado += detalle.Cantidad * detalle.GetPrecio();
        }
        return resultado;
    }
    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21;
    }
    public int CantidadProductos()
    {
        int resultado = 0;
        foreach (var detalle in ListadoDetalles)
        {
            resultado += detalle.Cantidad;
        }
        return resultado;
    }
}