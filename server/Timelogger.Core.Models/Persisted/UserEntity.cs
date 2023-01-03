namespace Timelogger.Core.Models.Persisted
{
    public class UserEntity
    {
        public string Username { get; set; }
        public string Password { get; set; } //This would normally be a one-way hashed value
    }
}
