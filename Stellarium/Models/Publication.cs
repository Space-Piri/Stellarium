namespace Stellarium.Models
{
    public class Publication : Entity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Title { get; set; }
        public string TitleText { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
        public bool IsPublicated { get; set; }

        public Publication(int id, int userId, DateTime dateTime, string? title, string titleText, string image, string text, bool isPublicated)
        {
            Id = id;
            UserId = userId;
            DateTime = dateTime;
            Title = title;
            TitleText = titleText;
            Image = image;
            Text = text;
            IsPublicated = isPublicated;
        }
    }
}
