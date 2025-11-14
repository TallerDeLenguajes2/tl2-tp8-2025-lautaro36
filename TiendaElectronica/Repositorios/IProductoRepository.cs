using TiendaElectronica.Models;
using Microsoft.Data.Sqlite;

namespace TiendaElectronica.Repositorios;
public interface IProductoRepository
{
    SqliteConnection GetOpenConnection();
    int CrearProducto(Producto producto);
    bool ModificarProducto(Producto producto);
    List<Producto> GetAll();
    Producto GetDetallesByID(int id);
    int DeleteByID(int id);
}