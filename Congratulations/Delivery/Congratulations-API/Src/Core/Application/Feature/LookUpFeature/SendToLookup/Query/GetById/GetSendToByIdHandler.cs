namespace _4EPlatform.Application.Feature.CategoryFeature.Query.GetById
{
    public class GetSendToByIdHandler : IRequestHandler<GetSendToByIdQuery, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public GetSendToByIdHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
        }
        public async Task<ResponseDTO> Handle(GetSendToByIdQuery request, CancellationToken cancellationToken)
        {
            var sendTo =await _unitOfWork.LK_Repository<LK_SendTo>()
                .GetAllAsNoTracking(x => x.Id == request.Id && x.State != State.Deleted)
                .Select(x => new GetSentToByIdDTO(x.NameAr, x.NameEn))
                .FirstOrDefaultAsync();
            if (sendTo is null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            return _responseHelper.RetrievedSuccessfully(sendTo, _stringLocalizer[SharedResourceKeys.RetrievedSuccessfully]);

        }
    }
}
