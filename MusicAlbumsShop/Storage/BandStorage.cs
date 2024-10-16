using MusicAlbumsShop.DTOs;
using MusicAlbumsShop.Models;

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
        private readonly BandsContext _context;

        public BandStorage(BandsContext context)
        {
            _context = context;
        }
        public Band? AddOrUpdateBand(string name, string origin, string yearsActive, int genreId)
        {
            if (!_context.Genres.Any(g => g.Id == genreId))
            {
                return null;
            }
            
            var found = _context.Bands.Find(name) ?? new Band();

            found.Name = name;
            found.Origin = origin;
            found.YearsActive = yearsActive;
            found.GenreId = genreId;

            return found;
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
