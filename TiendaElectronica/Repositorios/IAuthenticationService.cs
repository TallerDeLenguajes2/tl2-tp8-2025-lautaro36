using Microsoft.Data.Sqlite;

namespace TiendaElectronica.Repositorios;

public interface IAuthenticationService
{
    SqliteConnection GetOpenConnection();
    bool Login(string username, string password);
    void Logout();
    bool IsAuthenticated ();
    bool HasAccessLevel(string requiredAccessLevel);
}