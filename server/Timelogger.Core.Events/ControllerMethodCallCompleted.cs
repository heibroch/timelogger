using Timelogger.Core.Interfaces;

namespace Timelogger.Core.Events
{
    public record ControllerMethodCallCompleted(string MethodName) : IInternalMessage { }
}
