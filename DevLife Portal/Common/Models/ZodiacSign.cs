using System.Text.Json.Serialization;

namespace DevLife_Portal.Common.Models
{
    public class ZodiacSign
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Emoji { get; set; } = default!;
        [JsonIgnore]
        public int StartMonth { get; set; }
        [JsonIgnore]
        public int StartDay { get; set; }
        [JsonIgnore]
        public int EndMonth { get; set; }
        [JsonIgnore]
        public int EndDay { get; set; }
        [JsonIgnore]
        public string? DailyTip { get; set; }
        [JsonIgnore]
        public string? LuckyTechnology { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; }
    }
}
