using MusicAlbumsShop.DTOs;
using MusicAlbumsShop.Models;
using System.Reflection.Metadata.Ecma335;
using static MusicAlbumsShop.DTOs.AlbumWithTitle;

namespace MusicAlbumsShop.Storage
{
    public interface IAlbumStorage
    {
        public Album? AddOrUpdateAlbum(string title, DateTime releaseDate, int bandId);
        public AlbumWithTitle[]? GetAlbumsFromABand(int bandId);
        public Album? GetAlbumById(int albumId);
    }

    public class AlbumStorage : IAlbumStorage
    {
        private readonly BandsContext _context;

        public AlbumStorage(BandsContext context)
        {
            _context = context;
        }

        public Album? AddOrUpdateAlbum(string title, DateTime releaseDate, int bandId)
        {
            if (!_context.Bands.Any(b => b.Id == bandId))
            {
                return null;
            }

            var found = _context.Albums
                .Where(a => a.BandId == bandId)
                .Where(a => a.Title == title)
                .FirstOrDefault() ?? new Album();

            found.Title = title;
            found.ReleaseDate = releaseDate;
            found.BandId = bandId;

            return found;
        }

        public Album? GetAlbumById(int albumId)
        {
            var found = _context.Albums.Find(albumId);

            return found;
        }

        public AlbumWithTitle[]? GetAlbumsFromABand(int bandId)
        {
            var band = _context.Bands.Find(bandId);

            if (band == null)
            {
                return null; // Retorna nulo
            }

            var albums = _context.Albums
                .Where(a => a.BandId == band.Id)
                .Select(a => new AlbumWithTitle { AlbumId = a.Id, Title = a.Title, BandName = band.Name })
                .ToArray();

            return albums;
        }
    }
}
