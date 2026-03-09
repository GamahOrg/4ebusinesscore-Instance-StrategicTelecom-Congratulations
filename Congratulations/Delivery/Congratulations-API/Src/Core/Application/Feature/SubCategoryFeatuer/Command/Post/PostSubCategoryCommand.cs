namespace _4EPlatform.Application.Feature.SubCategoryFeature.Command.Post
{
    public class PostSubCategoryCommand : IRequest<ResponseDTO>
    {
        public PostSubCategoryDto postSubCategoryDto { get; set; }
    }
}
