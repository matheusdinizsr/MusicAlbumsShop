using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAlbumsShop.Models;
using MusicAlbumsShop.Shared.DTOs;
using System.Reflection.Metadata.Ecma335;

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
        private readonly MusicAlbumsContext _context;

        public AlbumStorage(MusicAlbumsContext context)
        {
            _context = context;
        }

        public Album? AddOrUpdateAlbum(string title, DateTime releaseDate, int bandId)
        {
            if (!_context.Bands.Any(b => b.Id == bandId))
            {
                return null;
            }

            var album = _context.Albums
                .Where(a => a.BandId == bandId)
                .Where(a => a.Title == title)
                .FirstOrDefault() ?? new Album();

            if (album.Id == 0)
            {
                _context.Albums.Add(album);
            }

            album.Title = title;
            album.ReleaseDate = releaseDate;
            album.BandId = bandId;

            _context.SaveChanges();
            return album;


        }

        public Album? GetAlbumById(int albumId)
        {
            var found = _context.Albums.Include(a => a.Band).FirstOrDefault(a => a.Id == albumId);

            return found;
        }

        public AlbumWithTitle[]? GetAlbumsFromABand(int bandId)
        {
            var band = _context.Bands.Find(bandId);

            if (band == null)
            {
                return null;
            }

            var albums = _context.Albums
                .Where(a => a.BandId == band.Id)
                .Select(a => new AlbumWithTitle { AlbumId = a.Id, Title = a.Title, BandName = band.Name })
                .ToArray();

            return albums;
        }
    }
}
