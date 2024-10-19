namespace MusicAlbumsShop.DTOs
{
    public class GetGenresResponse
    {
        public string[] Genres { get; set; }
    }

    public class BandWithName
    {
        public int BandId { get; set; }
        public string Name { get; set; }
    }

    public class BandDetails
    {
        public string Name { get; set; }
        public string Origin { get; set; }
        public string YearsActive { get; set; }

        public string GenreName { get; set; }

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
