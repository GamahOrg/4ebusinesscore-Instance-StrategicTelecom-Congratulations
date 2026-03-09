

namespace _4EPlatform.Application.Feature.GroupFeature.Command.Post
{
    public class PostGroupCommandHandler : IRequestHandler<PostGroupCommand, ResponseDTO>
    {
        private readonly IResponseHelper _responseHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public PostGroupCommandHandler(IStringLocalizer<SharedResource> stringLocalizer, IMapper mapper, IResponseHelper responseHelper, IUnitOfWork unitOfWork)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _responseHelper = responseHelper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseDTO> Handle(PostGroupCommand request, CancellationToken cancellationToken)
        {
            var validator = new PostGroupValidator(_unitOfWork, _stringLocalizer);
            var validation = validator.Validate(request);

            if (!validation.IsValid)
                return _responseHelper.FailedToSave(string.Join(" ", validation.Errors.Select(n => n.ErrorMessage).ToList()));

            var entity = _mapper.Map<Groups>(request.postGroupDto);

            if (request.postGroupDto.EmployeesGroupsIds.Any())
            {
                foreach (var item in request.postGroupDto.EmployeesGroupsIds)
                {
                    var employee = new EmployeesGroups()
                    {
                        UserId = item,
                        GroupsId = entity.Id,
                    };
                    entity.EmployeesGroups.Add(employee);
                }
            }

            await _unitOfWork.Repository<Groups>().AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(entity.Id, _stringLocalizer[SharedResourceKeys.AddSuccessfully]);
        }
    }
}
