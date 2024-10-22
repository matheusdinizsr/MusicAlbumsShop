using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var found = _context.Albums
                .Where(a => a.BandId == bandId)
                .Where(a => a.Title == title)
                .FirstOrDefault();

            if (found != null)
            {
                found.Title = title;
                found.ReleaseDate = releaseDate;
                found.BandId = bandId;
                _context.SaveChanges();
                return found;
            }
            else
            {
                var album = new Album();
                album.Title = title;
                album.ReleaseDate = releaseDate;
                album.BandId = bandId;
                _context.Albums.Add(album);
                _context.SaveChanges();
                return album;
            }

            //?? new Album();

            //found.Title = title;
            //found.ReleaseDate = releaseDate;
            //found.BandId = bandId;

            //_context.Albums.Add(found);

            //_context.SaveChanges();

            //return found;
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
