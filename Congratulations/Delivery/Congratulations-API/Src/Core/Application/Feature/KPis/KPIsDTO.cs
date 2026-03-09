namespace _4EPlatform.Application.Feature.KPis
{
    public class KPIsDTO
    {
        public int CountCongrats { get; set; }
        public int UsedTemplate { get; set; }

        public List<TemplateWithCongrat> templateWithCongrat { get; set; }

        public int CongratForGroups { get; set; }
        public int CongratForUsers { get; set; }
        public int CongratForOrgs { get; set; }
        public int CongratTypes { get; set; }
    }

    public class TemplateWithCongrat
    {
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int CongratCount { get; set; }
    }
}
