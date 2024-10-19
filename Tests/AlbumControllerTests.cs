using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MusicAlbumsShop.Controllers;
using MusicAlbumsShop.DTOs;
using MusicAlbumsShop.Models;
using MusicAlbumsShop.Services;
using MusicAlbumsShop.Storage;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void When_AddAlbum_Success()
        {
            // arrange
            var title = "";
            var releaseDate = DateTime.Now;
            var bandId = 1;

            var album = new Album();
            _bandStorageMock.Setup(x => x.GetBandById(bandId)).Returns(new Band()).Verifiable();
            _albumServiceMock.Setup(x => x.AddOrUpdateAlbum(title, releaseDate, bandId)).Returns(album).Verifiable();
            //_albumStorageMock.Setup(x => x.AddOrUpdateAlbum(title, releaseDate, bandId)).Returns(album).Verifiable();

            // act
            var result = _albumController.AddOrUpdateAlbum(title, releaseDate, bandId);

            // assert
            Assert.That(result, Is.Not.Null);
            var resultCast = result as OkObjectResult;
            var albumCast = resultCast.Value as Album;
            Assert.That(albumCast, Is.EqualTo(album));
            _bandStorageMock.Verify();
            _albumServiceMock.Verify();
        }

        [Test]
        public void When_GetAlbumsFromABand_Success()
        {
            // arrange
            var bandId = 1;
            var albumsReturned = new AlbumWithTitle[] { };
            
            _bandStorageMock.Setup(x => x.GetBandById(bandId)).Returns(new Band()).Verifiable();
            _albumStorageMock.Setup(x => x.GetAlbumsFromABand(bandId)).Returns(albumsReturned).Verifiable();

            // act
            var result = _albumController.GetAlbumsFromABand(bandId);

            // assert
            Assert.That(result, Is.Not.Null );
            var resultCast = result as OkObjectResult;
            var albumsCast = resultCast.Value as AlbumWithTitle[];
            Assert.That(albumsCast, Is.EqualTo(albumsReturned));
            _bandStorageMock.Verify();
            _albumServiceMock.Verify();
        }

        [Test]
        public void When_GetAlbumsFromABand_BandDoesntExist()
        {
            // arrange
            var bandId = 1;

            _bandStorageMock.Setup(x => x.GetBandById(bandId)).Returns(null as Band).Verifiable();

            // act
            var result = _albumController.GetAlbumsFromABand(bandId);

            // assert
            var resultCast = result as BadRequestObjectResult;
            Assert.That(resultCast.Value, Is.EqualTo("Band does not exist"));
        }

        public void When__GetAlbumDetails_Success()
        {
            // arrange


            // act


            // assert
        }
    }
}
