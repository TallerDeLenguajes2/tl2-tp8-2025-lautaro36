using TiendaElectronica.ViewModels;
using TiendaElectronica.Models;
using Microsoft.Data.Sqlite;

namespace TiendaElectronica.Repositorios;

public class UserRepository : IUserRepository
{
    string stringConnectionDb = "Data Source=Tienda.db;Cache=Shared";
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqliteConnection GetOpenConnection()
    {
        SqliteConnection connection = new SqliteConnection(stringConnectionDb);
        connection.Open();
        return connection;
    }

    public User? GetUser(string username, string password)
    {
        User? user = null;

        using var connection = GetOpenConnection();
        string queryString = "SELECT Id, Nombre, Username, Password, Rol  FROM Usuarios WHERE Username = @username AND Password = @password";

        var command = new SqliteCommand(queryString, connection);
        command.Parameters.Add(new SqliteParameter("@username", username));
        command.Parameters.Add(new SqliteParameter("@password", password));

        using(SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                user = new User(Convert.ToInt32(reader["Id"]), reader["Nombre"].ToString(), reader["Username"].ToString(), reader["Password"].ToString(), Convert.ToInt32(reader["Rol"]));
            }
        }
        connection.Close();
        return user;
    }
}