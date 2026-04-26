namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Post
{
    public class PostSendToHandler : IRequestHandler<PostSendToCommand, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public PostSendToHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
        }

        public async Task<ResponseDTO> Handle(PostSendToCommand request, CancellationToken cancellationToken)
        {
            var validator = new PostSendToValidator(_unitOfWork, _stringLocalizer);
            var validation = validator.Validate(request);

            if (!validation.IsValid)
            {
                return _responseHelper.FailedToSave(JsonSerializer.Serialize(validation.Errors.Select(n => n.ErrorMessage).ToList()));
            }

            var sendToAdd = new LK_SendTo
            {
                NameAr = request.postSendToDto.NameAr,
                NameEn = request.postSendToDto.NameEn,
            };
            await _unitOfWork.LK_Repository<LK_SendTo>().AddAsync(sendToAdd);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return _responseHelper.SavedSuccessfully(sendToAdd.Id, _stringLocalizer[SharedResourceKeys.AddSuccessfully]);
        }
    }
}
