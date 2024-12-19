using Microsoft.AspNetCore.Mvc;
using MusicAlbumsShop.Models;
using MusicAlbumsShop.Services;
using MusicAlbumsShop.Shared.DTOs;
using MusicAlbumsShop.Storage;

namespace MusicAlbumsShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BandController : ControllerBase
    {
        private readonly ILogger<BandController> _logger;
        private readonly IBandStorage _bandStorage;
        private readonly IBandService _bandService;
        private readonly IGenreService _genreService;

        public BandController(IBandStorage bandStorage, IBandService bandService, IGenreService genreService, ILogger<BandController> logger)
        {
            _bandStorage = bandStorage;
            _bandService = bandService;
            _genreService = genreService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddOrUpdateBand(int? bandId, string name, string origin, string yearsActive, int genreId)
        {
            if (_genreService.GetGenreById(genreId) == null)
            {
                return BadRequest("Genre ID does not exist.");
            }

            var band = _bandService.AddOrUpdateBand(bandId, name, origin, yearsActive, genreId);

            if (band == null)
            {
                return BadRequest("Band ID not found.");
            }

            var bandDto = new BandWithName() { Name = band.Name, BandId = band.Id };

            return Ok(bandDto);
        }

        [HttpGet("bands")]
        public BandWithName[] GetBands()
        {
            var array = _bandStorage.GetBands();
            return array;
        }

        [HttpGet("{id}/details")] // template
        public IActionResult GetBandDetails(int id)
        {
            var band = _bandStorage.GetBandById(id);

            if (band == null )
            {
                return NotFound("Id not found");
            }

            var genre = _genreService.GetGenreById(band.GenreId); 
                       if (genre == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var dto = new BandDetails()
            {
                Name = band.Name,
                Origin = band.Origin,
                YearsActive = band.YearsActive,
                GenreName = genre.Name,
                GenreId = genre.Id
            };

            return Ok(dto);
        }

        [HttpDelete]
        public IActionResult DeleteBandById(int id)
        {
            var band = _bandStorage.DeleteBandById(id);
            
            if (band == null)
            {
                return NotFound("Id not found.");
            }

            return Ok();

        }

    }
}
