using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TunaPiano;
using TunaPiano.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppContext.SetSwitch("Npgsql.EnableLegacyTimeStampBehavior", true);
builder.Services.AddNpgsql<TunaPianoDbContext>(builder.Configuration["TunaPianoDbConnectionString"]);
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Songs
app.MapGet("/api/songs", async (TunaPianoDbContext dbContext) =>
{
    var songs = await dbContext.Songs
    .ToListAsync();

    if (songs == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(songs);
});

app.MapGet("/api/songs/{id}", async (int id, TunaPianoDbContext dbContext) =>
{
    var songWithGenreAndArtist = await dbContext.Songs
   .Include(s => s.Artist)
   .Include("Genres")
   .FirstOrDefaultAsync(s => s.Id == id);

    if (songWithGenreAndArtist == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(songWithGenreAndArtist);
});

app.MapPost("/api/song", (TunaPianoDbContext dbContext, Song song) =>
{
    try
    {

        dbContext.Add(song);
        dbContext.SaveChanges();
        return Results.Created($"/api/songs/{song.Id}", song);

    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }

});

app.MapDelete("/api/song/{id}", (int id, TunaPianoDbContext dbContext) =>
{
    var songToDelete = dbContext.Songs.Find(id);

    if (songToDelete == null)
    {
        return Results.NotFound();
    }

    dbContext.Songs.Remove(songToDelete);
    dbContext.SaveChanges();

    return Results.NoContent();

});

app.MapPut("/api/song/{id}", (int id, TunaPianoDbContext dbContext, Song song) =>
{

    var songToUpdate = dbContext.Songs.Find(id);

    if (songToUpdate == null)
    {
        return Results.NotFound();
    }


    songToUpdate.Title = song.Title;
    songToUpdate.ArtistId = song.ArtistId;
    songToUpdate.Album = song.Album;
    songToUpdate.Length = song.Length;

    dbContext.SaveChanges();

    return Results.Ok(songToUpdate);

});


// Artists
app.MapGet("/api/artists", async (TunaPianoDbContext dbContext) =>
{
    var artist = await dbContext.Artists
    .ToListAsync();

    if (artist == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(artist);
});

app.MapGet("/api/artist/{id}", async (int id, TunaPianoDbContext dbContext) =>
{
    var artist = await dbContext.Artists
   .Include(a => a.songs)
   .ThenInclude(a => a.Genres)
   .FirstOrDefaultAsync(a => a.Id == id);

    if (artist == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(artist);
});

app.MapPost("/api/artist", (TunaPianoDbContext dbContext, Artist artist) =>
{
    try
    {

        dbContext.Add(artist);
        dbContext.SaveChanges();
        return Results.Created($"/api/artist/{artist.Id}", artist);

    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }

});

app.MapDelete("/api/artist/{id}", (int id, TunaPianoDbContext dbContext) =>
{
    var artistToDelete = dbContext.Artists.Find(id);

    if (artistToDelete == null)
    {
        return Results.NotFound();
    }

    dbContext.Artists.Remove(artistToDelete);
    dbContext.SaveChanges();

    return Results.NoContent();

});

app.MapPut("/api/artist/{id}", (int id, TunaPianoDbContext dbContext, Artist artist) =>
{

    var artistToUpdate = dbContext.Artists.Find(id);

    if (artistToUpdate == null)
    {
        return Results.NotFound();
    }


    artistToUpdate.Name = artist.Name;
    artistToUpdate.Age = artist.Age;
    artistToUpdate.Bio = artist.Bio;

    dbContext.SaveChanges();

    return Results.Ok(artistToUpdate);

});




app.Run();

