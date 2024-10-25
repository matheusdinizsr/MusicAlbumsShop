using Microsoft.EntityFrameworkCore;
using MusicAlbumsShop.DTOs;
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
        private MusicAlbumsContext _testContext;
        private DbContextOptions<MusicAlbumsContext> _dbContextOptions;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<MusicAlbumsContext>();
            builder.UseSqlServer("Data Source=Math;Initial Catalog=testmusicalbumsshop;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
            _dbContextOptions = builder.Options;
            context = new MusicAlbumsContext(_dbContextOptions);
            _storage = new BandStorage(context);
            _testContext = new MusicAlbumsContext(_dbContextOptions);
            context.Database.EnsureCreated();
        }

        [Test]
        public void When_AddNewBandSuccess()
        {
            // arrange
            var counter = _testContext.Bands.Count();
            
            // act
            var band = _storage.AddOrUpdateBand("Iron Maiden", "England", "1980 - present", 1);

            // assert
            Assert.That(_testContext.Bands.Count(), Is.EqualTo(counter + 1));
            Assert.That(band, Is.Not.Null);
            Assert.That(band.Name, Is.EqualTo("Iron Maiden"));
            Assert.That(band.Id, Is.EqualTo(1));
            Assert.That(band.Origin, Is.EqualTo("England"));
            Assert.That(band.YearsActive, Is.EqualTo("1980 - present"));
            Assert.That(band.GenreId, Is.EqualTo(1));

            //Assert.That(band, Is.EqualTo(fecthed));

        }

        [Test]
        public void When_UpdateBandSuccess()
        {
            // arrange
           // var counter = _context.Bands.Count;
            // act
            var band = _storage.AddOrUpdateBand("The Beatles", "United Kingdom", "1940 - 1980", 2);

            // assert
            Assert.That(band.Name, Is.EqualTo("The Beatles"));
            Assert.That(band.Origin, Is.EqualTo("United Kingdom"));
            Assert.That(band.YearsActive, Is.EqualTo("1940 - 1980"));
            Assert.That(band.GenreId, Is.EqualTo(2));
            //Assert.That(band, Is.EqualTo(_context.Bands.FirstOrDefault()));
            //Assert.That(_context.Bands.Count, Is.EqualTo(counter));
        }

        [Test]
        public void When_AddBandWithWrongGenreId_ReturnsNull()
        {
            // arrange
            //var counter = _context.Bands.Count;

            // act
            var band = _storage.AddOrUpdateBand("Calypso", "Pará", "2010 - 2015", 99);

            // assert
            Assert.That(band == null);
           // Assert.That(_context.Bands.Count, Is.EqualTo(counter) );
        }

        [Test]
        public void When_GetBandWithNameSuccess()
        {
            // arrange
            // act
            var bandWithName = _storage.GetBands();

            // assert
            Assert.That(bandWithName, Is.Not.Null);
            Assert.That(bandWithName.Count, Is.EqualTo(1));
            Assert.That(bandWithName[0].BandId, Is.EqualTo(1));
            Assert.That(bandWithName[0].Name, Is.EqualTo("The Beatles"));
        }

        [Test]
        public void When_GetBandByIdSuccess()
        {
            // arrange
            // act
            var bandById = _storage.GetBandById(1);

            //assert
            Assert.That(bandById, Is.Not.Null);
           // Assert.That(bandById, Is.EqualTo(_context.Bands[0]));

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
                _testContext.Dispose();
            }
        }
    }
}
