using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TunaPiano;

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


app.MapGet("/api/songs", async (TunaPianoDbContext dbContext) =>
{
    var songsWithGenre = await dbContext.Songs.Include("Genres").ToListAsync();

    return Results.Ok(songsWithGenre);
});


app.Run();

