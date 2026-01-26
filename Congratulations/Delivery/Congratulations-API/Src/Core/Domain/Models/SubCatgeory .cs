namespace _4EPlatform.Domain.Models
{
    public class SubCatgeory : AuditEntity<Guid>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Template> Templates { get; set; } = new HashSet<Template>();

    }
}
