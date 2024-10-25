using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MusicAlbumsShop.Controllers;
using MusicAlbumsShop.DTOs;
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
        public void When_GetBands_Success()
        {
            // arrange
            _bandStorageMock.Setup(x => x.GetBands()).Returns(new[] {
                new BandWithName()
                {
                    BandId = 1,
                    Name = "The Beatles"
                }
            });

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
            _bandStorageMock.Setup(x => x.GetBandById(2)).Returns((Band?)null);

            // act
            var result = _controller.GetBandDetails(2) as NotFoundObjectResult;

            // assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void When_GetDetails_Success()
        {
            // arrange
            _bandStorageMock.Setup(x => x.GetBandById(1))
                .Returns(new Band() { Id = 1, GenreId = 1, Name = "The Beatles", Origin = "England", YearsActive = "1950 - 1970" });
            _genreServiceMock.Setup(x => x.GetGenreById(1)).Returns(new Genre() { Id = 1, Name = "Rock" }); // adicionei

            // act
            var result = _controller.GetBandDetails(1) as OkObjectResult;

            // assert
            Assert.That(result, Is.Not.Null);
            var band = result.Value as BandDetails; // casting
            Assert.That(band.Name, Is.EqualTo("The Beatles"));
            Assert.That(band.GenreName, Is.EqualTo("Rock"));
            Assert.That(band.Origin, Is.EqualTo("England"));
            Assert.That(band.YearsActive, Is.EqualTo("1950 - 1970"));

        }

        [Test]
        public void When_AddBand_Success()
        {
            // arrange
            var name = "Iron Maiden";
            var origin = "England";
            var yearsActive = "1980 - present";
            var genreId = 1;

            var band = new Band();
            _genreServiceMock.Setup(x => x.GetGenreById(3)).Returns(new Genre());
            _bandServiceMock.Setup(x => x.AddOrUpdateBand(name, origin, yearsActive, genreId)).Returns(band);

            // act
            var result = _controller.AddBand(name, origin, yearsActive, genreId);

            // assert
            Assert.That(result, Is.Not.Null);
            var castResult = result as OkObjectResult;
            var bandCast = castResult.Value as Band;
            Assert.That(bandCast, Is.EqualTo(band));
        }

        [Test]
        public void When_AddBand_WrongGenreId_BadRequest()
        {
            // arrange
            var genreId = 99;

            _genreServiceMock.Setup(x => x.GetGenreById(genreId)).Returns((Genre?)null);

            // act
            var result = _controller.AddBand("", "", "", genreId);

            // assert
            var castResult = result as BadRequestObjectResult;
            Assert.That(castResult, Is.Not.Null);
            Assert.That(castResult.Value, Is.EqualTo("Genre ID does not exist"));
        }
    }
}
