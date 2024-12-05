using MusicAlbumsShop.Models;
using MusicAlbumsShop.Storage;

namespace MusicAlbumsShop.Services
{
    public interface IBandService
    {
        public Band? AddOrUpdateBand(int? bandId, string name, string origin, string yearsActive, int genreId);
    }

    public class BandService : IBandService
    {
        private IBandStorage _storage;
        private IGenreService _genreService;

        public BandService(IBandStorage storage, IGenreService genreService)
        {
            _storage = storage;
            _genreService = genreService;
        }

        public Band? AddOrUpdateBand(int? bandId, string name, string origin, string yearsActive, int genreId)
        {
            var band = new Band();
            
            if (bandId == null)
            {
                band = _storage.AddBand(name, origin, yearsActive, genreId);
                return band;
            }

            band = _storage.UpdateBand((int)bandId, name, origin, yearsActive, genreId);
            return band;


        }
    }
}
