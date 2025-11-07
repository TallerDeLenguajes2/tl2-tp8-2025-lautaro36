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
        List<ProductoViewModel> listadoViewModels = listadoModels.Select(producto => new ProductoViewModel(producto)).ToList();
        return View(listadoViewModels);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new ProductoCreateViewModel());
    }
    
    [HttpPost]
    public IActionResult Create(ProductoCreateViewModel viewModel)
    {
        Producto model = new Producto(viewModel);
        int resultado = _productoRepository.CrearProducto(model);
        if (resultado == 0) return RedirectToAction("Error", "Home");

        return RedirectToAction("Index", "Productos");
    }

    [HttpGet("Update/{IdProducto}")] 
    public IActionResult Update(int IdProducto, [FromQuery]string Descripcion, [FromQuery]int Precio)
    {
        ProductoViewModel ViewModel = new ProductoViewModel(IdProducto, Descripcion, Precio);
        return View(ViewModel);
    }

    [HttpPost("Update/{IdProducto}")] // [HttpPost("Update")]esto no funciona, porque a pesar de que supuestamente, el id del producto deberia llegar de ProductoViewModel, (ya q fue cargado en el httpget), asp net core tiene q rearmar el objeto mediante su model binder. 
    //como tiene q rearmar el objeto, necesita el id, entonces este tiene q llegar ya sea desde la ruta o desde un campo hidden del form
    public IActionResult Update(ProductoViewModel ProductoViewModel)
    {
        Producto productoModel = new Producto(ProductoViewModel.IdProducto, ProductoViewModel.Descripcion, ProductoViewModel.Precio);
        var updateCorrecto = _productoRepository.ModificarProducto(productoModel);
        if (!updateCorrecto) return RedirectToAction("Error", "Home");

        return RedirectToAction("Index", "Productos");
    }

    // [HttpDelete("Delete/{IdProducto}")] el navegador unicamente puede enviar solicitudes GET y POST, por eso no funciono. 
    // /El framework ignoró  esta acción, porque la solicitud que llegó desde el navegador era un GET, no un DELETE. El atributo [HttpDelete] solo responde a solicitudes DELETE.
    //luego no especifique un metodo HTTP, entonces  el framework asumio el metodo predeterminda, GET
    //Aunque usar el método GET soluciono,  las operaciones que cambian el estado del servidor (como eliminar un registro) nunca deben usar GET.
    [HttpPost("Delete/{IdProducto}")]//por eso, uso post
    public IActionResult Delete(int IdProducto)
    {
        var borradoCorrecto = _productoRepository.DeleteByID(IdProducto);
        if (borradoCorrecto == -1) return RedirectToAction("Error", "Home");
        else if (borradoCorrecto == 0) return RedirectToAction("Error", "Home");
        return RedirectToAction("Index", "Productos");
    }
}