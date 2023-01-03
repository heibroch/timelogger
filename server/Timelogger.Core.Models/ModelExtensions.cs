using Timelogger.Core.Models.Persisted;
using Timelogger.Core.Models.Public;

namespace Timelogger.Core.Models
{
    public static class ModelExtensions
    {
        //Static conversions are considerably faster than automappers
        public static LogEntryEntity ToEntity(this LogEntryDto projectWorkLogEntry)
        {
            return new LogEntryEntity
            {
                Id = projectWorkLogEntry.Id,
                ProjectId = projectWorkLogEntry.ProjectId,
                ProjectName = projectWorkLogEntry.ProjectName,
                WorkStarted = projectWorkLogEntry.WorkStarted,
                WorkStopped = projectWorkLogEntry.WorkStopped,
                ProjectCompleted = projectWorkLogEntry.ProjectCompleted,
            };
        }

        public static LogEntryDto ToPublic(this LogEntryEntity projectWorkLogEntryEntity)
        {
            return new LogEntryDto
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
