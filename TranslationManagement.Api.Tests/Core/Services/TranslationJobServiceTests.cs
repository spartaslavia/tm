using System.Collections.Generic;
using System.Threading.Tasks;
using External.ThirdParty.Services;
using Microsoft.Extensions.Logging;
using Moq;
using TranslationManagement.Api.Core.Models;
using TranslationManagement.Api.Core.Services;
using TranslationManagement.Api.Data.Repositories.Interfaces;
using Xunit;

namespace TranslationManagement.Api.Tests
{
    public class TranslationJobServiceTests
    {
        private readonly Mock<IJobRepository> _mockJobRepository;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly Mock<ILogger<TranslationJobService>> _mockLogger;
        private readonly TranslationJobService _service;

        public TranslationJobServiceTests()
        {
            _mockJobRepository = new Mock<IJobRepository>();
            _mockNotificationService = new Mock<INotificationService>();
            _mockLogger = new Mock<ILogger<TranslationJobService>>();
            _service = new TranslationJobService(_mockJobRepository.Object, _mockNotificationService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetJobsAsync_ShouldReturnJobList_WhenJobsExist()
        {
            var expectedJobs = new List<TranslationJob>
            {
                new TranslationJob { Id = 1, CustomerName = "TestCustomer1" },
                new TranslationJob { Id = 2, CustomerName = "TestCustomer2" }
            };
            _mockJobRepository.Setup(repo => repo.GetJobsAsync()).ReturnsAsync(expectedJobs);

            var result = await _service.GetJobsAsync();

            Assert.NotNull(result);
            Assert.Equal(expectedJobs, result);
            _mockJobRepository.Verify(x => x.GetJobsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetJobAsync_ShouldReturnJob_WhenJobExists()
        {
            var expectedJob = new TranslationJob { Id = 1, CustomerName = "TestCustomer1" };
            _mockJobRepository.Setup(repo => repo.GetJobAsync(It.IsAny<int>())).ReturnsAsync(expectedJob);

            var result = await _service.GetJobAsync(1);

            Assert.NotNull(result);
            Assert.Equal(expectedJob, result);
            _mockJobRepository.Verify(x => x.GetJobAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task CreateJobAsync_ShouldCreateJob()
        {
            var job = new TranslationJob { OriginalContent = "Test Content", TranslatedContent = "", CustomerName = "Test Customer" };
            _mockJobRepository.Setup(x => x.AddJobAsync(job)).Returns(Task.CompletedTask);
            _mockJobRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _mockNotificationService.Setup(x => x.SendNotification(It.IsAny<string>())).ReturnsAsync(true);

            var result = await _service.CreateJobAsync(job);

            Assert.NotNull(result);
            Assert.Equal(JobStatus.New, result.Status);
            Assert.Equal(0.01 * result.OriginalContent.Length, result.Price);
            _mockJobRepository.Verify(x => x.AddJobAsync(job), Times.Once);
            _mockJobRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            _mockNotificationService.Verify(x => x.SendNotification(It.IsAny<string>()), Times.Once);
        }
    }
}