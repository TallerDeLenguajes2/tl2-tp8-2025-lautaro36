using Microsoft.Data.Sqlite;

namespace TiendaElectronica.Repositorios;

public class AuthenticationService : IAuthenticationService
{
    private readonly string? _connectionString;

    public AuthenticationService(string? connectionString)
    {
        _connectionString = connectionString;
    }

    public SqliteConnection GetOpenConnection()
    {
        SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
    public bool Login(string username, string password)
    {
        //seguir con la logica de cada metodo. luego armar el model de authentication ?. seguir con punto 4
        return true;
    }
    public void Logout(){}
    public bool IsAuthenticated()
    {
        return true;
    }
    public bool HasAccessLevel(string requiredAccessLevel)
    {
        return true;
    }
}