namespace _4EPlatform.Application.Feature.CongratsFeature.Command.Put
{
    public class PutCongratsCommandHandler : IRequestHandler<PutCongratsCommand, ResponseDTO>
    {
        private readonly IResponseHelper _responseHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public PutCongratsCommandHandler(IStringLocalizer<SharedResource> stringLocalizer, IResponseHelper responseHelper, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _stringLocalizer = stringLocalizer;
            _responseHelper = responseHelper;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> Handle(PutCongratsCommand request, CancellationToken cancellationToken)
        {
            var validator = new PutCongratsValidator(_unitOfWork, _stringLocalizer);
            var validation = validator.Validate(request);

            if (!validation.IsValid)
                return _responseHelper.FailedToSave(string.Join("-", validation.Errors.Select(n => n.ErrorMessage).ToList()));


            var entity = _unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsTracking(x => x.State != State.Deleted && x.Id == request.putCongratsDto.Id).FirstOrDefault();
            if (entity == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            entity = _mapper.Map(request.putCongratsDto, entity);

            await DeleteExistingCongratsAsync(request.putCongratsDto.Id);
            #region group

            if (request.putCongratsDto.Groups != null)
            {
                foreach (var item in request.putCongratsDto.Groups)
                {
                    var congratsGroup = new GroupsCongrats()
                    {
                        GroupsId = item,
                    };
                    entity.GroupsCongrats.Add(congratsGroup);
                }
            }
            #endregion
            #region orgs
            if (request.putCongratsDto.Orgs != null)
            {
                foreach (var item in request.putCongratsDto.Orgs)
                {
                    var congratsOrg = new OrgsCongrats()
                    {
                        OrgId = item,
                    };
                    entity.OrgsCongrats.Add(congratsOrg);
                }
            }
            #endregion
            #region users
            if (request.putCongratsDto.Users != null)
            {
                foreach (var item in request.putCongratsDto.Users)
                {
                    var congratsUser = new UsersCongrats()
                    {
                        UserId = item,
                    };
                    entity.UsersCongrats.Add(congratsUser);
                }
            }
            #endregion
            await _unitOfWork.Repository<Domain.Models.Congrats>().UpdateAsync(entity);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(entity.Id, _stringLocalizer[SharedResourceKeys.UpdateSuccessfully]);

        }

        async Task DeleteExistingCongratsAsync(Guid id)
        {
            var orgs = await _unitOfWork.Repository<OrgsCongrats>().GetAllAsNoTracking(x => x.CongratsId == id).ToListAsync();
            var users = await _unitOfWork.Repository<UsersCongrats>().GetAllAsNoTracking(x => x.CongratsId == id).ToListAsync();
            var groups = await _unitOfWork.Repository<GroupsCongrats>().GetAllAsNoTracking(x => x.CongratsId == id).ToListAsync();

            orgs.ForEach(x => x.State = State.Deleted);
            users.ForEach(x => x.State = State.Deleted);
            groups.ForEach(x => x.State = State.Deleted);

            if (orgs.Any()) await _unitOfWork.Repository<OrgsCongrats>().UpdateRangeAsync(orgs);
            if (users.Any()) await _unitOfWork.Repository<UsersCongrats>().UpdateRangeAsync(users);
            if (groups.Any()) await _unitOfWork.Repository<GroupsCongrats>().UpdateRangeAsync(groups);
        }
    }
}
