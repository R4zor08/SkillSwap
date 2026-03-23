using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SkillSwap.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Use the same deterministic per-user path as ApplicationDbContext
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SkillSwap");
            Directory.CreateDirectory(folder);
            var dbPath = Path.Combine(folder, "skillswap.db");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
