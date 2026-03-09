namespace _4EPlatform.Application.Feature.SubCategoryFeature.Command.Put
{
    public record PutSubCategoryDto
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
