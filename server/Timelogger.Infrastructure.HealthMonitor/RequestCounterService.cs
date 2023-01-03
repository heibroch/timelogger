using System.Collections.Concurrent;
using System.Timers;
using Timelogger.Core.Events;
using Timelogger.Core.Interfaces;

namespace Timelogger.Infrastructure.HealthMonitor
{
    /// <summary>
    /// Service that receives request counts from controllers. It processes asynhronously in order to minimize effect on processing speed in the controllers
    /// 
    /// Having this allows for adding advanced logic on when to alert and what mediums to alert on. By having it as a separate "logical service" it makes service split, of what is currently a monolith, much easier.
    /// </summary>
    public class RequestCounterService : IDisposable
    {
        private readonly IInternalMessageBus _internalMessageBus;

        private readonly BlockingCollection<ControllerMethodCallStarted> _startedCalls;
        private readonly BlockingCollection<ControllerMethodCallCompleted> _completedCalls;
        private readonly Thread _startedCallsThread;
        private readonly Thread _completedCallsThread;
        private ulong _startedCallCount;
        private ulong _completedCallCount;
        private ulong _currentPendingLoginCallCount;
        private bool _isDisposed;

        public RequestCounterService(IInternalMessageBus internalMessageBus) 
        {
            //Start threads before subscribing or risk a null ref
            _startedCalls = new BlockingCollection<ControllerMethodCallStarted>();
            _completedCalls = new BlockingCollection<ControllerMethodCallCompleted>();

            _startedCallsThread = new Thread(ProcessStartedCalls);
            _completedCallsThread = new Thread(ProcessCompletedCalls);

            _startedCallsThread.Start();
            _completedCallsThread.Start();

            _internalMessageBus = internalMessageBus;
            _internalMessageBus.Subscribe<ControllerMethodCallStarted>(OnControllerMethodCallStarted);
            _internalMessageBus.Subscribe<ControllerMethodCallCompleted>(OnControllerMethodCallCompleted);

            var timer = new System.Timers.Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += IntervalTimer;
            timer.Start();
        }

        private void IntervalTimer(object? sender, ElapsedEventArgs e)
        {
            //E.g. check if disparity between started calls and completed calls becomes to large and inform through relevant mediums if necessary
            var startedCallCount = Interlocked.Read(ref _startedCallCount);
            var completedCallCount = Interlocked.Read(ref _completedCallCount);

            var uncompletedCalls = startedCallCount - completedCallCount;
            if (uncompletedCalls > 100) //Todo: Put the 100 in a configuration file
                Console.WriteLine($"{uncompletedCalls} calls have not currently run to complettion!");

            if (_currentPendingLoginCallCount > 1000)
                Console.WriteLine($"{_currentPendingLoginCallCount} logins did not complete. We might be experiencing a brute force attack!"); //Todo: Add logging of the source to the event so we can see where those attempts are coming from so we can block them on the fly in the future.
                    
            //Todo: Other monitoring mechanisms
        }

        private void ProcessCompletedCalls(object? obj)
        {
            while(!_isDisposed)
            {
                if (!_startedCalls.TryTake(out var controllerMethodCallStarted, TimeSpan.FromSeconds(2)))
                    continue; //This allows for closing down gracefully as the as "Take" would block indefinitely. We could also use a cancellation token.

                Interlocked.Increment(ref _startedCallCount);

                if (controllerMethodCallStarted.MethodName == "Login")
                    Interlocked.Increment(ref _currentPendingLoginCallCount);
            }            
        }

        private void ProcessStartedCalls(object? obj)
        {
            while (!_isDisposed)
            {
                if (!_startedCalls.TryTake(out var controllerMethodCallStarted, TimeSpan.FromSeconds(2)))
                    continue; //This allows for closing down gracefully as the as "Take" would block indefinitely. We could also use a cancellation token.

                Interlocked.Increment(ref _completedCallCount);

                if (controllerMethodCallStarted.MethodName == "Login")
                    Interlocked.Decrement(ref _currentPendingLoginCallCount);
            }
        }

        private void OnControllerMethodCallCompleted(ControllerMethodCallCompleted obj) => _completedCalls.Add(obj);

        private void OnControllerMethodCallStarted(ControllerMethodCallStarted obj) => _startedCalls.Add(obj);

        public void Dispose() => _isDisposed = true;
    }
}
