namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Delete
{
    public class DeleteSendToHandler : IRequestHandler<DeleteSendToCommand, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public DeleteSendToHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
        }
        public async Task<ResponseDTO> Handle(DeleteSendToCommand request, CancellationToken cancellationToken)
        {
            var sendTo = _unitOfWork.LK_Repository<LK_SendTo>()
                  .GetAllAsTracking(x => x.Id == request.Id && x.State != State.Deleted)
                  .FirstOrDefault();
            if (sendTo == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            sendTo.State = State.Deleted;
           await _unitOfWork.LK_Repository<LK_SendTo>().UpdateAsync(sendTo);
           await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();
            return _responseHelper.SavedSuccessfully(_stringLocalizer[SharedResourceKeys.DeleteSuccessfully]);
        }
    }
}
