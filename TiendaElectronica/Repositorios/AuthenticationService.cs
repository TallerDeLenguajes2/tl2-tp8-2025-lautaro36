using Microsoft.Data.Sqlite;
using TiendaElectronica.Models;
using BCrypt.Net;

namespace TiendaElectronica.Repositorios;

public class AuthenticationService : IAuthenticationService
{
    private readonly string? _connectionString;
    private readonly IUserRepository _userRepository;
    public IHttpContextAccessor _httpContextAccesor;

    public AuthenticationService(string? connectionString, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _connectionString = connectionString;
        _userRepository = userRepository;
        _httpContextAccesor = httpContextAccessor;
    }

    public SqliteConnection GetOpenConnection()
    {
        SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
    public bool Login(string username, string password)
    {
        var context = _httpContextAccesor.HttpContext;
        User? user = _userRepository.GetUser(username);
        if (user == null) return false;

        if (context == null) throw new InvalidOperationException("HttpContext no está disponible.");

        //validacion del hash
        bool isPassWordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!isPassWordValid) return false;

        context.Session.SetString("IsAuthenticated", "true");
        context.Session.SetString("User", user.Username);
        context.Session.SetString("Nombre", user.Nombre);
        context.Session.SetString("Rol", user.Rol.ToString());
        return true;
    }

    public void Logout()
    {
        var context = _httpContextAccesor.HttpContext;
        if(context == null) throw new InvalidOperationException("HttpContex no esta diponible.");

        context.Session.Clear();
    }

    public bool IsAuthenticated()
    {
        var context = _httpContextAccesor.HttpContext;
        if(context == null) throw new InvalidOperationException("HttpContext no esta disponible.");

        return context.Session.GetString("IsAuthenticated") == "true";
    }
    
    public bool HasAccessLevel(string requiredAccessLevel)
    {
        var context = _httpContextAccesor.HttpContext;
        if(context == null) throw new InvalidOperationException("HttpContext no esta disponible.");

        return context.Session.GetString("Rol") == requiredAccessLevel;
    }
}