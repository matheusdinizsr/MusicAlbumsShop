using MusicAlbumsShop.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal class AlbumStorageTests
    {
        private AlbumStorage _storage;
        private MusicAlbumsContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new BandsContext();
            _storage = new AlbumStorage(_context);
        }

        [Test]
        public void When_AddOrUpdateAlbum_Success()
        {
            // arrange
            var counter = _context.Albums.Count;
            var title = "";

            // act
            var album = _storage.AddOrUpdateAlbum(title, DateTime.Today, 1);

            // assert
            Assert.That(album, Is.Not.Null);
            Assert.That(album.Id, Is.EqualTo(counter + 1));
            Assert.That(album.Title, Is.EqualTo(title));
            var fetched = _context.Albums.LastOrDefault();
            Assert.That(album, Is.EqualTo(fetched));

        }

        [Test]
        public void When_AddAlbum_AlreadyExists()
        {
            // arrange
            var counter = _context.Albums.Count;

            // act
            var result = _storage.AddOrUpdateAlbum("Help!", DateTime.Now, 1);

            // assert
            Assert.That(result, Is.EqualTo(_context.Albums[0]));
        }

        [Test]
        public void When_GetAlbumById_Success()
        {
            // arrange
            // act
            var result = _storage.GetAlbumById(1);

            // assert
            Assert.That(result, Is.EqualTo(_context.Albums[0]));
        }

        [Test]
        public void When_GetAlbumsFromABand_Success()
        {
            // arrange
            // act
            var albums = _storage.GetAlbumsFromABand(1);

            // assert
            Assert.That(albums.Count, Is.EqualTo(1));
            Assert.That(albums[0].AlbumId, Is.EqualTo(1));
            Assert.That(albums[0].BandName, Is.EqualTo("The Beatles"));
            Assert.That(albums[0].Title, Is.EqualTo("Help!"));
        }
    }
}
