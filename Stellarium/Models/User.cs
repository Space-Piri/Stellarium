namespace Stellarium.Models
{
    public class User : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Icon { get; set; }
        public int Score { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastOnline { get; set; }
        public bool IsModerator { get; set; }

        public User(int id, string name, string email, string icon, int score, DateTime registrationDate, DateTime lastOnline, bool isModerator)
        {
            Id = id;
            Name = name;
            Email = email;
            Icon = icon;
            Score = score;
            RegistrationDate = registrationDate;
            LastOnline = lastOnline;
            IsModerator = isModerator;
        }
    }
}
