using PioresFilmes.Application.Interfaces;
using PioresFilmes.Configuration;
using PioresFilmes.Data;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddRouting(options => options.LowercaseUrls = true);
services.AddDbContext<MovieDbContext>();
    
services.AddApplicationServices();

var app = builder.Build();
var initService = app.Services.CreateScope()
                     .ServiceProvider
                     .GetRequiredService<IInitializeDataService>();

await initService.Initialize();

app.MapGet("/", () => "Hello World!");

app.Run();
