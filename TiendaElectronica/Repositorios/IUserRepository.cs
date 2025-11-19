using TiendaElectronica.Models;
using Microsoft.Data.Sqlite;

namespace TiendaElectronica.Repositorios;

public interface IUserRepository
{
    SqliteConnection GetOpenConnection();
    User? GetUser(string username);
}