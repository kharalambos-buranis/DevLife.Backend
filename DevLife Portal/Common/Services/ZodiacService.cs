using DevLife_Portal.Common.Models;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace DevLife_Portal.Common.Services
{
    public class ZodiacService
    {
        private readonly AppDbContext _context;

        public ZodiacService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ZodiacSign?> GetZodiacSignAsync(DateTime dateOfBirth, CancellationToken cancellationToken)
        {
            var (month, day) = (dateOfBirth.Month, dateOfBirth.Day);

            string? signName = (month, day) switch
            {
                (1, <= 19) or (12, >= 22) => "Capricorn",
                (1, >= 20) or (2, <= 18) => "Aquarius",
                (2, >= 19) or (3, <= 20) => "Pisces",
                (3, >= 21) or (4, <= 19) => "Aries",
                (4, >= 20) or (5, <= 20) => "Taurus",
                (5, >= 21) or (6, <= 20) => "Gemini",
                (6, >= 21) or (7, <= 22) => "Cancer",
                (7, >= 23) or (8, <= 22) => "Leo",
                (8, >= 23) or (9, <= 22) => "Virgo",
                (9, >= 23) or (10, <= 22) => "Libra",
                (10, >= 23) or (11, <= 21) => "Scorpio",
                (11, >= 22) or (12, <= 21) => "Sagittarius",
                _ => null
            };

            return signName is null
                ? null
                : await _context.Zodiacs.FirstOrDefaultAsync(z => z.Name == signName, cancellationToken);
        }

    }
}
