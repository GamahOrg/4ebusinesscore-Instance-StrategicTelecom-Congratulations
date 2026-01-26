namespace _4EPlatform.Domain.Models
{
    public class Groups : AuditEntity<Guid>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public virtual ICollection<EmployeesGroups> EmployeesGroups { get; set; } = new HashSet<EmployeesGroups>();

    }
}
