using Microsoft.Data.Sqlite;
using TiendaElectronica.Models;

namespace TiendaElectronica.Repositorios;

public interface IProductoRepository
{
    SqliteConnection GetOpenConnection();
    int CrearProducto(Producto producto);
    bool ModificarProducto(int id, Producto producto);
    List<Producto> GetAll();
    Producto GetDetallesByID(int id);
    int DeleteByID(int id);
}
public class ProductoRepository
{
    string stringConnection = "Data Source=Tienda.db;Cache=Shared";
    public SqliteConnection GetOpenConnection()
    {
        SqliteConnection connection = new SqliteConnection(stringConnection);
        connection.Open();
        return connection;
    }
    public int CrearProducto(Producto producto) //● Crear un nuevo Producto. (recibe un objeto Producto)
    {
        using (var connection = GetOpenConnection())
        {
            string queryString = $"INSERT INTO Productos (Descripcion, Precio) VALUES (@descripcion, @precio)";
            var command = new SqliteCommand(queryString, connection);

            command.Parameters.Add(new SqliteParameter("@descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@precio", producto.Precio));

            var resultado = command.ExecuteNonQuery(); //devolver el int para mostrar la fila afectada
            connection.Close();
            return resultado;
        }
    }
    public bool ModificarProducto(int id, Producto producto)
    {
        using (var connection = GetOpenConnection())
        {
            string queryString = $"UPDATE Productos SET Descripcion = @descripcion, Precio = @precio WHERE IdProducto = @id";
            var command = new SqliteCommand(queryString, connection);

            command.Parameters.Add(new SqliteParameter("@descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@precio", producto.Precio));
            command.Parameters.Add(new SqliteParameter("@id", id));

            var filasAfectadas = command.ExecuteNonQuery();
            connection.Close();
            return filasAfectadas >= 1;
        }
    }
    public List<Producto> GetAll() //● Listar todos los Productos registrados. (devuelve un List de Producto)
    {
        using var connection = GetOpenConnection();
        var queryString = $"SELECT * FROM Productos";
        List<Producto> productos = new List<Producto>();

        var command = new SqliteCommand(queryString, connection);

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var producto = new Producto
                (Convert.ToInt32(reader["idProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"]));
                productos.Add(producto);
            }
        }

        connection.Close();
        return productos;
    }
    public Producto GetDetallesByID(int id)
    {
        Producto producto;

        using var connection = GetOpenConnection();
        var queryString = $"SELECT IdProducto, Descripcion, Precio FROM Productos WHERE IdProducto = @id";
        var command = new SqliteCommand(queryString, connection);

        command.Parameters.Add(new SqliteParameter("@id", id));

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                var productoTemporal = new Producto
                (Convert.ToInt32(reader["idProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"]));
                producto = productoTemporal;
            }
            else producto = null;
        }

        connection.Close();
        return producto;
    }
    public int DeleteByID(int id)
    {
        using (var connection = GetOpenConnection())
        {
            string queryString = $"DELETE FROM Productos WHERE IdProducto = @id"; //DELETE IdProducto, Descripcion, Precio FROM mal, directamente DELETE FROM
            var command = new SqliteCommand(queryString, connection);

            command.Parameters.Add(new SqliteParameter("@id", id));
            int filasBorradas;
            try
            {
                filasBorradas = command.ExecuteNonQuery();
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // SQLite Error 19: 'FOREIGN KEY constraint failed' esta es la excepcion que arroja cuando el producto que se intenta borrar esta siendo referenciado por otra tabla
            {
                filasBorradas = -1;
            }

            connection.Close();
            return filasBorradas;
        }
    }

}