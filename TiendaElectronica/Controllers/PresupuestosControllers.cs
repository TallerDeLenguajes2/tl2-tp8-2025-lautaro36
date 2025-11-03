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
        Presupuesto model = new Presupuesto(IdPresupuesto, NombreDestinatario, FechaCreacion);
        return View(model);
    }

    [HttpPost("Presupuestos/Update/{IdPresupuesto}")]// [HttpPost("Update")]esto no funciona, porque a pesar de que supuestamente, el id del producto deberia llegar de ProductoIndexViewModel, (ya q fue cargado en el httpget), asp net core tiene q rearmar el objeto mediante su model binder. como tiene q rearmar el objeto, necesita el id, entonces este tiene q llegar ya sea desde la ruta o desde un campo hidden del form
    public IActionResult Update(Presupuesto presupuesto)
    {
        _presupuestoRepository.ModificarPresupuesto(presupuesto);
        return RedirectToAction("Index", "Presupuestos");
    }


    // [HttpPost("ProductoDetalle")]
    // public IActionResult AgregarAlPresupuesto(int id, int idProducto, int cantidadProducto)
    // {
    //     var resultado = _presupuestoRepository.AgregarAlPresupuesto(id, idProducto, cantidadProducto);
    //     if (resultado == -1) return Conflict("El ID del producto o presupuesto no existe, no puedo agregarse al presupuesto");
    //     else if (resultado == 0) return Conflict("No pudo agregarse al presupuesto");
    //     return Created();
    // }



    // [HttpDelete("{id}")]
    // public IActionResult DeleteById(int id)
    // {
    //     int filasAfectadas = _presupuestoRepository.DeleteById(id);
    //     if (filasAfectadas == -1) return Conflict("El producto ya fue agregado a uno o mas presupuestos, asegurese de borrar primero el/los presupuestos.");
    //     if (filasAfectadas == 0) return NotFound($"No se encontró un producto con el ID {id}.");
    //     return NoContent();
    // }

}