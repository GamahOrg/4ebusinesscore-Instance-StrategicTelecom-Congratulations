

namespace _4EPlatform.Application.Feature.CongratsFeature.Command.Post
{
    public class PostCongratsCommand : IRequest<ResponseDTO>
    {
        public required PostCongratsDto postCongratsDto { get; set; }
    }
}
