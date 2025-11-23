using TiendaElectronica.ViewModels;
using TiendaElectronica.Models;
using Microsoft.Data.Sqlite;

namespace TiendaElectronica.Repositorios;

public class UserRepository : IUserRepository
{
    private readonly string? _connectionString;

    public UserRepository(string? connectionString)
    {
        _connectionString = connectionString;
    }

    public SqliteConnection GetOpenConnection()
    {
        SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public User? GetUser(string? username)
    {
        User? user = null;

        using var connection = GetOpenConnection();
        string queryString = "SELECT IdUsuarios, Nombre, Username, Password, PasswordHash, Rol  FROM Usuarios WHERE Username = @username";

        var command = new SqliteCommand(queryString, connection);
        command.Parameters.Add(new SqliteParameter("@username", username));

        using(SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Enum.TryParse(reader["Rol"].ToString(), out Roles rol);//lo paso primero a string poruqe el tryparse solo recibe strings
                user = new User(Convert.ToInt32(reader["IdUsuarios"]), reader["Nombre"].ToString(), reader["Username"].ToString(), reader["Password"].ToString(), reader["PasswordHash"].ToString(), rol);
            }
        }
        connection.Close();
        return user;
    }
}