namespace _4EPlatform.Domain.Models
{
    public class OrgsCongrats : AuditEntity<Guid>
    {
        public Guid OrgId { get; set; }
        public Guid CongratsId { get; set; }
        public virtual Congrats Congrats { get; set; }


    }
}
