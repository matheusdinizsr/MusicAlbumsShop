using Microsoft.AspNetCore.Mvc;
using MusicAlbumsShop.DTOs;
using MusicAlbumsShop.Models;
using MusicAlbumsShop.Services;
using MusicAlbumsShop.Storage;
using System.Reflection.Metadata.Ecma335;

namespace MusicAlbumsShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly ILogger<AlbumController> _logger;
        private readonly IBandStorage _bandStorage;
        private readonly IAlbumService _albumService;
        private readonly IAlbumStorage _albumStorage;

        public AlbumController(IAlbumService albumService, ILogger<AlbumController> logger, IBandStorage bandStorage, IAlbumStorage albumStorage)
        {
            _logger = logger;
            _bandStorage = bandStorage;
            _albumService = albumService;
            _albumStorage = albumStorage;
        }

        [HttpPost]
        public IActionResult AddOrUpdateAlbum(string title, DateTime releaseDate, int bandId)
        {

            var album = _albumService.AddOrUpdateAlbum(title, releaseDate, bandId);

            if (album == null)
            {
                return BadRequest("Band does not exist");
            }

            var albumDto = new AlbumWithTitle() { AlbumId = album.Id, BandName = album.Band.Name, Title = title };

            return Ok(albumDto);
        }

        [HttpGet]
        public IActionResult GetAlbumsFromABand(int bandId)
        {
            var albums = _albumStorage.GetAlbumsFromABand(bandId);

            if (albums == null)
            {
                return NotFound("Band does not exist");
            }

            return Ok(albums);
        }


        [HttpGet("{id}/details")]
        public IActionResult GetAlbumDetails(int albumId)
        {
            var album = _albumStorage.GetAlbumById(albumId);

            if (album == null)
            {
                return NotFound("Album not found");
            }

            var albumDto = new AlbumDetails
            {
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                BandName = album.Band.Name
            };

            return Ok(albumDto);

        }
    }
}
