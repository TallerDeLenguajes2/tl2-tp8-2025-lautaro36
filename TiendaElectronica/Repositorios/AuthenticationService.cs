using Microsoft.Data.Sqlite;
using TiendaElectronica.Models;
using BCrypt.Net;

namespace TiendaElectronica.Repositorios;

public class AuthenticationService : IAuthenticationService
{
    private readonly string? _connectionString;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccesor;
    private readonly HttpContext? _context;

    public AuthenticationService(string? connectionString, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _connectionString = connectionString;
        _userRepository = userRepository;
        _httpContextAccesor = httpContextAccessor;
        _context = httpContextAccessor.HttpContext;
    }

    public SqliteConnection GetOpenConnection()
    {
        SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
    public bool Login(string username, string password)
    {
        User? user = _userRepository.GetUser(username);
        //validacion del hash
        if (user == null) return false;
        bool isPassWordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if(!isPassWordValid) Console.WriteLine("COntrasenia incorrecta");
        if (_context == null) throw new InvalidOperationException("HttpContext no está disponible.");

        _context.Session.SetString("IsAuthenticated", "true");
        _context.Session.SetString("User", user.Username);
        _context.Session.SetString("Nombre", user.Nombre);
        _context.Session.SetString("Rol", (user.Rol).ToString());

        return true;
    }
    public void Logout() { }
    public bool IsAuthenticated()
    {
        return true;
    }
    public bool HasAccessLevel(string requiredAccessLevel)
    {
        return true;
    }
}