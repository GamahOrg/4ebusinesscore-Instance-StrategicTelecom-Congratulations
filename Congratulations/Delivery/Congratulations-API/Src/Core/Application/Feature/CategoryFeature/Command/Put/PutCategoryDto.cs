namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Put
{
    public record PutCategoryDto
    {
        public Guid Id { get; set; }
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public required string NameAr { get; set; }
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public required string NameEn { get; set; }
    }
}
