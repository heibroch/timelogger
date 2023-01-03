using Timelogger.Core.Interfaces;

namespace Timelogger.Infrastructure.Persistence
{
    public class UserRepository
    {
        private readonly IInternalMessageBus _internalMessageBus;

        public UserRepository(IInternalMessageBus internalMessageBus) 
        {
            _internalMessageBus = internalMessageBus;
            
        }

        //todo stuff either asynchronously via messages if we want to easily split it into its own data store or inject it directly into the class. Depends on expected volumes. You wouldn't put a 500bhp engine in a lawnmower that only has to cut 5cm grass every other week.
    }
}
