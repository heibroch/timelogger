using Timelogger.Core.Interfaces;

namespace Timelogger.Infrastructure.Logging
{
    public class InternalLogger : IInternalLogger
    {
        public Action<string> LogInfoAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<string> LogWarningAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<string> LogErrorAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void LogError(string message) => Console.WriteLine(message);
        public void LogInfo(string message) => Console.WriteLine(message);
        public void LogWarning(string message) => Console.WriteLine(message);
    }
}
