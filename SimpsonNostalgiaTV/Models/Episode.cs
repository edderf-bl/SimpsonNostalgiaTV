namespace SimpsonNostalgiaTV.Models
{
    public class Episode
    {
        public int Season { get; set; }
        public int EpisodeNum { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public DateTime DatePlay { get; set; }
    }
}
