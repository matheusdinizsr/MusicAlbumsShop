using MusicAlbumsShop.Models;
using MusicAlbumsShop.Storage;

namespace MusicAlbumsShop.Services
{
    public interface IAlbumService
    {
        public Album? AddOrUpdateAlbum(string title, DateTime releaseDate, int bandId);
    }
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumStorage _albumStorage;

        public AlbumService(IAlbumStorage albumStorage)
        {
            _albumStorage = albumStorage;
        }

        public Album? AddOrUpdateAlbum(string title, DateTime releaseDate, int bandId)
        {
            var album = _albumStorage.AddOrUpdateAlbum(title, releaseDate, bandId);

            return album;
        }
    }
}
