using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Core.Models;
using TranslationManagement.Api.Data.DbContexts;
using TranslationManagement.Api.Data.Repositories.Interfaces;

namespace TranslationManagement.Api.Data.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;

        public JobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<TranslationJob>> GetJobsAsync()
        {
            return await _context.TranslationJobs.ToListAsync();
        }

        public async Task<TranslationJob> GetJobAsync(int id)
        {
            return await _context.TranslationJobs.FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task AddJobAsync(TranslationJob job)
        {
            await _context.TranslationJobs.AddAsync(job);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateJobAsync(TranslationJob job)
        {
            _context.TranslationJobs.Update(job);
            await _context.SaveChangesAsync();
        }
    }
}
