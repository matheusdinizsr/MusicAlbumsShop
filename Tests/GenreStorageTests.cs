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
        private MusicAlbumsContext _assertContext;
        private MusicAlbumsContext _arrangeContext;
        private DbContextOptions<MusicAlbumsContext> _dbContextOptions;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<MusicAlbumsContext>();
            builder.UseSqlServer("Data Source=Math;Initial Catalog=testmusicalbumsshop;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
            _dbContextOptions = builder.Options;
            context = new MusicAlbumsContext(_dbContextOptions);
            _storage = new GenreStorage(context);
            _arrangeContext = new MusicAlbumsContext(_dbContextOptions);
            _assertContext = new MusicAlbumsContext(_dbContextOptions);
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
            var counter = _assertContext.Genres.Count();
            Assert.That(counter, Is.EqualTo(1));
            var fetched = _assertContext.Genres.FirstOrDefault(g => g.Name == genre.Name);
            Assert.That(fetched, Is.Not.Null);
            Assert.That(genre.Id, Is.EqualTo(fetched.Id));
            Assert.That(genre.Name, Is.EqualTo(fetched.Name));
        }

        [Test]
        public void When_AddNewGenre_AlreadyExists()
        {
            //arrange
            var firstAddedGenre = _arrangeContext.Genres.Add(new Genre() { Id = 1, Name = "Rock"});

            //act
            var genre = _storage.AddGenre("Rock");

            //assert
            Assert.That(_assertContext.Genres.Count, Is.EqualTo(1));
            Assert.That(genre.Id, Is.EqualTo(1));
        }

        [Test]
        public void When_GetGenres_Success()
        {
            // arrange
            var rockGenre = _arrangeContext.Genres.Add(new Genre() { Name = "Rock"});
            var bluesGenre = _arrangeContext.Genres.Add(new Genre() { Name = "Blues"});
            _arrangeContext.SaveChanges();

            // act
            var gotGenres = _storage.GetGenres();

            // assert
            Assert.That(gotGenres.Count(x => x.Name == "Rock"), Is.EqualTo(1));
            Assert.That(gotGenres.Count(x => x.Name == "Blues"), Is.EqualTo(1));
            Assert.That(gotGenres.Length, Is.EqualTo(2));
            
        }

        [Test]
        public void When_GetGenresById_Success()
        {
            // arrange
            var rockGenre = _arrangeContext.Genres.Add(new Genre() { Name = "Rock"});
            _arrangeContext.SaveChanges();

            // act
            var genre = _storage.GetGenreById(1);

            // assert
            Assert.That(_assertContext.Genres.Count(), Is.EqualTo(1));
            Assert.That(genre, Is.Not.Null);
            Assert.That(genre.Id, Is.EqualTo(1));
            Assert.That(genre.Name, Is.EqualTo("Rock"));
        }

        [TearDown]
        public void TearDown()
        {
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Dispose();
                _assertContext.Dispose();
                _arrangeContext.Dispose();
            }
        }
    }
}