namespace _4EPlatform.Application.Feature.GroupFeature.Command.Put
{
    public class PutGroupCommandHandler : IRequestHandler<PutGroupCommand, ResponseDTO>
    {
        private readonly IResponseHelper _responseHelper;
        private readonly IUnitOfWork _unitOfWork;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;


        public PutGroupCommandHandler(IStringLocalizer<SharedResource> stringLocalizer, IResponseHelper responseHelper, IUnitOfWork unitOfWork)
        {
            _stringLocalizer = stringLocalizer;
            _responseHelper = responseHelper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> Handle(PutGroupCommand request, CancellationToken cancellationToken)
        {
            var validator = new PutGroupValidator(_unitOfWork, _stringLocalizer);
            var validation = validator.Validate(request);

            if (!validation.IsValid)
                return _responseHelper.FailedToSave(string.Join(" ", validation.Errors.Select(n => n.ErrorMessage).ToList()));

            var entity = _unitOfWork.Repository<Groups>().GetAllAsTracking(x => x.State != State.Deleted && x.Id == request.putGroupDto.Id).FirstOrDefault();
            if (entity == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            entity.NameAr = request.putGroupDto.NameAr;
            entity.NameEn = request.putGroupDto.NameEn;


            if (request.putGroupDto.EmployeesGroupsIds.Any())
            {
                var list = _unitOfWork.Repository<EmployeesGroups>().GetAllAsNoTracking(x => x.GroupsId == request.putGroupDto.Id).ToList();
                list.ForEach(x => x.State = State.Deleted);
                await _unitOfWork.Repository<EmployeesGroups>().UpdateRangeAsync(list);
                foreach (var item in request.putGroupDto.EmployeesGroupsIds)
                {
                    var employee = new EmployeesGroups()
                    {
                        UserId = item,
                        GroupsId = entity.Id,
                    };
                    entity.EmployeesGroups.Add(employee);
                }
            }

            await _unitOfWork.Repository<Groups>().UpdateAsync(entity);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(entity.Id, _stringLocalizer[SharedResourceKeys.UpdateSuccessfully]);

        }
    }
}
