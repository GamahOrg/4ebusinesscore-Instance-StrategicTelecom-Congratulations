
namespace _4EPlatform.Domain.Models
{
    public class LK_SendTo : AuditEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public virtual ICollection<Congrats> GroupsCongrats { get; set; } = new HashSet<Congrats>();

    }
}
