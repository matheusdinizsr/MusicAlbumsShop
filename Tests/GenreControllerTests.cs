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
    internal class GenreControllerTests
    {
        private Mock<IGenreService> _serviceMock;
        private Mock<IGenreStorage> _storageMock;
        private GenreController _controller;

        [SetUp]
        public void Setup()
        {
            _storageMock = new Mock<IGenreStorage>(MockBehavior.Strict);
            _serviceMock = new Mock<IGenreService>(MockBehavior.Strict);
            _controller = new GenreController(new Logger<GenreController>(new LoggerFactory()), _serviceMock.Object, _storageMock.Object);
        }

        [Test]
        public void When_AddOrGet_Success()
        {
            // arrange
            var genre = new Genre() {Id = 1, Name = "Rock" };
            _storageMock.Setup(x => x.AddGenre("Rock")).Returns(genre).Verifiable();

            // act
            var result = _controller.AddOrGetGenre("Rock");

            // assert
            var okObjectCast = result as OkObjectResult ;
            var resultCast = okObjectCast?.Value as GenreWithTitle;
            Assert.That(resultCast, Is.Not.Null);
            Assert.That(resultCast.Name, Is.EqualTo("Rock"));
            Assert.That(resultCast.GenreId, Is.EqualTo(1));
            _storageMock.Verify();

        }

       
    }
}
