using System.Collections.Concurrent;
using Timelogger.Core.Events;
using Timelogger.Core.Interfaces;

namespace Timelogger.Infrastructure.LoginManager
{
    /// <summary>
    /// Login service that makes an otherwise synhronous operation linked to a totally asycnhronous processing mechanism
    /// </summary>
    public class LoginService : IDisposable
    {
        private readonly IInternalMessageBus _internalMessageBus;

        private readonly BlockingCollection<LoginAttemptStarted> _loginAttempts;
        private readonly BlockingCollection<SessionCheckStarted> _sessionChecks;
        private readonly Thread _loginAttemptThread;
        private readonly Thread _sessionCheckThread;
        private bool _isDisposed;
        private SortedList<string, (string username, DateTime tokenCreatedAt)> _sessions;

        private ConcurrentDictionary<string, string> _logins;

        public LoginService(IInternalMessageBus internalMessageBus)
        {
            _sessions = new SortedList<string, (string username, DateTime tokenCreatedAt)>();

            //Add some logins
            _logins = new ConcurrentDictionary<string, string>();
            _logins.TryAdd("dwayne", "password");
            _logins.TryAdd("casper", "password");

            _loginAttempts = new BlockingCollection<LoginAttemptStarted>();
            _loginAttemptThread = new Thread(ProcessLoginAttempts);
            _loginAttemptThread.Start();

            _sessionChecks = new BlockingCollection<SessionCheckStarted>();
            _sessionCheckThread = new Thread(ProcessSessionChecks);
            _sessionCheckThread.Start();

            _internalMessageBus = internalMessageBus;
            _internalMessageBus.Subscribe<LoginAttemptStarted>(OnLoginAttemptStarted);
            _internalMessageBus.Subscribe<SessionCheckStarted>(OnSessionCheckStarted);
        }

        private void OnLoginAttemptStarted(LoginAttemptStarted obj) => _loginAttempts.Add(obj);
        private void OnSessionCheckStarted(SessionCheckStarted obj) => _sessionChecks.Add(obj);

        private void ProcessLoginAttempts(object? obj)
        {
            while (!_isDisposed)
            {
                if (!_loginAttempts.TryTake(out var loginAttemptStarted, TimeSpan.FromSeconds(2)))
                    continue; //This allows for closing down gracefully as the as "Take" would block indefinitely. We could also use a cancellation token.

                //No such user present
                if (!_logins.TryGetValue(loginAttemptStarted.Username, out var password))
                {
                    loginAttemptStarted.LoginSuccessful = false;
                    loginAttemptStarted.CompletionBlocker.Set();
                    _internalMessageBus.Publish(new LoginAttemptFailed(loginAttemptStarted.Username));
                    continue;
                }

                //Password mismatch
                if (password != loginAttemptStarted.Password)
                {
                    loginAttemptStarted.LoginSuccessful = false;
                    loginAttemptStarted.CompletionBlocker.Set();
                    _internalMessageBus.Publish(new LoginAttemptFailed(loginAttemptStarted.Username));
                    continue;
                }

                //Success
                var sessionToken = "SuperSecretTokenCreatedFor" + loginAttemptStarted.Username;
                _sessions.Add(sessionToken, (loginAttemptStarted.Username, DateTime.UtcNow));
                loginAttemptStarted.Token = sessionToken;
                loginAttemptStarted.LoginSuccessful = true;                
                loginAttemptStarted.CompletionBlocker.Set();                
                _internalMessageBus.Publish(new LoginAttemptCompleted(loginAttemptStarted.Username));
            }
        }
        private void ProcessSessionChecks(object? obj)
        {
            while (!_isDisposed)
            {
                if (!_sessionChecks.TryTake(out var sessionCheckStarted, TimeSpan.FromSeconds(2)))
                    continue; //This allows for closing down gracefully as the as "Take" would block indefinitely. We could also use a cancellation token.

                //No such token present
                if (!_sessions.TryGetValue(sessionCheckStarted.SessionToken, out var userAndTokenCreatedAt))
                {
                    sessionCheckStarted.IsTokenAuthenticationSuccessful = false;
                    sessionCheckStarted.CompletionBlocker.Set();
                    continue;
                }

                //Token timeout
                if (DateTime.UtcNow - userAndTokenCreatedAt.tokenCreatedAt > TimeSpan.FromSeconds(600))
                {
                    sessionCheckStarted.IsTokenAuthenticationSuccessful = false;
                    _sessions.Remove(sessionCheckStarted.SessionToken);
                    sessionCheckStarted.CompletionBlocker.Set();
                    continue;
                }

                //Success
                sessionCheckStarted.Username = userAndTokenCreatedAt.username;
                sessionCheckStarted.IsTokenAuthenticationSuccessful = true;
                sessionCheckStarted.CompletionBlocker.Set();
            }
        }

        public void Dispose() => _isDisposed = true;
    }
}
