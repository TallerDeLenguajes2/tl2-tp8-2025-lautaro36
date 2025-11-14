using Microsoft.Data.Sqlite;
using TiendaElectronica.Models;
using TiendaElectronica.ViewModels; 

namespace TiendaElectronica.Repositorios;
public interface IPresupuestoRepository
{
    SqliteConnection GetConnection();
    int CrearPresupuesto(Presupuesto presupuesto);
    int AgregarAlPresupuesto(int idPresupuesto, int idProducto, int cantidad);
    Presupuesto GetDetallesById(int id);
    List<Presupuesto> GetAll();
    int DeleteById(int id);
    bool ModificarPresupuesto(Presupuesto presupuesto);
    bool UpdateCantidades(DetalleUpCantidadesViewModel detalle);
    bool DeleteDetalle(int IdPresupuesto, int IdProducto);
}