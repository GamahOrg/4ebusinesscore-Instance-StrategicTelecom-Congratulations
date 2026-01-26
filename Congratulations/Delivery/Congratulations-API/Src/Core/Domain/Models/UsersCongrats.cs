namespace _4EPlatform.Domain.Models
{
    public class UsersCongrats : AuditEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid CongratsId { get; set; }
        public virtual Congrats Congrats { get; set; }



    }
}
