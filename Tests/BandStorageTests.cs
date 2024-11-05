using Microsoft.EntityFrameworkCore;
using MusicAlbumsShop.DTOs;
using MusicAlbumsShop.Models;
using MusicAlbumsShop.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal class BandStorageTests
    {
        private BandStorage _storage;
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
            _storage = new BandStorage(context);
            _assertContext = new MusicAlbumsContext(_dbContextOptions);
            _arrangeContext = new MusicAlbumsContext(_dbContextOptions);
            context.Database.EnsureCreated();
        }

        [Test]
        public void When_AddNewBandSuccess()
        {
            // arrange
            _arrangeContext.Genres.Add(new Genre() { Name = "Rock" });
            _arrangeContext.SaveChanges();


            // act
            var band = _storage.AddOrUpdateBand("Iron Maiden", "England", "1980 - present", 1);

            // assert
            Assert.That(_assertContext.Bands.Count(), Is.EqualTo(1));
            Assert.That(band.Id, Is.EqualTo(1));
            Assert.That(band.Name, Is.EqualTo("Iron Maiden"));
            var fetched = _assertContext.Bands.Find(1);
            Assert.That(fetched, Is.Not.Null);
            Assert.That(fetched.Name, Is.EqualTo("Iron Maiden"));
            Assert.That(fetched.Id, Is.EqualTo(1));
            Assert.That(fetched.Origin, Is.EqualTo("England"));
            Assert.That(fetched.YearsActive, Is.EqualTo("1980 - present"));
            Assert.That(fetched.GenreId, Is.EqualTo(1));
        }

        [Test]
        public void When_UpdateBandSuccess()
        {
            // arrange
            var rockGenre = new Genre() { Name = "Rock" };
            _arrangeContext.Genres.Add(rockGenre);
            _arrangeContext.Bands.Add(new Band() { Name = "The Beatles", Origin = "", YearsActive = "", Genre = rockGenre});
            _arrangeContext.SaveChanges();

            // act
            var band = _storage.AddOrUpdateBand("The Beatles", "England", "1940 - 1980", 1);

            // assert
            Assert.That(_assertContext.Bands.Count(), Is.EqualTo(1));
            Assert.That(band, Is.Not.Null);
            Assert.That(band.Id, Is.EqualTo(1));
            Assert.That(band.Name, Is.EqualTo("The Beatles"));
            var fetched = _assertContext.Bands.Find(1);
            Assert.That(fetched, Is.Not.Null);
            Assert.That(fetched.Id, Is.EqualTo(1));
            Assert.That(fetched.Name, Is.EqualTo("The Beatles"));
            Assert.That(fetched.Origin, Is.EqualTo("England"));
            Assert.That(fetched.YearsActive, Is.EqualTo("1940 - 1980"));
            Assert.That(fetched.GenreId, Is.EqualTo(1));
        }

        [Test]
        public void When_AddBandWithWrongGenreId_ReturnsNull()
        {
            // arrange

            // act
            var band = _storage.AddOrUpdateBand("", "", "", 99);

            // assert
            Assert.That(band == null);
            Assert.That(_assertContext.Bands.Count(), Is.EqualTo(0));
        }

        [Test]
        public void When_GetBandWithNameSuccess()
        {
            // arrange
            _arrangeContext.Genres.Add(new Genre() { Name = "Rock" });
            _arrangeContext.SaveChanges();
            _arrangeContext.Bands.Add(new Band() { Name = "The Beatles", Origin = "", YearsActive = "", GenreId = 1 });
            _arrangeContext.SaveChanges();


            // act
            var bandWithName = _storage.GetBands();

            // assert
            Assert.That(bandWithName, Is.Not.Null);
            Assert.That(bandWithName.Count(), Is.EqualTo(1));
            Assert.That(bandWithName[0].BandId, Is.EqualTo(1));
            Assert.That(bandWithName[0].Name, Is.EqualTo("The Beatles"));
        }

        [Test]
        public void When_GetBandByIdSuccess()
        {
            // arrange
            _arrangeContext.Genres.Add(new Genre() { Name = "Rock" });
            _arrangeContext.SaveChanges();
            _arrangeContext.Bands.Add(new Band() { Name = "The Beatles", Origin = "", YearsActive = "", GenreId = 1 });
            _arrangeContext.SaveChanges();

            // act
            var band = _storage.GetBandById(1);

            //assert
            Assert.That(band, Is.Not.Null);
            Assert.That(band.Id, Is.EqualTo(1));
            Assert.That(band.Name, Is.EqualTo("The Beatles"));

        }

        [Test]
        public void When_GetBandByIdFail()
        {
            // arrange

            // act
            var bandById = _storage.GetBandById(2);

            //assert
            Assert.That(bandById, Is.Null);

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
