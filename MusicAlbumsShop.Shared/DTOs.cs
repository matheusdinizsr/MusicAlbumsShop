using System.ComponentModel.DataAnnotations;

namespace MusicAlbumsShop.Shared.DTOs
{
    public class GetGenresResponse
    {
        public GenreWithTitle[] GenresAndIds { get; set; }
    }

    public class BandWithNameAndGenre
    {
        public int BandId { get; set; }
        public string Name { get; set; }
        public string GenreName { get; set; }

    }

    public class BandDetails
    {
        [Required]
        public int BandId { get; set;}
        [Required]
        public string Name { get; set; }
        [Required]
        public string Origin { get; set; }
        [Required]
        public string YearsActive { get; set; }
        public string GenreName { get; set; }
        public int GenreId { get; set; }

    }

    public class AlbumWithTitle
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string BandName { get; set; }
    }
    public class AlbumDetails
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string BandName { get; set; }
    }

    public class GenreWithTitle
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
    }
}
