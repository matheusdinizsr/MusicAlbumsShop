using MusicAlbumsShop.Models;
using MusicAlbumsShop.Shared.DTOs;
using MusicAlbumsShop.Storage;

namespace MusicAlbumsShop.Services // logica/regra de negocio
{
    public interface IGenreService
    {
        public GetGenresResponse GetGenres();
        public Genre? GetGenre(string genre);
        public Genre? GetGenreById(int genreId);
    }
    public class GenreService : IGenreService
    {
        private IGenreStorage _storage;
        public GenreService(IGenreStorage storage)
        {
            _storage = storage;
        }


        public GetGenresResponse GetGenres()
        {
            var genres = _storage.GetGenres();
            var response = new GetGenresResponse();
            response.GenresAndIds = genres.Select(x => new GenreWithTitle() {GenreId = x.Id, Name = x.Name }).ToArray(); // Linq // projeção
            return response;
        }
        public Genre? GetGenre(string genre)
        {
            return _storage.GetGenre(genre);
        }

        public Genre? GetGenreById(int genreId)
        {
            return _storage.GetGenreById(genreId);
        }
    }

}
