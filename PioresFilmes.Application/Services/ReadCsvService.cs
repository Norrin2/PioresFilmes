using PioresFilmes.Application.Interfaces;
using PioresFilmes.Domain;

namespace PioresFilmes.Application.Services
{
    public class ReadCsvService : IReadCsvService
    {
        public IEnumerable<Movie> ReadMovies(string filePath)
        {
            List<Movie> movies = new List<Movie>();

            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] fields = line.Split(';');

                    var movie = new Movie
                    {
                        Year = int.Parse(fields[0]),
                        Title = fields[1],
                        Studios = fields[2],
                        Producers = fields[3]
                            .Split(new string[] { " and ", ", " }, StringSplitOptions.None)
                            .Select(p => new Producer() { Name = p.Trim() }).ToList(),
                        Winner = fields[4] == "yes"
                    };

                    movies.Add(movie);
                }
            }

            return movies;
        }
    }
}
