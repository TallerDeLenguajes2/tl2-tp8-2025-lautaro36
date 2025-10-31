namespace TiendaElectronica.Models;

public class Presupuesto
{
    public int IdPresupuesto { get; set; }
    public string NombreDestinatario { get; set; }
    public DateTime FechaCreacion { get; set; }
    public List<PresupuestoDetalle> ListadoDetalles { get; set; } = new List<PresupuestoDetalle>();

    public Presupuesto(int id, string nombre, DateTime fecha)
    {
        IdPresupuesto = id;
        NombreDestinatario = nombre;
        FechaCreacion = fecha;
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