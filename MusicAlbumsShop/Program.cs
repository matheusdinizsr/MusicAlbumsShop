using MusicAlbumsShop.Services;
using MusicAlbumsShop.Storage;
using Microsoft.EntityFrameworkCore;

namespace MusicAlbumsShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddScoped<IGenreStorage, GenreStorage>();
            builder.Services.AddScoped<IBandStorage, BandStorage>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IBandService, BandService>();
            builder.Services.AddScoped<IAlbumService, AlbumService>();
            builder.Services.AddScoped<IAlbumStorage, AlbumStorage>();
            builder.Services.AddScoped<MusicAlbumsContext, MusicAlbumsContext>();

            builder.Services.AddDbContext<MusicAlbumsContext>(options =>
                options.UseSqlServer("Data Source=Math;Initial Catalog=musicalbumsshop;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
