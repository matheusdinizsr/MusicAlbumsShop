using Microsoft.EntityFrameworkCore;
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
        private MusicAlbumsContext _context;
        private DbContextOptions<MusicAlbumsContext> _dbContextOptions;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<MusicAlbumsContext>();
            builder.UseSqlServer("Data Source=Math;Initial Catalog=testmusicalbumsshop;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
            _dbContextOptions = builder.Options;
            _context = new MusicAlbumsContext(_dbContextOptions);
            _context.Database.EnsureCreated();
            _storage = new GenreStorage(_context);
        }

        [Test]
        public void When_AddNewGenre_Success()
        {
            // arrange

            // act
            var genre = _storage.AddGenre("Blues");

            // assert
            Assert.That(genre, Is.Not.Null);
            Assert.That(genre.Id, Is.EqualTo(1));
            Assert.That(genre.Name, Is.EqualTo("Blues"));
            var fetched = _context.Genres.FirstOrDefault(g => g.Name == genre.Name);
            Assert.That(genre, Is.EqualTo(fetched));

        }

        [Test]
        public void When_AddNewGenre_AlreadyExists()
        {
            //arrange
            _context.Genres.Add(new Genre { Name = "Rock" });
            var counter = _context.Genres.Count();

            //act
            var genre = _storage.AddGenre("Rock");

            //assert
            var fetched = _context.Genres.FirstOrDefault(g => g.Name == "Rock");
            Assert.That(genre, Is.EqualTo(fetched));
            Assert.That(_context.Genres.Count, Is.EqualTo(counter));
        }

        [Test]
        public void When_GetGenres_Success()
        {
            // arrange
            var rockGenre = _storage.AddGenre("Rock");
            var bluesGenre = _storage.AddGenre("Blues");
            var counter = _context.Genres.Count();

            // act
            var gotGenres = _storage.GetGenres();

            // assert
            Assert.That(gotGenres.Length, Is.EqualTo(counter));
            Assert.That(gotGenres.Count(x => x.Name == "Rock") == 1, Is.True);
            Assert.That(gotGenres.Count(x => x.Name == "Blues") == 1, Is.True);

        }

        [Test]
        public void When_GetGenresById_Success() // novo
        {
            // arrange

            // act
            var genre = _storage.GetGenreById(1);

            // assert
            Assert.That(genre, Is.Not.Null);
            //Assert.That(genre, Is.EqualTo(_context.Genres.Genres[0]));
        }

        [TearDown]
        public void TearDown()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}