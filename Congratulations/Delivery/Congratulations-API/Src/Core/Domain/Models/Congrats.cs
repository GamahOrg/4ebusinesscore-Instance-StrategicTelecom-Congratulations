namespace _4EPlatform.Domain.Models
{
    public class Congrats : AuditEntity<Guid>
    {
        public Guid TemplateId { get; set; }
        public virtual Template Template { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsStopped { get; set; }
        public int LK_SendToId { get; set; }
        public virtual LK_SendTo LK_SendTo { get; set; }
        public virtual ICollection<GroupsCongrats> GroupsCongrats { get; set; } = new HashSet<GroupsCongrats>();
        public virtual ICollection<UsersCongrats> UsersCongrats { get; set; } = new HashSet<UsersCongrats>();
        public virtual ICollection<OrgsCongrats> OrgsCongrats { get; set; } = new HashSet<OrgsCongrats>();
    }
}
