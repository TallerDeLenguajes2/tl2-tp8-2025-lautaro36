namespace TiendaElectronica.Models;

public class User
{
    public int Id {get; set;}
    public string? Nombre {get; set;} 
    public string? Username {get; set;}
    public string? Password {get; set;}
    public Roles Rol {get; set;}

    public User(int id, string? nombre, string? user, string? pass, int rol)
    {
        Id = id;
        Nombre = nombre;
        Username = user;
        Password = pass;
        Rol = (Roles)rol;
    }
}

public enum Roles
{
    Administrador = 0, 
    Cliente = 1
}