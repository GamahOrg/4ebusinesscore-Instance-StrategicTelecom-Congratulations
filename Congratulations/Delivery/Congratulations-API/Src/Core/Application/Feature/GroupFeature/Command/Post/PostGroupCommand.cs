

namespace _4EPlatform.Application.Feature.GroupFeature.Command.Post
{
    public class PostGroupCommand : IRequest<ResponseDTO>
    {
        public required PostGroupDto postGroupDto { get; set; }
    }
}
