namespace Stellarium.Models
{
    public class PublicationCategory
    {
        public int Id { get; set; }
        public int PublicationId { get; set; }
        public int CategoryId { get; set; }

        public PublicationCategory(int id, int publicationId, int categoryId)
        {
            Id = id;
            PublicationId = publicationId;
            CategoryId = categoryId;
        }
    }
}
