using Microsoft.EntityFrameworkCore;
using Moq;
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
    
    internal class GenreServiceTests
    {
        private GenreService _service;
        private Mock<IGenreStorage> _storageMock;

        [SetUp]
        public void Setup()
        {
            _storageMock = new Mock<IGenreStorage>(MockBehavior.Strict);
            _service = new GenreService(_storageMock.Object);
        }

        [Test]
        public void When_GetGenres_Success()
        {
            // arrange
            var genres = new Genre[] { new Genre() { Name = "Rock" } };

            _storageMock.Setup(x => x.GetGenres()).Returns(genres).Verifiable();

            // act
            var response = _service.GetGenres();

            // assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Genres.Length, Is.EqualTo(1));
            var item = response.Genres[0];
            Assert.That(item, Is.EqualTo("Rock"));
            _storageMock.Verify();
        }
      
    }
}
