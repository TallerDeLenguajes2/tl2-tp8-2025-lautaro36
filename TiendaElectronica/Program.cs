using Microsoft.AspNetCore.Http;
using TiendaElectronica.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Servicios de Sesión y Acceso a Contexto (CLAVE para la autenticación)
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// 18/11  "ConnectionStrings": {"Default": "Data Source=Tienda.db;Cache=Shared" } esto va en appsettings.json para poder obtener la cadena de conexion para el constructor de los repos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //18/11 obtengo una unica vez la cadena para todas instanciaciones de repos. esta bien porque el connection string no cambia en tiempo de ejecución (salvo raras excepciones). Captura el connectionString como variable externa. 19/11 esta no es la forma mas DI FULL, continua al final
//registros de DI
builder.Services.AddScoped<IPresupuestoRepository>(provider =>
{
    return new PresupuestoRepository(connectionString); 
});

builder.Services.AddScoped<IProductoRepository>(provider =>
{
    return new ProductoRepository(connectionString);
});

builder.Services.AddScoped<IUserRepository>(provider =>
{
    return new UserRepository(connectionString);
});

builder.Services.AddScoped<IAuthenticationService>(provider =>
{
    var userRepository = provider.GetRequiredService<IUserRepository>();
    var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

    return new AuthenticationService(connectionString, userRepository, contextAccessor);
});

/*19/11 para hacer esto de la forma mas correcta posible:
crear una clase DatabaseOptions con campo ConnectionString
agregar seccion dedicada DatabaseOptions a appsettings.json con ConnectionString
agregar en porgram.cs builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection("DatabaseOptions")); bindeo de opciones
a partir de aca habria 2 opciones para armar los servicios: 
1 
-que cada constructor siga recibiendo string connectionString
-agregar en cada registro de DI var dbOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
-luego return new AuthenticationService(dbOptions.connectionString...
2
-modificar el constructor de cada repo para que reciba, en vez de string connectionString, IOptions<DataBaseOptions> dboptions (obtengo una instancia de DatabaseOptions bindeada con las configuraciones de appsettings, respeteando el Options Pattern (muchas ventajas))
-los campos ConnectionString = dbOptions.value.ConnectionString
-luego, el repo usa DI para obtener la config
obteniendo todo desde appsettings (o archvios json), se respeta que El código no debería cambiar cuando cambian los valores de configuración.
teniendo cosas como URLs de APIs, Strings de conexión, Timeouts, Opciones de logging, Claves, tokens, endpoints  en archivos, la app usa los valores aunque cambien sin tocar codigo. permite configurar por ambiente, es mas seguro, se evitan valores magicos, hot reload, facilita mocking y testing, etc. 
por lo general se hardcodea solo valores de variables que no cambian nunca o constantes de logca 
a aplicar luego  de entregar el tp 10
*/

/*
builder.Services.AddScoped<IProductoRepository>(provider =>
    new ProductoRepository(
        builder.Configuration.GetConnectionString("Default")
    ));
el resultado en este caso es casi igual al implemetando, pero este no depende de la variable externa y se lee el config para cada instanciacion de repo, razon por la cual elegi la otra forma 
*/

/*
builder.Services.AddScoped<IProductoRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>(); 
    var connectionString = configuration.GetConnectionString("Default");
    
    return new ProductoRepository(connectionString);
});
Obtiene IConfiguration desde el contenedor mismo, no desde builder. dependo solo del contenedor de DI, no de variales externas. de esta forma el connString puede cambiar lo cual es rarisimo. es compleja al pedo porque esa es su unica ventaja. razon por la cual elegi la implementada
*/

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Configuración del Pipeline de Middleware
// El orden es CRUCIAL: UseSession debe ir ANTES de UseRouting/UseAuthorization
app.UseSession(); // → Habilita el uso de la sesión

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
