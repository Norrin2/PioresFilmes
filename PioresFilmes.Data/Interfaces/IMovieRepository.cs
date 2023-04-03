using PioresFilmes.Domain;

namespace PioresFilmes.Data.Interfaces
{
    public interface IMovieRepository
    {
        public Task CommitAsync();
        public Task CreateManyAsync(IEnumerable<Movie> movies);
    }
}
