using System.ComponentModel.DataAnnotations;

namespace TunaPiano.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string? Description { get; set; }
        public List<Song_Genre>? Song_Genres { get; set; }
    }
}
