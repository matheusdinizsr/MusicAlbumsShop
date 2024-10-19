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
        private readonly MusicAlbumsContext _context;

        public GenreStorage(MusicAlbumsContext context)
        {
            _context = context;
        }

        public Genre AddGenre(string name)
        {
            //var found = _context.Genres.Find(name); -> Dando exception

            var found = _context.Genres.Where(g => g.Name == name).FirstOrDefault();

            if (found != null)
            {
                return found;
            }
            else
            {
                var genre = new Genre() { Name = name };
                _context.Genres.Add(genre);
                _context.SaveChanges();
                return genre;
            }

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
