namespace _4EPlatform.Application.Feature.CategoryFeature.Query.GetAll
{
    public record GetAllCategoryQuery(bool? MyCongrats, string? Search, int PageSize, int PageNumber , Guid? CategoryId) : IRequest<ResponseDTO>;

}
