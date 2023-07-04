using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TranslationManagement.Api.Core.Models;

namespace TranslationManagement.Api.Core.Services.Interfaces
{
    public interface ITranslationJobService
    {
        Task<IList<TranslationJob>> GetJobsAsync();
        Task<TranslationJob> GetJobAsync(int jobId);
        Task<TranslationJob> CreateJobAsync(TranslationJob job);
        Task<TranslationJob> CreateJobWithFileAsync(IFormFile file, string customer);
        Task UpdateJobStatusAsync(int jobId, int translatorId, JobStatus newStatus);
    }
}