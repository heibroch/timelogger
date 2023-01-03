namespace Timelogger.Core.Interfaces
{
    public interface IInternalLogger
    {
        void LogInfo(string message);

        void LogWarning(string message);

        void LogError(string message);

        Action<string> LogInfoAction { get; set; }

        Action<string> LogWarningAction { get; set; }

        Action<string> LogErrorAction { get; set; }
    }
}
