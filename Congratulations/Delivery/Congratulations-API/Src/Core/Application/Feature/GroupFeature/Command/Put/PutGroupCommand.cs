

namespace _4EPlatform.Application.Feature.GroupFeature.Command.Put
{
    public class PutGroupCommand : IRequest<ResponseDTO>
    {
        public required PutGroupDto putGroupDto { get; set; }
    }

}
