using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var genre = new Genre();
            _storageMock.Setup(x => x.AddGenre("")).Returns(genre).Verifiable();

            // act
            var result = _controller.AddOrGetGenre("");

            // assert
            var okObjectCast = result as OkObjectResult;
            var resultCast = okObjectCast.Value as GenreWithTitle;
            Assert.That(resultCast, Is.Not.Null);

        }

       
    }
}
