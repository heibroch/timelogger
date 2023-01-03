using Timelogger.Core.Interfaces;
using Timelogger.Core.Models.Persisted;

namespace Timelogger.Core.Events.Persistence
{
    public class FetchingUserStarted : IInternalMessage
    {
        public string Username { get; set; }

        public ManualResetEvent CompletionBlocker { get; set; } = new ManualResetEvent(false);

        //Populated later
        public string Password { get; set; }
        public PermissionEntity Permissions { get; set; }
    }
}
