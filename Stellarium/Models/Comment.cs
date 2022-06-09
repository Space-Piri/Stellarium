namespace Stellarium.Models
{
    public class Comment : Entity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PublicationId { get; set; }
        public string Text { get; set; }
        public int? AnswerOnId { get; set; }
        public DateTime Date { get; set; }

        public Comment(int id, int userId, int publicationId, string text, int? answerOnId, DateTime date)
        {
            Id = id;
            UserId = userId;
            PublicationId = publicationId;
            Text = text;
            AnswerOnId = answerOnId;
            Date = date;
        }
    }
}
