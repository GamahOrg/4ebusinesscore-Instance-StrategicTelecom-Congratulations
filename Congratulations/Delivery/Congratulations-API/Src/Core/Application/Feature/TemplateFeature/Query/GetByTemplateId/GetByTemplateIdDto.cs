namespace _4EPlatform.Application.Feature.TemplateFeature.Query.GetByTemplateId
{
    public class GetByTemplateIdDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Text { get; init; }

        public string Description { get; set; }
        public string FontType { get; set; }
        public string FontFamily { get; set; }
        public string FontColor { get; set; }
        public string ImgUrl { get; set; }
        public string ImgName { get; set; }
        public int FontSize { get; set; }
        public bool Job { get; set; }
        public Guid SubCategoryId { get; set; }
    }
}
