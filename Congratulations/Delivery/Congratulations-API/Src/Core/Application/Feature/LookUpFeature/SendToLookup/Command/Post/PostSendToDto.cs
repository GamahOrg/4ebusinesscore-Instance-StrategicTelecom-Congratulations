namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Post
{
    public record PostSendToDto
    {
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public string NameAr { get; set; }
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public string NameEn { get; set; }
    }
}
