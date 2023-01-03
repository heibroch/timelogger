using Timelogger.Core.Interfaces;
using Timelogger.Core.Models.Persisted;

namespace Timelogger.Infrastructure.Persistence
{
    public class LogWorkRepository : ILogWorkRepository
    {
        private readonly MySuperFakeApiContext _mySuperFakeApiContext;

        public LogWorkRepository(MySuperFakeApiContext mySuperFakeApiContext) => _mySuperFakeApiContext = mySuperFakeApiContext;

        public void Add(LogEntryEntity projectWorkLogEntryEntity)
        {
            //Todo: Validation. There should be existing entities checks etc.

            _mySuperFakeApiContext.LoggedWork.Add(projectWorkLogEntryEntity.Id, projectWorkLogEntryEntity);
        }

        public IEnumerable<LogEntryEntity> Get() => _mySuperFakeApiContext.LoggedWork.Values.ToList(); //Not thread safe when doing this, but seeing as there is only a single client that bases itself on clicks, I'm saving the work here

        public void Remove(LogEntryEntity projectWorkLogEntryEntity) => throw new NotImplementedException();
    }
}
