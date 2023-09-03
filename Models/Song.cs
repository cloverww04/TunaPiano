using System.ComponentModel.DataAnnotations;

namespace TunaPiano.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public int ArtistId { get; set; }
        public string? Album { get; set; }
        public TimeSpan Length { get; set; }
        public List<Song_Genre>? Song_Genres { get; set; }
    }
}
