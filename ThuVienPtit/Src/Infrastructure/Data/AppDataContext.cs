using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThuVienPtit.Src.Domain.Entities;
using ThuVienPtit.Src.Infrastructure.Users.Respository;

namespace ThuVienPtit.Src.Infrastructure.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }
        public DbSet<users> users { get; set; }
        public DbSet<refresh_tokens> refresh_tokens { get; set; }
        public DbSet<password_reset_tokens> password_reset_tokens { get; set; }
        public DbSet<semesters> semesters { get; set; }
        public DbSet<courses> courses { get; set; }
        public DbSet<course_categories> course_categories { get; set; }
        public DbSet<documents> documents { get; set; }
        public DbSet<tags> tags { get; set; }
        public DbSet<document_tags> document_tags { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Ghi log truy vấn + thời gian thực thi ra Output Window
            optionsBuilder
                .LogTo(
                    message => Debug.WriteLine(message), // hoặc Console.WriteLine
                    new[] { DbLoggerCategory.Database.Command.Name },
                    LogLevel.Information
                )
                .EnableSensitiveDataLogging() // Hiển thị giá trị thực tế của parameter
                .EnableDetailedErrors();      // Hiển thị chi tiết lỗi SQL nếu có
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // entities cho users
            modelBuilder.Entity<users>()
                .HasKey(U => U.user_id);
            modelBuilder.Entity<users>()
                .HasMany(u => u.password_reset_tokens)
                .WithOne(pt => pt.user)
                .HasForeignKey(pt => pt.user_id);
            modelBuilder.Entity<users>()
                .HasMany(u=>u.refresh_tokens)
                .WithOne(r => r.user)
                .HasForeignKey(r=>r.user_id);
            modelBuilder.Entity<users>()
                .HasMany(u => u.documents)
                .WithOne(d => d.user)
                .HasForeignKey(d => d.user_id);
            // builder cho refresh_tokens
            modelBuilder.Entity<refresh_tokens>()
                .HasKey(rt => rt.token_id);
            modelBuilder.Entity<refresh_tokens>()
                .HasOne(rt => rt.user)
                .WithMany(u => u.refresh_tokens)
                .HasForeignKey(rt => rt.user_id);
            // buider cho password_reset_tokens
            modelBuilder.Entity<password_reset_tokens>()
                .HasKey(rt => rt.token_id);
            modelBuilder.Entity<password_reset_tokens>()
                .HasOne(pt=>pt.user)
                .WithMany(u=>u.password_reset_tokens)
                .HasForeignKey(pt=>pt.user_id);
            //builder cho semesters
            modelBuilder.Entity<semesters>()
                .HasKey(s => s.semester_id);
            modelBuilder.Entity<semesters>()
                .HasMany(s => s.courses)
                .WithOne(c => c.semester)
                .HasForeignKey(c => c.semester_id);
            // builder cho courses
            modelBuilder.Entity<courses>()
                .HasKey(c => c.course_id);
            modelBuilder.Entity<courses>()
                .HasOne(c => c.semester)
                .WithMany(s => s.courses)
                .HasForeignKey(c => c.semester_id);
            modelBuilder.Entity<courses>()
                .HasOne(c=>c.category)
                .WithMany(cat => cat.courses)
                .HasForeignKey(c => c.category_id);
            modelBuilder.Entity<courses>()
                .HasMany(c => c.documents)
                .WithOne(d => d.course)
                .HasForeignKey(d => d.course_id);
            // builder cho course_categories
            modelBuilder.Entity<course_categories>()
                .HasKey(cat => cat.category_id);
            modelBuilder.Entity<course_categories>()
                .HasMany(cat => cat.courses)
                .WithOne(c => c.category)
                .HasForeignKey(c => c.category_id);
            // builder cho documents
            modelBuilder.Entity<documents>()
                .HasKey(d => d.document_id);
            modelBuilder.Entity<documents>()
                .HasOne(d => d.course)
                .WithMany(c => c.documents)
                .HasForeignKey(d => d.course_id);
            modelBuilder.Entity<documents>()
                .HasOne(d => d.user)
                .WithMany(u => u.documents)
                .HasForeignKey(d => d.user_id);
            modelBuilder.Entity<documents>()
                .HasMany(d => d.document_tags)
                .WithOne(dt => dt.Document)
                .HasForeignKey(dt => dt.document_id);
            // builder cho tags
            modelBuilder.Entity<tags>()
                .HasKey(t => t.tag_id);
            modelBuilder.Entity<tags>()
                .HasMany(t => t.document_Tags)
                .WithOne(dt => dt.Tag)
                .HasForeignKey(dt => dt.tag_id);
            //builder cho document_tags
            modelBuilder.Entity<document_tags>()
                .HasKey(dt => new { dt.document_id, dt.tag_id });
            modelBuilder.Entity<document_tags>()
                .HasOne(dt => dt.Document)
                .WithMany(d => d.document_tags)
                .HasForeignKey(dt => dt.document_id)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<document_tags>()
                .HasOne(dt => dt.Tag)
                .WithMany(t => t.document_Tags)
                .HasForeignKey(dt => dt.tag_id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
