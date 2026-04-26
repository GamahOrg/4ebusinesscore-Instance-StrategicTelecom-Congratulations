namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Delete
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<ResponseDTO>;

}
