using DevLife_Portal.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace DevLife_Portal.Infrastructure.Database.PostgreSQL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ZodiacSign> Zodiacs { get; set; }
        public DbSet<DailyChallenge> DailyChallenges { get; set; }
        public DbSet<UserDailyChallenge> UserDailyChallenges { get; set; }
        public DbSet<UserStreak> UserStreaks { get; set; }
        public DbSet<BugChaseScore> BugChaseScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.Id).HasName("pk_users");

                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Name).HasColumnName("name");
                entity.Property(u => u.Lastname).HasColumnName("lastname");
                entity.Property(u => u.Username).HasColumnName("user_name");
                entity.Property(u => u.DateOfBirth).HasColumnName("date_of_birth");
                entity.Property(u => u.TechnoStack).HasColumnName("techno_stack");
                entity.Property(u => u.Experience).HasColumnName("experience");
                entity.Property(u => u.CreatedAt).HasColumnName("created_at");
                entity.Property(u => u.TechnoStack).HasColumnName("techno_stack");
                entity.Property(u => u.TotalPoints).HasColumnName("total_points");
                entity.Property(u => u.Streak).HasColumnName("streak");
                entity.Property(u => u.AvatarUrl).HasColumnName("avatar_url");
                entity.Property(u => u.ZodiacSignId).HasColumnName("zodiac_sign_id");

                entity.HasOne(u => u.ZodiacSign)
                      .WithMany(z => z.Users)
                      .HasForeignKey(u => u.ZodiacSignId)
                      .HasConstraintName("fk_users_zodiac_signs");

                entity.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("ux_users_usernames");
            });

            modelBuilder.Entity<ZodiacSign>(entity =>
            {
                entity.ToTable("zodiac_signs");

                entity.HasKey(s => s.Id).HasName("pk_zodiac_signs");

                entity.Property(s => s.Id).HasColumnName("id");
                entity.Property(s => s.Name).HasColumnName("name");
                entity.Property(s => s.Emoji).HasColumnName("emoji");
                entity.Property(s => s.StartMonth).HasColumnName("start_month");
                entity.Property(s => s.StartDay).HasColumnName("start_day");
                entity.Property(s => s.EndDay).HasColumnName("end_day");
                entity.Property(s => s.StartDay).HasColumnName("start_day");
                entity.Property(s => s.DailyTip).HasColumnName("daily_tip");
                entity.Property(s => s.LuckyTechnology).HasColumnName("lucky_technology");

                modelBuilder.Entity<ZodiacSign>().HasData(
                new ZodiacSign { Id = 1, Name = "Capricorn", Emoji = "♑" },
                new ZodiacSign { Id = 2, Name = "Aquarius", Emoji = "♒" },
                new ZodiacSign { Id = 3, Name = "Pisces", Emoji = "♓" },
                new ZodiacSign { Id = 4, Name = "Aries", Emoji = "♈" },
                new ZodiacSign { Id = 5, Name = "Taurus", Emoji = "♉" },
                new ZodiacSign { Id = 6, Name = "Gemini", Emoji = "♊" },
                new ZodiacSign { Id = 7, Name = "Cancer", Emoji = "♋" },
                new ZodiacSign { Id = 8, Name = "Leo", Emoji = "♌" },
                new ZodiacSign { Id = 9, Name = "Virgo", Emoji = "♍" },
                new ZodiacSign { Id = 10, Name = "Libra", Emoji = "♎" },
                new ZodiacSign { Id = 11, Name = "Scorpio", Emoji = "♏" },
                new ZodiacSign { Id = 12, Name = "Sagittarius", Emoji = "♐" });
            });

            modelBuilder.Entity<DailyChallenge>(entity =>
            {
                entity.ToTable("daily_challenges");

                entity.HasKey(e => e.Id).HasName("pk_daily_challenges");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.QuestionSlug).HasColumnName("question_slug");
                entity.Property(e => e.Date).HasColumnName("date");
            });

            modelBuilder.Entity<UserDailyChallenge>(entity =>
            {
                entity.ToTable("user_daily_challenges");

                entity.HasKey(e => e.Id).HasName("pk_user_daily_challenges");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.IsCorrect).HasColumnName("is_correct");

                entity.HasOne<User>() 
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("fk_user_daily_challenges_users");
            });

            modelBuilder.Entity<UserStreak>(entity =>
            {
                entity.ToTable("user_streaks");

                entity.HasKey(e => e.Id).HasName("pk_user_streaks");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.CurrentStreak).HasColumnName("current_streak");
                entity.Property(e => e.LastCompletedDate).HasColumnName("last_completed_date");

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("fk_user_streaks_users");
            });

            modelBuilder.Entity<BugChaseScore>(entity =>
            {
                entity.ToTable("bugchase_scores");

                entity.HasKey(e => e.Id).HasName("pk_bugchase_scores");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Score).HasColumnName("score");
                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .HasConstraintName("fk_bugchase_scores_users");
            });
        }



    }
}
