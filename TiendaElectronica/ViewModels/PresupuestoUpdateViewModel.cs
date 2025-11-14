using System.ComponentModel.DataAnnotations;
using TiendaElectronica.Models;

namespace TiendaElectronica.ViewModels; 
public class PresupuestoUpdateViewModel //se usa en update, deberia renombrarlo
{
    public int IdPresupuesto { get; set; }

    [Required(ErrorMessage = "El nombre del destinatario es obligatorio.")]
    [StringLength(100, ErrorMessage = "El {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 10)]
    public string? NombreDestinatario { get; set; }

    [Required(ErrorMessage = "La fecha de modificacion es obligatoria.")]
    public DateOnly? FechaCreacion { get; set; }

    public PresupuestoUpdateViewModel(){}
    public PresupuestoUpdateViewModel(int id, string? nombre, DateOnly fecha)
    {
        IdPresupuesto = id;
        NombreDestinatario = nombre;
        FechaCreacion = fecha;
    }
}