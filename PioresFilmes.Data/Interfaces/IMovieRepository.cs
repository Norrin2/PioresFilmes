using PioresFilmes.Domain;

namespace PioresFilmes.Data.Interfaces
{
    public interface IMovieRepository
    {
        public Task CreateManyAsync(IEnumerable<Movie> movies);
    }
}
