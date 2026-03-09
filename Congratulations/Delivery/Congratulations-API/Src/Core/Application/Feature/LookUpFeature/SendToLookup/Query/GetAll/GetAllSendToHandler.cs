namespace _4EPlatform.Application.Feature.CategoryFeature.Query.GetAll
{
    public class GetAllSendToHandler : IRequestHandler<GetAllSendToQuery, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public GetAllSendToHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
        }
        public async Task<ResponseDTO> Handle(GetAllSendToQuery request, CancellationToken cancellationToken)
        {
            var sendTo = await _unitOfWork.LK_Repository<LK_SendTo>()
                .GetAllAsNoTracking(x => x.State != State.Deleted)
                .Select(x => new GetAllSendToDTO(x.Id, x.NameAr, x.NameEn))
                .ToListAsync(cancellationToken);
            if (!sendTo.Any())
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            return _responseHelper.RetrievedSuccessfully(sendTo, _stringLocalizer[SharedResourceKeys.RetrievedSuccessfully]);
        }
    }
}
