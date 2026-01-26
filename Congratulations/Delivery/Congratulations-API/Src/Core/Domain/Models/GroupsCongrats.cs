namespace _4EPlatform.Domain.Models
{
    public class GroupsCongrats : AuditEntity<Guid>
    {
        public Guid CongratsId { get; set; }
        public virtual Congrats Congrats { get; set; }
        public Guid GroupsId { get; set; }
        public virtual Groups Groups { get; set; }



    }
}
