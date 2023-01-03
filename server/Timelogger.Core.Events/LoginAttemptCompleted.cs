using Timelogger.Core.Interfaces;

namespace Timelogger.Core.Events
{
    public record LoginAttemptCompleted(string Username) : IInternalMessage
    {
    }
}
