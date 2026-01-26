namespace _4EPlatform.Domain.Models
{
    public class EmployeesGroups : AuditEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid GroupsId { get; set; }
        public virtual Groups Groups { get; set; }


    }
}
