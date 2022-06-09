namespace Stellarium.Models
{
    public class BookMark
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PublicationId { get; set; }

        public BookMark(int id, int userId, int publicationId)
        {
            Id = id;
            UserId = userId;
            PublicationId = publicationId;
        }
    }
}
