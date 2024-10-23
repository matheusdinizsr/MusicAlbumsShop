using Microsoft.EntityFrameworkCore;
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
    
    internal class GenreServiceTests
    {
        private GenreService _service;
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
            _service = new GenreService(_storage);
        }

        [Test]
        public void When_GetGenres_Success()
        {
            // arrange
            var rock = _storage.AddGenre("Rock");
            var blues = _storage.AddGenre("Blues");
            var counter = _testContext.Genres.Count();

            // act
            var response = _service.GetGenres();

            // assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Genres.Count(), Is.EqualTo(counter));
            Assert.That(response.Genres.Contains("Rock"), Is.True);
            Assert.That(response.Genres.Contains("Blues"), Is.True);
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
