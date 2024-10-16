using MusicAlbumsShop.Models;
using MusicAlbumsShop.Storage;

namespace MusicAlbumsShop.Services
{
    public interface IBandService
    {
        public Band? AddOrUpdateBand(string name, string origin, string yearsActive, int genreId);
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

        public Band? AddOrUpdateBand(string name, string origin, string yearsActive, int genreId)
        {
            var band = _storage.AddOrUpdateBand(name, origin, yearsActive, genreId);

            return band;
        }
    }
}
