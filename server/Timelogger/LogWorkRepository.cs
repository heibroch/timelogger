using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelogger.Interfaces;
using Timelogger.Models.Persisted;

namespace Timelogger
{
    public class LogWorkRepository : ILogWorkRepository
    {
        private readonly MySuperFakeApiContext _mySuperFakeApiContext;

        public LogWorkRepository(MySuperFakeApiContext mySuperFakeApiContext) => _mySuperFakeApiContext = mySuperFakeApiContext;

        public void Add(ProjectWorkLogEntryEntity projectWorkLogEntryEntity)
        {
            //Todo: Validation. There should be existing entities checks etc.

            _mySuperFakeApiContext.Entities.Add(projectWorkLogEntryEntity.Id, projectWorkLogEntryEntity);
        }

        public IEnumerable<ProjectWorkLogEntryEntity> Get() => _mySuperFakeApiContext.Entities.Values.ToList(); //Not thread safe when doing this, but seeing as there is only a single client that bases itself on clicks, I'm saving the work here

        public void Remove(ProjectWorkLogEntryEntity projectWorkLogEntryEntity) => throw new NotImplementedException();
    }
}
