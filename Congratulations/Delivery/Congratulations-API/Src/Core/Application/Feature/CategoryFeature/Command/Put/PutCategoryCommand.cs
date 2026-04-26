namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Put
{
    public class PutCategoryCommand : IRequest<ResponseDTO>
    {


        public PutCategoryDto putCategoryDto { get; set; } 
    }
}
