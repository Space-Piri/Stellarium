namespace Stellarium.Models
{
    public class View : Entity
    {
        public int Id { get; set; }
        public int VisitorId { get; set; }
        public int PublicationId { get; set; }

        public View(int id, int visitorId, int publicationId)
        {
            Id = id;
            VisitorId = visitorId;
            PublicationId = publicationId;
        }
    }
}
