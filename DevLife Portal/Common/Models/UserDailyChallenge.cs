namespace DevLife_Portal.Common.Models
{
    public class UserDailyChallenge
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCorrect { get; set; }
    }
}
