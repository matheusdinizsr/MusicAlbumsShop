namespace MusicAlbumsShop.Models
{
    public class EntityBase
    {
        public int Id { get; set; }
    }
    public class Band : EntityBase
    {
        public string Name { get; set; }
        public string Origin { get; set; }
        public string YearsActive { get; set; }
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }

    public class Genre : EntityBase
    {
        public string Name { get; set; }
        public virtual IList<Band> Bands { get; set; }
    }

    public class Album : EntityBase
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int BandId { get; set; }

        public virtual Band Band { get; set; }

    }
}
