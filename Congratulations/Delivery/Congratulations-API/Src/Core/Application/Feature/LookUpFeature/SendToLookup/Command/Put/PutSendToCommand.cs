namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Put
{
    public class PutSendToCommand : IRequest<ResponseDTO>
    {
        public PutSendToDto putSendToDto { get; set; }
    }
}
