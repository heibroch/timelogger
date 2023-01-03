using Moq;
using Timelogger.Api.Controllers;
using Timelogger.Core.Interfaces;
using Timelogger.Core.Models.Persisted;
using Timelogger.Core.Models.Public;
using Timelogger.TestHelpers;
using Xunit;

namespace Timelogger.ApiTests.Controllers
{
    public class ProjectsControllerTests
    {
        [Fact]
        public async Task GivenAWorkStoppedThatComesBeforeWorkStarted_OnLogWork_ThenPersistenceNeverHappens()
        {
            var testTargetBuilder = new TestTargetBuilder<ProjectsController>(); //Mocks are dynamically created, so no need for boiler plate
                        
            var target = testTargetBuilder.Build();

            //Act
            _ = await target.LogWork(new LogEntryDto { WorkStopped = DateTime.Now.Subtract(TimeSpan.FromHours(1)), WorkStarted = DateTime.Now });

            //Assert
            testTargetBuilder.ResolveMock<ILogWorkRepository>().Verify(x => x.Add(It.IsAny<LogEntryEntity>()), Times.Never);
        }
    }
}
