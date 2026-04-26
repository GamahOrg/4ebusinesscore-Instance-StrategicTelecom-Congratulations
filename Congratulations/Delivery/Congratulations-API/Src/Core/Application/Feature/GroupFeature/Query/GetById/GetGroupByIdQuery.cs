

namespace _4EPlatform.Application.Feature.GroupFeature.Query.GetById
{
    public class GetGroupByIdQuery : IRequest<ResponseDTO>
    {
        public Guid Id { get; set; }
    }
}
