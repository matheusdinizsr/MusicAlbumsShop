using MusicAlbumsShop.Models;
using MusicAlbumsShop.Shared;
using MusicAlbumsShop.Shared.DTOs;
using static MusicAlbumsShop.Shared.ResultWrapper<MusicAlbumsShop.Models.Band>;

namespace MusicAlbumsShop.Storage
{
    public interface IBandStorage
    {
        public Band? AddBand(string name, string origin, string yearsActive, int genreId);
        public Band? UpdateBand(int bandId, string name, string origin, string yearsActive, int genreId);
        public BandWithName[] GetBands();
        public Band? GetBandById(int id);
        public Band? DeleteBandById(int id);
    }
    public class BandStorage : IBandStorage
    {
        private readonly MusicAlbumsContext _context;

        public BandStorage(MusicAlbumsContext context)
        {
            _context = context;
        }

        public Band? AddBand(string name, string origin, string yearsActive, int genreId)
        {
            if (!_context.Genres.Any(g => g.Id == genreId))
            {
                return null;
            }

            var band = new Band();
            band.Name = name;
            band.Origin = origin;
            band.YearsActive = yearsActive;
            band.GenreId = genreId;

            _context.Add(band);
            _context.SaveChanges();
            return band;
        }

        public Band? UpdateBand(int bandId, string name, string origin, string yearsActive, int genreId)
        {
            var band = _context.Bands.Find(bandId);

            if (band == null)
            {
                return null;
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

        public Band? DeleteBandById(int id)
        {
            var band = _context.Bands.Find(id);

            if (band == null)
            {
                return null;
            }

            _context.Bands.Remove(band);
            _context.SaveChanges();
            return band;

        }

    }
}
