using TiendaElectronica.Models;

namespace TiendaElectronica.ViewModels; 
public class PresupuestoUpdateViewModel //se usa en update, deberia renombrarlo
{
    public int IdPresupuesto { get; set; }
    public string? NombreDestinatario { get; set; }
    public DateOnly FechaCreacion { get; set; }

    public PresupuestoUpdateViewModel(){}
    public PresupuestoUpdateViewModel(int id, string? nombre, DateOnly fecha)
    {
        IdPresupuesto = id;
        NombreDestinatario = nombre;
        FechaCreacion = fecha;
    }
}