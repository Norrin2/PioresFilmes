using PioresFilmes.Application.Interfaces;
using PioresFilmes.Configuration;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddDatabase();    
services.AddApplicationServices();

services.AddControllers();
services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

var initService = app.Services.GetRequiredService<IInitializeDataService>();                 

await initService.Initialize();

app.Run();

public partial class Program { }