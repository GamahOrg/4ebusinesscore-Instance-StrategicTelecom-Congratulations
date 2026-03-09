
namespace _4EPlatform.Application.Feature.GroupFeature.Query.GetAll
{
    public class GetAllGroupsQuery : IRequest<ResponseDTO>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
