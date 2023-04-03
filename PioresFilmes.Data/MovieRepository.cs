using Microsoft.EntityFrameworkCore;
using PioresFilmes.Data.Interfaces;
using PioresFilmes.Domain;

namespace PioresFilmes.Data
{
    public class MovieRepository: IMovieRepository
    {
        private readonly MovieDbContext _dbContext;

        public MovieRepository(MovieDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateManyAsync(IEnumerable<Movie> movies)
        {
            foreach (var movie in movies)
            {
                foreach (var producer in movie.Producers)
                {
                    var existingProducer = await _dbContext.Producers.FirstOrDefaultAsync(p => p.Name == producer.Name);
                    if (existingProducer == null)
                    {
                        existingProducer = new Producer { Name = producer.Name };
                        _dbContext.Producers.Add(existingProducer);
                    }

                    producer.Id = existingProducer.Id;
                }

                _dbContext.Movies.Add(movie);
            }
        }
    }
}
