using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Linq;

namespace StackOverflow.Data
{
    public class QuestionsDbContext : DbContext
    {
        private readonly string _connectionString;

        public QuestionsDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<QuestionsTags> QuestionsTags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<QuestionsTags>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

            modelBuilder.Entity<QuestionsTags>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionsTags)
                .HasForeignKey(qt => qt.QuestionId);

            modelBuilder.Entity<QuestionsTags>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuestionsTags)
                .HasForeignKey(qt => qt.TagId);


            modelBuilder.Entity<Likes>()
                .HasKey(qu => new { qu.QuestionId, qu.UserId });

            modelBuilder.Entity<Likes>()
                .HasOne(qu => qu.Question)
                .WithMany(q => q.Likes)
                .HasForeignKey(qu => qu.QuestionId);

            modelBuilder.Entity<Likes>()
                .HasOne(qu => qu.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(qu => qu.UserId);
        }
    }
}
