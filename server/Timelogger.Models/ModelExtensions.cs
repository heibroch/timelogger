using Timelogger.Models.Persisted;
using Timelogger.Models.Public;

namespace Timelogger.Models
{
    public static class ModelExtensions
    {
        //Static conversions are considerably faster than automappers
        public static ProjectWorkLogEntryEntity ToEntity(this ProjectWorkLogEntry projectWorkLogEntry)
        {
            return new ProjectWorkLogEntryEntity
            {
                Id = projectWorkLogEntry.Id,
                ProjectId = projectWorkLogEntry.ProjectId,
                ProjectName = projectWorkLogEntry.ProjectName,
                WorkStarted = projectWorkLogEntry.WorkStarted,
                WorkStopped = projectWorkLogEntry.WorkStopped,
                ProjectCompleted = projectWorkLogEntry.ProjectCompleted,
            };
        }

        public static ProjectWorkLogEntry ToPublic(this ProjectWorkLogEntryEntity projectWorkLogEntryEntity)
        {
            return new ProjectWorkLogEntry
            {
                Id = projectWorkLogEntryEntity.Id,
                ProjectId = projectWorkLogEntryEntity.ProjectId,
                ProjectName = projectWorkLogEntryEntity.ProjectName,
                WorkStarted = projectWorkLogEntryEntity.WorkStarted,
                WorkStopped = projectWorkLogEntryEntity.WorkStopped,
                ProjectCompleted = projectWorkLogEntryEntity.ProjectCompleted,
            };
        }
    }
}
