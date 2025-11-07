using TiendaElectronica.Models;

namespace TiendaElectronica.ViewModels; 
public class PresupuestoViewModel
{
    public int IdPresupuesto { get; set; }
    public string? NombreDestinatario { get; set; }
    public DateOnly FechaCreacion { get; set; }

    public PresupuestoViewModel(){}
    public PresupuestoViewModel(int id, string? nombre, DateOnly fecha)
    {
        IdPresupuesto = id;
        NombreDestinatario = nombre;
        FechaCreacion = fecha;
    }
}