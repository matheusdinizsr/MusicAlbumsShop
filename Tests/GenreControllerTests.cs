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
    internal class GenreControllerTests
    {
        private GenreController _genreController;
        private Mock<IGenreService> _genreServiceMock;
        private Mock<IGenreStorage> _genreStorageMock;

        [SetUp]
        public void SetUp()
        {
            _genreServiceMock = new Mock<IGenreService>(MockBehavior.Strict);
            _genreStorageMock = new Mock<IGenreStorage>(MockBehavior.Strict);
            _genreController = new GenreController(new Logger<GenreController>(new LoggerFactory()), _genreServiceMock.Object, _genreStorageMock.Object);
        }

        [Test]
        public void When_AddOrGet_Success()
        {
            // arrange
            var genreName = "Jazz";
            var genre = new Genre();
            _genreStorageMock.Setup(x => x.AddGenre(genreName)).Returns(genre).Verifiable();

            // act
            var result = _genreController.AddOrGetGenre(genreName);

            // assert
            Assert.That(result, Is.EqualTo(genre));
            _genreStorageMock.Verify();
        }
    }
}
