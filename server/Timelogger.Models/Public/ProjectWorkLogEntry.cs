namespace Timelogger.Models.Public
{
    public class ProjectWorkLogEntry
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool ProjectCompleted { get; set; }
        public DateTime WorkStarted { get; set; }
        public DateTime WorkStopped { get; set; }
    }
}
