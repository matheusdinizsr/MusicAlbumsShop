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
        private MusicAlbumsContext _context;
        private DbContextOptions<MusicAlbumsContext> _dbContextOptions;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptions<MusicAlbumsContext>();
            _context = new MusicAlbumsContext(_dbContextOptions);
            _storage = new BandStorage(_context);
        }

        [Test]
        public void When_AddNewBandSuccess()
        {
            // arrange
            //var counter = _context.Bands.Count;
            // act
            var band = _storage.AddOrUpdateBand("Iron Maiden", "England", "1980 - present", 1);

            // assert
            Assert.That(band, Is.Not.Null);
            Assert.That(band.Name, Is.EqualTo("Iron Maiden"));
            Assert.That(band.Id, Is.EqualTo(2));
            Assert.That(band.Origin, Is.EqualTo("England"));
            Assert.That(band.YearsActive, Is.EqualTo("1980 - present"));
            Assert.That(band.GenreId, Is.EqualTo(1));
            //Assert.That(_context.Bands.Count, Is.EqualTo(counter + 1));
           // var fecthed = _context.Bands[1];

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
            Assert.That(band, Is.EqualTo(_context.Bands.FirstOrDefault()));
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
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
