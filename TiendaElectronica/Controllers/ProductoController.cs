using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TiendaElectronica.Repositorios;
using TiendaElectronica.Models;
using TiendaElectronica.ViewModels;

namespace TiendaElectronica.Controllers;

public class ProductosController : Controller
{
    private readonly ILogger<ProductosController> _logger;
    private ProductoRepository _productoRepository;
    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        _productoRepository = new ProductoRepository();
    }

    public IActionResult Index()
    {
        List<Producto> listadoModels = _productoRepository.GetAll();
        List<ProductoIndexViewModel> listadoViewModels = listadoModels.Select(producto => new ProductoIndexViewModel(producto)).ToList();
        return View(listadoViewModels);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Producto());
    }
    [HttpPost]
    public IActionResult Create(Producto producto)
    {
        int resultado = _productoRepository.CrearProducto(producto);
        if (resultado == 0) return RedirectToAction("Error", "Home");

        return RedirectToAction("Index", "Productos");
    }

    [HttpGet("Update/{IdProducto}")]
    public IActionResult Update(int IdProducto, [FromQuery]string Descripcion, [FromQuery]int Precio)
    {
        ProductoIndexViewModel ViewModel = new ProductoIndexViewModel(IdProducto, Descripcion, Precio);
        return View(ViewModel);
    }

    [HttpPost("Update/{IdProducto}")]
    public IActionResult Update(ProductoIndexViewModel productoViewModel, int IdProducto)
    {
        Producto productoModel = new Producto(productoViewModel.IdProducto, productoViewModel.Descripcion, productoViewModel.Precio);
        var updateCorrecto = _productoRepository.ModificarProducto(IdProducto, productoModel);
        if (!updateCorrecto) return RedirectToAction("Error", "Home");

        return RedirectToAction("Index", "Productos");
    }

    // [HttpDelete("Update/{id}")]
    // public IActionResult BorrarProducto(int id)
    // {
    //     var borradoCorrecto = _productoRepository.DeleteByID(id);
    //     if (borradoCorrecto == -1) return Conflict("El producto ya fue agregado a uno o mas presupuestos, asegurese de borrar primero el/los presupuestos.");
    //     else if (borradoCorrecto == 0) return NotFound($"No se encontró un producto con el ID {id}.");
    //     return NoContent();
    // }
    
    //result()

//     [HttpGet("{id}")] // antes lo tenia como [HttpGet("id")] pero devolvia este url https://localhost:7235/Producto/id?id=4
//     public IActionResult GetDetallesById(int id)
//     {
//         var producto = _productoRepository.GetDetallesByID(id);
//         if(producto == null) return NotFound($"No se encontró un producto con el ID {id}.");    
//         return Ok(producto);    
//     }


}