

namespace _4EPlatform.Application.Feature.CongratsFeature.Command.Delete
{
    public class DeleteCongratsCommand : IRequest<ResponseDTO>
    {
        public Guid Id { get; set; }
    }
}
