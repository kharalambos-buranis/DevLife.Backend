namespace DevLife_Portal.Common.Models
{
    public class UserStreak
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CurrentStreak { get; set; }
        public DateTime LastCompletedDate { get; set; }
    }
}
