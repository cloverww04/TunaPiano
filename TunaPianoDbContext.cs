using Microsoft.EntityFrameworkCore;
using TunaPiano.Models;

namespace TunaPiano
{
    public class TunaPianoDbContext : DbContext
    {
        public DbSet<Artist>? Artists { get; set; }
        public DbSet<Genre>? Genres { get; set; }
        public DbSet<Song>? Songs { get; set; }

        public TunaPianoDbContext(DbContextOptions<TunaPianoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Song>()
                .HasMany(g => g.Genres)
                .WithMany(s => s.Songs)
                .UsingEntity(sg => sg.ToTable("SongGenre"));





            modelBuilder.Entity<Artist>().HasData(new Artist[]
            {
                new Artist { Id = 1, Name = "Dave Matthews", Age = 56, Bio = "David John Matthews (born January 9, 1967) is an American musician, " +
                "songwriter, record producer and political activist. He is best known as the lead vocalist, songwriter, and guitarist for the Dave Matthews Band (DMB)."},

                new Artist { Id = 2, Name = "Gregory Alan Isakov", Age = 43, Bio = "Born in Johannesburg, South Africa, and now calling Colorado home, horticulturist/musician" +
                " Gregory Alan Isakov has cast an impressive presence on the indie-rock and folk worlds with his six full-length studio albums."}
            });

            modelBuilder.Entity<Song>().HasData(new Song[]
            {
                new Song { Id = 1, Title = "Last Stop", Album = "Before These Crowded Streets", ArtistId = 1, Length = "9:59" },
                new Song{ Id = 2, Title = "Liars", Album = "Gregory Alan Isakov With the Colorado Symphony", ArtistId =2, Length = "5:16"}
            });

            modelBuilder.Entity<Genre>().HasData(new Genre[]
            {
                new Genre { Id = 1, Description = "Rock"},
                new Genre { Id = 2, Description = "Indie-Rock"},
                new Genre { Id = 3, Description = "Folk"}
            });


        }
    }
}
