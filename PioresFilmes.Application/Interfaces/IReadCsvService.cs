using PioresFilmes.Domain;

namespace PioresFilmes.Application.Interfaces
{
    public interface IReadCsvService
    {
        IEnumerable<Movie> ReadMovies(string filePath);
    }
}