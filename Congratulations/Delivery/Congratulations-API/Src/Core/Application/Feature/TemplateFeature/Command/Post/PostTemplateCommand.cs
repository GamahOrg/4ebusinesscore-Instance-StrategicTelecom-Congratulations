using _4EPlatform.Application.CustomValidation;

namespace _4EPlatform.Application.Feature.TemplateFeature.Command.Post
{
    public class PostTemplateCommand : IRequest<ResponseDTO>
    {
        public PostTemplateDto postTemplateDto { get; set; }
    }

    public record PostTemplateDto
    {
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public string Name { get; init; }

        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public string? Text { get; init; }


        public string Description { get; init; }
        public string FontType { get; init; }
        public string FontFamily { get; init; }
        public string FontColor { get; init; }
        [PreventInjectionScriptFileAttribute]
        public IFormFile? ImgUrl { get; init; }
        public int FontSize { get; init; }
        public bool Job { get; init; }
        public Guid SubCategoryId { get; init; }
    }
}
