using PioresFilmes.Application.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

ReadCsvService.ReadMovies("movielist.csv");
app.MapGet("/", () => "Hello World!");

app.Run();
