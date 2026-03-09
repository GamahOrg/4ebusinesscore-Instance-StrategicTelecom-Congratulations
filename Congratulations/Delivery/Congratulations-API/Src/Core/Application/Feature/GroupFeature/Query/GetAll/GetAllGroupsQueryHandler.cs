namespace _4EPlatform.Application.Feature.GroupFeature.Query.GetAll
{
    public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        private ResponseDTO _responseDTO;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public GetAllGroupsQueryHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
            _responseDTO = new ResponseDTO();
        }

        public async Task<ResponseDTO> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
        {
            var list = await _unitOfWork.Repository<Groups>()
               .GetAllAsTracking(x => x.State != State.Deleted, request.PageIndex, request.PageSize, ref _responseDTO)
               .Select(x => new GetAllGroupsDto
               {
                   Id = x.Id,
                   NameAr = x.NameAr,
                   NameEn = x.NameEn,
                   Employess = x.EmployeesGroups.Where(x => x.State != State.Deleted).Select(x => x.UserId).ToList(),
                   CountOfEmployees = x.EmployeesGroups.Count(x => x.State != State.Deleted)
               })
               .ToListAsync(cancellationToken);
            if (!list.Any())
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            return _responseHelper.RetrievedSuccessfully(list, _stringLocalizer[SharedResourceKeys.RetrievedSuccessfully], ref _responseDTO);
        }
    }
}
