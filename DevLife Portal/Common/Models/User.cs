namespace DevLife_Portal.Common.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string TechnoStack { get; set; }
        public string Experience { get; set; }
        public int ZodiacSignId { get; set; }
        public ZodiacSign ZodiacSign { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TotalPoints { get; set; }
        public int Streak { get; set; }
        public string? AvatarUrl { get; set; }
        public string FullName => $"{Name} {Lastname}";


    }
}
