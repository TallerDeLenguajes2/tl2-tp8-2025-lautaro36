using Microsoft.Data.Sqlite;

namespace TiendaElectronica.Repositorios;

public class AuthenticationService : IAuthenticationService
{
    string stringConnectionDb = "Data Source=Tienda.db;Cache=Shared";
    private readonly string _connectionString;

    public AuthenticationService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqliteConnection GetOpenConnection()
    {
        SqliteConnection connection = new SqliteConnection(stringConnectionDb);
        connection.Open();
        return connection;
    }
    public bool Login(string username, string password)
    {
        
    }
    public void Logout();
    public bool IsAuthenticated ();
    public bool HasAccessLevel(string requiredAccessLevel);
}