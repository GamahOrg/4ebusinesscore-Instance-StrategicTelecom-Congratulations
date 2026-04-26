namespace _4EPlatform.Application.Feature.TemplateFeature.Command.TemplateUserException
{
    public class TemplateUserExceptionHandler : IRequestHandler<TemplateUserExceptionCommand, ResponseDTO>
    {
        private readonly IResponseHelper _responseHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Guid _loggedInUserId;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public TemplateUserExceptionHandler(IStringLocalizer<SharedResource> stringLocalizer, IResponseHelper responseHelper, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _stringLocalizer = stringLocalizer;
            _responseHelper = responseHelper;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedInUserId = LoggedInUserProvider.GetLoggedInUserId(httpContextAccessor);
        }

        public async Task<ResponseDTO> Handle(TemplateUserExceptionCommand request, CancellationToken cancellationToken)
        {
            var template = await _unitOfWork.Repository<Template>().GetAllAsNoTracking(x => x.State != State.Deleted && x.Id == request.TemplateId).FirstOrDefaultAsync();
            if (template == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            var existingModel = await _unitOfWork.Repository<Domain.Models.TemplateUserException>().GetAllAsNoTracking(x => x.State != State.Deleted && x.UserId == _loggedInUserId && x.TemplateId == request.TemplateId).FirstOrDefaultAsync();
            var message = string.Empty;
            if (existingModel != null)
            {
                existingModel.NewUserName = request.NewUserName;
                existingModel.NewJob = request.NewJob;
                existingModel.Job = request.Job;
                existingModel.TemplateId = request.TemplateId;
                await _unitOfWork.Repository<Domain.Models.TemplateUserException>().UpdateAsync(existingModel);
                message = _stringLocalizer[SharedResourceKeys.UpdateSuccessfully];

            }
            else
            {
                var entity = new Domain.Models.TemplateUserException();
                entity.UserId = _loggedInUserId;
                entity.NewUserName = request.NewUserName;
                entity.Job = request.Job;
                entity.TemplateId = request.TemplateId;
                await _unitOfWork.Repository<Domain.Models.TemplateUserException>().AddAsync(entity);
                message = _stringLocalizer[SharedResourceKeys.AddSuccessfully];
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(message);
        }
    }
}
