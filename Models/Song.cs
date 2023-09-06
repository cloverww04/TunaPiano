using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TunaPiano.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        [ForeignKey("Artist")]
        public int ArtistId { get; set; }
        public string? Album { get; set; }
        public string? Length { get; set; }
        public ICollection<Genre>? Genres { get; set; }
        public Artist? Artist { get; set; }
    }
}
