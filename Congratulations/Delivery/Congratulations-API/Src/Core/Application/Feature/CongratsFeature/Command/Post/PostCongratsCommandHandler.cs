

namespace _4EPlatform.Application.Feature.CongratsFeature.Command.Post
{
    public class PostCongratsCommandHandler : IRequestHandler<PostCongratsCommand, ResponseDTO>
    {
        private readonly IResponseHelper _responseHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public PostCongratsCommandHandler(IStringLocalizer<SharedResource> stringLocalizer, IResponseHelper responseHelper, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _stringLocalizer = stringLocalizer;
            _responseHelper = responseHelper;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> Handle(PostCongratsCommand request, CancellationToken cancellationToken)
        {
            var validator = new PostCongratsValidator(_stringLocalizer);
            var validation = validator.Validate(request);

            if (!validation.IsValid)
                return _responseHelper.FailedToSave(string.Join("-", validation.Errors.Select(n => n.ErrorMessage).ToList()));

            var entity = _mapper.Map<Domain.Models.Congrats>(request.postCongratsDto);

            //   await _unitOfWork.Repository<Domain.Models.Congrats>().AddAsync(entity);

            #region addOrgs
            if (request.postCongratsDto.Orgs != null)
            {
                //var listOfOrgs = new List<Domain.Models.OrgsCongrats>();
                //listOfOrgs.AddRange(request.Orgs.Select(x => new OrgsCongrats()
                //{
                //    OrgId = x ,
                //    CongratsId = entity.Id
                //}));
                //await _unitOfWork.Repository<Domain.Models.OrgsCongrats>().AddRangeAsync(listOfOrgs);
                foreach (var item in request.postCongratsDto.Orgs)
                {
                    var congratsOrg = new OrgsCongrats()
                    {
                        OrgId = item,
                    };
                    entity.OrgsCongrats.Add(congratsOrg);
                }
            }
            #endregion
            #region addUsers
            if (request.postCongratsDto.Users != null)
            {
                foreach (var item in request.postCongratsDto.Users)
                {
                    var congratsUser = new UsersCongrats()
                    {
                        UserId = item,
                    };
                    entity.UsersCongrats.Add(congratsUser);
                }
            }
            #endregion
            #region addGroups
            if (request.postCongratsDto.Groups != null)
            {
                foreach (var item in request.postCongratsDto.Groups)
                {
                    var congratsGroup = new GroupsCongrats()
                    {
                        GroupsId = item,
                    };
                    entity.GroupsCongrats.Add(congratsGroup);
                }
            }
            #endregion
            await _unitOfWork.Repository<Domain.Models.Congrats>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(entity.Id, _stringLocalizer[SharedResourceKeys.AddSuccessfully]);

        }
    }
}
