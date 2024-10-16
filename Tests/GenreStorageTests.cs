using MusicAlbumsShop.Controllers;
using MusicAlbumsShop.Models;
using MusicAlbumsShop.Storage;
using NUnit.Framework.Legacy;
using System.Runtime.CompilerServices;

namespace Tests
{
    public class GenreStorageTests
    {
        private GenreStorage _storage;
        private BandsContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new BandsContext();
            _storage = new GenreStorage(_context);
        }

        [Test]
        public void When_AddNewGenre_Success()
        {
            // arrange

            // act
            var genre = _storage.AddGenre("blues");

            // assert
            Assert.That(genre, Is.Not.Null);
            Assert.That(genre.Id, Is.EqualTo(4));
            Assert.That(genre.Name, Is.EqualTo("blues"));
            var fetched = _context.Genres.LastOrDefault(); // alterei
            Assert.That(genre, Is.EqualTo(fetched));

        }

        [Test]
        public void When_AddNewGenre_AlreadyExists()
        {
            //arrange
            _context.Genres.Add(new Genre { Id = 1, Name = "Rock" });
            var counter = _context.Genres.Count();

            //act
            var genre = _storage.AddGenre("Rock");

            //assert
            var fetched = _context.Genres.FirstOrDefault();
            Assert.That(genre, Is.EqualTo(fetched));
            Assert.That(_context.Genres.Count, Is.EqualTo(counter));
        }

        [Test]
        public void When_GetGenres_Success()
        {
            // arrange
            var counter = _context.Genres.Count();

            // act
            var gotGenres = _storage.GetGenres();

            // assert
            Assert.That(gotGenres.Length, Is.EqualTo(counter));
            Assert.That(gotGenres.Count(x => x.Name == "Rock") == 1, Is.True);
            Assert.That(gotGenres.Count(x => x.Name == "Pop") == 1, Is.True);
            Assert.That(gotGenres.Count(x => x.Name == "Heavy Metal") == 1, Is.True);

        }

        [Test]
        public void When_GetGenresById_Success() // novo
        {
            // arrange

            // act
            var genre = _storage.GetGenreById(1);

            // assert
            Assert.That(genre, Is.Not.Null);
            Assert.That(genre, Is.EqualTo(_context.Genres[0]));
        }
    }
}