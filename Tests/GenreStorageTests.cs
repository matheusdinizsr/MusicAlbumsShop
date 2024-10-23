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
        private MusicAlbumsContext context;
        private MusicAlbumsContext _testContext;
        private DbContextOptions<MusicAlbumsContext> _dbContextOptions;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<MusicAlbumsContext>();
            builder.UseSqlServer("Data Source=Math;Initial Catalog=testmusicalbumsshop;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
            _dbContextOptions = builder.Options;
            context = new MusicAlbumsContext(_dbContextOptions);
            _storage = new GenreStorage(context);
            _testContext = new MusicAlbumsContext(_dbContextOptions);
            context.Database.EnsureCreated();
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
            _testContext = new MusicAlbumsContext(_dbContextOptions);
            var counter = _testContext.Genres.Count();
            Assert.That(counter, Is.EqualTo(1));
            var fetched = _testContext.Genres.FirstOrDefault(g => g.Name == genre.Name);
            Assert.That(fetched, Is.Not.Null);
            Assert.That(genre.Id, Is.EqualTo(fetched.Id));
            Assert.That(genre.Name, Is.EqualTo(fetched.Name));
        }

        [Test]
        public void When_AddNewGenre_AlreadyExists()
        {
            //arrange
            var arrangeGenre = _storage.AddGenre("Rock");
            var counter = _testContext.Genres.Count();

            //act
            var genre = _storage.AddGenre("Rock");

            //assert
            Assert.That(arrangeGenre, Is.EqualTo(genre));
            Assert.That(_testContext.Genres.Count, Is.EqualTo(counter));
        }

        [Test]
        public void When_GetGenres_Success()
        {
            // arrange
            var rockGenre = _storage.AddGenre("Rock");
            var bluesGenre = _storage.AddGenre("Blues");
            var counter = _testContext.Genres.Count();

            // act
            var gotGenres = _storage.GetGenres();

            // assert
            Assert.That(gotGenres.Length, Is.EqualTo(counter));
            Assert.That(_testContext.Genres.Count(), Is.EqualTo(counter));
            Assert.That(gotGenres.Count(x => x.Name == "Rock") == 1, Is.True);
            Assert.That(_testContext.Genres.Count(x => x.Name == "Rock") == 1, Is.True);
            Assert.That(gotGenres.Count(x => x.Name == "Blues") == 1, Is.True);
            Assert.That(_testContext.Genres.Count(x => x.Name == "Blues") == 1, Is.True);

        }

        [Test]
        public void When_GetGenresById_Success()
        {
            // arrange
            var rock = _storage.AddGenre("Rock");
            var counter = _testContext.Genres.Count();

            // act
            var genre = _storage.GetGenreById(1);

            // assert
            Assert.That(genre, Is.Not.Null);
            var fetched = _testContext.Genres.FirstOrDefault(g => g.Name == rock.Name);
            Assert.That(fetched, Is.Not.Null);
            Assert.That(genre.Id, Is.EqualTo(fetched.Id));
            Assert.That(genre.Name, Is.EqualTo(fetched.Name));
        }

        [TearDown]
        public void TearDown()
        {
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Dispose();
                _testContext.Dispose();
            }
        }
    }
}