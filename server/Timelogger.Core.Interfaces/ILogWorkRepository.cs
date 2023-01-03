using Timelogger.Core.Models.Persisted;

namespace Timelogger.Core.Interfaces
{
    //Added this project for the sake of speed to have a place for contracts between assemblies in support of the substitution principle.

    public interface ILogWorkRepository
    {
        void Add(LogEntryEntity projectWorkLogEntryEntity);
        void Remove(LogEntryEntity projectWorkLogEntryEntity);
        IEnumerable<LogEntryEntity> Get();
    }
}
