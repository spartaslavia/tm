using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Core.Models;
using TranslationManagement.Api.Core.Services.Interfaces;
using TranslationManagement.Api.Data.Repositories.Interfaces;

namespace TranslationManagement.Api.Core.Services
{
    public class TranslationJobService : ITranslationJobService
    {
        private const double PricePerCharacter = 0.01;
        private readonly IJobRepository _jobRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<TranslationJobService> _logger;

        public TranslationJobService(IJobRepository jobRepository, INotificationService notificationService, ILogger<TranslationJobService> logger)
        {
            _jobRepository = jobRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<IList<TranslationJob>> GetJobsAsync()
        {
            return await _jobRepository.GetJobsAsync();
        }

        public async Task<TranslationJob> GetJobAsync(int jobId)
        {
            return await _jobRepository.GetJobAsync(jobId);
        }

        public async Task<TranslationJob> CreateJobAsync(TranslationJob job)
        {
            job.Status = JobStatus.New;
            SetPrice(job);
            await _jobRepository.AddJobAsync(job);
            var isSaved = await _jobRepository.SaveChangesAsync() > 0;

            if (isSaved)
            {
                bool isNotificationSent = false;
                while (!isNotificationSent)
                {
                    try
                    {
                        isNotificationSent = await _notificationService.SendNotification("Job created: " + job.Id);
                    }
                    catch (ApplicationException)
                    {
                        _logger.LogError("Error while sending notification, retrying...");
                    }
                }

                _logger.LogInformation("New job notification sent.");
            }

            return job;
        }

        public async Task<TranslationJob> CreateJobWithFileAsync(IFormFile file, string customer)
        {
            string content;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                if (file.FileName.EndsWith(".txt"))
                {
                    content = await reader.ReadToEndAsync();
                }
                else if (file.FileName.EndsWith(".xml"))
                {
                    var xdoc = XDocument.Parse(await reader.ReadToEndAsync());
                    content = xdoc.Root.Element("Content").Value;
                    customer = xdoc.Root.Element("Customer").Value.Trim();
                }
                else
                {
                    throw new NotSupportedException("Unsupported file type.");
                }
            }

            var newJob = new TranslationJob()
            {
                OriginalContent = content,
                TranslatedContent = "",
                CustomerName = customer,
            };

            SetPrice(newJob);

            return await CreateJobAsync(newJob);
        }

        //public async Task<TranslationJob> CreateJobWithFileAsync(IFormFile file, string customer)
        //{
        //    var reader = new StreamReader(file.OpenReadStream());
        //    string content;

        //    if (file.FileName.EndsWith(".txt"))
        //    {
        //        content = reader.ReadToEnd();
        //    }
        //    else if (file.FileName.EndsWith(".xml"))
        //    {
        //        var xdoc = XDocument.Parse(reader.ReadToEnd());
        //        content = xdoc.Root.Element("Content").Value;
        //        customer = xdoc.Root.Element("Customer").Value.Trim();
        //    }
        //    else
        //    {
        //        throw new NotSupportedException("Unsupported file type.");
        //    }

        //    var newJob = new TranslationJob()
        //    {
        //        OriginalContent = content,
        //        TranslatedContent = "",
        //        CustomerName = customer,
        //    };

        //    SetPrice(newJob);

        //    return await CreateJobAsync(newJob);
        //}

        public async Task UpdateJobStatusAsync(int jobId, int translatorId, JobStatus newStatus)
        {
            if (!Enum.IsDefined(typeof(JobStatus), newStatus))
            {
                throw new ArgumentException("Invalid status.");
            }

            var job = await _jobRepository.GetJobAsync(jobId);
            if ((job.Status == JobStatus.New && newStatus == JobStatus.Completed)
                || job.Status == JobStatus.Completed
                || newStatus == JobStatus.New)
            {
                throw new ArgumentException("Invalid status change.");
            }

            job.Status = newStatus;
             await _jobRepository.SaveChangesAsync();
        }

        private void SetPrice(TranslationJob job)
        {
            job.Price = job.OriginalContent.Length * PricePerCharacter;
        }
    }
}
