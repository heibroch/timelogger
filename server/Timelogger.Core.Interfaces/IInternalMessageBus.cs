namespace Timelogger.Core.Interfaces
{
    public interface IInternalMessageBus
    {
        void Subscribe<T>(Action<T> action) where T : IInternalMessage;

        void Unsubscribe<T>(Action<T> action) where T : IInternalMessage;

        void Publish<T>(T value) where T : IInternalMessage;
    }
}
