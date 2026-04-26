using Grpc.Core;

namespace _4EPlatform.Application.Services.Group
{
    public class GroupService : GroupProtoService.GroupProtoServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GroupService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async override Task<GetGrouByIdResponse> GetGrouById(GetGrouByIdRequest request, ServerCallContext context)
        {
            var entity = _mediator.Send(new GetGroupByIdQuery() { Id = Guid.Parse(request.Id) });
            var dtoMap = _mapper.Map<GroupProtoModel>(entity.Result.Result);
            var response = new GetGrouByIdResponse()
            {
                GroupProtoModel = dtoMap,
            };
            return await Task.FromResult(response);
        }
        public async override Task<GetAllGroupsResponse> GetAllGroups(GetAllGroupsRequest request, ServerCallContext context)
        {
            try
            {
                var list = _mediator.Send(new GetAllGroupsQuery() { PageSize = request.PageSize, PageIndex = request.PageIndex });
                var dto = _mapper.Map<List<GetAllGroupsProtoModel>>(list.Result.Result);
                var response = new GetAllGroupsResponse();
                response.GetAllGroupsProtoModel.AddRange(dto);
                response.PageIndex = list.Result.PageIndex;
                response.PageSize = list.Result.PageSize;
                response.TotalItems = list.Result.TotalItems;
                response.TotalPage = list.Result.TotalPages;
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
