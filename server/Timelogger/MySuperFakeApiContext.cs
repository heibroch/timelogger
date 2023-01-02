using System.Collections.Generic;
using Timelogger.Models.Persisted;

namespace Timelogger
{
    public class MySuperFakeApiContext
    {
        public SortedList<int, ProjectWorkLogEntryEntity> Entities { get; set; } = new SortedList<int, ProjectWorkLogEntryEntity> { };
    }
}
