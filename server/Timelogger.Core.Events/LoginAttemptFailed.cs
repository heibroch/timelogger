using Timelogger.Core.Interfaces;

namespace Timelogger.Core.Events
{
    public record LoginAttemptFailed(string Username) : IInternalMessage
    {        
    }
}
