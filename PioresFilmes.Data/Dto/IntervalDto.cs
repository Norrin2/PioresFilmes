namespace PioresFilmes.Data.Dto
{
    public record IntervalDto
    {
        public string Name { get; set; }
        public int PreviousWin { get; set; }
        public int FollowingWin { get; set; }
        public int Interval => FollowingWin - PreviousWin;
    }
}
