namespace _4EPlatform.Application.Feature.SubCategoryFeature.Command.Put
{
    public class PutSubCategoryCommand : IRequest<ResponseDTO>
    {
        public Guid Id { get; set; }
        public PutSubCategoryDto putSubCategoryDto { get; set; }
    }
}
