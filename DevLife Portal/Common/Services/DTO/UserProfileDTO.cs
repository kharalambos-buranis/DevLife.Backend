namespace DevLife_Portal.Common.Services.DTO
{
    public class UserProfileDTO
    {
        public string Username { get; set; } = default!;
        public string FullName {  get; set; } = default!;
        public string ZodiacName { get; set; } = default!;
        public string ZodiacEmoji { get; set; } = default!;
        public string TechStack { get; set; } = default!;
        public string ExperienceLevel { get; set; } = default!;
        public int TotalPoints { get; set; }
        public int Streak { get; set; }
    }
}
