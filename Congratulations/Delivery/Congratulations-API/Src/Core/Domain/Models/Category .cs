namespace _4EPlatform.Domain.Models
{
    public class Category : AuditEntity<Guid>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public virtual ICollection<SubCatgeory> SubCatgeory { get; set; } = new HashSet<SubCatgeory>();

    }
}
