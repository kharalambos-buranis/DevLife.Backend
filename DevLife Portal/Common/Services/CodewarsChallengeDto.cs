namespace DevLife_Portal.Common.Services
{
    public class CodewarsChallengeDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string Description { get; set; } = default!;
        public RankInfo Rank { get; set; } = default!;
        public string Url => $"https://www.codewars.com/kata/{Slug}";

        public List<TestCase> TestCases { get; set; } = new();
    }

    public class TestCase
    {
        public string Input { get; set; } = default!;
        public string ExpectedOutput { get; set; } = default!;
    }

    public class RankInfo
    {
        public int Id { get; set; }     
        public string Name { get; set; } = default!; 
        public string Color { get; set; } = default!;
    }
}
