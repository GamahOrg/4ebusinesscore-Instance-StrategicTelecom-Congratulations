namespace _4EPlatform.Application.Feature.CategoryFeature.Query.GetById
{
    public record GetSendToByIdQuery(int Id) : IRequest<ResponseDTO>;

}
