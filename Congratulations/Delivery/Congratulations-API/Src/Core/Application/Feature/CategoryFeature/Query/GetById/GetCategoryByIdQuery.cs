namespace _4EPlatform.Application.Feature.CategoryFeature.Query.GetById
{
    public record GetCategoryByIdQuery(Guid Id) : IRequest<ResponseDTO>;

}
