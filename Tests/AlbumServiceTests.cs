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
        private MusicAlbumsContext _context;
        private DbContextOptions<MusicAlbumsContext> _dbContextOptions;
        private Mock<IAlbumStorage> _albumStorageMock;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptions<MusicAlbumsContext>();
            _context = new MusicAlbumsContext(_dbContextOptions);
            _albumStorageMock = new Mock<IAlbumStorage>(MockBehavior.Strict);
            _albumService = new AlbumService(_albumStorageMock.Object);
        }

        [Test]
        public void When_AddAlbum_Success()
        {
            // arrange
            var title = "";
            var bandId = 1;
            var releaseDate = DateTime.Now;

            var album = new Album();
            _albumStorageMock.Setup(x => x.AddOrUpdateAlbum(title, releaseDate, bandId))
                .Returns(album)
                .Verifiable();

            // act
            var result = _albumService.AddOrUpdateAlbum(title, releaseDate, bandId);

            // assert
            Assert.That(result, Is.EqualTo(album));

        }

        [TearDown]
        public void TearDown()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
