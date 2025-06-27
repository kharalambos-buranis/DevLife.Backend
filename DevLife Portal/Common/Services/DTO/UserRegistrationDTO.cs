namespace DevLife_Portal.Common.Services.DTO
{
    public class UserRegistrationDTO
    {
        public string Username { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public string TechStack { get; set; } = default!;
        public string ExperienceLevel { get; set; } = default!;
    }
}
