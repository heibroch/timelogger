using Timelogger.Core.Interfaces;

namespace Timelogger.Core.Events
{
    public class LoginAttemptStarted : IInternalMessage
    {
        public ManualResetEvent CompletionBlocker { get; set; } = new ManualResetEvent(false);
        public string Username { get; set; }
        public string Password { get; set; }

        //Populated later
        public bool LoginSuccessful { get; set; }
        public string Token { get; set; }
    }
}
