using MusicAlbumsShop.Models;
using MusicAlbumsShop.Shared.DTOs;

namespace MusicAlbumsShop.Storage
{
    public interface IBandStorage
    {
        public Band? AddOrUpdateBand(string name, string origin, string yearsActive, int genreId);
        public BandWithName[] GetBands();
        public Band? GetBandById(int id);
    }
    public class BandStorage : IBandStorage
    {
        private readonly MusicAlbumsContext _context;

        public BandStorage(MusicAlbumsContext context)
        {
            _context = context;
        }
        public Band? AddOrUpdateBand(string name, string origin, string yearsActive, int genreId)
        {
            if (!_context.Genres.Any(g => g.Id == genreId))
            {
                return null;
            }

            var band = _context.Bands.FirstOrDefault(b => b.Name == name) ?? new Band();

            if (band.Id == 0)
            {
                _context.Bands.Add(band);
            }

            band.Name = name;
            band.Origin = origin;
            band.YearsActive = yearsActive;
            band.GenreId = genreId;

            _context.SaveChanges();
            return band;

        }

        public BandWithName[] GetBands()
        {
            var result = _context.Bands.Select(x => new BandWithName() { BandId = x.Id, Name = x.Name }).ToArray();
            return result;
        }

        public Band? GetBandById(int id)
        {

            var result = _context.Bands.Find(id);

            return result;
        }

    }
}
