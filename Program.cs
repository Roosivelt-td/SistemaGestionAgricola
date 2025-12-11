using SistemaGestionAgricola;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

Console.WriteLine("🚀 Aplicación iniciada en: " + (app.Environment.IsDevelopment() ? "http://localhost:5173" : "Producción"));
Console.WriteLine("📚 Swagger disponible en: http://localhost:5173");
Console.WriteLine("🔐 Recuerda usar el botón 'Authorize' en Swagger para probar endpoints protegidos");

app.Run();