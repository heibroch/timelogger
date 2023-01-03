using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net;
using Timelogger.Core.Events;
using Timelogger.Core.Interfaces;
using Timelogger.Core.Models;
using Timelogger.Core.Models.Public;

namespace Timelogger.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ILogger<ProjectsController> _logger;
    private readonly ILogWorkRepository _logWorkRepository;
    private readonly IInternalMessageBus _internalMessageBus;

    public ProjectsController(ILogger<ProjectsController> logger, ILogWorkRepository logWorkRepository, IInternalMessageBus internalMessageBus)
    {
        _logger = logger;
        _logWorkRepository = logWorkRepository;
        _internalMessageBus = internalMessageBus;
    }

    [HttpGet("All")]
    [ProducesResponseType(typeof(IEnumerable<LogEntryDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<LogEntryDto>>> GetAll() //Does not have to be async and it will just execute synchronously, but operations within would likely be
    {
        var sessionCheckStarted = ValidateAccessToken();
        if (sessionCheckStarted == null)
            return Unauthorized();

        var username = sessionCheckStarted.Username;
        var permissions = sessionCheckStarted.Permissions;

        _internalMessageBus.Publish(new ControllerMethodCallStarted(nameof(GetAll)));

        //Todo: Authorization and only get those that the user has permission to
        
        var workLogEntries = _logWorkRepository.Get(); //Split entity objects and public contracts as customer-facing contracts normally are considerably more rigid.

        var result = workLogEntries.Select(x => x.ToPublic()); //Convert to public objects for consumption

        _internalMessageBus.Publish(new ControllerMethodCallCompleted(nameof(GetAll)));

        return Ok(result);
    }

    [HttpPost("Log")]
    [ProducesResponseType(typeof(LogEntryDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<LogEntryDto>> LogWork([FromBody] LogEntryDto loggedWork)
    {
        var sessionCheckStarted = ValidateAccessToken();
        if (sessionCheckStarted == null)
            return Unauthorized();

        var username = sessionCheckStarted.Username;
        var permissions = sessionCheckStarted.Permissions;

        //If the derived user is not permitted, then reject.. TODO: Implement permission scheme <-- Huge job doing properly, so skipping
        //if (permissions.FirstOrDefault(x => x.ProjectId == loggedWork.ProjectId) == null)
        //    return Unauthorized();

        _internalMessageBus.Publish(new ControllerMethodCallStarted(nameof(LogWork)));

        //Todo: Authorization
        //Todo: Validation

        if (loggedWork.WorkStopped < loggedWork.WorkStarted)
            return BadRequest("Cannot start work after work stopped!");

        _logWorkRepository.Add(loggedWork.ToEntity());

        _internalMessageBus.Publish(new ControllerMethodCallCompleted(nameof(GetAll)));

        return Ok(loggedWork);
    }

    /// <summary>
    /// Logs a user in with specified credentials.
    /// </summary>
    [HttpPost("Login")]
    [ProducesResponseType(typeof(LogEntryDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(UnauthorizedResult), (int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<string>> Login([FromBody] LoginAttemptDto loginAttempt)
    {
        _internalMessageBus.Publish(new ControllerMethodCallStarted(nameof(Login)));

        var loginAttemptStarted = new LoginAttemptStarted { Username = loginAttempt.Username, Password = loginAttempt.Password };
        _internalMessageBus.Publish(loginAttemptStarted);

        if (!loginAttemptStarted.CompletionBlocker.WaitOne(30000))
            return Unauthorized("Timed out"); //Todo: Change to timeout of some sorts in the future or be able to inform the requesting user

        if (!loginAttemptStarted.LoginSuccessful)
            return Unauthorized();

        //Set claims principal and/or store tokens according to architecture
        
        _internalMessageBus.Publish(new ControllerMethodCallCompleted(nameof(Login)));
                
        return loginAttemptStarted.Token;
    }

    [HttpPost("CreateProject")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<LogEntryDto>> CreateProject([FromBody] LogEntryDto loggedWork)
    {
        throw new NotImplementedException();
    }

    [HttpPut("MarkProjectCompleted")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<LogEntryDto>> MarkProjectCompleted(int project)
    {
        throw new NotImplementedException();
    }

    private SessionCheckStarted ValidateAccessToken()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].FirstOrDefault()?.Replace("Bearer ", string.Empty);
        if (accessToken == null)
            return null;

        //We can also use the authorization and use an attribute for an off-the-shelf solution
        var sessionCheckStarted = new SessionCheckStarted { SessionToken = accessToken };
        _internalMessageBus.Publish(sessionCheckStarted);
        if (!sessionCheckStarted.CompletionBlocker.WaitOne(30000))
            return null;

        return sessionCheckStarted;
    }

}
