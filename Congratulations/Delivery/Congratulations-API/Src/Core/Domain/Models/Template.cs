
namespace _4EPlatform.Domain.Models
{
    public class Template : AuditEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Text { get; set; }

        public string FontType { get; set; }
        public string FontFamily { get; set; }
        public string FontColor { get; set; }
        public string? ImageName { get; set; }
        public string? ImageUrl { get; set; }
        public int FontSize { get; set; }
        public bool Job { get; set; }
        public Guid SubCatgeoryId { get; set; }
        public virtual SubCatgeory SubCatgeory { get; set; }
        public virtual ICollection<TemplateUserException> Exceptions { get; set; } = new HashSet<TemplateUserException>();

    }
}
