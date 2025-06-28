using DevLife_Portal.Common.Enums;
using System.Linq.Expressions;
using System.Text.Json;

namespace DevLife_Portal.Common.Services
{
    public class CodeWarsChallengeService : ICodeWarsChallengeService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private static readonly Dictionary<Experience, string[]> DifficultyMap = new()
        {
            [Experience.Junior] = ["8", "7"],
            [Experience.Middle] = ["6", "5"],
            [Experience.Senior] = ["4", "3"]
        };
        private static readonly Dictionary<string, List<string>> KataIdsByDifficulty = new()
        {
            ["8"] = [
          "5715eaedb436cf5606000381", // Sum of positive
          "554b4ac871d6813a03000035", // Squared sum
          "55a70521798b14d4750000a4"  // Returning Strings
    ],
            ["7"] = [
          "56606694ec01347ce800001b", // Even or Odd
          "5861d28f124b35723e00005e", // Logic Drills
          "563e320cee5dddcf77000158"  // MakeUpperCase
    ],
            ["6"] = [
          "546f922b54af40e1e90001da", // Replace With Alphabet Position
          "576bb3c4b1abc497ec000065", // Count characters
          "517abf86da9663f1d2000003"  // Convert string to camel case
    ],
            ["5"] = [
          "5592e3bd57b64d00f3000047", // Convert to camel case
          "5629db57620258aa9d000014", // Find the missing letter
          "55c6126177c9441a570000cc"  // Weight for weight
    ],
            ["4"] = [
          "52c31f8e6605bcc646000082", // Regex validate PIN code
          "52597aa56021e91c93000cb0", // Moving Zeros To The End
          "525f50e3b73515a6db000b83"  // Create Phone Number
    ],
            ["3"] = [
          "52742f58faf5485cae000b9a", // Binary multiple of 3
          "541c8630095125aba6000c00", // Count IP addresses
          "5282b48bb70058e4c4000fa7"  // Human readable duration format
    ]
        };

        public static readonly Dictionary<string, string> CorrectAnswers = new()
        {
            ["Sum of positive"] =
         "public static int PositiveSum(int[] numbers) { return numbers.Where(n => n > 0).Sum(); }",

            ["Squared sum"] =
         "public static int SquareSum(int[] numbers) { return numbers.Sum(n => n * n); }",

            ["Returning Strings"] =
         "public static string Greet(string name) { return $\"Hello, {name} how are you doing today?\"; }",

            ["Even or Odd"] =
         "public static string EvenOrOdd(int number) { return number % 2 == 0 ? \"Even\" : \"Odd\"; }",

            ["Thinkful - Logic Drills"] =
         "public static bool AreEqual(int a, int b) { return a == b; }",

            ["MakeUpperCase"] =
         "public static string MakeUpperCase(string str) { return str.ToUpper(); }",

            ["Replace With Alphabet Position"] =
         "public static string AlphabetPosition(string text) { return string.Join(\" \", text.ToLower().Where(char.IsLetter).Select(c => c - 'a' + 1)); }",

            ["Count characters"] =
         "public static Dictionary<char, int> Count(string str) { return str.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count()); }",

            ["Convert string to camel case"] =
         "public static string ToCamelCase(string str) { if (string.IsNullOrEmpty(str)) return str; var words = str.Split(new[] { '-', '_' }); return words[0] + string.Concat(words.Skip(1).Select(w => char.ToUpper(w[0]) + w.Substring(1))); }",

            ["Convert to camel case"] =
         "public static string ToCamelCase(string str) { if (string.IsNullOrEmpty(str)) return str; var words = str.Split(new[] { '-', '_' }); return words[0] + string.Concat(words.Skip(1).Select(w => char.ToUpper(w[0]) + w.Substring(1))); }",

            ["Find the missing letter"] =
         "public static char FindMissingLetter(char[] array) { for (int i = 0; i < array.Length - 1; i++) { if (array[i + 1] != array[i] + 1) return (char)(array[i] + 1); } return ' '; }",

            ["Weight for weight"] =
         "public static string OrderWeight(string strng) { return string.Join(\" \", strng.Split(' ').OrderBy(s => s.Select(c => c - '0').Sum()).ThenBy(s => s)); }",

            ["Regex validate PIN code"] =
         "public static bool ValidatePin(string pin) { return Regex.IsMatch(pin, \"^\\d{4}$|^\\d{6}$\"); }",

            ["Moving Zeros To The End"] =
         "public static int[] MoveZeroes(int[] arr) { return arr.Where(n => n != 0).Concat(arr.Where(n => n == 0)).ToArray(); }",

            ["Create Phone Number"] =
         "public static string CreatePhoneNumber(int[] numbers) { return $\"({numbers[0]}{numbers[1]}{numbers[2]}) {numbers[3]}{numbers[4]}{numbers[5]}-{numbers[6]}{numbers[7]}{numbers[8]}{numbers[9]}\"; }",

            ["Binary multiple of 3"] =
         "public static bool IsMultipleOfThree(string binary) { if (string.IsNullOrEmpty(binary) || binary.Any(c => c != '0' && c != '1')) return false; int odd = 0, even = 0; for (int i = 0; i < binary.Length; i++) { if (binary[binary.Length - 1 - i] == '1') { if (i % 2 == 0) odd++; else even++; } } return (odd - even) % 3 == 0; }",

            ["Count IP addresses"] =
         "public static long IpsBetween(string start, string end) { var s = start.Split('.').Select(long.Parse).ToArray(); var e = end.Split('.').Select(long.Parse).ToArray(); return (e[0] - s[0]) * 256 * 256 * 256 + (e[1] - s[1]) * 256 * 256 + (e[2] - s[2]) * 256 + (e[3] - s[3]); }",

            ["Human readable duration format"] =
         "public static string FormatDuration(int seconds) { if (seconds == 0) return \"now\"; var times = new[] { (3600 * 24 * 365, \"year\"), (3600 * 24, \"day\"), (3600, \"hour\"), (60, \"minute\"), (1, \"second\") }; var parts = new List<string>(); foreach (var (unit, name) in times) { var val = seconds / unit; if (val > 0) { parts.Add($\"{val} {name}{(val > 1 ? \"s\" : \"\")}\"); seconds %= unit; } } return parts.Count == 1 ? parts[0] : string.Join(\", \", parts.Take(parts.Count - 1)) + \" and \" + parts.Last(); }"
        };

        public CodeWarsChallengeService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<CodewarsChallengeDto?> GetRandomChallengeAsync(string language, Experience difficulty, CancellationToken ct)
        {
            if (!DifficultyMap.TryGetValue(difficulty, out var codewarsDifficulties))
            {
                throw new ArgumentException("Invalid difficulty level");

            }

            var random = new Random();
            var selectedLevel = codewarsDifficulties[random.Next(codewarsDifficulties.Length)];

            if (!KataIdsByDifficulty.TryGetValue(selectedLevel, out var kataIds))
            {
                return null;

            }

            var challengeId = kataIds[random.Next(kataIds.Count)];

            var apiKey = _config["Codewars:ApiKey"];
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", apiKey);

            var response = await _httpClient.GetAsync($"https://www.codewars.com/api/v1/code-challenges/{challengeId}", ct);

            if (!response.IsSuccessStatusCode)
            {
                return null;

            }

            var json = await response.Content.ReadAsStringAsync(ct);
            var data = JsonSerializer.Deserialize<CodewarsChallengeDto>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return data;
        }
    }
}
