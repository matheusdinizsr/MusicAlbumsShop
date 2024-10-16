using MusicAlbumsShop.Models;

namespace MusicAlbumsShop.Storage
{

    public interface IGenreStorage
    {
        public Genre AddGenre(string name);
        public Genre[] GetGenres();
        public Genre? GetGenre(string name);
        public Genre? GetGenreById(int genreId);
    }

    public class GenreStorage : IGenreStorage // manipular dados
    {
        private readonly BandsContext _context;

        public GenreStorage(BandsContext context)
        {
            _context = context;
        }

        public Genre AddGenre(string name)
        {
            var found = _context.Genres.Find(name);

            if (found != null)
            {
                return found;
            }

            var genre = new Genre() { Name = name};
            _context.Genres.Add(genre);
            return genre;

        }


        public Genre[] GetGenres()
        {
            return _context.Genres.ToArray();
        }

        public Genre? GetGenre(string name)
        {
            return _context.Genres.Where(g => g.Name == name).FirstOrDefault();
        }

        public Genre? GetGenreById(int genreId) // novo
        {
            return _context.Genres.Where(g => g.Id == genreId).FirstOrDefault();
        }
    }
}
