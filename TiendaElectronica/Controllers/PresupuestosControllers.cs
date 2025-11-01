using Microsoft.AspNetCore.Mvc;
using TiendaElectronica.Repositorios;
using TiendaElectronica.ViewModels;

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
        if (presupuestoModel == null) return View("Error"); // Muestra "error" sin salir de la pagina details.cshtml. Renderiza Views/Shared/Error.cshtml directamente 
        // sino return RedirectToAction("Error", "Home") Redirige a la página de error global. Va al método Error() del HomeController
        var presupuestoViewModel = new PresupuestoIndexViewModels(presupuestoModel);
        
        return View(presupuestoViewModel);
    }
    // [HttpPost("AltaPresupuesto")]
    // public IActionResult CrearPresupuesto(Presupuesto presupuesto)
    // {
    //     _presupuestoRepository.CrearPresupuesto(presupuesto);
    //     return Created("Presupuesto dado de alta correctamente", presupuesto);
    // }

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