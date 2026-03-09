namespace _4EPlatform.Application.Feature.SubCategoryFeature.Command.Post
{
    public record PostSubCategoryDto
    {
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public string NameAr { get; init; }
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public string NameEn { get; init; }
        public string DescriptionEn { get; init; }
        public string DescriptionAr { get; init; }
        public Guid CategoryId { get; init; }
    }
}
