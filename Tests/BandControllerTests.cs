using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MusicAlbumsShop.Controllers;
using MusicAlbumsShop.Models;
using MusicAlbumsShop.Services;
using MusicAlbumsShop.Shared.DTOs;
using MusicAlbumsShop.Storage;

namespace Tests
{
    internal class BandControllerTests
    {
        private BandController _controller;
        private Mock<IBandStorage> _bandStorageMock;
        private Mock<IBandService> _bandServiceMock;
        private Mock<IGenreService> _genreServiceMock;

        [SetUp]
        public void Setup()
        {
            _bandStorageMock = new Mock<IBandStorage>(MockBehavior.Strict);
            _genreServiceMock = new Mock<IGenreService>(MockBehavior.Strict);
            _bandServiceMock = new Mock<IBandService>(MockBehavior.Strict);
            _controller = new BandController(_bandStorageMock.Object, _bandServiceMock.Object, _genreServiceMock.Object, new Logger<BandController>(new LoggerFactory()));
        }

        [Test]
        public void When_AddBand_Success()
        {
            // arrange
            var id = 1;
            var name = "";
            var origin = "";
            var yearsActive = "";
            var genreId = 1;

            var band = new Band() { Id = id, Name = name };
            _genreServiceMock.Setup(x => x.GetGenreById(1)).Returns(new Genre() { Id = 1 });
            _bandServiceMock.Setup(x => x.AddOrUpdateBand(name, origin, yearsActive, genreId)).Returns(band);

            // act
            var result = _controller.AddOrUpdateBand(name, origin, yearsActive, genreId);

            // assert
            Assert.That(result, Is.Not.Null);
            var castResult = result as OkObjectResult;
            var bandCast = castResult?.Value as BandWithName;
            Assert.That(bandCast?.BandId, Is.EqualTo(1));
            Assert.That(bandCast.Name, Is.EqualTo(""));
        }

        [Test]
        public void When_AddBand_WrongGenreId_BadRequest()
        {
            // arrange
            var genreId = 99;

            _genreServiceMock.Setup(x => x.GetGenreById(genreId)).Returns((Genre?)null);

            // act
            var result = _controller.AddOrUpdateBand("", "", "", genreId);

            // assert
            var castResult = result as BadRequestObjectResult;
            Assert.That(castResult, Is.Not.Null);
            Assert.That(castResult.Value, Is.EqualTo("Genre ID does not exist"));
        }

        [Test]
        public void When_GetBands_Success()
        {
            // arrange
            _bandStorageMock.Setup(x => x.GetBands()).Returns(new BandWithName[] {new BandWithName() {BandId = 1, Name = "The Beatles"} });

            // act
            var result = _controller.GetBands();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].BandId, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("The Beatles"));
        }

        [Test]
        public void When_GetDetails_NotFound()
        {
            // arrange
            _bandStorageMock.Setup(x => x.GetBandById(1)).Returns((Band?)null);

            // act
            var result = _controller.GetBandDetails(1) as NotFoundObjectResult;

            // assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void When_GetDetails_Success()
        {
            // arrange
            _bandStorageMock.Setup(x => x.GetBandById(1))
                .Returns(new Band() { Id = 1, GenreId = 1, Name = "The Beatles", Origin = "England", YearsActive = "1950 - 1970" });
            _genreServiceMock.Setup(x => x.GetGenreById(1)).Returns(new Genre() { Id = 1, Name = "Rock" });

            // act
            var result = _controller.GetBandDetails(1) as OkObjectResult;

            // assert
            Assert.That(result, Is.Not.Null);
            var band = result.Value as BandDetails; // casting
            Assert.That(band?.Name, Is.EqualTo("The Beatles"));
            Assert.That(band.GenreName, Is.EqualTo("Rock"));
            Assert.That(band.Origin, Is.EqualTo("England"));
            Assert.That(band.YearsActive, Is.EqualTo("1950 - 1970"));

        }

        [Test]
        public void When_DeleteBandById_Success()
        {
            // arrange
            _bandStorageMock.Setup(x => x.DeleteBandById(1))
                .Returns(new Band() { Id = 1, GenreId = 1, Name = "", Origin = "", YearsActive = "" });

            // act
            var result = _controller.DeleteBandById(1) as OkObjectResult;

            // assert
            Assert.That(result, Is.Not.Null);
            var band = result.Value as BandWithName;
            Assert.That(band?.BandId, Is.EqualTo(1));
            Assert.That(band.Name, Is.EqualTo(""));
        }

       

        
    }
}
