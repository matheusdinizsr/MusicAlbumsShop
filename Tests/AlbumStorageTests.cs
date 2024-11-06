using Microsoft.EntityFrameworkCore;
using MusicAlbumsShop.Models;
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
        private MusicAlbumsContext context;
        private MusicAlbumsContext _assertContext;
        private MusicAlbumsContext _arrangeContext;
        private DbContextOptions<MusicAlbumsContext> _dbContextOptions;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<MusicAlbumsContext>();
            builder.UseSqlServer("Data Source=Math;Initial Catalog=testmusicalbumsshop;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

            _dbContextOptions = builder.Options;
            context = new MusicAlbumsContext(_dbContextOptions);
            _storage = new AlbumStorage(context);
            _assertContext = new MusicAlbumsContext(_dbContextOptions);
            _arrangeContext = new MusicAlbumsContext(_dbContextOptions);
            context.Database.EnsureCreated();
        }

        [Test]
        public void When_AddAlbum_AlreadyExists()
        {
            // arrange
            var genre = new Genre() { Name = "" };
            _arrangeContext.Genres.Add(genre);
            var band = new Band() { Name = "", Origin = "", Genre = genre, YearsActive = ""};
            _arrangeContext.Bands.Add(band);
            _arrangeContext.Albums.Add(new Album() { Title = "", Band = band, ReleaseDate = DateTime.Today });
            _arrangeContext.SaveChanges();

            // act
            var result = _storage.AddOrUpdateAlbum("", DateTime.Today, 1);

            // assert
            Assert.That(_assertContext.Albums.Count(), Is.EqualTo(1));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo(""));
            var fetched = _assertContext.Albums.Find(1);
            Assert.That(fetched, Is.Not.Null);
            Assert.That(fetched.Id, Is.EqualTo(result.Id));
            Assert.That(fetched.Title, Is.EqualTo(result.Title));
            Assert.That(fetched.ReleaseDate, Is.EqualTo(result.ReleaseDate));

        }

        [Test]
        public void When_AddOrUpdateAlbum_Success()
        {
            // arrange
            var genre = new Genre() { Name = "" };
            _arrangeContext.Genres.Add(genre);
            var band = new Band() { Name = "", Origin = "", Genre = genre, YearsActive = "" };
            _arrangeContext.Bands.Add(band);
            _arrangeContext.SaveChanges();

            // act
            var album = _storage.AddOrUpdateAlbum("", DateTime.Today, 1);

            // assert
            Assert.That(album, Is.Not.Null);
            Assert.That(album.Id, Is.EqualTo(1));
            Assert.That(album.Title, Is.EqualTo(""));
            var fetched = _assertContext.Albums.Find(1);
            Assert.That(fetched, Is.Not.Null);
            Assert.That(fetched.Id, Is.EqualTo(album.Id));
            Assert.That(fetched.Title, Is.EqualTo(album.Title));
            Assert.That(fetched.ReleaseDate, Is.EqualTo(album.ReleaseDate));

        }


        [Test]
        public void When_GetAlbumById_Success()
        {
            // arrange
            var genre = new Genre() { Name = "" };
            _arrangeContext.Genres.Add(genre);
            var band = new Band() { Name = "", Origin = "", Genre = genre, YearsActive = "" };
            _arrangeContext.Bands.Add(band);
            _arrangeContext.Albums.Add(new Album() { Title = "", Band = band, ReleaseDate = DateTime.Today });
            _arrangeContext.SaveChanges();

            // act
            var result = _storage.GetAlbumById(1);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo(""));
        }

        [Test]
        public void When_GetAlbumsFromABand_Success()
        {
            // arrange
            var genre = new Genre() { Name = "" };
            _arrangeContext.Genres.Add(genre);
            var band = new Band() { Name = "", Origin = "", Genre = genre, YearsActive = "" };
            _arrangeContext.Bands.Add(band);
            _arrangeContext.Albums.Add(new Album() { Title = "", Band = band, ReleaseDate = DateTime.Today });
            _arrangeContext.SaveChanges();

            // act
            var albums = _storage.GetAlbumsFromABand(1);

            // assert
            Assert.That(albums.Count, Is.EqualTo(1));
            Assert.That(albums[0].AlbumId, Is.EqualTo(1));
            Assert.That(albums[0].BandName, Is.EqualTo(""));
            Assert.That(albums[0].Title, Is.EqualTo(""));
        }

        [TearDown]
        public void TearDown()
        {
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Dispose();
                _assertContext.Dispose();
                _arrangeContext.Dispose();
            }
        }
    }
}
