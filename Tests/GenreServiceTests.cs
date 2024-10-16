using MusicAlbumsShop.Models;
using MusicAlbumsShop.Services;
using MusicAlbumsShop.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal class FakeGenreStorage : IGenreStorage
    {
        public Genre AddGenre(string name)
        {
            throw new NotImplementedException();
        }

        public Genre? GetGenre(string name)
        {
            throw new NotImplementedException();
        }

        public Genre? GetGenreById(int genreId)
        {
            throw new NotImplementedException();
        }

        public Genre[] GetGenres()
        {
            var genres = new Genre[]
            { 
                new Genre { Id = 1, Name = "Rock" },
                new Genre { Id = 2, Name = "Pop" }
            };

            return genres;
        }
    }
    internal class GenreServiceTests
    {
        private GenreService _service;
        private FakeGenreStorage _storage;

        [SetUp]
        public void Setup()
        {
            _storage = new FakeGenreStorage();
            _service = new GenreService(_storage);
        }

        [Test]
        public void When_GetGenres_Success()
        {
            // arrange

            // act
            var response = _service.GetGenres();

            // assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Genres.Count, Is.EqualTo(2));
            Assert.That(response.Genres.Contains("Rock"), Is.True);
            Assert.That(response.Genres.Contains("Pop"), Is.True);
        }
    }
}
