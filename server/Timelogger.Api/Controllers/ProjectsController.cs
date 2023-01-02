using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Timelogger.Interfaces;
using Timelogger.Models;
using Timelogger.Models.Public;

namespace Timelogger.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ILogger<ProjectsController> _logger;
    private readonly ILogWorkRepository _logWorkRepository;

    public ProjectsController(ILogger<ProjectsController> logger, ILogWorkRepository logWorkRepository)
    {
        _logger = logger;
        _logWorkRepository = logWorkRepository;
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<ProjectWorkLogEntry>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IEnumerable<ProjectWorkLogEntry>> GetAll() //Does not have to be async and it will just execute synchronously, but operations within would likely be
    {
        //Todo: Authorization
        
        var workLogEntreies = _logWorkRepository.Get(); //Split entity objects and public contracts as customer-facing contracts normally are considerably more rigid.

        return workLogEntreies.Select(x => x.ToPublic()); //Convert to public objects for consumption
    }

    [HttpPost("log")]
    [ProducesResponseType(typeof(ProjectWorkLogEntry), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ProjectWorkLogEntry> LogWork([FromBody] ProjectWorkLogEntry loggedWork)
    {
        //Todo: Authorization
        //Todo: Validation
        if (loggedWork.WorkStopped < loggedWork.WorkStarted)
            BadRequest("Cannot start work after work stopped!");

        _logWorkRepository.Add(loggedWork.ToEntity());
        
        return loggedWork;
    }
}
