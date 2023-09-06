using System.ComponentModel.DataAnnotations;

namespace TunaPiano.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        [MaxLength(300)]
        public string? Bio { get; set; }
        public Song? songs { get; set; }
    }
}
