using Timelogger.Models.Persisted;

namespace Timelogger.Interfaces
{
    //Added this project for the sake of speed to have a place for contracts between assemblies in support of the substitution principle.

    public interface ILogWorkRepository
    {
        void Add(ProjectWorkLogEntryEntity projectWorkLogEntryEntity);
        void Remove(ProjectWorkLogEntryEntity projectWorkLogEntryEntity);
        IEnumerable<ProjectWorkLogEntryEntity> Get();
    }
}
