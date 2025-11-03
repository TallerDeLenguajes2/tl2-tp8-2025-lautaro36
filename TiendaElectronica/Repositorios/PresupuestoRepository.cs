using Microsoft.Data.Sqlite;
using TiendaElectronica.Models;

namespace TiendaElectronica.Repositorios;

public class PresupuestoRepository
{
    string stringConnection = "Data Source=Tienda.db;Cache=Shared";
    public SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(stringConnection);
        connection.Open();
        return connection;
    }
    public int CrearPresupuesto(Presupuesto presupuesto)
    {
        using (var connection = GetConnection())
        {
            string queryString = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@nombre, @fecha); SELECT last_insert_rowid()";
            var command = new SqliteCommand(queryString, connection);

            command.Parameters.Add(new SqliteParameter("@nombre", presupuesto.NombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@fecha", presupuesto.FechaCreacion));

            int IdPresupuesto = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            return IdPresupuesto;
        }
    }

    public int AgregarAlPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        using (var connection = GetConnection())
        {
            string stringQuery = "INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)";
            var command = new SqliteCommand(stringQuery, connection);

            command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
            command.Parameters.Add(new SqliteParameter("@cantidad", cantidad));

            int filasAfectadas;
            try
            {
                filasAfectadas = command.ExecuteNonQuery();
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) //excepecion que arroja al intentar agregar un registro a con una foreign key(IdProducto o IdPresupuesto) inexistente 
            {
                filasAfectadas = -1;
                connection.Close();
                return filasAfectadas;
            }
            connection.Close();
            return filasAfectadas;
        }
    }

    public Presupuesto GetDetallesById(int id)
    {
        Presupuesto presupuesto = null;

        using var connection = GetConnection();
        string queryString = "SELECT  pre.IdPresupuesto AS IdPresupuesto, pre.NombreDestinatario AS NombreDestinatario, pre.FechaCreacion AS FechaCreacion, d.Cantidad AS Cantidad, pro.IdProducto AS IdProducto, pro.Descripcion AS Descripcion, pro.Precio AS Precio FROM PresupuestosDetalle d JOIN Presupuestos pre ON d.IdPresupuesto = pre.IdPresupuesto JOIN Productos pro ON d.IdProducto = pro.IdProducto WHERE pre.IdPresupuesto = @id";
        var command = new SqliteCommand(queryString, connection);

        command.Parameters.Add(new SqliteParameter("@id", id));
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                if (presupuesto == null)
                {
                    var fecha = DateOnly.Parse(reader["FechaCreacion"].ToString());
                    var presupuestoTemporal = new Presupuesto(id, reader["NombreDestinatario"].ToString(), fecha); //creo un nuevo presupuesto
                    presupuesto = presupuestoTemporal;
                }
                Producto producto = new Producto(Convert.ToInt32(reader["IdProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"]));
                presupuesto.agregarDetalles(producto, Convert.ToInt32(reader["Cantidad"]));
            }
        }
        connection.Close();
        return presupuesto;
    }

    public List<Presupuesto> GetAll()
    {
        List<Presupuesto> listadoPresupuestos = new List<Presupuesto>();

        using var connection = GetConnection();
        string queryString = "SELECT * FROM Presupuestos";
        var command = new SqliteCommand(queryString, connection);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var fecha = DateOnly.Parse(reader["FechaCreacion"].ToString());
                var presupuesto = new Presupuesto(Convert.ToInt32(reader["IdPresupuesto"]), reader["NombreDestinatario"].ToString(), fecha); //creo un nuevo presupuesto
                listadoPresupuestos.Add(presupuesto);
            }
        }
        connection.Close();
        return listadoPresupuestos;
    }

    // public List<Presupuesto> GetAll()
    // {
    //     List<Presupuesto> listadoPresupuestos = new List<Presupuesto>();

    //     using var connection = GetConnection();

    //     string queryString = "SELECT  pre.IdPresupuesto AS IdPresupuesto, pre.NombreDestinatario AS NombreDestinatario, pre.FechaCreacion AS FechaCreacion, d.Cantidad AS Cantidad, pro.IdProducto AS IdProducto, pro.Descripcion AS Descripcion, pro.Precio AS Precio FROM PresupuestosDetalle d JOIN Presupuestos pre ON d.IdPresupuesto = pre.IdPresupuesto JOIN Productos pro ON d.IdProducto = pro.IdProducto";

    //     var command = new SqliteCommand(queryString, connection);

    //     using (var reader = command.ExecuteReader())
    //     {
    //         while (reader.Read())
    //         {
    //             Producto producto = new Producto(Convert.ToInt32(reader["IdProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"])); //creo un nuevo  producto

    //             int id = Convert.ToInt32(reader["IdPresupuesto"]);
    //             int indice = listadoPresupuestos.FindIndex(p => p.IdPresupuesto == id); //busco si ya existe un presupuesto en el listado a traves de su IdPresupuesto
    //             if (indice >= 0)
    //             {
    //                 listadoPresupuestos[indice].agregarDetalles(producto, Convert.ToInt32(reader["Cantidad"])); //creo un detalle para ese producto en el  presupuesto y lo agrego al listado de presupuestos (si este ya existe)
    //             }
    //             else
    //             {
    //                 var fecha = DateOnly.Parse(reader["FechaCreacion"].ToString());
    //                 var presupuesto = new Presupuesto(id, reader["NombreDestinatario"].ToString(), fecha); //creo un nuevo presupuesto
    //                 presupuesto.agregarDetalles(producto, Convert.ToInt32(reader["Cantidad"])); //agrego el detalle al listado
    //                 listadoPresupuestos.Add(presupuesto); //agrego el nuevo presupuesto al listado
    //             }
    //         }
    //     }
    //     connection.Close();
    //     return listadoPresupuestos;
    // }

    public int DeleteById(int id)
    {
        using (var connection = GetConnection())
        {
            string stringQuery = "DELETE FROM Presupuestos WHERE IdPresupuesto = @id";
            var command = new SqliteCommand(stringQuery, connection);

            command.Parameters.Add(new SqliteParameter("@id", id));

            int lineasAfectadas;
            try
            {
                lineasAfectadas = command.ExecuteNonQuery();
            }
            catch (SqliteException ex) when (ex.SqliteExtendedErrorCode == 19)
            {
                lineasAfectadas = -1;
            }

            connection.Close();
            return lineasAfectadas;
        }
    }

    public bool ModificarPresupuesto(Presupuesto presupuesto)
    {
        using (var connection = GetConnection())
        {
            string queryString = "UPDATE presupuestos SET NombreDestinatario = @nombre, FechaCreacion = @fecha WHERE IdPresupuesto = @id";
            var command = new SqliteCommand(queryString, connection);

            command.Parameters.Add(new SqliteParameter("@nombre", presupuesto.NombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@fecha", presupuesto.FechaCreacion));
            command.Parameters.Add(new SqliteParameter("@id", presupuesto.IdPresupuesto));

            var filasAfectadas = command.ExecuteNonQuery();
            connection.Close();
            return filasAfectadas >= 1;
        }
    }
}