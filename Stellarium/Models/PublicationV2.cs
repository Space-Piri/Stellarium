namespace Stellarium.Models
{
    public class PublicationV2 : Entity
    {
        public Publication Publication { get; set; }
        public User User { get; set; }
        public string Date { get; set; }
        public int Views { get; set; }
        public int Comments { get; set; }
        public string CommentsString { get; set; }
        public List<Category> Categories { get; set; }

        public PublicationV2(Publication publication, User user, int views, int comments, List<Category> categories)
        {
            Publication = publication;
            User = user;
            Views = views;
            Comments = comments;
            Date = OverDay(publication.DateTime);
            switch ((comments).ToString().Last())
            {
                case '0': case '5': case '6': case '7': case '8': case '9': CommentsString = "комментариев"; break;
                case '1': CommentsString = "комментарий"; break;
                case '2': case '3': case '4': CommentsString = "комментария"; break;
            }
            Categories = categories;
        }
    }
}
