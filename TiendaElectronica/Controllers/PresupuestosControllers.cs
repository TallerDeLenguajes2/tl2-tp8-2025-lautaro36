using Microsoft.AspNetCore.Mvc;
using TiendaElectronica.Repositorios;
using TiendaElectronica.ViewModels;
using TiendaElectronica.Models;

namespace TiendaElectronica.Controllers;

public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;
    private PresupuestoRepository _presupuestoRepository;
    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
        _presupuestoRepository = new PresupuestoRepository();
    }

    public IActionResult Index()
    {
        var listadoModels = _presupuestoRepository.GetAll();
        var listadoViewModels = listadoModels.Select(elementoModel => new PresupuestoIndexViewModels(elementoModel)).ToList();//mapeo cada presupuesto del listado a un listado de presupuesto view model, usando select. paso cada presupuesto individualmente al constructor de PresupuestoIndexViewModels
        return View(listadoViewModels);
    }

    [HttpGet("Details/{id}")]
    public IActionResult Details(int id)
    {
        var presupuestoModel = _presupuestoRepository.GetDetallesById(id);
        if (presupuestoModel == null) return RedirectToAction("Error", "Home");
        // View("Error") Muestra "error" sin salir de la pagina details.cshtml. Renderiza Views/Shared/Error.cshtml directamente. De esta forma, arroja NullReferenceException en Error.cshtml. AVERIGUAR COMO RESOLVERLO
        // return RedirectToAction("Error", "Home") Redirige a la página de error global. Va al método Error() del HomeController. muestra la vista Error.cshtml correctamente
        var presupuestoViewModel = new PresupuestoIndexViewModels(presupuestoModel);

        return View(presupuestoViewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ProductoRepository productoRepository = new ProductoRepository(); //creo un producto repository para poder traer el listado de productos y guardarlo en el view model.
        var ViewModel = new PresupuestoCreateViewModels();
        ViewModel.ListadoProductos = productoRepository.GetAll(); //teniendo el listado de productos aca, puedo listar cada producto en la view y agregar un producto al presupuesto a la vez que es creado
        return View(ViewModel);
    }

    [HttpPost]
    public IActionResult Create(PresupuestoCreateViewModels ViewModel)
    {
        Presupuesto model = new Presupuesto(ViewModel.IdPresupuesto, ViewModel.NombreDestinatario, ViewModel.FechaCreacion);
        int IdPresupuesto = _presupuestoRepository.CrearPresupuesto(model);

        if (IdPresupuesto == 0) return RedirectToAction("Error", "Home");

        _presupuestoRepository.AgregarAlPresupuesto(IdPresupuesto, ViewModel.IdProducto, ViewModel.Cantidad);

        return RedirectToAction("Index", "Presupuestos");
    }

    [HttpGet("Presupuestos/Update/{IdPresupuesto}")]
    public IActionResult Update(int IdPresupuesto, [FromQuery] string NombreDestinatario, [FromQuery] DateOnly FechaCreacion)
    {
        PresupuestoViewModel model = new PresupuestoViewModel(IdPresupuesto, NombreDestinatario, FechaCreacion);
        return View(model);//haciendo q la vista update no reciba un model sino un viewmodel, tp9
    }

    // [HttpPost("Update")]esto no funciona, porque a pesar de que supuestamente, el id del producto deberia llegar de ProductoViewModel, (ya q fue cargado en el httpget), asp net core tiene q rearmar el objeto mediante su model binder. como tiene q rearmar el objeto, necesita el id, entonces este tiene q llegar ya sea desde la ruta o desde un campo hidden del form
    [HttpPost("Presupuestos/Update/{IdPresupuesto}")]
    public IActionResult Update(PresupuestoViewModel viewModel)
    {
        Presupuesto model = new Presupuesto(viewModel);
        _presupuestoRepository.ModificarPresupuesto(model);
        return RedirectToAction("Index", "Presupuestos");
    } 

    [HttpPost("Presupuestos/Delete/{IdPresupuesto}")]
    public IActionResult Delete(int IdPresupuesto)
    {
        int filasAfectadas = _presupuestoRepository.DeleteById(IdPresupuesto);
        if (filasAfectadas == -1) return RedirectToAction("Error", "Home");
        if (filasAfectadas == 0) return RedirectToAction("Error", "Home");
        return RedirectToAction("Index", "Presupuestos");
    }

    // [HttpGet] esto funciona, ejecuta correctamente la sentencia, pero al llegar redirect, no me redirige a "Details/IdPresupuesto", porque el endpoint al no tener parametros personalizados, usa su binding [controller][action] y agrega "Presupuestos" a la url
    //[HttpGet("Presupuestos/UpdateCantidades/{IdPresupuesto}")] esto tambien funciona, pero la url termina siendo Presupuestos/UpdateCantidades/Details/12, cuando deberia ser solo Details/12 por ej
    [HttpGet("UpdateCantidades")]
    public IActionResult UpdateCantidades(int IdPresupuesto, [FromQuery] int IdProducto, [FromQuery] string Descripcion, [FromQuery] int Cantidad)
    {
        DetalleUpCantidadesViewModel detalleViewModel = new DetalleUpCantidadesViewModel(IdPresupuesto, IdProducto, Descripcion, Cantidad);
        return View(detalleViewModel);
    }

    // [HttpPost] esto funciona, ejecuta correctamente la sentencia, pero al llegar redirect, no me redirige a "Details/IdPresupuesto", porque el endpoint al no tener parametros personalizados, usa su binding [controller][action] y agrega "Presupuestos" a la url
    //[HttpPost("Presupuestos/UpdateCantidades/{IdPresupuesto}")] esto tambien funciona, pero la url termina siendo Presupuestos/UpdateCantidades/Details/12, cuando deberia ser solo Details/12 por ej
    [HttpPost("UpdateCantidades")]
    public IActionResult UpdateCantidades(DetalleUpCantidadesViewModel detalleViewModel)
    {
        bool resultado = _presupuestoRepository.UpdateCantidades(detalleViewModel);
        if (!resultado) return RedirectToAction("Error", "Home");
        return Redirect($"Details/{detalleViewModel.IdPresupuesto}"); //segun chatgpt esto no es seguro, pero no encontre otra forma de hacer q funcione sin tener q modificar el metodo details
    }

    [HttpPost]
    public IActionResult DeleteDetalle(int IdPresupuesto, int IdProducto)
    {
        bool resultado = _presupuestoRepository.DeleteDetalle(IdPresupuesto, IdProducto);
        if (!resultado) return RedirectToAction("Error", "Home");
        return Redirect($"Details/{IdPresupuesto}");
    }

    [HttpGet("CreateDetalle/{IdPresupuesto}")]
    public IActionResult CreateDetalle(int IdPresupuesto)
    {
        ProductoRepository productosRepository = new ProductoRepository();
        List<Producto> listadoModels = productosRepository.GetAll();
        return View(new DetalleCreateViewModel(IdPresupuesto, listadoModels));
    }

    [HttpPost("CreateDetalle/{IdPresupuesto}")]
    public IActionResult CreateDetalle(int IdPresupuesto, int IdProducto, int Cantidad)
    {
        var resultado = _presupuestoRepository.AgregarAlPresupuesto(IdPresupuesto, IdProducto, Cantidad);
        if (resultado == -1) return RedirectToAction("Error", "Home");
        else if (resultado == 0) return RedirectToAction("Error", "Home");
        return Redirect($"Details/{IdPresupuesto}");
    }

}