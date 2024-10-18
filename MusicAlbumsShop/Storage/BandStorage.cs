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

            var found = _context.Bands.Where(b => b.Name == name).FirstOrDefault();

            if (found != null)
            {
                found.Name = name;
                found.Origin = origin;
                found.YearsActive = yearsActive;
                found.GenreId = genreId;
                _context.SaveChanges();
                return found;
            }
            else
            {
                var band = new Band();
                band.Name = name;
                band.Origin = origin;
                band.YearsActive = yearsActive;
                band.GenreId = genreId;

                _context.Bands.Add(band);
                _context.SaveChanges();
                return band;
            }


            //var found = _context.Bands.Find(name) ?? new Band();
            //var found = _context.Bands.Where(b => b.Name == name).FirstOrDefault() ?? new Band();

            //found.Name = name;
            //found.Origin = origin;
            //found.YearsActive = yearsActive;
            //found.GenreId = genreId;

            //_context.Bands.Add(found);
            //_context.SaveChanges();

            //return found;
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
