using System.Collections.Generic;
using System.Threading.Tasks;
using TranslationManagement.Api.Core.Models;

namespace TranslationManagement.Api.Data.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<IList<TranslationJob>> GetJobsAsync();
        Task<TranslationJob> GetJobAsync(int jobId);
        Task AddJobAsync(TranslationJob job);
        Task UpdateJobAsync(TranslationJob job);
        Task<int> SaveChangesAsync();
    }
}
