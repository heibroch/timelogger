using Timelogger.Core.Models.Persisted;

namespace Timelogger
{
    public class MySuperFakeApiContext //With dummy-data so we don't have to waste time on adding it
    {
        public SortedList<int, LogEntryEntity> LoggedWork { get; set; } = new SortedList<int, LogEntryEntity> { };
        public SortedList<int, ProjectEntity> Projects { get; set; } = new SortedList<int, ProjectEntity> 
        {

        };
        public SortedList<int, UserEntity> Users { get; set; } = new SortedList<int, UserEntity>() 
        {
            {0, new UserEntity{ Username = "dwayne", Password = "password" } },
            {1, new UserEntity{ Username = "casper", Password = "password" } },
        };
    }
}
