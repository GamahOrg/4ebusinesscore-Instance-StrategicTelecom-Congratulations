namespace _4EPlatform.Application.Feature.SubCategoryFeature.Command.Delete
{
    public class DeleteSubCategoryCommand : IRequest<ResponseDTO>
    {
        public Guid Id { get; set; }
    }
}
