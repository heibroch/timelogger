using Microsoft.EntityFrameworkCore;
using Timelogger.Models.Persisted;

namespace Timelogger
{
	public class ApiContext : DbContext //There was a problem with this, so on account of time, I just used a singleton for data-handling and hid operations behind interfaces as it should be anyway
	{
		public ApiContext(DbContextOptions<ApiContext> options) : base(options)
		{
		}

		public DbSet<ProjectWorkLogEntryEntity> WorkLogEntires { get; set; }
	}
}
