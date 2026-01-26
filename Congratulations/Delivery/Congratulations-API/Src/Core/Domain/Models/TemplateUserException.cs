namespace _4EPlatform.Domain.Models
{
    public class TemplateUserException : AuditEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string? NewUserName { get; set; }
        public string? NewJob { get; set; }
        public bool Job { get; set; }
        public Guid TemplateId { get; set; }
        public virtual Template Template { get; set; }


    }
}
