
namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Put
{
    public class PutSendToHandler : IRequestHandler<PutSendToCommand, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public PutSendToHandler(IUnitOfWork unitOfWork, IResponseHelper responseHelper, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
            _stringLocalizer = stringLocalizer;
        }
        public async Task<ResponseDTO> Handle(PutSendToCommand request, CancellationToken cancellationToken)
        {
            var validator = new PutSendToValidator(_unitOfWork, _stringLocalizer);
            var validation = validator.Validate(request);

            if (!validation.IsValid)
            {
                return _responseHelper.FailedToSave(JsonSerializer.Serialize(validation.Errors.Select(n => n.ErrorMessage).ToList()));
            }
            var sendTo = _unitOfWork.LK_Repository<LK_SendTo>()
                .GetAllAsTracking(x => x.Id == request.putSendToDto.Id && x.State != State.Deleted)
                .FirstOrDefault();
            if (sendTo == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound] ?? "ERROR Localizer");
            }
            sendTo.NameEn = request.putSendToDto.NameEn;
            sendTo.NameAr = request.putSendToDto.NameAr;
            await _unitOfWork.LK_Repository<LK_SendTo>().UpdateAsync(sendTo);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return _responseHelper.SavedSuccessfully(_stringLocalizer[SharedResourceKeys.UpdateSuccessfully] ?? "ERROR Localizer");

        }
    }
}
