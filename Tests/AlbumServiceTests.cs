using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
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
    internal class AlbumServiceTests
    {
        private AlbumService _albumService;
        private MusicAlbumsContext context;
        private DbContextOptions<MusicAlbumsContext> _dbContextOptions;
        private Mock<IAlbumStorage> _albumStorageMock;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptions<MusicAlbumsContext>();
            context = new MusicAlbumsContext(_dbContextOptions);
            _albumStorageMock = new Mock<IAlbumStorage>(MockBehavior.Strict);
            _albumService = new AlbumService(_albumStorageMock.Object);
        }

        [Test]
        public void When_AddAlbum_Success()
        {
            // arrange

            var album = new Album() {Id = 1, Title = "", BandId = 1};
            _albumStorageMock.Setup(x => x.AddOrUpdateAlbum("", DateTime.Today, 1))
                .Returns(album)
                .Verifiable();

            // act
            var result = _albumService.AddOrUpdateAlbum("", DateTime.Today, 1);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo(""));
            Assert.That(result.BandId, Is.EqualTo(1));

        }

        [TearDown]
        public void TearDown()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
    }
}
