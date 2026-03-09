namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Delete
{
    public record DeleteSendToCommand(int Id) : IRequest<ResponseDTO>;

}
