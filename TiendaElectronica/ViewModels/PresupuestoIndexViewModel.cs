using TiendaElectronica.Models;

namespace TiendaElectronica.ViewModels;

public class PresupuestoIndexViewModels
{
    public int IdPresupuesto { get; set; }
    public string NombreDestinatario { get; set; }
    public DateOnly FechaCreacion { get; set; }
    public List<DetalleIndexViewModel> ListadoDetalles { get; set; } = new List<DetalleIndexViewModel>();
    public int MontoPresupuesto { get; set; }
    public double MontoPresupuestoConIva { get; set; }
    public int CantidadProductos { get; set; }

    public PresupuestoIndexViewModels(Presupuesto presupuesto)
    {
        IdPresupuesto = presupuesto.IdPresupuesto;
        NombreDestinatario = presupuesto.NombreDestinatario;
        FechaCreacion = presupuesto.FechaCreacion;
        ListadoDetalles = presupuesto.ListadoDetalles.Select(elementoDetalle => new DetalleIndexViewModel(elementoDetalle)).ToList();//recibo un presupuesto model. desde ahi mapeo cada campo, el listado detalles con el select, asi voy pasando uno por uno los detalles al constructor de DetalleIndexViewModel. hago tolist al final porque  select devuele IEnumerable
        MontoPresupuesto = presupuesto.MontoPresupuesto();
        MontoPresupuestoConIva = presupuesto.MontoPresupuestoConIva();
        CantidadProductos = presupuesto.CantidadProductos();
    }
}