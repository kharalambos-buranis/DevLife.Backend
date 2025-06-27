namespace DevLife_Portal.Common.Models
{
    public class DailyChallenge
    {
        public int Id { get; set; }
        public string QuestionSlug { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
