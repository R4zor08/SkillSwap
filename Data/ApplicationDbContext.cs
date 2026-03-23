using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using SkillSwap.Data.Entities;

namespace SkillSwap.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<TalentEntity> Talents { get; set; }
        public DbSet<TradeRequestEntity> TradeRequests { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentEntity>().HasKey(s => s.StudentId);
            modelBuilder.Entity<TalentEntity>().HasKey(t => t.TalentId);
            modelBuilder.Entity<TradeRequestEntity>().HasKey(r => r.TradeId);

            modelBuilder.Entity<StudentEntity>()
                .HasMany(s => s.Talents)
                .WithOne(t => t.Owner)
                .HasForeignKey(t => t.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Updated trade request relationships for the new structure
            modelBuilder.Entity<TradeRequestEntity>()
                .HasOne(r => r.Requester)
                .WithMany()
                .HasForeignKey(r => r.RequesterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TradeRequestEntity>()
                .HasOne(r => r.TargetStudent)
                .WithMany()
                .HasForeignKey(r => r.TargetStudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TradeRequestEntity>()
                .HasOne(r => r.RequestedTalent)
                .WithMany()
                .HasForeignKey(r => r.RequestedTalentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TradeRequestEntity>()
                .HasOne(r => r.OfferedTalent)
                .WithMany()
                .HasForeignKey(r => r.OfferedTalentId)
                .OnDelete(DeleteBehavior.Cascade);

            // map TradeRequest.Status as int and allow Rating
            modelBuilder.Entity<TradeRequestEntity>().Property(r => r.Status).HasConversion<int>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
