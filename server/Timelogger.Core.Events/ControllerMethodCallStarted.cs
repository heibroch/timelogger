using Timelogger.Core.Interfaces;

namespace Timelogger.Core.Events
{
    public record ControllerMethodCallStarted(string MethodName) : IInternalMessage { }
}
