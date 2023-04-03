namespace PioresFilmes.Domain
{
    public class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Producer> Producers { get; set; }
        public int Year { get; set; }
        public bool Winner { get; set; }
    }
}
