using System.Text.Json.Serialization;

namespace DevLife_Portal.Common.Models
{
    public class ZodiacSign
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Emoji { get; set; } = default!;
        public int StartMonth { get; set; }
        public int StartDay { get; set; }
        public int EndMonth { get; set; }
        public int EndDay { get; set; }
        public string? DailyTip { get; set; }
        public string? LuckyTechnology { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; }
    }
}
