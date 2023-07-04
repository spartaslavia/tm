using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranslationManagement.Api.Core.Models;
using TranslationManagement.Api.Core.Models.Dto;
using TranslationManagement.Api.Core.Services.Interfaces;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranslationJobController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITranslationJobService _jobService;

        public TranslationJobController(IMapper mapper, ITranslationJobService jobService)
        {
            _mapper = mapper;
            _jobService = jobService;
        }



        [HttpGet("{id}")]
        public async Task<TranslationJobDto> GetJob(int id)
        {
            var job = await _jobService.GetJobsAsync();
            return _mapper.Map<TranslationJobDto>(job);
        }
        
        [HttpGet]
        public async Task<IList<TranslationJobDto>> GetJobs()
        {
            var jobs = await _jobService.GetJobsAsync();
            return _mapper.Map<IList<TranslationJobDto>>(jobs);
        }

        [HttpPost("create-job")]
        public async Task<IActionResult> CreateJob(TranslationJob job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _jobService.CreateJobAsync(job);
            return CreatedAtAction(nameof(GetJob), new {id = job.Id}, job);
        }

        [HttpPost("create-job-file")]
        public async Task<IActionResult> CreateJobWithFile(IFormFile file, string customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var job = await _jobService.CreateJobWithFileAsync(file, customer);
            return CreatedAtAction(nameof(GetJob), new { id = job.Id }, job);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateJobStatus(int jobId, int translatorId, JobStatus newStatus)
        {
            await _jobService.UpdateJobStatusAsync(jobId, translatorId, newStatus);
            return NoContent();
        }
    }
}