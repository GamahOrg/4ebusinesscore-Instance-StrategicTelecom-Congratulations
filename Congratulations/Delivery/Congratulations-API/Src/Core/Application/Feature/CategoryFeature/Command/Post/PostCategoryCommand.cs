namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Post
{
    public class PostCategoryCommand : IRequest<ResponseDTO>
    {
        public required PostCategoryDto postCategoryDto { get; set; }
    }


    public record PostCategoryDto
    {
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public required string NameAr { get; init; }
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public required string NameEn { get; init; }
    }
}
