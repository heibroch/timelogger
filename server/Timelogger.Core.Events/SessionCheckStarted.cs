using Timelogger.Core.Interfaces;
using Timelogger.Core.Models.Persisted;

namespace Timelogger.Core.Events
{
    public class SessionCheckStarted : IInternalMessage
    {
        public string SessionToken { get; set; }
        public ManualResetEvent CompletionBlocker { get; set; } = new ManualResetEvent(false);

        //Populated later
        public string Username { get; set; }
        public List<PermissionEntity> Permissions { get; set; } = new List<PermissionEntity>();
        public bool IsTokenAuthenticationSuccessful { get; set; }
    }
}
