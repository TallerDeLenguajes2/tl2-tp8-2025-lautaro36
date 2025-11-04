namespace TiendaElectronica.ViewModels;

public class DetalleUpCantidadesViewModel
{

    public int IdPresupuesto { get; set; }
    public int IdProducto { get; set; }
    public string? Descripcion { get; set; }
    public int Cantidad { get; set; }

    public DetalleUpCantidadesViewModel() { }
    //InvalidOperationException: Could not create an instance of type 'TiendaElectronica.ViewModels.DetalleUpCantidadesViewModel'. Model bound complex types must not be abstract or value types and must have a parameterless constructor. Record types must have a single primary constructor. Alternatively, give the 'detalleViewModel' parameter a non-null default value. 
    //ASP.NET Core model binder tries to create an instance of the class using the data from an HTTP request, it specifically looks for a public, parameterless constructor by default. It doesn't know how to fill the arguments of the existing constructor 

    public DetalleUpCantidadesViewModel(int IdPresupuesto, int IdProducto, string Descripcion, int Cantidad)
    {
        this.IdPresupuesto = IdPresupuesto;
        this.IdProducto = IdProducto;
        this.Descripcion = Descripcion;
        this.Cantidad = Cantidad;
    }
}