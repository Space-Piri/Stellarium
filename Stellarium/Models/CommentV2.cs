namespace Stellarium.Models
{
    public class CommentV2 : Entity
    {
        public Comment Comment { get; set; }
        public User User { get; set; }
        public string Date { get; set; }
        public Comment? AnsverAt { get; set; }

        public CommentV2(Comment comment, User user, DateTime date, Comment? ansverAt)
        {
            Comment = comment;
            User = user;
            Date = OverDay(comment.Date);
            AnsverAt = ansverAt;
        }
    }
}
