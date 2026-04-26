namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Put
{
    public record PutSendToDto
    {
        public int Id { get; set; }
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
