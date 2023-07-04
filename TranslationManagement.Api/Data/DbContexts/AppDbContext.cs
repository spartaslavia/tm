using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Core.Models;

namespace TranslationManagement.Api.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TranslationJob> TranslationJobs { get; set; }
        public DbSet<TranslatorModel> Translators { get; set; }
    }
}