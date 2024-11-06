using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MusicAlbumsShop.Controllers;
using MusicAlbumsShop.Models;
using MusicAlbumsShop.Services;
using MusicAlbumsShop.Shared.DTOs;
using MusicAlbumsShop.Storage;
using NUnit.Framework.Internal;

namespace Tests
{
    internal class AlbumControllerTests
    {
        private AlbumController _albumController;
        private Mock<IBandStorage> _bandStorageMock;
        private Mock<IAlbumService> _albumServiceMock;
        private Mock<IAlbumStorage> _albumStorageMock;

        [SetUp]
        public void Setup()
        {
            _bandStorageMock = new Mock<IBandStorage>(MockBehavior.Strict);
            _albumServiceMock = new Mock<IAlbumService>(MockBehavior.Strict);
            _albumStorageMock = new Mock<IAlbumStorage>(MockBehavior.Strict);
            _albumController = new AlbumController(_albumServiceMock.Object, new Logger<AlbumController>(new LoggerFactory()), _bandStorageMock.Object, _albumStorageMock.Object);
        }

        [Test]
        public void When_AddOrUpdateAlbum_Success()
        {
            // arrange
            var title = "";
            var releaseDate = DateTime.Today;
            var bandId = 1;
            var band = new Band() { Id = 1, Name = "" };
            var album = new Album() { Id = 1, Title = title, ReleaseDate = releaseDate, Band = band };

            _albumServiceMock.Setup(x => x.AddOrUpdateAlbum(title, releaseDate, bandId)).Returns(album).Verifiable();

            // act
            var result = _albumController.AddOrUpdateAlbum(title, releaseDate, bandId);

            // assert
            Assert.That(result, Is.Not.Null);
            var resultCast = result as OkObjectResult;
            var albumCast = resultCast?.Value as AlbumWithTitle;
            Assert.That(albumCast?.AlbumId, Is.EqualTo(album.Id));
            Assert.That(albumCast.Title, Is.EqualTo(album.Title));
            _albumServiceMock.Verify();
        }

        [Test]
        public void When_GetAlbumsFromABand_BandDoesntExist()
        {
            // arrange
            _albumStorageMock.Setup(x => x.GetAlbumsFromABand(1)).Returns(null as AlbumWithTitle[]).Verifiable();

            // act
            var result = _albumController.GetAlbumsFromABand(1);

            // assert
            var resultCast = result as NotFoundObjectResult;
            Assert.That(resultCast?.Value, Is.EqualTo("Band does not exist"));
        }

        [Test]
        public void When_GetAlbumsFromABand_Success()
        {
            // arrange
            var albumsReturned = new AlbumWithTitle[] { };

            _albumStorageMock.Setup(x => x.GetAlbumsFromABand(1)).Returns(albumsReturned).Verifiable();

            // act
            var result = _albumController.GetAlbumsFromABand(1);

            // assert
            Assert.That(result, Is.Not.Null);
            var resultCast = result as OkObjectResult;
            var albumsCast = resultCast?.Value as AlbumWithTitle[];
            Assert.That(albumsCast, Is.Not.Null);
            _albumStorageMock.Verify();
        }

       

        public void When_GetAlbumDetails_Success()
        {
            // arrange
            var band = new Band() { Name = "" };
            var album = new Album() { Id = 1, Title = "", Band = band, ReleaseDate = DateTime.Today };
            _albumStorageMock.Setup(x => x.GetAlbumById(1)).Returns(album).Verifiable();

            // act
            var result = _albumController.GetAlbumDetails(1);

            // assert
            Assert.That(result, Is.Not.Null );
            var resultCast = result as OkObjectResult;
            var albumCast = resultCast?.Value as AlbumDetails;
            Assert.That(albumCast?.Title, Is.EqualTo(album.Title));
            Assert.That(albumCast.ReleaseDate, Is.EqualTo(album.ReleaseDate));
            Assert.That(albumCast.BandName, Is.EqualTo(album.Band.Name));
            _albumStorageMock.Verify();

        }
    }
}
