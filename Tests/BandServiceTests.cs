using Microsoft.Extensions.Logging;
using Moq;
using MusicAlbumsShop.Controllers;
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
    internal class BandServiceTests
    {
        public BandService _bandService;
        private Mock<IBandStorage> _bandStorageMock;
        private Mock<IGenreService> _genreServiceMock;

        [SetUp]
        public void Setup()
        {
            _bandStorageMock = new Mock<IBandStorage>(MockBehavior.Strict);
            _genreServiceMock = new Mock<IGenreService>(MockBehavior.Strict);
            _bandService = new BandService(_bandStorageMock.Object, _genreServiceMock.Object);
        }

        [Test]
        public void When_AddBand_Success()
        {
            // arrange
            var name = "Iron Maiden";
            var origin = "England";
            var yearsActive = "1980 - present";
            var genreId = 3;

            var band = new Band();
            _bandStorageMock.Setup(x => x.AddOrUpdateBand(name, origin, yearsActive, genreId)).Returns(band);

            // act
            var result = _bandService.AddOrUpdateBand(name, origin, yearsActive, genreId);
            
            // assert
            Assert.That(band, Is.EqualTo(result));
        }

    }
}
