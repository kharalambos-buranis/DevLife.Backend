namespace DevLife_Portal.Common.Services
{
    public class ExcuseCategories
    {
        public static readonly List<string> Categories = new()
    {
        "Daily Standup",
        "Sprint Planning",
        "Client Meeting",
        "Team Building"
    };
    }

    public static class ExcuseBank
    {
        public static readonly Dictionary<string, List<string>> Excuses = new()
        {
            ["Daily Standup"] = new()
        {
            "Sorry, my cat just walked on the router.",
            "Updating dependencies... in life."
        },
            ["Sprint Planning"] = new()
        {
            "Still recovering from last sprint.",
            "Backlog gave me anxiety."
        },
            ["Client Meeting"] = new()
        {
            "Client timezone confusion.",
            "Laptop decided to update now."
        },
            ["Team Building"] = new()
        {
            "Stuck in a solo-building session.",
            "Building team spirit asynchronously."
        }
        };
    }
}
