

namespace _4EPlatform.Application.Feature.GroupFeature.Command.Delete
{
    public class DeleteGroupCommand : IRequest<ResponseDTO>
    {
        public Guid Id { get; set; }
    }
}
