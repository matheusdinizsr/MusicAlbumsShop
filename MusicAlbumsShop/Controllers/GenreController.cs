using Microsoft.AspNetCore.Mvc;
using MusicAlbumsShop.DTOs;
using MusicAlbumsShop.Models;
using MusicAlbumsShop.Services;
using MusicAlbumsShop.Storage;

namespace MusicAlbumsShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly ILogger<GenreController> _logger;
        private readonly IGenreService _genreService;
        private readonly IGenreStorage _genreStorage;
        public GenreController(ILogger<GenreController> logger, IGenreService genreService, IGenreStorage genreStorage)
        {
            _logger = logger;
            _genreService = genreService;
            _genreStorage = genreStorage;
        }

        [HttpGet]
        public GetGenresResponse Get()
        {
            _logger.LogInformation("Get controller called");
            return _genreService.GetGenres();
        }

        [HttpPost]
        public Genre AddOrGetGenre(string name)
        {
            var genre = _genreStorage.AddGenre(name);

            return genre;
        }
    }
}
